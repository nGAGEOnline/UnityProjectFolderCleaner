using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;
using UnityProjectFolderCleaner.Processing;

namespace UnityProjectFolderCleaner.Services;

public class UnityProjectProcessingService : IProcessingService<UnityProjectProcessingInfo>
{
	private readonly DirectoryInfo _unityFolder;
	private readonly IDataDisplay _dataDisplay;
	private readonly IOutputWriter _outputWriter;
	private readonly IFolderTypeService _folderTypeService;
    
	public UnityProjectProcessingService(DirectoryInfo unityFolder, IDataDisplay dataDisplay, IOutputWriter outputWriter, IFolderTypeService folderTypeService)
	{
		_unityFolder = unityFolder;
		_dataDisplay = dataDisplay;
		_outputWriter = outputWriter;
		_folderTypeService = folderTypeService;
	}

	public UnityProjectProcessingInfo Process()
	{
		var unityProject = new UnityProjectInfo(_unityFolder.FullName);
		var unityProjectProcessingInfo = new UnityProjectProcessingInfo(unityProject);

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
				unityProjectProcessingInfo.Children.Add(directoryInfo);
			}

			_dataDisplay.DisplayFolderSizeInfo(directoryInfo, new SizeInfo(size), recommendation);
		}

		unityProjectProcessingInfo.Size = folderSize;
		unityProjectProcessingInfo.SizeToClean = folderSizeToClean;
  
		_outputWriter.LineWithPrefix('-', '\t');
		_dataDisplay.DisplayConclusion(new SizeInfo(folderSize), new SizeInfo(folderSizeToClean));

		return unityProjectProcessingInfo;
	}
}