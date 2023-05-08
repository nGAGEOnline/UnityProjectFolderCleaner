using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.VisualBasic.FileIO;
using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Services;

public class FolderCleaningService : BaseCleaningService, IFolderCleaningService
{
	public event Action<SizeInfo> OnDeletedNotifySizeTotal;

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
	            var sizeInfo = new SizeInfo(directoryInfo);
	            OutputWriter.WriteInColor("Deleting: ", Color.Gray);
	            OutputWriter.WriteInColor($"{directoryInfo.Parent?.Name}\\{directoryInfo.Name}", Color.White);

	            OutputWriter.WriteInColor(" (", Color.Gray);
	            OutputWriter.WriteInColor(sizeInfo.ToString(), Color.Yellow);
	            OutputWriter.WriteInColor(")", Color.Gray);

                try
                {
	                if (directoryInfo.Name == ".git")
	                {
		                var directorySecurity = directoryInfo.GetAccessControl();
		                var identity = WindowsIdentity.GetCurrent();
		                var fileSystemAccessRule = new FileSystemAccessRule(identity.Name, FileSystemRights.FullControl, AccessControlType.Allow);
		                directorySecurity.AddAccessRule(fileSystemAccessRule);
		                directoryInfo.SetAccessControl(directorySecurity);
	                }
	                else
		                FileSystem.DeleteDirectory(directoryInfo.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
	                // directoryInfo.Delete(true);
	                
	                OnDeletedNotifySizeTotal?.Invoke(sizeInfo);
	                
	                OutputWriter.WriteStatus("OK", Color.Green);
                }
                catch (UnauthorizedAccessException)
                {
	                OutputWriter.WriteStatus("FAILED", Color.Red);
	                OutputWriter.WriteError(directoryInfo, "Unable to delete", "Access denied.");
                }
                catch (IOException ex)
                {
	                OutputWriter.WriteStatus("FAILED", Color.Red);
	                OutputWriter.WriteError(directoryInfo, "IO error", ex);
                }
                catch (Exception ex)
                {
	                OutputWriter.WriteStatus("FAILED", Color.Red);
	                OutputWriter.WriteError(directoryInfo, "Unable to delete", ex);
                }
            }
        }
        OutputWriter.NewLine();
    }
}