using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner;

public class CleanerSettings
{
	public bool UseAutomaticMode { get; init; }
	public bool UseMockMode { get; init; }
	public IEnumerable<string>? TargetFolders { get; init; }
}

public class UnityProjectCleanerSettings
{
	public IDataDisplay? DataDisplay { get; init; }
	public IFolderTypeService? FolderTypeService { get; init; }
	public IFolderCleaningService? FolderCleaningService { get; init; }
	public IOutputWriter? OutputWriter { get; init; }
	public IUserInputHandler? UserInputHandler { get; init; }
	public IEnumerable<string>? TargetFolders { get; init; }
}
