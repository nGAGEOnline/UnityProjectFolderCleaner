using System.Security.AccessControl;
using System.Security.Principal;
using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

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

            if (unityProjectInfo.Children.Count == 0)
			{
				OutputWriter.WriteInColor("Nothing to clean: ", Color.Red);
				OutputWriter.WriteLineInColor(unityProjectInfo.Target.Name, Color.Yellow);
				continue;
			}
            
            foreach (var directoryInfo in unityProjectInfo.Children)
            {
                OutputWriter.WriteInColor("Deleting: ", Color.Gray);
                OutputWriter.WriteInColor(directoryInfo.FullName, Color.Red);

                try
                {
	                directoryInfo.Delete(true);

	                OutputWriter.WriteInColor(" (", Color.Gray);
	                OutputWriter.WriteInColor(SizeInfo.Calculate(directoryInfo).ToString(), Color.Red);
	                OutputWriter.WriteInColor(")\n", Color.Gray);
                }
				catch (UnauthorizedAccessException)
				{
					OutputError(directoryInfo, "Unable to delete", "Access denied.");
				}
                catch (Exception ex)
                {
	                OutputError(directoryInfo, "Unable to delete", ex);
                }
            }
        }
        OutputWriter.NewLine();
    }

	private void OutputError(DirectoryInfo directoryInfo, string message)
	{
		OutputErrorBase(directoryInfo, message);
		OutputWriter.NewLine();
	}
	private void OutputError(DirectoryInfo directoryInfo, string message, string error)
	{
		OutputErrorBase(directoryInfo, message);
		OutputWriter.WriteLineInColor($" ({error})", Color.Red);
	}

	private void OutputError(DirectoryInfo directoryInfo, string message, Exception ex)
	{
		OutputErrorBase(directoryInfo, message);
		OutputWriter.WriteLineInColor($" {ex.Message}", Color.Red);
	}

	private void OutputErrorBase(DirectoryInfo directoryInfo, string message)
	{
		OutputWriter.Position(0);
		OutputWriter.WriteInColor("ERROR: ", Color.Red);
		OutputWriter.WriteInColor($"{message} ", Color.Gray);
		OutputWriter.WriteInColor($"{directoryInfo.FullName}", Color.Yellow);
		OutputWriter.WriteInColor(".", Color.Gray);
	}
}