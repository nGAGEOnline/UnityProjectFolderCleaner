using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IDataDisplay
{
	void DisplayUnityProjectTitle(UnityProjectInfo unityProjectInfo);
	void DisplayFolderSizeInfo(DirectoryInfo directoryInfo, SizeInfo sizeInfo, FolderType type);
	void DisplayConclusion(SizeInfo sizeInfo, SizeInfo sizeInfoToClean);
}