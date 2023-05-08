using UnityProjectFolderCleaner.Data;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IFolderCleaningService
{
	event Action<SizeInfo> OnDeletedNotifySizeTotal;
		
	void Clean(TotalProcessingInfo foldersToClean);
}