using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Data;

public class UnityProjectProcessingInfo : ProcessingInfo, IProcessingInfoWithChildren<DirectoryInfo, UnityProjectInfo>
{
	public UnityProjectInfo Target { get; init; }
	public List<DirectoryInfo> Children { get; set; } = new();
	
	public UnityProjectProcessingInfo(UnityProjectInfo target) => Target = target;

	protected override long GetTotalSize() => Target.GetDirectories().Sum(SizeInfo.Calculate);
	protected override long GetTotalSizeToClean() => Children.Sum(SizeInfo.Calculate);
}