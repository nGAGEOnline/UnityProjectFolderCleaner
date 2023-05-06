using UnityProjectFolderCleaner.Terminal.Processing;

namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IFolderCleaningService
{
	void Clean(TotalProcessingInfo foldersToClean);
}