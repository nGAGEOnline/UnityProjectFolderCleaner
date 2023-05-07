using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;
using UnityProjectFolderCleaner.Services;

namespace UnityProjectFolderCleaner;

public class UnityProjectCleaner
{
	private readonly IDataDisplay _dataDisplay;
	private readonly IUserInputHandler _userInputHandler;
	private readonly IOutputWriter _outputWriter;
	private readonly IFolderCleaningService _folderCleaningService;
	private readonly IEnumerable<string> _targetFolders;
	private readonly IFolderTypeService _folderTypeService;

	private readonly TotalProcessingInfo _totalProcessingInfo = new();
	
	public UnityProjectCleaner(UnityProjectCleanerSettings settings)
	{
		_dataDisplay = settings.DataDisplay;
		_userInputHandler = settings.UserInputHandler;
		_outputWriter = settings.OutputWriter;
		_folderTypeService = settings.FolderTypeService;
		_folderCleaningService = settings.FolderCleaningService;
		_targetFolders = settings.TargetFolders;
	}

	public void Run()
	{
		foreach (var targetFolder in _targetFolders)
		{
			var targetFolderProcessingService = new TargetFolderProcessingService(targetFolder, _dataDisplay, _outputWriter, _folderTypeService);
			var targetFolderProcessingInfo = targetFolderProcessingService.Process();
			_totalProcessingInfo.Children.Add(targetFolderProcessingInfo);
			_totalProcessingInfo.Size += targetFolderProcessingInfo.Size;
			_totalProcessingInfo.SizeToClean += targetFolderProcessingInfo.SizeToClean;
		}
        
		DisplaySummary();
		DisplayTotals();
        
		if (DisplaySummaryAndConfirmCleaning())
			CleanFolders();
		else
			_outputWriter.WriteLineInColor("\nCleaning aborted.\n", Color.Red);
	}

	private void DisplaySummary()
	{
		_outputWriter.NewLine();
		_outputWriter.LineWithInsert('_', "[ SUMMARIES ]", 48);

		foreach (var targetSize in _totalProcessingInfo.Children)
		{
			_outputWriter.LineWithInsert('.', $"[ {targetSize.Target.FullName} ]", 48);
			_dataDisplay.DisplayConclusion(new SizeInfo(targetSize.TotalSize), new SizeInfo(targetSize.TotalSizeToClean));
		}
		_outputWriter.Line('_', 48);
	}

	private void DisplayTotals()
	{
		_outputWriter.NewLine();
		_outputWriter.LineWithInsert('=', "[ TOTAL ]", 48);
		_dataDisplay.DisplayConclusion(new SizeInfo(_totalProcessingInfo.TotalSize), new SizeInfo(_totalProcessingInfo.TotalSizeToClean));
		_outputWriter.Line('=', 48);
		_outputWriter.NewLine();
	}

	private bool DisplaySummaryAndConfirmCleaning() 
		=> _userInputHandler.ConfirmCleaning();

	private void CleanFolders()
	{
		_outputWriter.WriteLineInColor("\nCleaning...", Color.Yellow);
		
		// TODO: Should get actual cleaned size total
		_folderCleaningService.Clean(_totalProcessingInfo);

		_outputWriter.WriteLineInColor("\nCleaning Completed!", Color.Yellow);
		_outputWriter.WriteLineInColor($"Total size: {new SizeInfo(_totalProcessingInfo.TotalSize)}", Color.Red);
		_outputWriter.WriteLineInColor($"Total cleaned: {new SizeInfo(_totalProcessingInfo.TotalSizeToClean)}", Color.Green);
		_outputWriter.WriteLineInColor($"Total remaining: {new SizeInfo(_totalProcessingInfo.TotalSize - _totalProcessingInfo.TotalSizeToClean)}", Color.Yellow);
	}
}