using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Processing;

public class TotalProcessingInfo : IProcessingInfoWithChildren<TargetFolderProcessingInfo>
{
	public long Size { get; set; }
	public long SizeToClean { get; set; }
	public List<TargetFolderProcessingInfo> Children { get; set; } = new();

	public long TotalSize => GetTotalSize();
	public long TotalSizeToClean => GetTotalSizeToClean();
	
	private long GetTotalSize() => Children.Sum(x => x.Size);
	private long GetTotalSizeToClean() => Children.Sum(x => x.SizeToClean);
}