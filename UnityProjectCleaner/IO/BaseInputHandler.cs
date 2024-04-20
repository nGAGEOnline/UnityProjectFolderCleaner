using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.IO;

public abstract class BaseInputHandler
{
	protected readonly IOutputWriter OutputWriter;

	protected BaseInputHandler(IOutputWriter outputWriter) => OutputWriter = outputWriter;

	protected void DisplayCleaningConfirmationPrompt(string prompt, string prefix = "")
	{
		if (prefix != string.Empty)
			OutputWriter.WriteInColor($"{prefix} ", Color.DarkCyan);
		OutputWriter.WriteInColor($"{prompt} ", Color.White);
		OutputWriter.WriteInColor("\t[Y]es", Color.Green);
		OutputWriter.WriteInColor("/", Color.White);
		OutputWriter.WriteInColor("[N]o ", Color.Red);
	}
}
