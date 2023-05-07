using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner;

public class UnityProjectCleanerSettings
{
	public IDataDisplay DataDisplay { get; init; }
	public IFolderTypeService FolderTypeService { get; init; }
	public IFolderCleaningService FolderCleaningService { get; init; }
	public IOutputWriter OutputWriter { get; init; }
	public IUserInputHandler UserInputHandler { get; init; }
	public IEnumerable<string> TargetFolders { get; init; }
}
