using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Services;

public class TargetFolderProcessingService : IProcessingService<TargetProcessingInfo>
{
	private readonly string _targetFolder;
	private readonly IDataDisplay? _dataDisplay;
	private readonly IOutputWriter? _outputWriter;
	private readonly IFolderTypeService _folderTypeService;

	public TargetFolderProcessingService(string targetFolder, IDataDisplay? dataDisplay, IOutputWriter? outputWriter, IFolderTypeService folderTypeService)
	{
		_targetFolder = targetFolder;
		_dataDisplay = dataDisplay;
		_outputWriter = outputWriter;
		_folderTypeService = folderTypeService;
	}

	public TargetProcessingInfo Process()
	{
		var targetInfo = new DirectoryInfo(_targetFolder);
		var unityFolders = targetInfo.GetDirectories();
		var targetFolderProcessingInfo = new TargetProcessingInfo(targetInfo);

		foreach (var unityFolder in unityFolders)
		{
			var unityProject = new UnityProjectInfo(unityFolder.FullName);
			if (!unityProject.HasSolutionFiles())
				continue;
			
			var unityProjectProcessingService = new UnityProjectProcessingService(unityFolder, _dataDisplay, _outputWriter, _folderTypeService);
			var unityProjectProcessingInfo = unityProjectProcessingService.Process();
			targetFolderProcessingInfo.Children.Add(unityProjectProcessingInfo);
			targetFolderProcessingInfo.Size += unityProjectProcessingInfo.Size;
			targetFolderProcessingInfo.SizeToClean += unityProjectProcessingInfo.SizeToClean;
		}
		
		_outputWriter.LineWithInsert('=', $"[ {_targetFolder} ]", 48);
		_dataDisplay.DisplayConclusion(new SizeInfo(targetFolderProcessingInfo.Children.Sum(x => x.Size)), new SizeInfo(targetFolderProcessingInfo.Children.Sum(x => x.SizeToClean)));
		_outputWriter.Line('=', 48);

		return targetFolderProcessingInfo;
	}
}