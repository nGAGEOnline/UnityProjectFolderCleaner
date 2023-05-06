﻿using UnityProjectFolderCleaner.Terminal.Enums;
using UnityProjectFolderCleaner.Terminal.Helpers;
using UnityProjectFolderCleaner.Terminal.Interfaces;

namespace UnityProjectFolderCleaner.Terminal
{
    public class ConsoleDataDisplay : IDataDisplay
    {
        private const int CURSOR_LEFT = 48;

        private readonly IOutputWriter _outputWriter;

        public ConsoleDataDisplay(IOutputWriter outputWriter) 
	        => _outputWriter = outputWriter;

        public void DisplayUnityProjectTitle(UnityProject unityProject)
        {
	        _outputWriter.WriteInColor($"\t{unityProject.Name}", Color.White);
            Console.CursorLeft = CURSOR_LEFT - (unityProject.UnityVersion.Length + 2);
            _outputWriter.WriteInColor("[", Color.White);
            _outputWriter.WriteInColor($"{unityProject.UnityVersion}", Color.Cyan);
            _outputWriter.WriteLineInColor("]", Color.White);
        }

        public void DisplayFolderSizeInfo(DirectoryInfo directoryInfo, SizeInfo sizeInfo, FolderType type)
        {
	        _outputWriter.WriteInColor($"\t{directoryInfo.Name}", Color.White);
            Console.CursorLeft = CURSOR_LEFT - (sizeInfo.ToString().Length + 2);
            _outputWriter.WriteInColor("[", Color.White);
            _outputWriter.WriteInColor($"{sizeInfo}", type == FolderType.Clean ? Color.Red : Color.Green);
            _outputWriter.WriteLineInColor("]", Color.White);
        }

        public void DisplayConclusion(SizeInfo sizeInfo, SizeInfo sizeInfoToClean)
        {
            DisplayFileSizeLine("\tTotal size:", sizeInfo, Color.Yellow);
            DisplayFileSizeLine("\tRecommended to clean:", sizeInfoToClean, Color.Red);

            var afterSize = new SizeInfo(sizeInfo.Size - sizeInfoToClean.Size);
            DisplayFileSizeLine("\tAfter cleaning:", afterSize, Color.Green);

            Console.WriteLine();
        }

        private void DisplayFileSizeLine(string label, SizeInfo sizeInfo, Color color)
        {
	        _outputWriter.WriteInColor(label, Color.White);
            Console.CursorLeft = CURSOR_LEFT - (sizeInfo.ToString().Length + 2);
            _outputWriter.WriteInColor("[", Color.White);
            _outputWriter.WriteInColor($"{sizeInfo}", color);
            _outputWriter.WriteLineInColor("]", Color.White);
        }
    }
}
