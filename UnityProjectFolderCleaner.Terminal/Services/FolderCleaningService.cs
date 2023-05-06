using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.Services;

public class FolderCleaningService : IFolderCleaningService
{
	public void Clean(Dictionary<string, List<DirectoryInfo>> foldersToClean)
	{
		foreach (var unityFolder in foldersToClean)
		{
			Console.WriteLine($"Cleaning Unity folder: {unityFolder.Key}");
			foreach (var directoryInfo in unityFolder.Value)
			{
				Console.WriteLine($"Deleting folder: {directoryInfo.FullName}");
				directoryInfo.Delete(true);
			}
		}
	}
}