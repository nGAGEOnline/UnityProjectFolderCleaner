using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;
using UnityProjectFolderCleaner.IO;
using UnityProjectFolderCleaner.Services;

namespace UnityProjectFolderCleaner.Providers;

public class ConsoleSettingsProvider(IOutputWriter outputWriter, IEnumerable<string> targetFolders) : ISettingsProvider
{
	private readonly List<string> _selectedMenuOptions = [];

	public UnityProjectCleanerSettings? GetSettings()
	{
		_selectedMenuOptions.Clear();
		if (JsonSettingsService.SettingsExist())
		{
			DisplayHeader("Settings file found.", Color.DarkCyan);
			var loadSettingsResult = outputWriter.Confirm("Load settings?");
			if (loadSettingsResult)
			{
				var loadedSettings = JsonSettingsService.LoadSettings();
				if (loadedSettings != null) return new UnityProjectCleanerSettings
				{
					DataDisplay = new ConsoleDataDisplay(outputWriter),
					FolderTypeService = new UnityFolderTypeService(),
					FolderCleaningService = loadedSettings.UseMockMode
						? new MockFolderCleaningService(outputWriter)
						: new FolderCleaningService(outputWriter),
					OutputWriter = outputWriter,
					UserInputHandler = loadedSettings.UseAutomaticMode
						? new AutomaticInputHandler(outputWriter, targetFolders, _selectedMenuOptions)
						: new ConsoleUserInputHandler(outputWriter, _selectedMenuOptions),
					TargetFolders = loadedSettings.TargetFolders
				};
			}
		}
		
		var userInputHandler = ChooseUserInputHandler();
		if (userInputHandler == null) return null;

		var cleaningService = ChooseCleaningService();
		if (cleaningService == null) return null;

		var cleanerSettings = new CleanerSettings()
		{
			TargetFolders = userInputHandler.GetTargetFolders(),
			UseAutomaticMode = userInputHandler is AutomaticInputHandler,
			UseMockMode = cleaningService is MockFolderCleaningService
		};
		var settings = new UnityProjectCleanerSettings
		{
			DataDisplay = new ConsoleDataDisplay(outputWriter),
			FolderTypeService = new UnityFolderTypeService(),
			FolderCleaningService = cleaningService,
			OutputWriter = outputWriter,
			UserInputHandler = userInputHandler,
			TargetFolders = userInputHandler.GetTargetFolders()
		};
		
		DisplayHeader("Save Settings.", Color.DarkCyan);
		var saveSettingsResult = outputWriter.Confirm("Save settings to JSON file?");
		if (saveSettingsResult)
		{
			JsonSettingsService.SaveSettings(cleanerSettings);
		}

		return settings;
	}

	private IUserInputHandler? ChooseUserInputHandler()
	{
		var inputModeOptions = new List<string> { "Automatic", "Manual" };
		DisplayMenuOptions(inputModeOptions, Color.DarkCyan, "Input Mode Selection");
        
		var inputModeSelection = SelectMenuOption(inputModeOptions.Count);
		if (inputModeSelection == -1) return null;
		_selectedMenuOptions.Add(inputModeOptions[inputModeSelection - 1]);

		return inputModeSelection == 1
			? new AutomaticInputHandler(outputWriter, targetFolders, _selectedMenuOptions)
			: new ConsoleUserInputHandler(outputWriter, _selectedMenuOptions);
	}

	private IFolderCleaningService? ChooseCleaningService()
	{
		var cleaningModeOptions = new List<string> { "Mock Cleaning", "Real Cleaning" };
		DisplayMenuOptions(cleaningModeOptions, Color.DarkCyan, "Clean Mode Selection");
        
		var cleaningModeSelection = SelectMenuOption(cleaningModeOptions.Count);
		if (cleaningModeSelection == -1) return null;
		_selectedMenuOptions.Add(cleaningModeOptions[cleaningModeSelection - 1]);

		return cleaningModeSelection == 1
			? new MockFolderCleaningService(outputWriter)
			: new FolderCleaningService(outputWriter);
	}

	private void DisplayHeader(string title, Color textColor)
	{
		outputWriter.Clear();
		outputWriter.WriteInColor(title, textColor);
		if (_selectedMenuOptions.Count > 0)
		{
			outputWriter.WriteInColor($" [", Color.DarkCyan);
			outputWriter.WriteInColor(string.Join(", ", _selectedMenuOptions), Color.Cyan);
			outputWriter.WriteInColor("]\n", Color.DarkCyan);
		}
		else
			outputWriter.NewLine();
		outputWriter.WriteLineInColor(new string('=', title.Length), Color.White);
	}
	
	private void DisplayMenuOptions(IEnumerable<string> menuOptions, Color textColor, string title)
	{
		DisplayHeader(title, textColor);
		
		int optionIndex = 1;
		foreach (var option in menuOptions)
		{
			outputWriter.WriteLineInColor($"[{optionIndex}] {option}", textColor);
			optionIndex++;
		}
            
		outputWriter.WriteLineInColor("[ESC] Abort", Color.Red);
	}

	private static int SelectMenuOption(int optionCount)
	{
		while (true)
		{
			var input = Console.ReadKey(true);
			if (input.Key == ConsoleKey.Escape)
				return -1;

			var selectedOption = input.Key switch
			{
				>= ConsoleKey.NumPad1 and <= ConsoleKey.NumPad9 => input.Key - ConsoleKey.NumPad1 + 1,
				_ => input.Key - ConsoleKey.D1 + 1
			};

			if (selectedOption >= 1 && selectedOption <= optionCount)
				return selectedOption;
		}
	}
}