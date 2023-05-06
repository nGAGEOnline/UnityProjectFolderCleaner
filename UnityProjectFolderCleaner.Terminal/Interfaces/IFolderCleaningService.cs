namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IFolderCleaningService
{
	void Clean(Dictionary<string, List<DirectoryInfo>> foldersToClean);
}