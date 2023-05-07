using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Data;

public class TotalProcessingInfo : IProcessingInfoWithChildren<TargetProcessingInfo>
{
	public long Size { get; set; }
	public long SizeToClean { get; set; }
	public List<TargetProcessingInfo> Children { get; set; } = new();

	public long TotalSize => GetTotalSize();
	public long TotalSizeToClean => GetTotalSizeToClean();
	
	private long GetTotalSize() => Children.Sum(x => x.Size);
	private long GetTotalSizeToClean() => Children.Sum(x => x.SizeToClean);
}