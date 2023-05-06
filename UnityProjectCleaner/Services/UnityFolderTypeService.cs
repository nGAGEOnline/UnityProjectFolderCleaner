using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Services;

public class UnityFolderTypeService : IFolderTypeService
{
	public FolderType GetRecommendation(UnityProjectInfo unityProjectInfo, DirectoryInfo directoryInfo)
		=> unityProjectInfo.IsProtected(directoryInfo.Name)
			? FolderType.Protected
			: FolderType.Clean;
}