namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IProcessingInfo
{
	long Size { get; set; }
	long SizeToClean { get; set; }
	
	public long TotalSize { get; }
	public long TotalSizeToClean { get; }
}

public interface IProcessingInfo<T> : IProcessingInfo
{
	T Target { get; init; }
}