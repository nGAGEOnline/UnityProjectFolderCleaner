using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Services;

public class MockFolderCleaningService : BaseCleaningService, IFolderCleaningService
{
	public MockFolderCleaningService(IOutputWriter outputWriter) 
		: base(outputWriter) { }

	public void Clean(TotalProcessingInfo foldersToClean)
	{
		foreach (var unityProjectInfo in foldersToClean.Children.SelectMany(targetFolderInfo => targetFolderInfo.Children))
		{
			OutputWriter.WriteInColor("\n[MOCK] ", Color.DarkCyan);
			OutputWriter.WriteInColor("Cleaning: ", Color.White);
			OutputWriter.WriteLineInColor(unityProjectInfo.Target.Name, Color.Yellow);

			Thread.Sleep(Random.Shared.Next(20, 100));
			foreach (var directoryInfo in unityProjectInfo.Children)
			{
				OutputWriter.WriteInColor("[MOCK] ", Color.DarkCyan);
				OutputWriter.WriteInColor("Deleting: ", Color.Gray);
				OutputWriter.WriteLineInColor(directoryInfo.FullName, Color.Red);

				Thread.Sleep(Random.Shared.Next(20, 100));
			}
		}
		OutputWriter.NewLine();
	}
}