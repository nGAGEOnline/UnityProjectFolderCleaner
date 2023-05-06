namespace UnityProjectFolderCleaner.Interfaces;

public interface IProcessingService<out T>
{
	T Process();
}