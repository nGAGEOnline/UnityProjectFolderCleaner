using UnityProjectFolderCleaner.Terminal.Interfaces;
using UnityProjectFolderCleaner.Terminal.Processing;

namespace UnityProjectFolderCleaner.Terminal.Services;

public class FolderCleaningService : IFolderCleaningService
{
	public void Clean(TotalProcessingInfo foldersToClean)
	{
		foreach (var unityProjectInfo in foldersToClean.Children.SelectMany(targetFolderInfo => targetFolderInfo.Children))
		{
			Console.WriteLine($"Cleaning Unity folder: {unityProjectInfo.Target.Name}");
			foreach (var directoryInfo in unityProjectInfo.Children)
			{
				Console.WriteLine($"Deleting folder: {directoryInfo.FullName}");
				directoryInfo.Delete(true);
			}
		}
	}
}