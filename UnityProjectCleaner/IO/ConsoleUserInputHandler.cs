using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.IO
{
	public class ConsoleUserInputHandler : BaseInputHandler, IUserInputHandler
    {
	    public ConsoleUserInputHandler(IOutputWriter outputWriter) 
		    : base(outputWriter) { }

	    public IEnumerable<string> GetTargetFolders()
        {
            var targetFolders = new List<string>();
            var isFirstPath = true;

            OutputWriter.WriteLineInColor("Enter the target folder paths one at a time. To finish, enter an empty line.", Color.White);

            while (true)
            {
	            OutputWriter.WriteInColor("Enter Folder Path: ", Color.White);
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    if (targetFolders.Count == 0)
                    {
	                    OutputWriter.WriteLineInColor("At least one target folder is required.", Color.White);
                        continue;
                    }
                    break;
                }

                if (Directory.Exists(input))
                {
                    targetFolders.Add(input);

                    if (isFirstPath)
                    {
                        isFirstPath = false;
                        OutputWriter.WriteLineInColor("Enter an empty line to finish adding target folders.", Color.Yellow);
                    }
                }
                else
                {
	                OutputWriter.WriteLineInColor("The provided path does not exist. Please check for typos and try again.", Color.Red);
                }
            }

            return targetFolders;
        }

        public bool ConfirmCleaning()
        {
	        DisplayCleaningConfirmationPrompt();
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
            } while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

            Console.WriteLine();

            return keyInfo.Key is ConsoleKey.Y or ConsoleKey.Enter;
        }
    }
}
