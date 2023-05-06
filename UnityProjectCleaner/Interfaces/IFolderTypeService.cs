using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IFolderTypeService
{
	FolderType GetRecommendation(UnityProjectInfo unityProjectInfo, DirectoryInfo directoryInfo);
}