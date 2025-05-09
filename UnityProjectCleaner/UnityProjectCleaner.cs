﻿using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;
using UnityProjectFolderCleaner.Services;

namespace UnityProjectFolderCleaner;

public class UnityProjectCleaner
{
	private readonly UnityProjectCleanerSettings _settings;
	private readonly IDataDisplay? _dataDisplay;
	private readonly IUserInputHandler? _userInputHandler;
	private readonly IOutputWriter? _outputWriter;
	private readonly IFolderCleaningService? _folderCleaningService;
	private readonly IEnumerable<string>? _targetFolders;
	private readonly IFolderTypeService? _folderTypeService;

	private readonly TotalProcessingInfo _totalProcessingInfo = new();
	private long _actyualDeletedSizeTotal;

	public UnityProjectCleaner(UnityProjectCleanerSettings settings)
	{
		_settings = settings;
		_dataDisplay = settings.DataDisplay;
		_outputWriter = settings.OutputWriter;
		_targetFolders = settings.TargetFolders;
		_userInputHandler = settings.UserInputHandler;
		_folderTypeService = settings.FolderTypeService;
		_folderCleaningService = settings.FolderCleaningService;
		if (_folderCleaningService != null)
		{
			_folderCleaningService.OnDeletedNotifySizeTotal += AddToActualTotal;
		}
	}
	~UnityProjectCleaner()
	{
		if (_folderCleaningService != null)
		{
			_folderCleaningService.OnDeletedNotifySizeTotal -= AddToActualTotal;
		}
	}

	public void Run()
	{
		_actyualDeletedSizeTotal = 0L;
		if (_targetFolders != null)
		{
			foreach (var targetFolder in _targetFolders)
			{
				var targetFolderProcessingService = new TargetFolderProcessingService(targetFolder, _dataDisplay, _outputWriter, _folderTypeService);
				var targetFolderProcessingInfo = targetFolderProcessingService.Process();
				_totalProcessingInfo.Children.Add(targetFolderProcessingInfo);
				_totalProcessingInfo.Size += targetFolderProcessingInfo.Size;
				_totalProcessingInfo.SizeToClean += targetFolderProcessingInfo.SizeToClean;
			}
		}

		DisplaySummary();
		DisplayTotals();
        
		if (DisplaySummaryAndConfirmCleaning())
			CleanFolders();
		else
			_outputWriter?.WriteLineInColor("\nCleaning aborted.\n", Color.Red);
	}

	private void DisplaySummary()
	{
		_outputWriter?.NewLine();
		_outputWriter?.LineWithInsert('_', "[ SUMMARIES ]", 48);

		foreach (var targetSize in _totalProcessingInfo.Children)
		{
			_outputWriter?.LineWithInsert('.', $"[ {targetSize.Target.FullName} ]", 48);
			_dataDisplay?.DisplayConclusion(new SizeInfo(targetSize.TotalSize), new SizeInfo(targetSize.TotalSizeToClean));
		}
		_outputWriter?.Line('_', 48);
	}

	private void DisplayTotals()
	{
		_outputWriter?.NewLine();
		_outputWriter?.LineWithInsert('=', "[ TOTAL ]", 48);
		_dataDisplay?.DisplayConclusion(new SizeInfo(_totalProcessingInfo.TotalSize), new SizeInfo(_totalProcessingInfo.TotalSizeToClean));
		_outputWriter?.Line('=', 48);
		_outputWriter?.NewLine();
	}

	private bool DisplaySummaryAndConfirmCleaning() 
		=> _userInputHandler != null && _userInputHandler.ConfirmCleaning(_settings.FolderCleaningService is MockFolderCleaningService ? "Mock" : "Clean");

	private void CleanFolders()
	{
		_outputWriter?.WriteLineInColor("\nCleaning...", Color.Yellow);
		
		_folderCleaningService?.Clean(_totalProcessingInfo);

		_outputWriter?.WriteLineInColor("\nCleaning Completed!", Color.Yellow);
		_outputWriter?.WriteLineInColor($"Total size: {new SizeInfo(_totalProcessingInfo.TotalSize)}", Color.Red);
		_outputWriter?.WriteLineInColor($"Total cleaned: {new SizeInfo(_actyualDeletedSizeTotal)}", Color.Green);
		_outputWriter?.WriteLineInColor($"Total remaining: {new SizeInfo(_totalProcessingInfo.TotalSize - _actyualDeletedSizeTotal)}", Color.Yellow);
	}
	
	private void AddToActualTotal(SizeInfo sizeInfo) 
		=> _actyualDeletedSizeTotal += sizeInfo.Bytes;
}