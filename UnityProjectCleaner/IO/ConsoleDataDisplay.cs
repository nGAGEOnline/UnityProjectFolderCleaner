using UnityProjectFolderCleaner.Data;
using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.IO
{
    public class ConsoleDataDisplay(IOutputWriter outputWriter) : IDataDisplay
    {
        private const int CURSOR_LEFT = 48;

        public void DisplayUnityProjectTitle(UnityProjectInfo unityProjectInfo)
        {
	        outputWriter.WriteInColor($"\t{unityProjectInfo.Name}", Color.Cyan);
            Console.CursorLeft = CURSOR_LEFT - (unityProjectInfo.UnityVersion.Length + 2);
            outputWriter.WriteInColor("[", Color.White);
            outputWriter.WriteInColor($"{unityProjectInfo.UnityVersion}", Color.DarkCyan);
            outputWriter.WriteLineInColor("]", Color.White);
        }

        public void DisplayFolderSizeInfo(DirectoryInfo directoryInfo, SizeInfo sizeInfo, FolderType type)
        {
	        outputWriter.WriteInColor($"\t{directoryInfo.Name}", Color.White);
            Console.CursorLeft = CURSOR_LEFT - (sizeInfo.ToString().Length + 2);
            outputWriter.WriteInColor("[", Color.White);
            outputWriter.WriteInColor($"{sizeInfo}", type == FolderType.Clean ? Color.Red : Color.Green);
            outputWriter.WriteLineInColor("]", Color.White);
        }

        public void DisplayConclusion(SizeInfo sizeInfo, SizeInfo sizeInfoToClean)
        {
            DisplayFileSizeLine("\tTotal size:", sizeInfo, Color.Yellow);
            DisplayFileSizeLine("\tRecommended to clean:", sizeInfoToClean, Color.Red);

            var afterSize = new SizeInfo(sizeInfo.Bytes - sizeInfoToClean.Bytes);
            DisplayFileSizeLine("\tAfter cleaning:", afterSize, Color.Green);

            Console.WriteLine();
        }

        private void DisplayFileSizeLine(string label, SizeInfo sizeInfo, Color color)
        {
	        outputWriter.WriteInColor(label, Color.White);
            Console.CursorLeft = CURSOR_LEFT - (sizeInfo.ToString().Length + 2);
            outputWriter.WriteInColor("[", Color.White);
            outputWriter.WriteInColor($"{sizeInfo}", color);
            outputWriter.WriteLineInColor("]", Color.White);
        }
    }
}
