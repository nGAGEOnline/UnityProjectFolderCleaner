using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.Services;

public class UnityFolderTypeService : IFolderTypeService
{
	public FolderType GetRecommendation(UnityProject unityProject, DirectoryInfo directoryInfo)
		=> unityProject.IsProtected(directoryInfo.Name)
			? FolderType.Protected
			: FolderType.Clean;
}