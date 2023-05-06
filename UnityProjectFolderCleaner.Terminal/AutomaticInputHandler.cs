using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Helpers;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal;

public class AutomaticInputHandler : IUserInputHandler
{
	private readonly IOutputWriter _outputWriter;
	private readonly IEnumerable<string> _targetFolders = new[] { @"D:\UnityProjects", @"F:\UnityProjects" };

	public AutomaticInputHandler(IOutputWriter outputWriter) 
		=> _outputWriter = outputWriter;

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
		_outputWriter.WriteInColor("\n(Mock)", Color.Gray);
		_outputWriter.WriteInColor(" Do you want to proceed with cleaning?", Color.White);
		_outputWriter.WriteInColor(" [Y]es", Color.Green);
		_outputWriter.WriteInColor("/", Color.White);
		_outputWriter.WriteInColor("[N]o", Color.Red);
		_outputWriter.WriteInColor(" (Default: Yes)", Color.White);

		ConsoleKeyInfo keyInfo;

		do
		{
			keyInfo = Console.ReadKey(true);
		} while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

		Console.WriteLine();

		return keyInfo.Key is ConsoleKey.Y or ConsoleKey.Enter;
	}
}
