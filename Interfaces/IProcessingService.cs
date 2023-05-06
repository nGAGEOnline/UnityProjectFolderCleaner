namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IProcessingService<out T>
{
	T Process();
}