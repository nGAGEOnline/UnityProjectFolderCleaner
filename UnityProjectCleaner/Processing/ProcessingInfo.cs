using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Processing;

public abstract class ProcessingInfo : IProcessingInfo
{
	public long Size { get; set; }
	public long SizeToClean { get; set; }
	
	public long TotalSize => GetTotalSize();
	public long TotalSizeToClean => GetTotalSizeToClean();
	
	protected abstract long GetTotalSize();
	protected abstract long GetTotalSizeToClean();
}