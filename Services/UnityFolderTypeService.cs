using UnityProjectFolderCleaner.Terminal.Data;
using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.Services;

public class UnityFolderTypeService : IFolderTypeService
{
	public FolderType GetRecommendation(UnityProjectInfo unityProjectInfo, DirectoryInfo directoryInfo)
		=> unityProjectInfo.IsProtected(directoryInfo.Name)
			? FolderType.Protected
			: FolderType.Clean;
}