using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Services;

public class MockFolderCleaningService : BaseCleaningService, IFolderCleaningService
{
	public event Action<SizeInfo> OnDeletedNotifySizeTotal;
	
	public MockFolderCleaningService(IOutputWriter outputWriter) 
		: base(outputWriter) { }

	public void Clean(TotalProcessingInfo foldersToClean)
	{
		foreach (var unityProjectInfo in foldersToClean.Children.SelectMany(targetFolderInfo => targetFolderInfo.Children))
		{

			OutputWriter.WriteInColor("\n[MOCK] ", Color.DarkCyan);
			OutputWriter.WriteInColor("Cleaning: ", Color.White);
			OutputWriter.WriteLineInColor(unityProjectInfo.Target.Name, Color.Yellow);

			Thread.Sleep(Random.Shared.Next(20, 100));

			foreach (var directoryInfo in unityProjectInfo.Children)
			{
				var sizeInfo = new SizeInfo(directoryInfo);
				OutputWriter.WriteInColor("[MOCK] ", Color.DarkCyan);
				OutputWriter.WriteInColor("Deleting: ", Color.Gray);
				OutputWriter.WriteInColor($"{directoryInfo.Parent.Name}\\{directoryInfo.Name}", Color.White);

				OutputWriter.WriteInColor(" (", Color.Gray);
				OutputWriter.WriteInColor(sizeInfo.ToString(), Color.Yellow);
				OutputWriter.WriteInColor(")", Color.Gray);

				Thread.Sleep(Random.Shared.Next(20, 100));

				if (directoryInfo.Name == ".git")
				{
					OutputWriter.WriteStatus("FAILED", Color.Red);
					OutputWriter.WriteError(directoryInfo, "Unable to delete", "Access denied.", true);
					continue;
				}
				OutputWriter.WriteStatus("OK", Color.Green);

				OnDeletedNotifySizeTotal?.Invoke(sizeInfo);
			}
		}
		OutputWriter.NewLine();
	}
}