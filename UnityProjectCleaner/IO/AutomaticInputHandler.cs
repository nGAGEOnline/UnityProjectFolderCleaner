using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.IO;

public class AutomaticInputHandler : BaseInputHandler, IUserInputHandler
{
	private readonly IEnumerable<string> _targetFolders = new[] { @"F:\UnityProjects" };

	public AutomaticInputHandler(IOutputWriter outputWriter) 
		: base(outputWriter) { }
	
	public AutomaticInputHandler(IOutputWriter outputWriter, IEnumerable<string> targetFolders) 
		: base(outputWriter)
	{
		var array = targetFolders as string[] ?? targetFolders.ToArray();
		if (array.Any())
			_targetFolders = array;
	}

	public IEnumerable<string> GetTargetFolders()
	{
		_outputWriter.WriteLineInColor("Skipping user-input. Automatically assigning target folders.", Color.Red);
		Thread.Sleep(1000);
		_outputWriter.WriteInColor("Automatically assigning: ", Color.DarkCyan);
		Thread.Sleep(1000);
		_outputWriter.WriteLineInColor($"{string.Join(", ", _targetFolders)}", Color.White);
		Thread.Sleep(2000);
		_outputWriter.NewLine();
		return _targetFolders;
	}

	public bool ConfirmCleaning()
	{
		DisplayCleaningConfirmationPrompt("(Mock) ");
		ConsoleKeyInfo keyInfo;

		do
		{
			keyInfo = Console.ReadKey(true);
		} while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

		Console.WriteLine();

		return keyInfo.Key is ConsoleKey.Y or ConsoleKey.Enter;
	}
}
