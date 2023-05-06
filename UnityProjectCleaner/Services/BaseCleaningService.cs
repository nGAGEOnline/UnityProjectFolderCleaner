using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Services;

public class BaseCleaningService
{
	protected readonly IOutputWriter OutputWriter;

	protected BaseCleaningService(IOutputWriter outputWriter) => OutputWriter = outputWriter;
}
