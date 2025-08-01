using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Helpers;
using UnityProjectFolderCleaner.Providers;

namespace UnityProjectFolderCleaner;

internal static class Program
{
	private static readonly string[] TestFolders = { "F:\\UnityProjects",
		"F:\\UnityProjects_Old",
		"F:\\UnityProjects_Test" }; // @"D:\UnityProjects", @"F:\UnityProjects", 
	
	private static void Main(string[] args)
	{
		var outputWriter = new ConsoleOutputWriter();
		var settingsProvider = new ConsoleSettingsProvider(outputWriter, TestFolders);

		var settings = settingsProvider.GetSettings();
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
