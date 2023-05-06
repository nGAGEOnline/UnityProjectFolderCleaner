using UnityProjectFolderCleaner.Terminal.Helpers;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal;

public class UnityProjectCleanerSettings
{
	public IDataDisplay DataDisplay { get; init; }
	public IFolderTypeService FolderTypeService { get; init; }
	public IFolderCleaningService FolderCleaningService { get; init; }
	public IOutputWriter OutputWriter { get; set; }
	public IUserInputHandler UserInputHandler { get; init; }
	public IEnumerable<string> TargetFolders { get; init; }
}
