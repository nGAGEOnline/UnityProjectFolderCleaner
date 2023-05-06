using UnityProjectFolderCleaner.Processing;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IFolderCleaningService
{
	void Clean(TotalProcessingInfo foldersToClean);
}