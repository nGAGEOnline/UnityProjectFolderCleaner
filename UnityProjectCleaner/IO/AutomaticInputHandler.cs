using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.IO;

public class AutomaticInputHandler : BaseInputHandler, IUserInputHandler
{
	private readonly IEnumerable<string> _menuOptions;
	private readonly IEnumerable<string> _targetFolders = new[] { @"F:\UnityProjects" };

	public AutomaticInputHandler(IOutputWriter outputWriter, IEnumerable<string> menuOptions) 
		: base(outputWriter)
	{
		_menuOptions = menuOptions;
	}
	
	public AutomaticInputHandler(IOutputWriter outputWriter, IEnumerable<string> targetFolders, IEnumerable<string> menuOptions) 
		: base(outputWriter)
	{
		_menuOptions = menuOptions;
		var array = targetFolders as string[] ?? targetFolders.ToArray();
		if (array.Any())
			_targetFolders = array;
	}

	public IEnumerable<string> GetTargetFolders()
	{
		const string title = "UnityProject Folder Cleaner";

		OutputWriter.Clear();
		OutputWriter.WriteInColor(title, Color.DarkCyan);
		if (_menuOptions.Any())
		{
			OutputWriter.WriteInColor($" [", Color.DarkCyan);
			OutputWriter.WriteInColor(string.Join(", ", _menuOptions), Color.Cyan);
			OutputWriter.WriteInColor("]\n", Color.DarkCyan);
		}
		else
			OutputWriter.NewLine();
		OutputWriter.WriteLineInColor(new string('=', title.Length), Color.White);

		OutputWriter.WriteLineInColor("Skipping user-input. Automatically assigning target folders.", Color.Red);
		Thread.Sleep(1000);
		OutputWriter.WriteInColor("Automatically assigning: ", Color.DarkCyan);
		Thread.Sleep(1000);
		OutputWriter.WriteLineInColor($"{string.Join(", ", _targetFolders)}", Color.White);
		Thread.Sleep(2000);
		OutputWriter.NewLine();
		return _targetFolders;
	}

	public bool ConfirmCleaning()
	{
		DisplayCleaningConfirmationPrompt("[Mock] ");
		ConsoleKeyInfo keyInfo;

		do
		{
			keyInfo = Console.ReadKey(true);
		} while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

		Console.WriteLine();

		return keyInfo.Key is ConsoleKey.Y or ConsoleKey.Enter;
	}
}
