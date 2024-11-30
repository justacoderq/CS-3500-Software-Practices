// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain, Fall 2021, Fall 2024
//     - Updated return types
//     - Updated documentation
// <authors> Prachi Aswani </authors>
// <date> 10/18/2024 </date>
namespace CS3500.Spreadsheet;

using CS3500.Formula;
using CS3500.DependencyGraph;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{
}

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception
{
}

/// <summary>
/// <para>
///   Thrown to indicate that a read or write attempt has failed with
///   an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    ///   <para>
    ///     Creates the exception with a message defining what went wrong.
    ///   </para>
    /// </summary>
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException(string msg)
    : base(msg)
    {
    }
}

/// <summary>
///   <para>
///     An Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet represents an infinite number of named cells.
///   </para>
/// <para>
///     Valid Cell Names: A string is a valid cell name if and only if it is one or
///     more letters followed by one or more numbers, e.g., A5, BC27.
/// </para>
/// <para>
///    Cell names are case insensitive, so "x1" and "X1" are the same cell name.
///    Your code should normalize (uppercased) any stored name but accept either.
/// </para>
/// <para>
///     A spreadsheet represents a cell corresponding to every possible cell name.  (This
///     means that a spreadsheet contains an infinite number of cells.)  In addition to
///     a name, each cell has a contents and a value.  The distinction is important.
/// </para>
/// <para>
///     The <b>contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
///     If the contents of a cell is set to the empty string, the cell is considered empty.
/// </para>
/// <para>
///     By analogy, the contents of a cell in Excel is what is displayed on
///     the editing line when the cell is selected.
/// </para>
/// <para>
///     In a new spreadsheet, the contents of every cell is the empty string. Note:
///     this is by definition (it is IMPLIED, not stored).
/// </para>
/// <para>
///     The <b>value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
///     in the grid.) We are not concerned with cell values yet, only with their contents,
///     but for context:
/// </para>
/// <list type="number">
///   <item>If a cell's contents is a string, its value is that string.</item>
///   <item>If a cell's contents is a double, its value is that double.</item>
///   <item>
///     <para>
///       If a cell's contents is a Formula, its value is either a double or a FormulaError,
///       as reported by the Evaluate method of the Formula class.  For this assignment,
///       you are not dealing with values yet.
///     </para>
///   </item>
/// </list>
/// <para>
///     Spreadsheets are never allowed to contain a combination of Formulas that establish
///     a circular dependency.  A circular dependency exists when a cell depends on itself,
///     either directly or indirectly.
///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
///     dependency.
/// </para>
/// </summary>
public class Spreadsheet
{
    // The DependencyGraph tracks dependencies between cells for recalculation.
    private DependencyGraph graph;

    [JsonInclude]
    // Dictionary that stores all the nonempty cells
    private Dictionary<string, Cell> Cells;

    /// <summary>
    /// True if this spreadsheet has been changed since it was 
    /// created or saved (whichever happened most recently),
    /// False otherwise.
    /// </summary>
    [JsonIgnore]
    public bool Changed { get; private set; }

    /// <summary>
    ///   Constructor that initializes a new instance of the Spreadsheet
    ///   class, initializing the dictionary of cells and the dependency
    ///   graph for managing dependencies between cells.
    /// </summary>
    public Spreadsheet()
    {
        this.Cells = new Dictionary<string, Cell>();
        this.graph = new DependencyGraph();
    }

    /// <summary>
    ///   Provides a copy of the normalized names of all of the cells in the spreadsheet
    ///   that contain information (i.e., non-empty cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        // Returns the name using the dictionary
        return new HashSet<string>(this.Cells.Keys);
    }

    /// <summary>
    ///   Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        // Uses helper method to normalize the name to upper case
        name = NormalizeName(name);

        // If the name does not follow letter followed by numbers pattern, this throws
        // InvalidNameException
        if (!IsValidName(name))
        {
            throw new InvalidNameException();
        }

        // Return the contents if the cell exists
        if (Cells.ContainsKey(name))
        {
            return Cells[name].Contents;
        }

        return string.Empty;  // By default, empty cells have empty string contents
    }

    // Helper methods for Name Operations

    /// <summary>
    ///   Helper method that determines if the provided cell name is valid based
    ///   on specific rules for naming cells.
    /// </summary>
    /// <param name="name">The name of the spreadsheet cell to validate.</param>
    /// <returns>
    ///   True if the cell name is valid; otherwise, false.
    /// </returns>
    private bool IsValidName(string name)
    {
        // Valid cell names must start with letters followed by numbers
        return Regex.IsMatch(name, @"^[a-zA-Z]+\d+$");
    }

    /// <summary>
    ///  Helper methos that converts a cell name to uppercase for consistent normalization.
    /// </summary>
    /// <param name="name">The name of the spreadsheet cell to normalize.</param>
    /// <returns>
    ///   The normalized name in uppercase.
    /// </returns>
    private string NormalizeName(string name)
    {
        return name.ToUpper();
    }

    /// <summary>
    ///  Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new contents of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other cells whose value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    ///   <para>
    ///     The order must correspond to a valid dependency ordering for recomputing
    ///     all of the cells, i.e., if you re-evaluate each cells in the order of the list,
    ///     the overall spreadsheet will be correctly updated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///     evaluated, followed by B1, followed by C1.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        // Remove old dependencies for the cell if it already exists
        if (Cells.ContainsKey(name))
        {
            graph.ReplaceDependees(name, new List<string>());
        }

        // Set the contents to the number
        if (!Cells.ContainsKey(name))
        {
            Cells[name] = new Cell();
        }
        Cells[name].Contents = number;
        Cells[name].Value = number;
        // Get the list of cells to recalculate based on this change
        List<string> dependents = GetCellsToRecalculate(name).ToList();

        // After setting the contents, calculate the value of the cell
        //Cells[name].CalculateValue(CellLookup);

        // Return the list of cells in the correct recalculation order
        return dependents;
    }

    /// <summary>
    ///   The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new contents of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {
        // Remove old dependencies for the cell if it already exists
        if (Cells.ContainsKey(name))
        {
            graph.ReplaceDependees(name, new List<string>());
        }
        else
        {
            Cells[name] = new Cell();
        }

        // Set the contents to the text
        Cells[name].Contents = text;
        Cells[name].Value = text;

        // If the text is empty, remove the cell and its dependencies
        if (text == "")
        {
            Cells.Remove(name);
            graph.ReplaceDependees(name, new List<string>());
            return new List<string>(); // No cells to recalculate if empty.
        }

        // Get the list of cells to recalculate based on this change
        List<string> dependents = GetCellsToRecalculate(name).ToList();
        // After setting the contents, calculate the value of the cell
        //Cells[name].CalculateValue(CellLookup);
        // Return the list of cells in the correct recalculation order
        return dependents;
    }

    /// <summary>
    ///   Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException, and no
    ///     change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new contents of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetCellContents(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        try
        {
            // If the cell already has contents, clear its old dependencies
            if (Cells.ContainsKey(name))
            {
                graph.ReplaceDependees(name, new List<string>());
            }

            // Update the dependency graph with the new dependents from the formula
            List<string> newDependents = formula.GetVariables().ToList();
            graph.ReplaceDependees(name, newDependents);

            // Check for circular dependencies before actually updating the cell contents
            List<string> recalculatedCells = GetCellsToRecalculate(name).ToList();

            // There's no circular dependency, update the cell contents
            if (!Cells.ContainsKey(name))
            {
                Cells[name] = new Cell();
            }
            Cells[name].Contents = formula;
            Cells[name].Value = new FormulaError("");
            // After setting the contents, calculate the value of the cell
            //Cells[name].CalculateValue(CellLookup);

            return recalculatedCells;
        }
        catch (CircularException)
        {
            // If a circular dependency is detected, throw the exception and perform no changes
            throw new CircularException();
        }
    }

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all cells whose
    ///   values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all cells
    ///     that contain formulas containing name.
    ///   </para>
    ///   <para>For example, suppose that: </para>
    ///   <list type="bullet">
    ///      <item>A1 contains 3</item>
    ///      <item>B1 contains the formula A1 * A1</item>
    ///      <item>C1 contains the formula B1 + A1</item>
    ///      <item>D1 contains the formula B1 - C1</item>
    ///   </list>
    ///   <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    public IEnumerable<string> GetDirectDependents(string name)
    {
        // Normalize and validate the cell name
        name = NormalizeName(name);

        // If the name does not follow letter followed by numbers pattern, this throws
        // InvalidNameException
        if (!IsValidName(name))
        {
            throw new InvalidNameException();
        }

        // Get Dependents using Dependency Graph
        return graph.GetDependents(name);
    }


    /// <summary>
    ///   <para>
    ///     This method is implemented for you, but makes use of your GetDirectDependents.
    ///   </para>
    ///   <para>
    ///     Returns an enumeration of the names of all cells whose values must
    ///     be recalculated, assuming that the contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    ///   <para>
    ///     For example, suppose that:
    ///   </para>
    ///   <list type="number">
    ///     <item>
    ///       A1 contains 5
    ///     </item>
    ///     <item>
    ///       B1 contains the formula A1 + 2.
    ///     </item>
    ///     <item>
    ///       C1 contains the formula A1 + B1.
    ///     </item>
    ///     <item>
    ///       D1 contains the formula A1 * 7.
    ///     </item>
    ///     <item>
    ///       E1 contains 15
    ///     </item>
    ///   </list>
    ///   <para>
    ///     If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///     and they must be recalculated in an order which has A1 first, and B1 before C1
    ///     (there are multiple such valid orders).
    ///     The method will produce one of those enumerations.
    ///   </para>
    ///   <para>
    ///      PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///      IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///   </para>
    /// </summary>
    /// <param name="name"> The name of the cell.  Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    ///   A helper method for the <see cref="GetCellsToRecalculate"/> method. 
    ///   This method performs a depth-first search to detect circular dependencies and to order the cells 
    ///   that need to be recalculated after a change in a cell's contents.
    /// </summary>
    /// <param name="start">
    ///   The name of the cell that started the recalculation process. This is used to detect circular dependencies.
    /// </param>
    /// <param name="name">
    ///   The current cell being visited during the depth-first search.
    /// </param>
    /// <param name="visited">
    ///   A set of cells that have already been visited during the depth-first search to prevent revisiting the same cell.
    /// </param>
    /// <param name="changed">
    ///   A linked list of cells that need to be recalculated. Cells are added in reverse topological order 
    ///   to ensure that dependent cells are recalculated after the cells they depend on.
    /// </param>
    /// <exception cref="CircularException">
    ///   Thrown if a circular dependency is detected, i.e., if a cell is found to depend (directly or indirectly)
    ///   on itself.
    /// </exception>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        visited.Add(name);
        foreach (string n in GetDirectDependents(name))
        {
            if (n.Equals(start))
            {
                throw new CircularException();
            }
            else if (!visited.Contains(n))
            {
                Visit(start, n, visited, changed);
            }
        }

        changed.AddFirst(name);
    }

    // Added for PS6

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell, as defined by
    ///     <see cref="GetCellValue(string)"/>.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   <see cref="GetCellValue(string)"/>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object this[string name]
    {
        get
        {
            if (!IsValidName(name))
            {
                throw new InvalidNameException();
            }
            return GetCellValue(name);
        }
    }

    /// <summary>
    /// Constructs a spreadsheet using the saved data in the file refered to by
    /// the given filename. 
    /// <see cref="Save(string)"/>
    /// </summary>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   Thrown if the file can not be loaded into a spreadsheet for any reason
    /// </exception>
    /// <param name="filename">The path to the file containing the spreadsheet to load</param>
    public Spreadsheet(string filename)
    {
        Cells = new Dictionary<string, Cell>();
        graph = new DependencyGraph();

        try
        {
            // Read all text from the specified file
            string jsonData = File.ReadAllText(filename);

            // Deserialize JSON data into a Dictionary
            Spreadsheet? deserializedData = JsonSerializer.Deserialize<Spreadsheet>(jsonData);

            // Populate the cells dictionary from deserialized data
            if (deserializedData != null)
            {
                foreach (var entry in deserializedData.Cells)
                {
                    string cellName = entry.Key; // Cell name (e.g., "A1")
                    string cellContent = entry.Value.StringForm; // Assuming each cell has a StringForm property

                    // Use the SetContentsOfCell method to handle content assignment
                    SetContentsOfCell(cellName, cellContent);
                }
            }
            Changed = false;
        }
        catch (Exception)
        {
            throw new SpreadsheetReadWriteException("An error occurred while loading the spreadsheet.");
        }
    }

    /// <summary>
    ///   <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it.
    ///   </para>
    ///   <para>
    ///     The output JSON should look like the following.
    ///   </para>
    ///   <para>
    ///     For example, consider a spreadsheet that contains a cell "A1" 
    ///     with contents being the double 5.0, and a cell "B3" with contents 
    ///     being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    ///   </para>
    ///   <para>
    ///      This method would produce the following JSON string:
    ///   </para>
    ///   <code>
    ///   {
    ///     "Cells": {
    ///       "A1": {
    ///         "StringForm": "5"
    ///       },
    ///       "B3": {
    ///         "StringForm": "=A1+2"
    ///       },
    ///       "C4": {
    ///         "StringForm": "hello"
    ///       }
    ///     }
    ///   }
    ///   </code>
    ///   <para>
    ///     You can achieve this by making sure your data structure is a dictionary 
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName 
    ///     attribute).
    ///   </para>
    ///   <para>
    ///     There can be 0 cells in the dictionary, resulting in { "Cells" : {} } 
    ///   </para>
    ///   <para>
    ///     Further, when writing the value of each cell...
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       If the contents is a string, the value of StringForm is that string
    ///     </item>
    ///     <item>
    ///       If the contents is a double d, the value of StringForm is d.ToString()
    ///     </item>
    ///     <item>
    ///       If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file, 
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        try
        {
            // Get the JSON string representation of the spreadsheet
            string json = GetJson();

            // Write the JSON string to the specified file
            File.WriteAllText(filename, json);

            // Mark the spreadsheet as unchanged after saving
            Changed = false;
        }
        catch (Exception ex)
        {
            throw new SpreadsheetReadWriteException("Failed to save the spreadsheet: " + ex.Message);
        }
    }

    // GUI changes
    /// <summary>
    /// Returns the JSON string representing the current state of the spreadsheet.
    /// </summary>
    /// <returns>A JSON string representing the spreadsheet.</returns>
    public string GetJson()
    {
        // Set up JSON serialization options
        var jsonOptions = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true // Enable pretty printing
        };

        // Serialize the current spreadsheet state to JSON
        return JsonSerializer.Serialize(this, jsonOptions);
    }

    /// <summary>
    /// Replaces the existing spreadsheet data with the new one represented by a JSON string.
    /// </summary>
    /// <param name="jsonString">The JSON string representing the spreadsheet to load.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    /// Thrown if the JSON string cannot be deserialized into a spreadsheet for any reason.
    /// </exception>
    public void LoadFromJson(string jsonString)
    {
        try
        {
            // Deserialize the JSON data into a Spreadsheet object
            Spreadsheet? deserializedData = JsonSerializer.Deserialize<Spreadsheet>(jsonString);

            // Clear existing cells and graph
            Cells = new Dictionary<string, Cell>();
            graph = new DependencyGraph();

            // Populate the cells dictionary from deserialized data
            if (deserializedData != null)
            {
                foreach (var entry in deserializedData.Cells)
                {
                    string cellName = entry.Key; // Cell name (e.g., "A1")
                    string cellContent = entry.Value.StringForm; // Assuming each cell has a StringForm property

                    // Use the SetContentsOfCell method to handle content assignment
                    SetContentsOfCell(cellName, cellContent);
                }
            }

            Changed = false; // Mark as unchanged after loading
        }
        catch (Exception)
        {
            throw new SpreadsheetReadWriteException("An error occurred while loading the spreadsheet from JSON.");
        }
    }


    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value should be either a string, a double, or a CS3500.Formula.FormulaError.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string name)
    {
        name = NormalizeName(name);
        // Validate cell name
        if (!IsValidName(name))
        {
            throw new InvalidNameException();
        }

        // If it is not in the dictionary return an empty string
        if (!Cells.ContainsKey(name))
        {
            return string.Empty;
        }

        // Return the computed value (can be a double, string, or FormulaError)
        return Cells[name].Value;
    }

    /// <summary>
    ///   <para>
    ///     Set the contents of the named cell to be the provided string
    ///     which will either represent (1) a string, (2) a number, or 
    ///     (3) a formula (based on the prepended '=' character).
    ///   </para>
    ///   <para>
    ///     Rules of parsing the input string:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       <para>
    ///         If 'content' parses as a double, the contents of the named
    ///         cell becomes that double.
    ///       </para>
    ///     </item>
    ///     <item>
    ///         If the string does not begin with an '=', the contents of the 
    ///         named cell becomes 'content'.
    ///     </item>
    ///     <item>
    ///       <para>
    ///         If 'content' begins with the character '=', an attempt is made
    ///         to parse the remainder of content into a Formula f using the Formula
    ///         constructor.  There are then three possibilities:
    ///       </para>
    ///       <list type="number">
    ///         <item>
    ///           If the remainder of content cannot be parsed into a Formula, a 
    ///           CS3500.Formula.FormulaFormatException is thrown.
    ///         </item>
    ///         <item>
    ///           Otherwise, if changing the contents of the named cell to be f
    ///           would cause a circular dependency, a CircularException is thrown,
    ///           and no change is made to the spreadsheet.
    ///         </item>
    ///         <item>
    ///           Otherwise, the contents of the named cell becomes f.
    ///         </item>
    ///       </list>
    ///     </item>
    ///   </list>
    /// </summary>
    /// <returns>
    ///   <para>
    ///     The method returns a list consisting of the name plus the names 
    ///     of all other cells whose value depends, directly or indirectly, 
    ///     on the named cell. The order of the list should be any order 
    ///     such that if cells are re-evaluated in that order, their dependencies 
    ///     are satisfied by the time they are evaluated.
    ///   </para>
    ///   <example>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list {A1, B1, C1} is returned.
    ///   </example>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If name is invalid, throws an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///     If a formula would result in a circular dependency, throws CircularException.
    /// </exception>
    public IList<string> SetContentsOfCell(string name, string content)
    {
        IList<string> affectedCells;
        // Normalize the name and validate it
        name = NormalizeName(name);
        if (!IsValidName(name))
        {
            throw new InvalidNameException();
        }

        // Check if the content represents a number
        if (double.TryParse(content, out double number))
        {
            // Call the private SetCellContents for double
            affectedCells = SetCellContents(name, number);
            Changed = true; // Mark spreadsheet as changed
        }

        // If the content does not start with '=', treat it as a string
        else if (!content.StartsWith("="))
        {
            // Call the private SetCellContents for string
            affectedCells = SetCellContents(name, content);
            Changed = true; // Mark spreadsheet as changed

        }
        else
        {
            string formulaContent = content.Substring(1); // Remove the '=' character
            Formula formula = new Formula(formulaContent);
            affectedCells = SetCellContents(name, formula); // Call the private SetCellContents for Formula
            Changed = true; // Mark spreadsheet as changed
        }

        foreach (var cell in affectedCells)
        {
            if (Cells.ContainsKey(cell) && Cells[cell].Contents is Formula formula)
            {
                Cells[cell].Value = formula.Evaluate(CellLookup);
            }
        }

        return affectedCells;
    }

    /// <summary>
    /// Looks up the value of the cell with the specified name.
    /// </summary>
    /// <param name="name">The name of the cell to look up (e.g., "A1").</param>
    /// <returns>
    /// The numeric value of the cell if it exists.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the cell with the specified name does not exist.
    /// </exception>
    private double CellLookup(string name)
    {
        // Check if the cell exists in the Cells dictionary
        if (!Cells.ContainsKey(name))
        {
            // Throw an exception if the cell does not exist
            throw new ArgumentException($"Cell {name} does not exist.");
        }

        if (Cells[name].Value is double d)
        {
            return d;
        }

        else
        {
            throw new ArgumentException();
        }
    }

    /// <summary>
    /// Represents a cell in the spreadsheet that can hold various types of contents such as a string, double, or formula.
    /// </summary>
    private class Cell
    {
        /// <summary>
        /// Gets or sets the contents of the cell.
        /// This can be a string, double, or Formula.
        /// </summary>
        [JsonIgnore] public object Contents { get; set; }

        /// <summary>
        /// Gets or sets the value of the cell.
        /// This can be a string, double, or FormulaError.
        /// </summary>
        [JsonIgnore] public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class
        /// with default empty contents and value.
        /// </summary>
        public Cell()
        {
            Contents = string.Empty;
            Value = string.Empty;
        }

        /// <summary>
        /// Gets the string representation of the cell's contents.
        /// </summary>
        public string StringForm
        {
            get
            {
                if (Contents is double d)
                {
                    return d.ToString(); // Double to string
                }
                else if (Contents is Formula f)
                {
                    return "=" + f.ToString(); // Formula prefixed with '='
                }
                else
                {
                    return (string)Contents; // Return the string directly
                }

            }

            set
            {
                // Deserialize the string back into the correct type for Contents
                if (double.TryParse(value, out double d))
                {
                    Contents = d;
                }
                else if (value.StartsWith("="))
                {
                    // Remove the '=' and treat it as a formula
                    Contents = new Formula(value.Substring(1));
                }
                else
                {
                    // Treat it as a regular string
                    Contents = value;
                }
            }

        }

        /// <summary>
        /// Calculates the value of the cell based on its contents.
        /// </summary>
        /// <param name="lookup">The lookup function to resolve cell dependencies.</param>
        public void CalculateValue(Lookup lookup)
        {
            // If the contents is a number or a string, just assign it as the value
            if (Contents is double || Contents is string)
            {
                Value = Contents; // Directly assign if it's a number or string
            }
            else if (Contents is Formula formula)
            {
                // Evaluate the formula by passing the lookup function that resolves dependencies
                Value = formula.Evaluate(lookup);
            }
        }
    }
}
