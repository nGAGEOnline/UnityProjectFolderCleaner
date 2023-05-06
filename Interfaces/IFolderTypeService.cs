using UnityProjectFolderCleaner.Terminal.Data;
using UnityProjectFolderCleaner.Terminal.Enums;

namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IFolderTypeService
{
	FolderType GetRecommendation(UnityProjectInfo unityProjectInfo, DirectoryInfo directoryInfo);
}