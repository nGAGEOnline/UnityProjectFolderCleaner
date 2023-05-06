// ConsoleMenu.cs

using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;
using UnityProjectFolderCleaner.Services;

namespace UnityProjectFolderCleaner.IO
{
    public class ConsoleMenu
    {
        private readonly IOutputWriter _outputWriter;
        private readonly IEnumerable<string> _targetFolders;

        public ConsoleMenu(IOutputWriter outputWriter, IEnumerable<string> targetFolders)
        {
	        _outputWriter = outputWriter;
	        _targetFolders = targetFolders;
        }

        public UnityProjectCleanerSettings? GetSettings()
        {
            var inputModeOptions = new List<string> { "Automatic", "Manual" };
            DisplayMenuOptions(inputModeOptions, Color.DarkCyan, "Input Mode Selection");
            
            var inputModeSelection = SelectMenuOption(inputModeOptions.Count);
            if (inputModeSelection == -1) 
	            return null;

            IUserInputHandler userInputHandler = inputModeSelection == 1
                ? new AutomaticInputHandler(_outputWriter, _targetFolders)
                : new ConsoleUserInputHandler(_outputWriter);

            var cleaningModeOptions = new List<string> { "Mock Cleaning", "Cleaning" };
            DisplayMenuOptions(cleaningModeOptions, Color.DarkCyan, "Clean Mode Selection");
            
            int cleaningModeSelection = SelectMenuOption(cleaningModeOptions.Count);
            if (cleaningModeSelection == -1) 
	            return null;
            
            IFolderCleaningService cleaningService = cleaningModeSelection == 1
				? new MockFolderCleaningService(_outputWriter)
				: new FolderCleaningService(_outputWriter);

            return new UnityProjectCleanerSettings
            {
                DataDisplay = new ConsoleDataDisplay(_outputWriter),
                FolderTypeService = new UnityFolderTypeService(),
                FolderCleaningService = cleaningService,
                OutputWriter = _outputWriter,
                UserInputHandler = userInputHandler,
                TargetFolders = userInputHandler.GetTargetFolders()
            };
        }

        public void DisplayMenuOptions(IEnumerable<string> menuOptions, Color textColor, string title)
        {
	        Console.Clear();
	        _outputWriter.WriteLineInColor(title, textColor);
	        _outputWriter.WriteLineInColor(new string('=', title.Length), Color.White);

	        int optionIndex = 1;
	        foreach (var option in menuOptions)
	        {
		        _outputWriter.WriteLineInColor($"[{optionIndex}] {option}", textColor);
		        optionIndex++;
	        }
            
            _outputWriter.WriteLineInColor("[ESC] Abort", Color.Red);
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
}
