namespace UnityProjectFolderCleaner.Interfaces;

public interface IProcessingInfoWithChildren<T> : IProcessingInfo where T : IProcessingInfo
{
	List<T> Children { get; set; }
}

public interface IProcessingInfoWithChildren<T, T2> : IProcessingInfo<T2>
{
	List<T> Children { get; set; }
}