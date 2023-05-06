// Program.cs

using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Helpers;

namespace UnityProjectFolderCleaner.Terminal;

internal static class Program
{
	private static void Main(string[] args)
	{
		IOutputWriter outputWriter = new ConsoleOutputWriter();
		ConsoleMenu consoleMenu = new ConsoleMenu(outputWriter);

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

		Thread.Sleep(3000);
		// Console.WriteLine("Press any key to exit...");
		// Console.ReadKey(true);
	}
}
