using UnityProjectFolderCleaner.Terminal.Enums;

namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IFolderTypeService
{
	FolderType GetRecommendation(UnityProject unityProject, DirectoryInfo directoryInfo);
}