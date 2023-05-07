using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Data;

public class TargetProcessingInfo : ProcessingInfo, IProcessingInfoWithChildren<UnityProjectProcessingInfo, DirectoryInfo>
{
	public DirectoryInfo Target { get; init; }
	public List<UnityProjectProcessingInfo> Children { get; set; } = new();
	
	public TargetProcessingInfo(DirectoryInfo target) => Target = target;
	
	protected override long GetTotalSize() => Children.Sum(x => x.Size);
	protected override long GetTotalSizeToClean() => Children.Sum(x => x.SizeToClean);
}