using UnityProjectFolderCleaner.Data;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IFolderCleaningService
{
	void Clean(TotalProcessingInfo foldersToClean);
}