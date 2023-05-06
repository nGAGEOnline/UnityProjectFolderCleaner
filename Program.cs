using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Helpers;
using UnityProjectFolderCleaner.Terminal.Interfaces;
using UnityProjectFolderCleaner.Terminal.IO;

namespace UnityProjectFolderCleaner.Terminal;

internal static class Program
{
	private static readonly string[] TestFolders = { @"D:\UnityProjects", @"F:\UnityProjects" };
	
	private static void Main(string[] args)
	{
		IOutputWriter outputWriter = new ConsoleOutputWriter();
		var consoleMenu = new ConsoleMenu(outputWriter, TestFolders);

		var settings = consoleMenu.GetSettings();
		if (settings == null)
		{
			outputWriter.WriteLineInColor("\nOperation aborted.\n", Color.Red);
			Console.WriteLine("Press any key to exit...");
			Console.ReadKey(true);
			return;
		}

		var cleaner = new UnityProjectCleaner(settings);
		cleaner.Run();

		Console.WriteLine("Press any key to exit...");
		Console.ReadKey(true);
	}
}
