using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Helpers;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal;

public class UnityProjectCleaner
{
	private readonly IDataDisplay _dataDisplay;
	private readonly IFolderTypeService _folderTypeService;
	private readonly IFolderCleaningService _folderCleaningService;
	private readonly IUserInputHandler _userInputHandler;
	private readonly IEnumerable<string> _targetFolders;
	private readonly Dictionary<string, List<DirectoryInfo>> _toClean = new();
	private readonly Dictionary<string, (long size, long sizeToClean)> _targetSizes = new();
	private long _targetSize = 0L;
	private long _targetSizeToClean = 0L;
	private long _totalSize = 0L;
	private long _totalSizeToClean = 0L;
	private readonly IOutputWriter _outputWriter;

	public UnityProjectCleaner(UnityProjectCleanerSettings settings)
	{
		_dataDisplay = settings.DataDisplay;
		_folderTypeService = settings.FolderTypeService;
		_folderCleaningService = settings.FolderCleaningService;
		_outputWriter = settings.OutputWriter;
		_userInputHandler = settings.UserInputHandler;
		_targetFolders = settings.TargetFolders;
	}

	private void AddToCleanList(string unityFolder, DirectoryInfo directoryInfo)
	{
		if (!_toClean.ContainsKey(unityFolder))
			_toClean.Add(unityFolder, new List<DirectoryInfo>());

		_toClean[unityFolder].Add(directoryInfo);
	}

	public void Run()
	{
		foreach (var targetFolder in _targetFolders)
		{
			ProcessTargetFolder(targetFolder);
			_totalSize += _targetSize;
			_totalSizeToClean += _targetSizeToClean;
		}
		
		_outputWriter.LineWithInsert('_', "[ SUMMARIES ]", 48);
		foreach (var targetSize in _targetSizes)
		{
			_outputWriter.LineWithInsert('.', $"[ {targetSize.Key} ]", 48);
			_dataDisplay.DisplayConclusion(new SizeInfo(targetSize.Value.size), new SizeInfo(targetSize.Value.sizeToClean));
		}
		_outputWriter.Line('_', 48);
		_outputWriter.NewLine(2);
		_outputWriter.LineWithInsert('=', "[ TOTAL ]", 48);
		_dataDisplay.DisplayConclusion(new SizeInfo(_totalSize), new SizeInfo(_totalSizeToClean));
		_outputWriter.Line('=', 48);
		
		if (DisplaySummaryAndConfirmCleaning())
			CleanFolders();
		else
			_outputWriter.WriteLineInColor("\nCleaning aborted.\n", Color.Red);
	}

	private void ProcessTargetFolder(string targetFolder)
	{
		var targetInfo = new DirectoryInfo(targetFolder);
		var unityFolders = targetInfo.GetDirectories();
		_targetSize = 0L;
		_targetSizeToClean = 0L; 

		foreach (var unityFolder in unityFolders)
		{
			var unityProject = new UnityProject(unityFolder.FullName);
			if (unityProject.HasSolutionFiles())
				ProcessUnityProject(unityFolder, unityProject);
			else
				_outputWriter.WriteLineInColor($"\n{unityFolder.Name} -> No solution files found. Skipping...\n", Color.Red);
		}

		_targetSizes.Add(targetInfo.FullName, (_targetSize, _targetSizeToClean));

		_outputWriter.LineWithInsert('=', $"[ {targetFolder} ]", 48);
		_dataDisplay.DisplayConclusion(new SizeInfo(_targetSize), new SizeInfo(_targetSizeToClean));
	}

	private void ProcessUnityProject(DirectoryInfo unityFolder, UnityProject unityProject)
	{
		_dataDisplay.DisplayUnityProjectTitle(unityProject);
		_outputWriter.LineWithPrefix('-', '\t');

		var folderSize = 0L;
		var folderSizeToClean = 0L;

		foreach (var directoryInfo in unityProject.GetDirectories())
		{
			var size = SizeInfo.Calculate(directoryInfo);
			folderSize += size;

			var recommendation = _folderTypeService.GetRecommendation(unityProject, directoryInfo);
			if (recommendation == FolderType.Clean)
			{
				folderSizeToClean += size;
				AddToCleanList(unityFolder.Name, directoryInfo);
			}

			_dataDisplay.DisplayFolderSizeInfo(directoryInfo, new SizeInfo(size), recommendation);
		}
		_targetSize += folderSize;
		_targetSizeToClean += folderSizeToClean;
		
		_outputWriter.LineWithPrefix('-', '\t');
		_dataDisplay.DisplayConclusion(new SizeInfo(folderSize), new SizeInfo(folderSizeToClean));
	}

	private bool DisplaySummaryAndConfirmCleaning() 
		=> _userInputHandler.ConfirmCleaning();

	private void CleanFolders()
	{
		_outputWriter.WriteLineInColor("\nCleaning...", Color.White);
		_folderCleaningService.Clean(_toClean);

		_outputWriter.WriteLineInColor("\nCleaning Completed!", Color.Yellow);
		_outputWriter.WriteLineInColor($"Total size: {new SizeInfo(_totalSize)}", Color.Red);
		_outputWriter.WriteLineInColor($"Total cleaned: {new SizeInfo(_totalSizeToClean)}", Color.Green);
		_outputWriter.WriteLineInColor($"Total remaining: {new SizeInfo(_totalSize - _totalSizeToClean)}", Color.Yellow);
	}
}

