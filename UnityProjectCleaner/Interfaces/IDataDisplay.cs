using UnityProjectFolderCleaner.Terminal.Data;
using UnityProjectFolderCleaner.Terminal.Enums;

namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IDataDisplay
{
	void DisplayUnityProjectTitle(UnityProjectInfo unityProjectInfo);
	void DisplayFolderSizeInfo(DirectoryInfo directoryInfo, SizeInfo sizeInfo, FolderType type);
	void DisplayConclusion(SizeInfo sizeInfo, SizeInfo sizeInfoToClean);
}