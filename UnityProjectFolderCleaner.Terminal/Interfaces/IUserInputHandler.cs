namespace UnityProjectFolderCleaner.Terminal.Interfaces;

public interface IUserInputHandler
{
	IEnumerable<string> GetTargetFolders();
	bool ConfirmCleaning();
}