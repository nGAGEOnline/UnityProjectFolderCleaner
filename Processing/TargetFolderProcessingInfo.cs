using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal.Processing;

public class TargetFolderProcessingInfo : ProcessingInfo, IProcessingInfoWithChildren<UnityProjectProcessingInfo, DirectoryInfo>
{
	public DirectoryInfo Target { get; init; }
	public List<UnityProjectProcessingInfo> Children { get; set; } = new();
	
	public TargetFolderProcessingInfo(DirectoryInfo target) => Target = target;
	
	protected override long GetTotalSize() => Children.Sum(x => x.Size);
	protected override long GetTotalSizeToClean() => Children.Sum(x => x.SizeToClean);
}