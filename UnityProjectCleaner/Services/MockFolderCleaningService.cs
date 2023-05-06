using UnityProjectFolderCleaner.Terminal.Interfaces;
using UnityProjectFolderCleaner.Terminal.Processing;

namespace UnityProjectFolderCleaner.Terminal.Services;

public class MockFolderCleaningService : IFolderCleaningService
{
	public void Clean(TotalProcessingInfo foldersToClean)
	{
		foreach (var unityProjectInfo in foldersToClean.Children.SelectMany(targetFolderInfo => targetFolderInfo.Children))
		{
			Console.WriteLine($"[MOCK] Cleaning Unity folder: {unityProjectInfo.Target.Name}");
			Thread.Sleep(10);
			foreach (var directoryInfo in unityProjectInfo.Children)
			{
				Console.WriteLine($"[MOCK] Deleting folder: {directoryInfo.FullName}");
				Thread.Sleep(10);
			}
		}
	}
}