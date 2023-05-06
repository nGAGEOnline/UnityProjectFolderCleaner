using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;
using UnityProjectFolderCleaner.Processing;

namespace UnityProjectFolderCleaner.Services;

public class FolderCleaningService : BaseCleaningService, IFolderCleaningService
{
	public FolderCleaningService(IOutputWriter outputWriter) 
		: base(outputWriter) { }

	public void Clean(TotalProcessingInfo foldersToClean)
	{
		foreach (var unityProjectInfo in foldersToClean.Children.SelectMany(targetFolderInfo => targetFolderInfo.Children))
		{
			OutputWriter.WriteInColor("\nCleaning: ", Color.White);
			OutputWriter.WriteLineInColor(unityProjectInfo.Target.Name, Color.Yellow);

			foreach (var directoryInfo in unityProjectInfo.Children)
			{
				OutputWriter.WriteInColor("Deleting: ", Color.Gray);
				OutputWriter.WriteLineInColor(directoryInfo.FullName, Color.Red);

				directoryInfo.Delete(true);
			}
		}
		OutputWriter.NewLine();
	}

}