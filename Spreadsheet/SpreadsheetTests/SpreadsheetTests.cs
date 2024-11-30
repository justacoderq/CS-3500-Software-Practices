// <authors> Prachi Aswani </authors>
// <date> 10/18/2024 </date>

using CS3500.Formula;
using CS3500.Spreadsheet;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.Json;
using static CS3500.Spreadsheet.Spreadsheet;

namespace SpreadsheetTests
{
    /// <summary>
    /// This is a test class for SpreadsheetTests and is intended
    /// to contain all SpreadsheetTests Unit Tests for all the methods.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        // Tests for GetNamesofAllNonEmptyCells method

        /// <summary>
        /// Tests that when a single non-empty cell is present,
        /// GetNamesOfAllNonemptyCells returns the correct name.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_SingleCell_ReturnsCorrectName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(1, nonEmptyCells.Count);
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
        }

        /// <summary>
        /// Tests that multiple non-empty cells return the correct names.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_MultipleCells_ReturnsCorrectNames()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "3.0");
            sheet.SetContentsOfCell("C1", "2.0");

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(3, nonEmptyCells.Count);
            CollectionAssert.AreEquivalent(new List<string> { "A1", "B1", "C1" }, nonEmptyCells.ToList());
        }

        /// <summary>
        /// Tests that no cells return an empty set of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_NoCells_ReturnsEmptySet()
        {
            Spreadsheet sheet = new Spreadsheet();
            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, nonEmptyCells.Count);
        }

        /// <summary>
        /// Tests that when a cell is cleared (set to empty string),
        /// it is no longer included in the set of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_CellCleared_ReturnsCorrectNames()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("A1", "");  // Clearing the contents

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(0, nonEmptyCells.Count);
        }

        /// <summary>
        /// Tests that a cell with a formula is correctly included in
        /// the set of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_CellWithFormula_ReturnsCorrectName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 1");

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(1, nonEmptyCells.Count);
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
        }

        /// <summary>
        /// Tests that a cell with a string value is correctly included
        /// in the set of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_CellWithString_ReturnsCorrectName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Hello");

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(1, nonEmptyCells.Count);
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
        }

        /// <summary>
        /// Tests that a cell with an empty string is not considered non-empty
        /// and is not included in the set of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_CellWithEmptyString_NotIncludedInNonEmptyCells()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "");  // Set cell to an empty string

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(0, nonEmptyCells.Count);
        }

        /// <summary>
        /// Tests that when multiple cells contain various contents (numbers, strings, formulas),
        /// all non-empty cells are correctly returned.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_MultipleNonemptyCells_ReturnsCorrectNames()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B2", "Hello");
            sheet.SetContentsOfCell("C3", "=A1 + 1");  // Formula depends on A1

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(3, nonEmptyCells.Count);
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
            Assert.IsTrue(nonEmptyCells.Contains("B2"));
            Assert.IsTrue(nonEmptyCells.Contains("C3"));
        }

        /// <summary>
        /// Tests that when cells contain mixed types (numbers, strings, empty strings, formulas),
        /// only non-empty cells are included in the set of non-empty cells.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_MixedCellContents_ReturnsCorrectNames()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "10.0");
            sheet.SetContentsOfCell("B1", "Test");
            sheet.SetContentsOfCell("C1", "");  // Empty cell
            sheet.SetContentsOfCell("D1", "=A1 * 2");
            sheet.SetContentsOfCell("E1", "");  // Another empty cell
            sheet.SetContentsOfCell("F1", "3.14");

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(4, nonEmptyCells.Count);  // A1, B1, D1, F1
            Assert.IsTrue(nonEmptyCells.Contains("A1"));
            Assert.IsTrue(nonEmptyCells.Contains("B1"));
            Assert.IsTrue(nonEmptyCells.Contains("D1"));
            Assert.IsTrue(nonEmptyCells.Contains("F1"));
        }

        /// <summary>
        /// Tests that case insensitivity is handled correctly, ensuring that cell names
        /// are treated consistently regardless of case.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonemptyCells_CaseInsensitivity_ReturnsSameName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("b1", "Hello");  // Same as A1, should be treated as non-empty

            ISet<string> nonEmptyCells = sheet.GetNamesOfAllNonemptyCells();

            Assert.AreEqual(2, nonEmptyCells.Count); // Should only return one entry
            Assert.IsTrue(nonEmptyCells.Contains("A1")); // The stored name should be normalized to "A1"
            Assert.IsFalse(nonEmptyCells.Contains("a1")); // "a1" should not be present in the result
            Assert.IsTrue(nonEmptyCells.Contains("B1"));
        }

        // Tests for SetContentsOfCells method

        /// <summary>
        /// Tests that SetContentsOfCells updates the cell contents correctly when a valid
        /// name and number are provided, and that the updated cell is returned in the result.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_ValidNameNumber_CellContentsUpdated()
        {
            Spreadsheet sheet = new Spreadsheet();
            var result = sheet.SetContentsOfCell("A1", "5.0");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A1", result[0]);
            Assert.AreEqual(5.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that dependent cells are recalculated in the correct order after
        /// updating a cell's value that affects the dependent cells.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_DependentCellsOrder_ValidOrderReturned()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "=A1 * 2");
            sheet.SetContentsOfCell("C1", "=B1 + A1");

            var result = sheet.SetContentsOfCell("A1", "10.0");

            var expectedOrder = new List<string> { "A1", "B1", "C1" };
            CollectionAssert.AreEqual(expectedOrder, result.ToList());
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when an invalid cell name is passed
        /// to SetContentsOfCells.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCells_InvalidName_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1A1A", "1.5");
        }

        /// <summary>
        /// Tests that case insensitivity is handled correctly, ensuring that cell names
        /// are treated consistently regardless of case.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_CaseInsensitivity_ValidNames()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            var result = sheet.SetContentsOfCell("a1", "15.0"); // Same as A1

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A1", result[0]); // Verify the case normalization
            Assert.AreEqual(15.0, sheet.GetCellContents("A1")); // Verify the value is updated
        }

        /// <summary>
        /// Tests that clearing a cell's content returns an empty dependency list and
        /// removes the cell from the set of non-empty cells.
        [TestMethod]
        public void TestSetContentsOfCells_CellCleared_ReturnsEmptyDependencyList()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            var result = sheet.SetContentsOfCell("A1", ""); // Clearing the contents

            Assert.AreEqual(0, result.Count); // No dependencies after clearing
            Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().Contains("A1"));
        }

        /// <summary>
        /// Tests that cells with dependencies return the correct order of recalculations
        /// when a cell's value is updated.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_CellWithDependencies_ReturnsCorrectOrder()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "=A1 * 2"); // B1 depends on A1
            var result = sheet.SetContentsOfCell("A1", "10.0"); // Update A1

            var expectedOrder = new List<string> { "A1", "B1" };
            CollectionAssert.AreEqual(expectedOrder, result.ToList());
        }

        /// <summary>
        /// Ensures that a CircularException is thrown when circular dependencies are detected
        /// while setting cell contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetContentsOfCells_CircularDependency_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 1");
            sheet.SetContentsOfCell("B1", "=A1 + 2");

            // The exception should be thrown when trying to set the second cell.
        }

        /// <summary>
        /// Tests that all dependent cells are returned in the correct order after updating
        /// a cell with multiple dependencies.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_MultipleCells_ReturnsAllDependencies()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1.0");
            sheet.SetContentsOfCell("B1", "=A1 + 2");
            sheet.SetContentsOfCell("C1", "=B1 * 3");

            var result = sheet.SetContentsOfCell("B1", "5.0"); // Update B1

            var expectedOrder = new List<string> { "B1", "C1" }; // B1 should be first, then C1
            CollectionAssert.AreEqual(expectedOrder, result.ToList());
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when an invalid cell name (starting with a number)
        /// is passed to SetContentsOfCells.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCells_InvalidName_ExceptionThrown()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("1A", "5.0");
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when an empty string is passed as a cell name
        /// to SetContentsOfCells.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCells_EmptyStringName_ExceptionThrown()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("", "5.0");
        }

        /// <summary>
        /// Tests that setting a negative number as cell contents is handled correctly,
        /// and the value is stored as expected in the cell.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_NegativeNumber_ValidResultReturned()
        {
            Spreadsheet sheet = new Spreadsheet();
            var result = sheet.SetContentsOfCell("A1", "-10.0");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("A1", result[0]);
            Assert.AreEqual(-10.0, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when another invalid cell name (starting with a number)
        /// is passed to SetContentsOfCells.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCells_InvalidCellName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("1A", "3.6");
        }

        /// <summary>
        /// Tests that when setting contents of a cell that has dependencies, the correct order of dependencies
        /// is returned when a cell's content is updated.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_DuplicateName_CorrectOrderReturned()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "=A1 + 1");
            sheet.SetContentsOfCell("C1", "=B1 + 1");

            var result = sheet.SetContentsOfCell("A1", "10.0");

            var expectedOrder = new List<string> { "A1", "B1", "C1" };
            CollectionAssert.AreEqual(expectedOrder, result.ToList());
        }

        /// <summary>
        /// Tests that when a valid string is set as cell contents, the contents are updated correctly,
        /// and the cell is returned in the dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_ValidString_UpdatesCellContents()
        {
            Spreadsheet sheet = new Spreadsheet();

            IList<string> result = sheet.SetContentsOfCell("A1", "Hello");

            Assert.AreEqual(1, result.Count);  // Only A1 should be recalculated
            Assert.IsTrue(result.Contains("A1"));
            Assert.AreEqual("Hello", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that updating an existing cell with a new string value updates correctly,
        /// replacing the old content while returning the correct dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_UpdateExistingCellWithNewString_UpdatesCorrectly()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "OldContent");

            IList<string> result = sheet.SetContentsOfCell("A1", "NewContent");

            Assert.AreEqual(1, result.Count);  // Only A1 should be recalculated
            Assert.IsTrue(result.Contains("A1"));
            Assert.AreEqual("NewContent", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that replacing a formula cell with a string updates the cell correctly
        /// and reflects the change in the spreadsheet.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_FormulaCellReplacedByString_UpdatesCorrectly()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 1");

            IList<string> result = sheet.SetContentsOfCell("A1", "ReplacedContent");

            Assert.AreEqual(1, result.Count);  // Only A1 should be recalculated
            Assert.IsTrue(result.Contains("A1"));
            Assert.AreEqual("ReplacedContent", sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that an InvalidNameException is thrown when an invalid cell name (starting with a number)
        /// is used while setting text contents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCellsText_InvalidCellName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("1A", "Invalid");
        }

        /// <summary>
        /// Tests that setting an empty string as content for a cell removes the cell from the spreadsheet,
        /// returning an empty dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_EmptyStringContent_RemovesCell()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "SomeContent");

            IList<string> result = sheet.SetContentsOfCell("A1", "");

            Assert.AreEqual(0, result.Count);  // No cells should need recalculation
            Assert.AreEqual("", sheet.GetCellContents("A1"));  // A1 should no longer exist or be empty
        }

        /// <summary>
        /// Tests that setting an empty string to a non-existing cell
        /// does nothing and returns an empty dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_SetEmptyStringToNonExistingCell_DoesNothing()
        {
            Spreadsheet sheet = new Spreadsheet();

            IList<string> result = sheet.SetContentsOfCell("A1", "");

            Assert.AreEqual(0, result.Count);  // No cells should need recalculation
            Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().Contains("A1"));  // A1 should not exist
        }

        /// <summary>
        /// Tests that updating a cell that has dependents correctly updates the dependents and returns them in the list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_SetStringToAnotherCellWithDependents_UpdatesDependents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "=A1 + 2");  // B1 depends on A1

            IList<string> result = sheet.SetContentsOfCell("A1", "NewValue");

            Assert.AreEqual(2, result.Count);  // Both A1 and B1 should be recalculated
            Assert.IsTrue(result.Contains("A1"));
            Assert.IsTrue(result.Contains("B1"));
        }

        /// <summary>
        /// Tests that setting a valid formula updates the cell contents correctly and returns the cell in the dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_ValidFormula_UpdatesCellContents()
        {
            Spreadsheet sheet = new Spreadsheet();
            Formula formula = new Formula("A1 + B1");

            IList<string> result = sheet.SetContentsOfCell("C1", "=A1+B1");

            Assert.AreEqual(1, result.Count);  // Only C1 should be recalculated
            Assert.IsTrue(result.Contains("C1"));
            Assert.AreEqual(formula, sheet.GetCellContents("C1"));
        }

        /// <summary>
        /// Tests that updating an existing cell with a new formula correctly updates the cell
        /// and reflects the change in the dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_UpdateExistingCellWithNewFormula_UpdatesCorrectly()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 2");

            Formula newFormula = new Formula("B1 * 2");
            IList<string> result = sheet.SetContentsOfCell("A1", "=B1*2");

            Assert.AreEqual(1, result.Count);  // Only A1 should be recalculated
            Assert.IsTrue(result.Contains("A1"));
            Assert.AreEqual(newFormula, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that replacing an existing string cell with a formula updates the cell correctly
        /// and reflects the change in the dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_FormulaCellReplacesAnotherType_UpdatesCorrectly()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Some text");  // Initial content is a string

            Formula formula = new Formula("B1 + 2");
            IList<string> result = sheet.SetContentsOfCell("A1", "=B1+2");

            Assert.AreEqual(1, result.Count);  // Only A1 should be recalculated
            Assert.IsTrue(result.Contains("A1"));
            Assert.AreEqual(formula, sheet.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests that setting a valid formula to a cell updates the cell contents correctly and returns the cell in the dependency list.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_ValidNameFormula_CellContentsUpdated()
        {
            Spreadsheet sheet = new Spreadsheet();
            var formula = new Formula("A1 + 2");
            var result = sheet.SetContentsOfCell("B1", "=A1+2");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("B1", result[0]); // B1 should be recalculated
            Assert.AreEqual(formula, sheet.GetCellContents("B1"));
        }
        /// <summary>
        /// Tests that an InvalidNameException is thrown when trying to set a formula to an invalid cell name (starting with a number).
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetContentsOfCells_InvalidName_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            var formula = new Formula("A1 + 2");
            sheet.SetContentsOfCell("1B", "=A1+2"); // Invalid cell name should throw exception
        }

        /// <summary>
        /// Tests that setting a formula with a case-insensitive cell name works correctly,
        /// ensuring the contents of cell "B1" are updated and dependencies are tracked appropriately.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_CaseInsensitivityFormula_ValidNames()
        {
            Spreadsheet sheet = new Spreadsheet();
            var formula = new Formula("A1 + 2");
            sheet.SetContentsOfCell("A1", "5.0");
            var result = sheet.SetContentsOfCell("b1", "=A1+2");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("B1", result[0]);
            Assert.AreEqual(formula, sheet.GetCellContents("B1"));
        }

        /// <summary>
        /// Ensures that when a cell with dependencies is updated, the correct dependencies are returned,
        /// verifying that the new formula correctly updates the contents of cell "B1".
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_CellWithDependencies_ReturnsCorrectDependencies()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("B1", "=A1 + 2");

            var result = sheet.SetContentsOfCell("B1", "=A1 * 3");

            var expectedOrder = new List<string> { "B1" };
            CollectionAssert.AreEqual(expectedOrder, result.ToList());
        }

        /// <summary>
        /// Tests that a CircularException is thrown when attempting to create a circular dependency
        /// by setting a formula that references another cell that, in turn, references the original cell.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetContentsOfCells_CircularDependencFormulay_ThrowsException()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1 + 1");
            sheet.SetContentsOfCell("B1", "=A1 + 2");

            // This will throw a CircularException when trying to set the second cell.
            sheet.SetContentsOfCell("A1", "=B1 + 1");
        }

        /// <summary>
        /// Tests that setting an empty formula as contents clears the cell and updates the dependency graph,
        /// ensuring that no dependencies are returned after clearing the contents of cell "B1".
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_EmptyFormula_ValidDependencyGraph()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            var result = sheet.SetContentsOfCell("B1", "=A1 + 2");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("B1", result[0]);

            result = sheet.SetContentsOfCell("B1", ""); // Should clear the contents

            Assert.AreEqual(0, result.Count); // No dependencies after clearing
            Assert.IsFalse(sheet.GetNamesOfAllNonemptyCells().Contains("B1"));
        }

        /// <summary>
        /// Tests that when a valid formula with multiple dependencies is updated,
        /// all dependent cells are returned in the correct order, verifying proper dependency tracking.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCells_ValidFormulaWithMultipleDependencies_ReturnsAllDependencies()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "1.0");
            sheet.SetContentsOfCell("B1", "=A1 + 2");
            sheet.SetContentsOfCell("C1", "=B1 * 3");

            var result = sheet.SetContentsOfCell("A1", "=5 + 5");

            var expectedOrder = new List<string> { "A1", "B1", "C1" };
            CollectionAssert.AreEqual(expectedOrder, result.ToList());
        }

        // Tests for GetCellsContent method

        /// <summary>
        /// Tests that retrieving the content of an existing string cell returns the correct string value.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents_ExistingStringCell_ReturnsStringContent()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "Hello World");

            var content = sheet.GetCellContents("A1");

            Assert.AreEqual("Hello World", content);
        }

        /// <summary>
        /// Tests that retrieving the content of an existing number cell returns the correct number value.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents_ExistingNumberCell_ReturnsNumberContent()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B2", " 3.14");

            var content = sheet.GetCellContents("B2");

            Assert.AreEqual(3.14, content);
        }

        /// <summary>
        /// Tests that retrieving the content of an existing formula cell returns the correct formula object.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents_ExistingFormulaCell_ReturnsFormulaContent()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0");
            sheet.SetContentsOfCell("C3", "=A1+2");

            var content = sheet.GetCellContents("C3");

            Assert.AreEqual(new Formula("A1+2"), content);
        }

        /// <summary>
        /// Tests that retrieving the content of an empty cell returns an empty string as expected.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents_EmptyCell_ReturnsEmptyString()
        {
            Spreadsheet sheet = new Spreadsheet();

            var content = sheet.GetCellContents("D4"); // D4 is empty

            Assert.AreEqual(string.Empty, content);
        }

        /// <summary>
        /// Tests that retrieving the content of a cell with an invalid name throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContents_InvalidName_ThrowsInvalidNameException()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.GetCellContents("1A"); // Invalid cell name
        }

        /// <summary>
        /// Tests that retrieving the content of a cell with a valid name is case-insensitive,
        /// verifying that content can be retrieved using different casing for the cell name.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents_CaseInsensitivity_ValidName()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("e5", "Test Content");

            var content = sheet.GetCellContents("E5"); // Case should be normalized

            Assert.AreEqual("Test Content", content);
        }

        /// <summary>
        /// Tests that retrieving the content of a non-existing cell returns an empty string as expected.
        /// </summary>
        [TestMethod]
        public void TestGetCellContents_NonExistingCell_ReturnsEmptyString()
        {
            Spreadsheet sheet = new Spreadsheet();

            var content = sheet.GetCellContents("Z9"); // Z9 does not exist

            Assert.AreEqual(string.Empty, content);
        }

        /// <summary>
        /// Tests that retrieving the content of a cell with an invalid name throws an InvalidNameException.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellContents_InvalidName_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }

        /// <summary>
        /// Tests that retrieving the content of an empty spreadsheet returns an empty string as expected.
        /// </summary>
        [TestMethod()]
        public void TestGetCellContents_EmptySpreasheet_ReturnsEmptyContents()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }

        // Tests for GetDirectDependents method

        /// <summary>
        /// Tests that a valid cell with no dependents correctly returns an empty enumeration,
        /// verifying that the dependency tracking functions as expected for cell "A1".
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_ValidCellWithNoDependents_ReturnsEmptyEnumeration()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "5.0"); // A1 has no dependents

            var dependents = sheet.GetDirectDependents("A1");

            Assert.IsFalse(dependents.Any(), "Expected no dependents for A1");
        }

        /// <summary>
        /// Tests that a valid cell with dependents returns the correct direct dependents,
        /// ensuring that only cells that directly depend on "A1" are included in the result.
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_ValidCellWithDependents_ReturnsCorrectDependents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1 * 2"); // B1 depends on A1
            sheet.SetContentsOfCell("C1", "=B1 + 1"); // C1 depends on B1

            var dependents = sheet.GetDirectDependents("A1").ToList();

            // Only B1 should be returned because C1 depends on B1, not directly on A1
            CollectionAssert.AreEqual(new List<string> { "B1" }, dependents);
        }

        /// <summary>
        /// Tests that a valid cell with multiple dependents returns all unique dependents,
        /// verifying that both B1 and C1 are returned as they both depend on "A1".
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_ValidCellWithMultipleDependents_ReturnsUniqueDependents()
        {
            Spreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "2");
            sheet.SetContentsOfCell("B1", "=A1 + 1"); // B1 depends on A1
            sheet.SetContentsOfCell("C1", "=A1 + 2"); // C1 also depends on A1

            var dependents = sheet.GetDirectDependents("A1").ToList();

            CollectionAssert.AreEqual(new List<string> { "B1", "C1" }, dependents);
        }

        /// <summary>
        /// Tests that a cell with multiple dependent formulas correctly returns all direct dependents,
        /// verifying the accuracy of dependency tracking for complex formulas involving multiple dependencies.
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_CellWithMultipleDependentFormulas_ReturnsCorrectDependents()
        {
            Spreadsheet sheet = new Spreadsheet();

            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1 * A1");
            sheet.SetContentsOfCell("C1", "=B1 + A1");
            sheet.SetContentsOfCell("D1", "=B1 - C1");

            var directDependentsOfA1 = sheet.GetDirectDependents("A1").ToList();

            CollectionAssert.AreEquivalent(new List<string> { "B1", "C1" }, directDependentsOfA1);
        }

        /// <summary>
        /// Tests that attempting to get direct dependents for a cell with an invalid name throws an InvalidNameException,
        /// ensuring proper validation of cell names.
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_InvalidName_ThrowsInvalidNameException()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.ThrowsException<InvalidNameException>(() => sheet.GetDirectDependents("1A")); // Invalid name
        }

        /// <summary>
        /// Tests that attempting to get direct dependents for a cell with an empty name throws an InvalidNameException,
        /// verifying that empty cell names are handled correctly.
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_EmptyName_ThrowsInvalidNameException()
        {
            Spreadsheet sheet = new Spreadsheet();
            Assert.ThrowsException<InvalidNameException>(() => sheet.GetDirectDependents("")); // Empty name
        }

        /// <summary>
        /// Tests that retrieving direct dependents for a non-existing cell returns an empty enumeration,
        /// ensuring that the method handles cases where the specified cell does not exist in the spreadsheet.
        /// </summary>
        [TestMethod]
        public void TestGetDirectDependents_NonExistingCell_ReturnsEmptyEnumeration()
        {
            Spreadsheet sheet = new Spreadsheet();

            var dependents = sheet.GetDirectDependents("Z9"); // Z9 does not exist

            Assert.IsFalse(dependents.Any(), "Expected no dependents for non-existing cell Z9");
        }

        // PS 6 Tests

        /// <summary>
        /// Tests that the Normalize method correctly handles a lowercase cell name.
        /// </summary>
        [TestMethod]
        public void TestNormalize_Lowercase_Valid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("b1"));
        }

        /// <summary>
        /// Tests that the Normalize method correctly normalizes and evaluates a formula with complex references.
        /// </summary>
        [TestMethod]
        public void TestNormalize_Complex_Valid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("A1", "6");
            s.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(6.0, (double)s.GetCellValue("B1"), 1e-9);
        }

        /// <summary>
        /// Tests that the indexer method correctly returns a string value from the specified cell.
        /// </summary>
        [TestMethod]
        public void TestThis_String_ReturnsValidValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            var value = s["A1"];
            Assert.AreEqual("Hello", value);
        }

        /// <summary>
        /// Tests that the indexer method correctly returns a numeric value from the specified cell.
        /// </summary>
        [TestMethod]
        public void TestThis_Double_ReturnsValidValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "42");
            var value = s["A1"];
            Assert.AreEqual(42.0, value); // Numeric values are returned as doubles
        }

        /// <summary>
        /// Tests that the indexer method correctly returns a value when the cell contains a formula.
        /// </summary>
        [TestMethod]
        public void TestThis_Formula_ReturnsValidValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=2+2");
            var value = s["A1"];
            Assert.AreEqual(4.0, value); // Formula should evaluate to 4.0
        }

        /// <summary>
        /// Tests that accessing a cell with an empty string as the name throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestThis_EmptyString_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            var value = s[""];
        }

        /// <summary>
        /// Tests that accessing a cell with an invalid name throws an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestThis_InvalidName_Throws()
        {
            Spreadsheet s = new Spreadsheet();
            var value = s["1A"]; // Invalid format
        }


        /// <summary>
        /// Tests that when the Spreadsheet constructor is given a JSON file that cannot be deserialized,
        /// it initializes an empty spreadsheet.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_SpreadsheetCannotDeserialize_ReturnsEmptySpreadsheet()
        {
            var filename = "test_spreadsheet_single_cell.json";
            File.WriteAllText(filename, @"{ ""A1"": { ""StringForm"": ""Hello"" } }");

            // Since the provided JSON cannot be deserialized correctly, expect an empty spreadsheet
            Spreadsheet spreadsheet = new Spreadsheet(filename);

            // Assert that the spreadsheet is initialized to be empty
            Assert.AreEqual(0, spreadsheet.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// Tests that when the Spreadsheet constructor is provided with multiple cells 
        /// but cannot deserialize them, it returns an empty spreadsheet.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_SpreadsheetWithMultipleCellsNotDeserialized_ReturnsEmptySpreadsheet()
        {
            var filename = "test_spreadsheet_multiple_cells.json";
            File.WriteAllText(filename, @"{ ""A1"": { ""StringForm"": ""Hello"" }, ""B1"": { ""StringForm"": ""World"" } }");

            Spreadsheet spreadsheet = new Spreadsheet(filename);

            // Assert that the spreadsheet is initialized to be empty
            Assert.AreEqual(0, spreadsheet.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor throws a SpreadsheetReadWriteException 
        /// when trying to load a missing file.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSpreadsheetConstructor_InvalidSpreadsheetFile_Throws()
        {
            var filename = "/missing/save.json";
            Spreadsheet spreadsheet = new Spreadsheet(filename);
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor throws a SpreadsheetReadWriteException 
        /// when provided with an empty file.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSpreadsheetConstructor_EmptyFile_Throws()
        {
            var filename = "test_empty_file.json";
            File.WriteAllText(filename, string.Empty); // Write empty content to file
            Spreadsheet s = new Spreadsheet(filename);
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor correctly loads a spreadsheet 
        /// with a single cell and returns the valid value.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_SpreadsheetWithSingleCell_ReturnsValidValue1()
        {
            var filename = "test_spreadsheet_single_cell.json";
            File.WriteAllText(filename, @"{ ""Cells"": { ""A1"": { ""StringForm"": ""Hello"" } } }");
            Spreadsheet spreadsheet = new Spreadsheet(filename);
            Assert.AreEqual("Hello", (string)spreadsheet.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor correctly loads a spreadsheet 
        /// with multiple cells and returns valid values.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_SpreadsheetWithMultipleCells_ReturnsValidValues1()
        {
            var filename = "test_spreadsheet_multiple_cells.json";
            File.WriteAllText(filename, @"{ ""Cells"": { ""A1"": { ""StringForm"": ""Hello"" }, ""B1"": { ""StringForm"": ""World"" } } }");
            Spreadsheet spreadsheet = new Spreadsheet(filename);
            Assert.AreEqual("Hello", (string)spreadsheet.GetCellValue("A1"));
            Assert.AreEqual("World", (string)spreadsheet.GetCellValue("B1"));
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor throws a SpreadsheetReadWriteException 
        /// when trying to load a file that does not exist.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSpreadsheetConstructor_InvalidSpreadsheetFile_Throws1()
        {
            var filename = "/missing/save.json"; // This file does not exist
            Spreadsheet spreadsheet = new Spreadsheet(filename); // Expect SpreadsheetReadWriteException
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor correctly loads and returns contents 
        /// and values from a spreadsheet with multiple cells.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_MultipleCells_ReturnsCorrectContents1()
        {
            var filename = "test_multiple_cells.json";
            string jsonData = @"{
        ""Cells"": {
            ""A1"": { ""StringForm"": ""Hello"" },
            ""B1"": { ""StringForm"": ""=2+2"" },
            ""C1"": { ""StringForm"": ""5.5"" }
        }
    }";
            File.WriteAllText(filename, jsonData); // Write multiple cells data to file

            Spreadsheet spreadsheet = new Spreadsheet(filename);

            // Validate the contents of the cells
            Assert.AreEqual("Hello", spreadsheet.GetCellContents("A1"));
            Assert.AreEqual(new Formula("2+2"), spreadsheet.GetCellContents("B1"));
            Assert.AreEqual(5.5, spreadsheet.GetCellContents("C1"));

            // Validate the values of the cells
            Assert.AreEqual("Hello", spreadsheet.GetCellValue("A1"));
            Assert.AreEqual(4.0, spreadsheet.GetCellValue("B1")); // Expecting 2+2 to evaluate to 4.0
        }

        /// <summary>
        /// Tests that the Spreadsheet constructor throws a SpreadsheetReadWriteException 
        /// when trying to load a file with invalid JSON format.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSpreadsheetConstructor_InvalidJson_Throws1()
        {
            var filename = "test_invalid_json.json";
            string invalidJson = @"{ ""Cells"": { ""A1"": { ""StringForm"": ""Hello"" "; // Missing closing brace
            File.WriteAllText(filename, invalidJson); // Write invalid JSON to file

            Spreadsheet spreadsheet = new Spreadsheet(filename); // Expect SpreadsheetReadWriteException
        }

        /// <summary>
        /// Tests saving and loading an empty spreadsheet. The test ensures that 
        /// when an empty spreadsheet is saved to a file and then loaded, 
        /// it remains empty.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_EmptySpreadsheet_ReturnsEmpty()
        {
            string filename = "test_empty_spreadsheet.json";
            Spreadsheet spreadsheet = new Spreadsheet(); // Create an empty spreadsheet

            // Save the spreadsheet to file
            spreadsheet.Save(filename);

            // Load the spreadsheet from the saved file
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);

            // Assert that the loaded spreadsheet is also empty
            Assert.AreEqual(0, loadedSpreadsheet.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// Tests saving and loading a spreadsheet with multiple cells.
        /// The test verifies that after saving the spreadsheet to a file and 
        /// loading it back, the correct values are preserved.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_MultipleCells_ReturnsCorrectValues()
        {
            string filename = "test_multiple_cells.json";
            Spreadsheet spreadsheet = new Spreadsheet();

            // Add some cells
            spreadsheet.SetContentsOfCell("A1", "Hello");
            spreadsheet.SetContentsOfCell("B1", "=2+3");
            spreadsheet.SetContentsOfCell("C1", "5.5");

            // Save the spreadsheet to file
            spreadsheet.Save(filename);

            // Load the spreadsheet from the saved file
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);

            // Assert that the loaded spreadsheet contains the correct values
            Assert.AreEqual("Hello", loadedSpreadsheet.GetCellValue("A1"));
            Assert.AreEqual(5.0, loadedSpreadsheet.GetCellValue("B1")); // Expect formula result
            Assert.AreEqual(5.5, loadedSpreadsheet.GetCellValue("C1"));
        }

        /// <summary>
        /// Tests saving and loading a spreadsheet with formulas and string contents. 
        /// The test ensures that the spreadsheet retains both formulas and string values
        /// when saved and loaded back.
        /// </summary>
        [TestMethod]
        public void TestSpreadsheetConstructor_FormulasAndStrings_ReturnsCorrectValues()
        {
            string filename = "test_formulas_and_strings.json";
            Spreadsheet spreadsheet = new Spreadsheet();

            // Add some formulas and strings
            spreadsheet.SetContentsOfCell("A1", "=B1 + B2");
            spreadsheet.SetContentsOfCell("B1", "2");
            spreadsheet.SetContentsOfCell("B2", "3");
            spreadsheet.SetContentsOfCell("C1", "Hello");

            // Save the spreadsheet to file
            spreadsheet.Save(filename);

            // Load the spreadsheet from the saved file
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);

            // Assert that the loaded spreadsheet contains the correct values
            Assert.AreEqual(5.0, loadedSpreadsheet.GetCellValue("A1")); // Formula result
            Assert.AreEqual("Hello", loadedSpreadsheet.GetCellValue("C1"));
        }

        /// <summary>
        /// Tests loading a spreadsheet with invalid or malformed JSON.
        /// The test ensures that a SpreadsheetReadWriteException is thrown 
        /// when the file contains invalid JSON.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSpreadsheetConstructor_InvalidJson_ThrowsException()
        {
            string filename = "test_invalid_json.json";
            // Write malformed JSON to file
            File.WriteAllText(filename, @"{ ""Cells"": { ""A1"": { ""StringForm"": ""Hello"" } "); // Missing closing braces

            // Try to load the spreadsheet with the malformed JSON
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename); // Should throw exception
        }

        // Tests for Save method

        /// <summary>
        /// Tests that saving a spreadsheet with valid data works as expected.
        /// The test saves the spreadsheet to a file and reloads it to verify the data integrity.
        /// </summary>
        [TestMethod]
        public void TestSaveWithValidData()
        {
            string filename = "test_spreadsheet.json";
            Spreadsheet s = new Spreadsheet();

            // Set contents of cells
            s.SetContentsOfCell("A1", "5");
            s.SetContentsOfCell("B3", "=A1+2");
            s.SetContentsOfCell("C4", "hello");

            // Save the spreadsheet to the specified file
            s.Save(filename);

            // Reload the spreadsheet and verify the contents of the cells
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);
            Assert.AreEqual(5.0, loadedSpreadsheet.GetCellContents("A1"));
            Assert.AreEqual(new Formula("A1+2"), loadedSpreadsheet.GetCellContents("B3"));
            Assert.AreEqual("hello", loadedSpreadsheet.GetCellContents("C4"));

            // Verify that the Changed property is false after saving
            Assert.IsFalse(s.Changed);
        }

        /// <summary>
        /// Tests saving a spreadsheet with multiple cells to ensure all data is correctly saved and reloaded.
        /// </summary>
        [TestMethod]
        public void TestSaveWithMultipleCells()
        {
            var filename = "test_spreadsheet_multiple_cells.json";
            Spreadsheet s = new Spreadsheet();

            // Set contents for multiple cells
            s.SetContentsOfCell("A1", "5");
            s.SetContentsOfCell("B3", "=A1+2");
            s.SetContentsOfCell("C4", "hello");

            // Save the spreadsheet
            s.Save(filename);

            // Reload the spreadsheet and verify the data in each cell
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);
            Assert.AreEqual(5.0, loadedSpreadsheet.GetCellContents("A1")); // Verify A1
            Assert.AreEqual(new Formula("A1+2"), loadedSpreadsheet.GetCellContents("B3")); // Verify B3
            Assert.AreEqual("hello", loadedSpreadsheet.GetCellContents("C4")); // Verify C4
            Assert.IsFalse(s.Changed); // Ensure Changed is false after save
        }

        /// <summary>
        /// Tests saving a spreadsheet with a single cell and verifies that the data is saved and reloaded correctly.
        /// </summary>
        [TestMethod]
        public void TestSaveWithSingleCell()
        {
            var filename = "test_spreadsheet_single_cell.json";
            Spreadsheet s = new Spreadsheet();

            // Set contents for a single cell
            s.SetContentsOfCell("A1", "Hello");

            // Save the spreadsheet
            s.Save(filename);

            // Reload the spreadsheet and verify the content of A1
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);
            Assert.AreEqual("Hello", loadedSpreadsheet.GetCellContents("A1")); // Verify A1
            Assert.IsFalse(s.Changed); // Ensure Changed is false after save
        }

        /// <summary>
        /// Tests saving an empty spreadsheet to ensure that no data is written and the spreadsheet reloads as empty.
        /// </summary>
        [TestMethod]
        public void TestSaveEmptySpreadsheet()
        {
            var filename = "test_spreadsheet_empty.json";
            Spreadsheet s = new Spreadsheet();

            // Save the empty spreadsheet
            s.Save(filename);

            // Reload the spreadsheet and verify that no cells are non-empty
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);
            Assert.AreEqual(0, loadedSpreadsheet.GetNamesOfAllNonemptyCells().Count); // Verify no non-empty cells
            Assert.IsFalse(s.Changed); // Ensure Changed is false after save
        }

        /// <summary>
        /// Tests that attempting to save a spreadsheet to an invalid filename throws a SpreadsheetReadWriteException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveWithInvalidFilename()
        {
            Spreadsheet s = new Spreadsheet();

            // Set content of a cell
            s.SetContentsOfCell("A1", "5");

            // Attempt to save to an invalid file path (e.g., missing directory)
            s.Save("/missing/save.json");
        }


        // Tests for SetContentsOfCell method

        /// <summary>
        /// Tests setting the contents of a single cell with a double value.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SingleCellWithDouble_StoresValueInA1()
        {
            Spreadsheet s = new Spreadsheet();

            // Set contents of various cells
            s.SetContentsOfCell("B1", "hello");
            s.SetContentsOfCell("C1", "5");

            // Set A1 with a double value and verify the return value (list of affected cells)
            var result = s.SetContentsOfCell("A1", "17.2");
            CollectionAssert.AreEquivalent(new List<string> { "A1" }, result.ToList());
        }

        /// <summary>
        /// Tests setting the contents of a single cell with a string value.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SingleCellWithString_StoresValueInB1()
        {
            Spreadsheet s = new Spreadsheet();

            // Set contents of various cells
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "5");

            // Set B1 with a string value and verify the return value (list of affected cells)
            var result = s.SetContentsOfCell("B1", "hello");
            CollectionAssert.AreEquivalent(new List<string> { "B1" }, result.ToList());
        }

        /// <summary>
        /// Tests setting the contents of a single cell with a formula.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SingleCellWithFormula_StoresValueInC1()
        {
            Spreadsheet s = new Spreadsheet();

            // Set contents of various cells
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("B1", "hello");

            // Set C1 with a formula and verify the return value (list of affected cells)
            var result = s.SetContentsOfCell("C1", "5");
            CollectionAssert.AreEquivalent(new List<string> { "C1" }, result.ToList());
        }

        /// <summary>
        /// Tests setting a chain of dependent cells and verifies the correct propagation of changes.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SettingChainOfCells_StoresAffectedCells()
        {
            Spreadsheet s = new Spreadsheet();

            // Set up a chain of dependencies between cells
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "6");
            s.SetContentsOfCell("A3", "=A2+A4");
            s.SetContentsOfCell("A4", "=A2+A5");

            // Set the value of A5 and verify the affected cells
            var result = s.SetContentsOfCell("A5", "82.5");
            CollectionAssert.AreEquivalent(new List<string> { "A5", "A4", "A3", "A1" }, result.ToList());
        }

        /// <summary>
        /// Tests changing the contents of a cell from a formula to a double and verifies the update.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_ChangingFormulaToDouble_UpdatesCellValue()
        {
            Spreadsheet s = new Spreadsheet();

            // Set A1 with a formula and then change it to a double value
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "2.5");

            // Verify the updated value
            Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
        }

        /// <summary>
        /// Tests changing the contents of a cell from a formula to a string and verifies the update.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_ChangingFormulaToString_UpdatesCellValue()
        {
            Spreadsheet s = new Spreadsheet();

            // Set A1 with a formula and then change it to a string value
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "Hello");

            // Verify the updated value
            Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests changing the contents of a cell from a string to a formula and verifies the update.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_ChangingStringToFormula_UpdatesCellValue()
        {
            Spreadsheet s = new Spreadsheet();

            // Set A1 with a string and then change it to a formula
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A1", "=23");

            // Verify the updated formula
            Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
            Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
        }

        /// <summary>
        /// Tests retrieving the names of all non-empty cells after setting contents and verifies correct behavior.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_GetNamesOfAllNonempty_StoresAndRetrievesNonemptyCells()
        {
            Spreadsheet s = new Spreadsheet();

            // Set contents of various cells
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "5.0");
            s.SetContentsOfCell("A4", "=A2+A3");

            // Verify the contents and values of the cells
            Assert.AreEqual(5.0, s.GetCellContents("A3")); // Verify A3 content
            Assert.AreEqual(new Formula("A2+A3"), s.GetCellContents("A4")); // Verify A4 formula
            Assert.AreEqual(5.0, s.GetCellValue("A3")); // Verify A3 value
            Assert.AreEqual(6.0, s.GetCellValue("A4")); // Verify A4 value

            // Verify the names of non-empty cells
            ISet<string> expectedNonEmptyCells = new HashSet<string> { "A1", "A2", "A3", "A4" };
            ISet<string> actualNonEmptyCells = new HashSet<string>(s.GetNamesOfAllNonemptyCells());
            CollectionAssert.AreEquivalent(expectedNonEmptyCells.ToList(), actualNonEmptyCells.ToList());
        }

        /// <summary>
        /// Tests setting and retrieving the contents of complex cells, 
        /// including formulas and dependencies. Verifies that changes 
        /// are propagated correctly through dependent cells.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SetCellContentsGetComplex_StoresAndRetrievesValuesCorrectly()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");

            // Set initial values and formulas
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1-1.0");
            s.SetContentsOfCell("A3", "2.0");
            s.SetContentsOfCell("A4", "=A2+A3");
            s.SetContentsOfCell("A3", "5.0"); // Modify A3 value
            s.SetContentsOfCell("B1", "7.5");
            s.SetContentsOfCell("B2", "=A4+B1");

            // Verify the contents and values of the cells
            Assert.AreEqual(5.0, s.GetCellContents("A3")); // Verify A3 content
            Assert.AreEqual(f, s.GetCellContents("A4"));   // Verify A4 formula
            Assert.AreEqual(5.0, s.GetCellValue("A3"));    // Verify A3 value
            Assert.AreEqual(6.0, s.GetCellValue("A4"));    // Verify A4 value
            Assert.AreEqual(13.5, s.GetCellValue("B2"));   // Verify B2 value

            // Modify A4's content and check dependent values
            s.SetContentsOfCell("A4", "=A2");
            Assert.AreEqual(8.5, s.GetCellValue("B2"));    // Verify updated B2 value after A4 change
        }

        /// <summary>
        /// Tests updating the values of dependent cells when 
        /// the original cell changes.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SetCellContentsGetComplex2_UpdatesValuesBasedOnDependencies()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");

            // Set initial values and formulas
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A2", "=A1"); // A2 depends on A1
            s.SetContentsOfCell("A1", "3.0"); // Modify A1's value

            // Verify that A2 reflects the change in A1
            Assert.AreEqual(3.0, s.GetCellValue("A2"));
        }

        /// <summary>
        /// Tests setting a cell's content as a string 
        /// and verifies the string is correctly stored.
        /// </summary>
        [TestMethod]
        public void TestSetContentsOfCell_SetCellContentsString_CorrectStringForm()
        {
            Spreadsheet s = new Spreadsheet();
            string cellName = "A1";
            string expectedStringContent = "Hello, World!";

            // Set a string in A1 and retrieve it using the indexer
            s.SetContentsOfCell(cellName, expectedStringContent);
            var actualValue = s[cellName];

            // Verify that the cell contains the correct string
            Assert.AreEqual(expectedStringContent, actualValue);
        }

        /// <summary>
        /// Tests updating a cell's value to a string after it initially held a numeric value or formula.
        /// </summary>
        [TestMethod()]
        public void TestSetContentsOfCell_SetCellContentsGetComplex3_UpdatesValueWhenChangingToString()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("A2+A3");

            // Set a formula in A1, then replace it with a string
            s.SetContentsOfCell("A1", "2.0");
            s.SetContentsOfCell("A1", "Jim");

            // Verify that A1 now holds the string value
            Assert.AreEqual("Jim", s.GetCellValue("A1"));
        }

        /// <summary>
        /// Tests getting the values of cells containing a string, 
        /// double, and formula. Also checks handling of division by zero.
        /// </summary>
        [TestMethod]
        public void TestGetCellValue_StringAndDouble_ReturnsCorrectValues()
        {
            Spreadsheet s = new Spreadsheet();

            // Set string, double, and formula in different cells
            s.SetContentsOfCell("A1", "String");
            s.SetContentsOfCell("A2", "1.0");
            s.SetContentsOfCell("A3", "=A2 + 2");

            // Verify the correct values
            Assert.AreEqual("String", s.GetCellValue("A1"));
            Assert.AreEqual(1.0, s.GetCellValue("A2"));
            Assert.AreEqual(3.0, s.GetCellValue("A3"));

            // Check for division by zero and a resulting FormulaError
            s.SetContentsOfCell("A3", "=3/0");
            Assert.AreEqual(typeof(FormulaError), s.GetCellValue("A3").GetType());
        }

        /// <summary>
        /// Tests getting the value of an existing cell with a valid name.
        /// </summary>
        [TestMethod]
        public void TestGetCellValue_ValidNameAndCellExists_ReturnsCorrectValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Test");

            object value = s.GetCellValue("A1");

            // Ensure the value returned is correct
            Assert.AreEqual("Test", value);
        }

        /// <summary>
        /// Tests getting the value of a cell with an invalid name (empty string),
        /// which should throw an InvalidNameException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetCellValue_InvalidName_ThrowsInvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellValue(""); // Empty name should trigger the exception
        }

        /// <summary>
        /// Tests getting the value of a valid name, but the cell does not exist.
        /// This should return an empty string
        /// </summary>
        [TestMethod]
        public void TestGetCellValue_ValidNameButNonExistentCell_ReturnsEmptyString()
        {
            Spreadsheet s = new Spreadsheet();

            var value = s.GetCellValue("B2");
            Assert.AreEqual("", value);
        }

        /// <summary>
        /// Tests getting the value of a cell that contains a formula and ensures
        /// the formula result is returned correctly.
        /// </summary>
        [TestMethod]
        public void TestGetCellValue_ValidNameWithFormula_ReturnsFormulaResult()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("C3", "=2+3");

            object value = s.GetCellValue("C3");

            // Ensure the formula result is returned correctly
            Assert.AreEqual(5.0, value);
        }

        /// <summary>
        /// Tests that references to a cell are correctly updated when the 
        /// referenced cell's value changes.
        /// </summary>
        [TestMethod]
        public void TestGetCellValue_ReferencesUpdate_WhenValueChanges()
        {
            Spreadsheet spreadsheet = new Spreadsheet();

            // Set initial value for A1
            spreadsheet.SetContentsOfCell("A1", "5.0"); // A1 is a double

            // Set B3 and C1 to reference A1
            spreadsheet.SetContentsOfCell("B3", "=A1+1"); // B3 references A1
            spreadsheet.SetContentsOfCell("C1", "=A1+2"); // C1 references A1

            // Change the value of A1
            spreadsheet.SetContentsOfCell("A1", "10.0");

            // Verify that references to A1 are updated
            Assert.AreEqual(11.0, spreadsheet.GetCellValue("B3")); // B3 reflects new A1 value
            Assert.AreEqual(12.0, spreadsheet.GetCellValue("C1")); // C1 reflects new A1 value
        }

        /// <summary>
        /// Tests getting the value of a cell containing a formula that results in a negative value.
        /// </summary>
        [TestMethod]
        public void TestGetCellValue_FormulaResultsInNegativeValue_ReturnsCorrectValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("A2", "=A1 - 20");

            // Ensure the formula result is returned correctly
            Assert.AreEqual(-10.0, s.GetCellValue("A2"));
        }

        /// <summary>
        /// Tests that attempting to get the value of a non-existent cell 
        /// returns an empty string.
        /// </summary>
        [TestMethod]
        public void TestCellLookup_NonExistentCellReturnsEmptyString()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            var value = spreadsheet.GetCellValue("A1");
            Assert.AreEqual("",value);
        }

        // Tests for Changed property

        /// <summary>
        /// Test that the Changed property is false when a new spreadsheet is created.
        /// A new spreadsheet should not be marked as changed.
        /// </summary>
        [TestMethod]
        public void TestChangedProperty_NewSpreadsheet_ReturnsFalse()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            Assert.IsFalse(spreadsheet.Changed, "New spreadsheet should not be marked as changed.");
        }

        /// <summary>
        /// Test that the Changed property is true after setting the content of a cell.
        /// Modifying the spreadsheet should mark it as changed.
        /// </summary>
        [TestMethod]
        public void TestChangedProperty_SetCellContents_ReturnsTrue()
        {
            Spreadsheet spreadsheet = new Spreadsheet();

            // Set a cell's content and expect Changed to be true
            spreadsheet.SetContentsOfCell("A1", "Hello");
            Assert.IsTrue(spreadsheet.Changed, "Spreadsheet should be marked as changed after setting cell contents.");
        }

        /// <summary>
        /// Test that the Changed property is false after saving the spreadsheet.
        /// Once the spreadsheet is saved, it should no longer be marked as changed.
        /// </summary>
        [TestMethod]
        public void TestChangedProperty_AfterSave_ReturnsFalse()
        {
            Spreadsheet spreadsheet = new Spreadsheet();

            // Set cell content to trigger change
            spreadsheet.SetContentsOfCell("A1", "Hello");

            // Save the spreadsheet and expect Changed to be reset to false
            string filename = "test_save_spreadsheet.json";
            spreadsheet.Save(filename);

            Assert.IsFalse(spreadsheet.Changed, "Spreadsheet should not be marked as changed after saving.");
        }

        /// <summary>
        /// Test that the Changed property is false after loading a saved spreadsheet.
        /// Loading a previously saved spreadsheet should not mark it as changed.
        /// </summary>
        [TestMethod]
        public void TestChangedProperty_AfterLoad_ReturnsFalse()
        {
            string filename = "test_load_spreadsheet.json";

            // Create a spreadsheet, set a cell value, and save it
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "Hello");
            spreadsheet.Save(filename);

            // Load the saved spreadsheet and expect Changed to be false
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);

            Assert.IsFalse(loadedSpreadsheet.Changed, "Spreadsheet should not be marked as changed after loading.");
        }

        /// <summary>
        /// Test that the Changed property is true after loading a saved spreadsheet and making modifications.
        /// Modifying a loaded spreadsheet should mark it as changed.
        /// </summary>
        [TestMethod]
        public void TestChangedProperty_AfterLoadAndModify_ReturnsTrue()
        {
            string filename = "test_load_and_modify_spreadsheet.json";

            // Create a spreadsheet, set a cell value, and save it
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "Hello");
            spreadsheet.Save(filename);

            // Load the saved spreadsheet
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);

            // Modify the loaded spreadsheet and expect Changed to be true
            loadedSpreadsheet.SetContentsOfCell("B1", "World");
            Assert.IsTrue(loadedSpreadsheet.Changed, "Spreadsheet should be marked as changed after modifying a loaded spreadsheet.");
        }

        /// <summary>
        /// Test that the Changed property remains false after loading a saved spreadsheet without making any modifications.
        /// Simply loading a spreadsheet should not mark it as changed if no modifications are made.
        /// </summary>
        [TestMethod]
        public void TestChangedProperty_AfterLoadWithoutModifications_ReturnsFalse()
        {
            string filename = "test_load_without_modifications.json";

            // Create a spreadsheet, set a cell value, and save it
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "Hello");
            spreadsheet.Save(filename);

            // Load the saved spreadsheet and do nothing
            Spreadsheet loadedSpreadsheet = new Spreadsheet(filename);

            // Assert that Changed is still false
            Assert.IsFalse(loadedSpreadsheet.Changed, "Spreadsheet should not be marked as changed if no modifications are made after loading.");
        }

        // STRESS TESTS

        /// <summary>
        /// Tests a complex chain of formulas where multiple cells depend on each other.
        /// Sets values starting from E1 and propagates changes through dependencies. 
        /// Verifies that all dependent cells are correctly updated when the value of E1 is set.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        public void TestStress1()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=B1+B2");
            s.SetContentsOfCell("B1", "=C1-C2");
            s.SetContentsOfCell("B2", "=C3*C4");
            s.SetContentsOfCell("C1", "=D1*D2");
            s.SetContentsOfCell("C2", "=D3*D4");
            s.SetContentsOfCell("C3", "=D5*D6");
            s.SetContentsOfCell("C4", "=D7*D8");
            s.SetContentsOfCell("D1", "=E1");
            s.SetContentsOfCell("D2", "=E1");
            s.SetContentsOfCell("D3", "=E1");
            s.SetContentsOfCell("D4", "=E1");
            s.SetContentsOfCell("D5", "=E1");
            s.SetContentsOfCell("D6", "=E1");
            s.SetContentsOfCell("D7", "=E1");
            s.SetContentsOfCell("D8", "=E1");
            IList<String> cells = s.SetContentsOfCell("E1", "0");

            // Verify that all cells dependent on E1 are updated
            Assert.IsTrue(new HashSet<string>() { "A1", "B1", "B2", "C1", "C2", "C3", "C4", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "E1" }.SetEquals(cells));
        }

        /// <summary>
        /// Tests adding 200 cells where each cell depends on the next one in a chain (e.g., A1 = A2, A2 = A3, etc.).
        /// Verifies that all cells in the chain are correctly updated when a new cell is set.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        public void TestStress2()
        {
            Spreadsheet s = new Spreadsheet();
            ISet<String> cells = new HashSet<string>();

            for (int i = 1; i < 200; i++)
            {
                cells.Add("A" + i);
                Assert.IsTrue(cells.SetEquals(s.SetContentsOfCell("A" + i, "=A" + (i + 1))));
            }
        }

        /// <summary>
        /// Tests for circular dependency detection. A circular dependency is introduced by setting 
        /// A150 to depend on A50 in a chain that references earlier cells. The test ensures 
        /// that a CircularException is thrown.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        public void TestStress3()
        {
            Spreadsheet s = new Spreadsheet();

            for (int i = 1; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i + 1));
            }

            // Attempt to create a circular dependency and verify that it throws a CircularException
            try
            {
                s.SetContentsOfCell("A150", "=A50");
                Assert.Fail();
            }
            catch (CircularException)
            {
            }
        }

        /// <summary>
        /// Tests a spreadsheet with multiple interdependent cells, including formulas and constants.
        /// Validates that the correct values are computed for cells based on their dependencies.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        public void TestStress5()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2");
            s.SetContentsOfCell("A2", "3");
            s.SetContentsOfCell("A3", "=A1 + A2");
            s.SetContentsOfCell("B1", "=A3 * 2");
            s.SetContentsOfCell("B2", "=A1 - A2");
            s.SetContentsOfCell("B3", "=B1 + B2"); // B3 depends on B1 and B2
            s.SetContentsOfCell("C1", "=B3 / A1");
            s.SetContentsOfCell("C2", "=C1 * 10"); // C2 depends on C1

            // Verify the final computed values
            Assert.AreEqual(5.0, s.GetCellValue("A3"));
            Assert.AreEqual(10.0, s.GetCellValue("B1"));
            Assert.AreEqual(9.0, s.GetCellValue("B3"));
            Assert.AreEqual(4.5, s.GetCellValue("C1"));
            Assert.AreEqual(45.0, s.GetCellValue("C2"));
        }

        /// <summary>
        /// Simulates rapid updates to a set of 100 cells, each cell being updated with a new value multiple times.
        /// Verifies that the final values are correctly computed after multiple updates.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        public void TestStress6()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> cells = new List<string>();

            for (int i = 1; i <= 100; i++)
            {
                cells.Add("A" + i);
                s.SetContentsOfCell("A" + i, i.ToString());
            }

            // Simulate rapid updates to the same cells
            for (int j = 1; j <= 100; j++)
            {
                for (int i = 1; i <= 100; i++)
                {
                    s.SetContentsOfCell("A" + i, (i + j).ToString());
                }
            }

            // Verify the final values
            for (double i = 1; i <= 100; i++)
            {
                Assert.AreEqual(100 + i, s.GetCellValue("A" + i)); // Since we incremented each cell by 100
            }
        }

        /// <summary>
        /// Tests the performance and correctness of setting and retrieving values 
        /// from a large number of cells (10,000 in this case). Verifies that 
        /// all cells are correctly set and retrieved.
        /// </summary>
        [TestMethod()]
        [Timeout(2000)]
        public void TestStress7()
        {
            Spreadsheet s = new Spreadsheet();
            IList<string> cells = new List<string>();

            for (int i = 1; i <= 10000; i++) // Assuming a maximum cell limit of 10,000
            {
                string cellName = "A" + i;
                s.SetContentsOfCell(cellName, "1"); // Set all cells to a constant value
                cells.Add(cellName);
            }

            // Verify that the last cell is correctly set
            Assert.AreEqual(1.0, s.GetCellValue("A10000"));
        }

        /// <summary>
        /// Tests that the Save method writes the contents of the spreadsheet to the specified file
        /// in JSON format, and overwrites the file if it already exists.
        /// </summary>
        [TestMethod]
        public void StressTest8()
        {
            // Define a filename for testing
            string filename = "test_overwrite_spreadsheet.json";

            // Create an initial spreadsheet and set some cell values
            Spreadsheet spreadsheet1 = new Spreadsheet();
            spreadsheet1.SetContentsOfCell("A1", "InitialValue");
            spreadsheet1.SetContentsOfCell("B1", "5");

            // Save the initial spreadsheet to the file
            spreadsheet1.Save(filename);

            // Assert that the file contains the initial values
            Spreadsheet loadedSpreadsheet1 = new Spreadsheet(filename);
            Assert.AreEqual("InitialValue", loadedSpreadsheet1.GetCellValue("A1"));
            Assert.AreEqual(5.0, loadedSpreadsheet1.GetCellValue("B1"));

            // Create a new spreadsheet with different contents
            Spreadsheet spreadsheet2 = new Spreadsheet();
            spreadsheet2.SetContentsOfCell("A1", "NewValue");
            spreadsheet2.SetContentsOfCell("C1", "=2+3");

            // Save the new spreadsheet, which should overwrite the existing file
            spreadsheet2.Save(filename);

            // Assert that the file now contains the new contents (overwritten)
            Spreadsheet loadedSpreadsheet2 = new Spreadsheet(filename);
            Assert.AreEqual("NewValue", loadedSpreadsheet2.GetCellValue("A1"));
            Assert.AreEqual(5.0, loadedSpreadsheet2.GetCellValue("C1")); // Formula result
            Assert.AreEqual("", loadedSpreadsheet2.GetCellValue("B1")); // The previous value in "B1" should be gone
        }


        // PS6 grading unit tests
        /// <summary>
        /// Helper method to verify an arbitrary spreadsheet's values
        /// Cell names and eexpeced values are given in an array in alternating pairs
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="constraints"></param>
        public void VerifyValues(Spreadsheet sheet, params object[] constraints)
        {
            for (int i = 0; i < constraints.Length; i += 2)
            {
                if (constraints[i + 1] is double)
                {
                    Assert.AreEqual((double)constraints[i + 1], (double)sheet.GetCellValue((string)constraints[i]), 1e-9);
                }
                else
                {
                    Assert.AreEqual(constraints[i + 1], sheet.GetCellValue((string)constraints[i]));
                }
            }
        }


        /// <summary>
        /// Helper method to set the contents of a given cell for a given spreadsheet
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="name"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public IEnumerable<string> Set(Spreadsheet sheet, string name, string contents)
        {
            List<string> result = new List<string>(sheet.SetContentsOfCell(name, contents));
            return result;
        }

        // Tests IsValid
        [TestMethod, Timeout(2000)]
        [TestCategory("1")]
        public void SetContentsOfCell_SetString_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "x");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("2")]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCell_InvalidName_Throws()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("1a", "x");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("3")]
        public void SetContentsOfCell_SetFormula_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "= A1 + C1");
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("4")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SetContentsOfCell_SetInvalidFormula_Throws() // try construct an invalid formula
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("B1", "= A1 + 1C");
        }

        // Tests Normalize
        [TestMethod, Timeout(2000)]
        [TestCategory("5")]
        public void GetCellContents_LowerCaseName_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", s.GetCellContents("b1"));
        }

        /// <summary>
        /// Increase the weight by repeating the previous test
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("6")]
        public void GetCellContents_LowerCaseName_IsValid2()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("B1", "hello");
            Assert.AreEqual("hello", ss.GetCellContents("b1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("7")]
        public void GetCellValue_CaseSensitivity_IsValid()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("B1", "= A1");
            Assert.AreEqual(5.0, (double)s.GetCellValue("B1"), 1e-9);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("8")]
        public void GetCellValue_CaseSensitivity_IsValid2()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.SetContentsOfCell("A1", "5");
            ss.SetContentsOfCell("B1", "= a1");
            Assert.AreEqual(5.0, (double)ss.GetCellValue("B1"), 1e-9);
        }

        // Simple tests
        [TestMethod, Timeout(2000)]
        [TestCategory("9")]
        public void Constructor_Empty_CorrectValue()
        {
            Spreadsheet ss = new Spreadsheet();
            VerifyValues(ss, "A1", "");
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("10")]
        public void GetCellValue_GetString_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            OneString(ss);
        }

        /// <summary>
        /// Helper method that sets one string in one cell and verifies the value
        /// </summary>
        /// <param name="ss"></param>
        public void OneString(Spreadsheet ss)
        {
            Set(ss, "B1", "hello");
            VerifyValues(ss, "B1", "hello");
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("11")]
        public void GetCellValue_GetNumber_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            OneNumber(ss);
        }

        /// <summary>
        /// Helper method that sets one number in one cell and verifies the value
        /// </summary>
        /// <param name="ss"></param>
        public void OneNumber(Spreadsheet ss)
        {
            Set(ss, "C1", "17.5");
            VerifyValues(ss, "C1", 17.5);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("12")]
        public void GetCellValue_GetFormula_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            OneFormula(ss);
        }

        /// <summary>
        /// Helper method that sets one formula in one cell and verifies the value
        /// </summary>
        /// <param name="ss"></param>
        public void OneFormula(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "5.2");
            Set(ss, "C1", "= A1+B1");
            VerifyValues(ss, "A1", 4.1, "B1", 5.2, "C1", 9.3);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("13")]
        public void Changed_AfterModify_IsTrue()
        {
            Spreadsheet ss = new Spreadsheet();
            Assert.IsFalse(ss.Changed);
            Set(ss, "C1", "17.5");
            Assert.IsTrue(ss.Changed);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("13b")]
        public void Changed_AfterSave_IsFalse()
        {
            Spreadsheet ss = new Spreadsheet();
            Set(ss, "C1", "17.5");
            ss.Save("changed.txt");
            Assert.IsFalse(ss.Changed);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("14")]
        public void GetCellValue_DivideByZero_ReturnsError()
        {
            Spreadsheet ss = new Spreadsheet();
            DivisionByZero1(ss);
        }

        /// <summary>
        /// Helper method to test a formula that indirectly divides by zero
        /// </summary>
        /// <param name="ss"></param>
        public void DivisionByZero1(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "0.0");
            Set(ss, "C1", "= A1 / B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("15")]
        public void GetCellValue_DivideByZero_ReturnsError2()
        {
            Spreadsheet ss = new Spreadsheet();
            DivisionByZero2(ss);
        }

        /// <summary>
        /// Helper method that directly divides by zero
        /// </summary>
        /// <param name="ss"></param>
        public void DivisionByZero2(Spreadsheet ss)
        {
            Set(ss, "A1", "5.0");
            Set(ss, "A3", "= A1 / 0.0");
            Assert.IsInstanceOfType(ss.GetCellValue("A3"), typeof(FormulaError));
        }



        [TestMethod, Timeout(2000)]
        [TestCategory("16")]
        public void GetCellValue_FormulaBadVariable_ReturnsError()
        {
            Spreadsheet ss = new Spreadsheet();
            EmptyArgument(ss);
        }

        /// <summary>
        /// Helper method that tests a formula that references an empty cell
        /// </summary>
        /// <param name="ss"></param>
        public void EmptyArgument(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("17")]
        public void GetCellValue_FormulaBadVariable_ReturnsError2()
        {
            Spreadsheet ss = new Spreadsheet();
            StringArgument(ss);
        }

        /// <summary>
        /// Helper method that tests a formula that references a non-empty string cell
        /// </summary>
        /// <param name="ss"></param>
        public void StringArgument(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "hello");
            Set(ss, "C1", "= A1 + B1");
            Assert.IsInstanceOfType(ss.GetCellValue("C1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("18")]
        public void GetCellValue_FormulaIndirectBadVariable_ReturnsError()
        {
            Spreadsheet ss = new Spreadsheet();
            ErrorArgument(ss);
        }

        /// <summary>
        /// Helper method that creates a formula that indirectly references an empty cell
        /// </summary>
        /// <param name="ss"></param>
        public void ErrorArgument(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "B1", "");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= C1");
            Assert.IsInstanceOfType(ss.GetCellValue("D1"), typeof(FormulaError));
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("19")]
        public void GetCellValue_FormulaWithVariable_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            NumberFormula1(ss);
        }

        /// <summary>
        /// Helper method that creates a simple formula with a variable reference
        /// </summary>
        /// <param name="ss"></param>
        public void NumberFormula1(Spreadsheet ss)
        {
            Set(ss, "A1", "4.1");
            Set(ss, "C1", "= A1 + 4.2");
            VerifyValues(ss, "C1", 8.3);
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("20")]
        public void GetCellValue_FormulaWithNumber_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            NumberFormula2(ss);
        }

        /// <summary>
        /// Helper method that creates a simple formula that's just a number
        /// </summary>
        /// <param name="ss"></param>
        public void NumberFormula2(Spreadsheet ss)
        {
            Set(ss, "A1", "= 4.6");
            VerifyValues(ss, "A1", 4.6);
        }


        // Repeats the simple tests all together
        [TestMethod, Timeout(2000)]
        [TestCategory("21")]
        public void StressTestVariety()
        {
            Spreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "17.32");
            Set(ss, "B1", "This is a test");
            Set(ss, "C1", "= A1+B1");
            OneString(ss);
            OneNumber(ss);
            OneFormula(ss);
            DivisionByZero1(ss);
            DivisionByZero2(ss);
            StringArgument(ss);
            ErrorArgument(ss);
            NumberFormula1(ss);
            NumberFormula2(ss);
        }

        // Four kinds of formulas
        [TestMethod, Timeout(2000)]
        [TestCategory("22")]
        public void StressTestFormulas()
        {
            Spreadsheet ss = new Spreadsheet();
            Formulas(ss);
        }

        public void Formulas(Spreadsheet ss)
        {
            Set(ss, "A1", "4.4");
            Set(ss, "B1", "2.2");
            Set(ss, "C1", "= A1 + B1");
            Set(ss, "D1", "= A1 - B1");
            Set(ss, "E1", "= A1 * B1");
            Set(ss, "F1", "= A1 / B1");
            VerifyValues(ss, "C1", 6.6, "D1", 2.2, "E1", 4.4 * 2.2, "F1", 2.0);
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("23")]
        public void StressTestFormulas2()
        {
            StressTestFormulas();
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("24")]
        public void StressTestFormulas3()
        {
            StressTestFormulas();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("25")]
        public void Constructor_MultipleSpreadsheets_DontIntefere()
        {
            Spreadsheet s1 = new Spreadsheet();
            Spreadsheet s2 = new Spreadsheet();
            Set(s1, "X1", "hello");
            Set(s2, "X1", "goodbye");
            VerifyValues(s1, "X1", "hello");
            VerifyValues(s2, "X1", "goodbye");
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("26")]
        public void Constructor_MultipleSpreadsheets_DontIntefere2()
        {
            Constructor_MultipleSpreadsheets_DontIntefere();
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("27")]
        public void Constructor_MultipleSpreadsheets_DontIntefere3()
        {
            Constructor_MultipleSpreadsheets_DontIntefere();
        }

        /// <summary>
        /// Repeated for increased weight
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("28")]
        public void Constructor_MultipleSpreadsheets_DontIntefere4()
        {
            Constructor_MultipleSpreadsheets_DontIntefere();
        }

        // Reading/writing spreadsheets
        [TestMethod, Timeout(2000)]
        [TestCategory("29")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Save_InvalidPath_Throws()
        {
            Spreadsheet ss = new Spreadsheet();
            ss.Save(Path.GetFullPath("/missing/save.txt"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("30")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Load_InvalidPath_Throws()
        {
            Spreadsheet ss = new Spreadsheet(Path.GetFullPath("/missing/save.txt"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("31")]
        public void SaveLoad_SimpleSheet_IsValid()
        {
            Spreadsheet s1 = new Spreadsheet();
            Set(s1, "A1", "hello");
            s1.Save("save1.txt");
            s1 = new Spreadsheet("save1.txt");
            Assert.AreEqual("hello", s1.GetCellContents("A1"));
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("32")]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void Load_InvalidJson_Throws()
        {
            using (StreamWriter writer = new StreamWriter("save2.txt"))
            {
                writer.WriteLine("This");
                writer.WriteLine("is");
                writer.WriteLine("a");
                writer.WriteLine("test!");
            }
            Spreadsheet ss = new Spreadsheet("save2.txt");
        }



        [TestMethod, Timeout(2000)]
        [TestCategory("35")]
        public void Load_FromManualJson_IsValid()
        {
            var sheet = new
            {
                Cells = new
                {
                    A1 = new { StringForm = "hello" },
                    A2 = new { StringForm = "5.0" },
                    A3 = new { StringForm = "4.0" },
                    A4 = new { StringForm = "= A2 + A3" }
                },
            };

            File.WriteAllText("save5.txt", JsonSerializer.Serialize(sheet));


            Spreadsheet ss = new Spreadsheet("save5.txt");
            VerifyValues(ss, "A1", "hello", "A2", 5.0, "A3", 4.0, "A4", 9.0);
        }

        /// <summary>
        /// This test saves your spreadsheet and then loads it into 
        /// a general (dynamic) object, not using your spreadsheet's load constructor
        /// </summary>
        [TestMethod, Timeout(2000)]
        [TestCategory("36")]
        public void Save_ToGeneralObject_IsValid()
        {
            Spreadsheet ss = new Spreadsheet();
            Set(ss, "A1", "hello");
            Set(ss, "A2", "5.0");
            Set(ss, "A3", "4.0");
            Set(ss, "A4", "= A2 + A3");
            ss.Save("save6.txt");

            string fileContents = File.ReadAllText("save6.txt");

            dynamic? o = JObject.Parse(fileContents);

            Assert.IsNotNull(o);

            Assert.AreEqual("hello", o?.Cells.A1.StringForm.ToString());
            Assert.AreEqual(5.0, double.Parse(o?.Cells.A2.StringForm.ToString()), 1e-9);
            Assert.AreEqual(4.0, double.Parse(o?.Cells.A3.StringForm.ToString()), 1e-9);
            Assert.AreEqual("=A2+A3", o?.Cells.A4.StringForm.ToString().Replace(" ", ""));
        }


        // Fun with formulas
        [TestMethod, Timeout(2000)]
        [TestCategory("37")]
        public void FormulaStress1()
        {
            Formula1(new Spreadsheet());
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void Formula1(Spreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= b1 + b2");
            Assert.IsInstanceOfType(ss.GetCellValue("a1"), typeof(FormulaError));
            Assert.IsInstanceOfType(ss.GetCellValue("a2"), typeof(FormulaError));
            Set(ss, "a3", "5.0");
            Set(ss, "b1", "2.0");
            Set(ss, "b2", "3.0");
            VerifyValues(ss, "a1", 10.0, "a2", 5.0);
            Set(ss, "b2", "4.0");
            VerifyValues(ss, "a1", 11.0, "a2", 6.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("38")]
        public void FormulaStress2()
        {
            Formula2(new Spreadsheet());
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void Formula2(Spreadsheet ss)
        {
            Set(ss, "a1", "= a2 + a3");
            Set(ss, "a2", "= a3");
            Set(ss, "a3", "6.0");
            VerifyValues(ss, "a1", 12.0, "a2", 6.0, "a3", 6.0);
            Set(ss, "a3", "5.0");
            VerifyValues(ss, "a1", 10.0, "a2", 5.0, "a3", 5.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("39")]
        public void FormulaStress3()
        {
            Formula3(new Spreadsheet());
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void Formula3(Spreadsheet ss)
        {
            Set(ss, "a1", "= a3 + a5");
            Set(ss, "a2", "= a5 + a4");
            Set(ss, "a3", "= a5");
            Set(ss, "a4", "= a5");
            Set(ss, "a5", "9.0");
            VerifyValues(ss, "a1", 18.0);
            VerifyValues(ss, "a2", 18.0);
            Set(ss, "a5", "8.0");
            VerifyValues(ss, "a1", 16.0);
            VerifyValues(ss, "a2", 16.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("40")]
        public void FormulaStress4()
        {
            Spreadsheet ss = new Spreadsheet();
            Formula1(ss);
            Formula2(ss);
            Formula3(ss);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("41")]
        public void FormulaStress5()
        {
            FormulaStress4();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("42")]
        public void MediumStress()
        {
            Spreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
        }

        /// <summary>
        /// Helper method for formula stress tests
        /// </summary>
        /// <param name="ss"></param>
        public void MediumSheet(Spreadsheet ss)
        {
            Set(ss, "A1", "1.0");
            Set(ss, "A2", "2.0");
            Set(ss, "A3", "3.0");
            Set(ss, "A4", "4.0");
            Set(ss, "B1", "= A1 + A2");
            Set(ss, "B2", "= A3 * A4");
            Set(ss, "C1", "= B1 + B2");
            VerifyValues(ss, "A1", 1.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 3.0, "B2", 12.0, "C1", 15.0);
            Set(ss, "A1", "2.0");
            VerifyValues(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 4.0, "B2", 12.0, "C1", 16.0);
            Set(ss, "B1", "= A1 / A2");
            VerifyValues(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("43")]
        public void MediumStress2()
        {
            MediumStress();
        }


        [TestMethod, Timeout(2000)]
        [TestCategory("44")]
        public void MediumStressSave()
        {
            Spreadsheet ss = new Spreadsheet();
            MediumSheet(ss);
            ss.Save("save7.txt");
            ss = new Spreadsheet("save7.txt");
            VerifyValues(ss, "A1", 2.0, "A2", 2.0, "A3", 3.0, "A4", 4.0, "B1", 1.0, "B2", 12.0, "C1", 13.0);
        }

        [TestMethod, Timeout(2000)]
        [TestCategory("45")]
        public void MediumStressSave2()
        {
            MediumStressSave();
        }


        // A long chained formula. Solutions that re-evaluate 
        // cells on every request, rather than after a cell changes,
        // will timeout on this test.
        // This test is repeated to increase its scoring weight
        [TestMethod, Timeout(6000)]
        [TestCategory("46")]
        public void StressLongFormulaChain()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("47")]
        public void StressLongFormulaChain2()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("48")]
        public void StressLongFormulaChain3()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("49")]
        public void StressLongFormulaChain4()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        [TestMethod, Timeout(6000)]
        [TestCategory("50")]
        public void StressLongFormulaChain5()
        {
            object result = "";
            LongFormulaHelper(out result);
            Assert.AreEqual("ok", result);
        }

        /// <summary>
        /// Helper method for long formula stress tests
        /// </summary>
        /// <param name="result"></param>
        public void LongFormulaHelper(out object result)
        {
            try
            {
                Spreadsheet s = new Spreadsheet();
                s.SetContentsOfCell("sum1", "= a1 + a2");
                int i;
                int depth = 100;
                for (i = 1; i <= depth * 2; i += 2)
                {
                    s.SetContentsOfCell("a" + i, "= a" + (i + 2) + " + a" + (i + 3));
                    s.SetContentsOfCell("a" + (i + 1), "= a" + (i + 2) + "+ a" + (i + 3));
                }
                s.SetContentsOfCell("a" + i, "1");
                s.SetContentsOfCell("a" + (i + 1), "1");
                Assert.AreEqual(Math.Pow(2, depth + 1), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + i, "0");
                Assert.AreEqual(Math.Pow(2, depth), (double)s.GetCellValue("sum1"), 1.0);
                s.SetContentsOfCell("a" + (i + 1), "0");
                Assert.AreEqual(0.0, (double)s.GetCellValue("sum1"), 0.1);
                result = "ok";
            }
            catch (Exception e)
            {
                result = e;
            }
        }


    }
}