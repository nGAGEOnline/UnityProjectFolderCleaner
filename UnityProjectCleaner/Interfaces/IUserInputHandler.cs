namespace UnityProjectFolderCleaner.Interfaces;

public interface IUserInputHandler
{
	IEnumerable<string> GetTargetFolders();
	bool ConfirmCleaning();
}