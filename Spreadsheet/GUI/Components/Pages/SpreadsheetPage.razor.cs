// <copyright file="SpreadsheetPage.razor.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <author> Prachi Aswani</author>
// <version> 25 October, 2024</version>

namespace GUI.Client.Pages;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics;
using CS3500.Spreadsheet;
using CS3500.Formula;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;

/// <summary>
/// The partial class for handling Spreadsheet Page interactions.
/// </summary>
public partial class SpreadsheetPage
{
	/// <summary>
	/// Based on your computer, you could shrink/grow this value based on performance.
	/// </summary>
	private const int ROWS = 50;

	/// <summary>
	/// Number of columns, which will be labeled A-Z.
	/// </summary>
	private const int COLS = 26;

	/// <summary>
	/// Provides an easy way to convert from an index to a letter (0 -> A)
	/// </summary>
	private char[] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();


	/// <summary>
	///   Gets or sets the name of the file to be saved
	/// </summary>
	private string FileSaveName { get; set; } = "Spreadsheet.sprd";


	/// <summary>
	///   <para> Gets or sets the data for all of the cells in the spreadsheet GUI. </para>
	///   <remarks>Backing Store for HTML</remarks>
	/// </summary>
	private string[,] CellsBackingStore { get; set; } = new string[ROWS, COLS];

	/// <summary>
	/// Gets or sets the name of the currently selected cell in the spreadsheet.
	/// The cell name is in the format "A1", "B2", etc.
	/// </summary>
	private string SelectedCellName { get; set; } = "A1";

	/// <summary>
	/// Gets or sets the contents of the currently selected cell.
	/// The contents can be a string, number, or formula (e.g., "=A1+B2").
	/// </summary>
	private string SelectedCellContents { get; set; } = "";

	/// <summary>
	/// Gets or sets the value of the currently selected cell.
	/// The value is computed from the contents of the cell and can be a string, number, or error message.
	/// </summary>
	private string SelectedCellValue { get; set; } = "";

	/// <summary>
	/// The underlying spreadsheet data model that handles cell content and formula evaluation.
	/// </summary>
	private Spreadsheet _spreadsheet = new Spreadsheet();

	/// <summary>
	/// Reference to the input field where the user can edit the selected cell's contents.
	/// </summary>
	private ElementReference cellInput;

	/// <summary>
	/// Indicates whether the page is in a loading state, such as when reading or writing a file.
	/// </summary>
	private bool isLoading = false;

	/// <summary>
	/// Indicates whether an error modal is currently displayed.
	/// </summary>
	private bool showErrorModal = false;

	/// <summary>
	/// Holds the error message to be displayed in the error modal when an error occurs.
	/// </summary>
	private string ErrorMessage = string.Empty;

	/// <summary>
	/// 
	/// </summary>
	private bool isDarkMode = false;

	/// <summary>
	/// Handler for when a cell is clicked
	/// </summary>
	/// <param name="row">The row component of the cell's coordinates</param>
	/// <param name="col">The column component of the cell's coordinates</param>
	private void CellClicked(int row, int col)
	{
		// Convert column index to a letter (0 -> A, 1 -> B, etc.)
		string columnLetter = Alphabet[col].ToString();
		string cellName = $"{columnLetter}{row + 1}";

		// Update selected cell name and its displayed contents.
		SelectedCellName = cellName;
		string contentDisplayed = _spreadsheet.GetCellContents(cellName)!.ToString() ?? "";
		if (_spreadsheet.GetCellContents(cellName) is Formula)
			contentDisplayed = "=" + contentDisplayed;
		SelectedCellContents = contentDisplayed; // Load the content of the selected cell

		// Set focus on the input field and update the value displayed.
		cellInput.FocusAsync();

		// Update the value displayed based on the contents/formula
		UpdateSelectedCellValue(cellName);

		StateHasChanged(); // Refresh UI
	}

	/// <summary>
	/// Updates the value displayed for the selected cell based on its contents.
	/// </summary>
	/// <param name="cellName">The name of the cell (e.g., A1, B2).</param>
	private void UpdateSelectedCellValue(string cellName)
	{
		// Get the cell's value and handle potential errors.
		var cellValue = _spreadsheet.GetCellValue(cellName);

		SelectedCellValue = cellValue switch
		{
			string strValue => strValue,
			double doubleValue => doubleValue.ToString(),
			_ => "Formula Error" // Handle invalid formulas.
		};
		StateHasChanged(); // Refresh UI
	}

	/// <summary>
	/// Updates the contents of the selected cell in the spreadsheet and the UI.
	/// </summary>
	private void UpdateCellContents()
	{
		// Only update if the selected cell has non-empty content.
		if (string.IsNullOrWhiteSpace(SelectedCellContents)) return;

		try
		{
			// Update the cell's contents and refresh all affected cells.
			var cellName = SelectedCellName.ToUpper(); // Case insensitive
			var affectedCells = _spreadsheet.SetContentsOfCell(cellName, SelectedCellContents);

			// Update backing store for affected cells
			foreach (var affectedCell in affectedCells)
			{
				var affectedValue = _spreadsheet.GetCellValue(affectedCell);
				CellsBackingStore[GetRowIndex(affectedCell), GetColumnIndex(affectedCell)] = affectedValue switch
				{
					string strValue => strValue,
					double doubleValue => doubleValue.ToString(),
					_ => "FormulaError"
				};
			}

			// Update the displayed value of the selected cell.
			UpdateSelectedCellValue(cellName);
			StateHasChanged();
		}
		catch (InvalidNameException)
		{
			// Show error message for invalid cell names.
			ErrorMessage = "Error: Invalid cell name.";
			showErrorModal = true;
		}
		catch (CircularException)
		{
			// Show error message for circular dependcies.
			ErrorMessage = "Error: Circular dependency detected.";
			showErrorModal = true;
		}
		catch (Exception ex)
		{
			// Handle any other errors that may occur.
			ErrorMessage = $"Error: {ex.Message}";
			showErrorModal = true;
		}
	}

	/// <summary>
	/// Closes the error modal and clears the error message.
	/// </summary>
	private void CloseErrorModal()
	{
		showErrorModal = false;
		ErrorMessage = string.Empty;
		StateHasChanged(); // Refresh the UI
	}


	// Method to get the row index based on cell name

	/// <summary>
	/// Gets the row index from a cell name (e.g., "A1" -> row 0).
	/// </summary>
	/// <param name="cellName">The cell name (e.g., "A1").</param>
	/// <returns>The zero-based row index.</returns>
	private int GetRowIndex(string cellName)
	{
		return int.Parse(cellName.Substring(1)) - 1; // Convert to zero-based index
	}

	/// <summary>
	/// Gets the column index from a cell name (e.g., "A1" -> column 0).
	/// </summary>
	/// <param name="cellName">The cell name (e.g., "A1").</param>
	/// <returns>The zero-based column index.</returns>
	private int GetColumnIndex(string cellName)
	{
		return cellName[0] - 'A'; // Convert letter to zero-based index
	}

	/// <summary>
	/// Saves the current spreadsheet, by providing a download of a file
	/// containing the json representation of the spreadsheet.
	/// </summary>
	private async Task SaveFile()
	{
		// Convert the spreadsheet to a JSON string and trigger the download.
		var json = _spreadsheet.GetJson();
		await JSRuntime.InvokeVoidAsync("downloadFile", FileSaveName, json);
	}

	/// <summary>
	/// This method will run when the file chooser is used, for loading a file.
	/// Uploads a file containing a json representation of a spreadsheet, and 
	/// replaces the current sheet with the loaded one.
	/// </summary>
	/// <param name="args">The event arguments, which contains the selected file name</param>
	private async void HandleFileChooser(EventArgs args)
	{
		isLoading = true; // Start loading
		StateHasChanged(); // Refresh UI

		try
		{
			string fileContent = string.Empty;

			// Cast the EventArgs to InputFileChangeEventArgs or throw an exception if failed
			InputFileChangeEventArgs eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("unable to get file name");
			if (eventArgs.FileCount == 1)
			{
				// Read the file content and load the spreadsheet from the JSON data.
				var file = eventArgs.File;
				if (file is null)
				{
					return;
				}

				// Read the file content as a string
				using var stream = file.OpenReadStream();
				using var reader = new StreamReader(stream);

				fileContent = await reader.ReadToEndAsync();

				// Load the spreadsheet data from JSON
				_spreadsheet.LoadFromJson(fileContent);

				// Populate the UI with the loaded cell data
				var nonEmptyCellNames = _spreadsheet.GetNamesOfAllNonemptyCells();

				// Retrieve the content for each non-empty cell and update the UI accordingly
				foreach (var cellName in nonEmptyCellNames)
				{
					var cellContent = _spreadsheet.GetCellContents(cellName);
					var rowIndex = GetRowIndex(cellName);
					var colIndex = GetColumnIndex(cellName);
					CellsBackingStore[rowIndex, colIndex] = cellContent switch
					{
						string strValue => strValue,
						double doubleValue => doubleValue.ToString(),
						_ => string.Empty // Handle other cases (like Formula)
					};
				}

				StateHasChanged(); // Trigger UI update
			}
		}
		catch (Exception e)
		{
			Debug.WriteLine("An error occurred while loading the file: " + e);
		}
		finally
		{
			isLoading = false; // End loading
			StateHasChanged(); // Refresh UI
		}
	}

	/// <summary>
	/// Clears the content of the currently selected cell.
	/// </summary>
	private void ClearCell()
	{
		if (!string.IsNullOrWhiteSpace(SelectedCellName))
		{
			// Clear the selected cell's contents
			_spreadsheet.SetContentsOfCell(SelectedCellName, string.Empty);
			CellsBackingStore[GetRowIndex(SelectedCellName), GetColumnIndex(SelectedCellName)] = string.Empty;

			// Refresh the displayed value
			SelectedCellValue = string.Empty;
			SelectedCellContents = string.Empty;

			StateHasChanged(); // Refresh UI
		}
	}

	/// <summary>
	/// Clears the contents of all cells in the spreadsheet.
	/// </summary>
	private void ClearAllCells()
	{
		// Iterate through every cell in the backing store and clear its content
		for (int row = 0; row < ROWS; row++)
		{
			for (int col = 0; col < COLS; col++)
			{
				// Clear each cell in the backing store
				CellsBackingStore[row, col] = string.Empty;
			}
		}

		// Reset selected cell values
		SelectedCellValue = string.Empty;
		SelectedCellContents = string.Empty;

		StateHasChanged(); // Refresh UI
	}

	/// <summary>
	/// Executes logic after the component is rendered for the first time.
	/// </summary>
	/// <param name="firstRender">True if it's the first render.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			SelectedCellName = "A1";
			SelectedCellContents = CellsBackingStore[0, 0];
			await cellInput.FocusAsync();
		}
	}

	/// <summary>
	/// Executes logic after the component is initialized.
	/// </summary>
	/// <returns>A task that represents the asynchronous operation.</returns>
	protected override async Task OnInitializedAsync()
	{
		SelectedCellName = "A1";
		SelectedCellContents = CellsBackingStore[0, 0];
		await cellInput.FocusAsync(); // Set focus when initialized
	}


	/// <summary>
	/// Toggles between light and dark mode for the spreadsheet UI.
	/// </summary>
	private void ToggleDarkMode()
	{
		// Toggle dark mode state
		isDarkMode = !isDarkMode;
		if (isDarkMode)
		{
			// Apply dark mode styles by adding the class to the body element
			JSRuntime.InvokeVoidAsync("document.body.classList.add", "dark-mode");
		}
		else
		{
			// Remove dark mode styles by removing the class from the body element
			JSRuntime.InvokeVoidAsync("document.body.classList.remove", "dark-mode");
		}
	}

	/// <summary>
	/// Handles key press events in the spreadsheet, such as navigating or editing cells.
	/// </summary>
	/// <param name="e">Keyboard event arguments.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private async Task HandleKeyDown(KeyboardEventArgs e)
	{
		if (e.Key == "Enter")
		{

			// Update the cell contents first
			UpdateCellContents();

			// Display the current cell value
			var currentCellValue = _spreadsheet.GetCellValue(SelectedCellName);
			SelectedCellValue = currentCellValue switch
			{
				string strValue => strValue,
				double doubleValue => doubleValue.ToString(),
				_ => "Formula Error" // Handle formula errors
			};

			// Clear the contents of the input field
			SelectedCellContents = string.Empty;

			// Get the current row and column
			int currentRow = GetRowIndex(SelectedCellName);
			int currentCol = GetColumnIndex(SelectedCellName);

			// Move to the cell below (next row, same column)
			if (currentRow < ROWS - 1) // Ensure it doesn't exceed row limit
			{
				currentRow++;
			}

			// Update the selected cell name
			SelectedCellName = $"{Alphabet[currentCol]}{currentRow + 1}";

			// Load the content for the next selected cell
			string contentDisplayed = _spreadsheet.GetCellContents(SelectedCellName)!.ToString() ?? "";
			if (_spreadsheet.GetCellContents(SelectedCellName) is Formula)
				contentDisplayed = "=" + contentDisplayed;

			SelectedCellContents = contentDisplayed; // Load the content of the next selected cell
			UpdateSelectedCellValue(SelectedCellName);
			StateHasChanged();

			// Focus the input field for the next entry
			await cellInput.FocusAsync();
		}
	}
}
