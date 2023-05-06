using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.Services;

public class MockFolderCleaningService : IFolderCleaningService
{
	public void Clean(Dictionary<string, List<DirectoryInfo>> foldersToClean)
	{
		foreach (var unityFolder in foldersToClean)
		{
			Console.WriteLine($"[MOCK] Cleaning Unity folder: {unityFolder.Key}");
			foreach (var directoryInfo in unityFolder.Value)
			{
				Console.WriteLine($"[MOCK] Deleting folder: {directoryInfo.FullName}");
			}
		}
	}
}