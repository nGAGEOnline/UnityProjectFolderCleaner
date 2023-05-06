using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.IO;

public abstract class BaseInputHandler
{
	protected readonly IOutputWriter _outputWriter;

	protected BaseInputHandler(IOutputWriter outputWriter)
	{
		_outputWriter = outputWriter;
	}

	protected void DisplayCleaningConfirmationPrompt(string prefix = "")
	{
		_outputWriter.WriteInColor($"{prefix}Do you want to proceed with cleaning?", Color.White);
		_outputWriter.WriteInColor(" [Y]es", Color.Green);
		_outputWriter.WriteInColor("/", Color.White);
		_outputWriter.WriteInColor("[N]o", Color.Red);
		_outputWriter.WriteInColor(" (Default: Yes)", Color.White);
	}
}
