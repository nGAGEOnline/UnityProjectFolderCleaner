using UnityProjectFolderCleaner.Enums;
using UnityProjectFolderCleaner.Interfaces;

namespace UnityProjectFolderCleaner.Helpers
{
	public class ConsoleOutputWriter : IOutputWriter
	{
		public void Clear() => Console.Clear();
		public void WriteInColor(string text, Color color)
		{
			var consoleColor = ConvertToConsoleColor(color);
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = consoleColor;
			Console.Write(text);
			Console.ForegroundColor = previousColor;
		}
		public void WriteLineInColor(string text, Color color)
		{
			var consoleColor = ConvertToConsoleColor(color);
			var previousColor = Console.ForegroundColor;
			Console.ForegroundColor = consoleColor;
			Console.WriteLine(text);
			Console.ForegroundColor = previousColor;
		}
		
		public void NewLine(int count = 1)
		{
			count = Math.Clamp(count, 1, Console.WindowHeight);
			for (var i = 0; i < count; i++)
				Console.WriteLine();
		}
		
		public bool Confirm(string prompt)
		{
			DisplayCleaningConfirmationPrompt(prompt);
			ConsoleKeyInfo keyInfo;

			do
			{
				keyInfo = Console.ReadKey(true);
			} while (keyInfo.Key != ConsoleKey.Y && keyInfo.Key != ConsoleKey.N && keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

			Console.WriteLine();

			return keyInfo.Key is ConsoleKey.Y or ConsoleKey.Enter;
		}

		private void DisplayCleaningConfirmationPrompt(string prompt, string prefix = "")
		{
			if (string.IsNullOrEmpty(prompt))
				return;
			
			if (!string.IsNullOrEmpty(prefix))
				WriteInColor($"{prefix} ", Color.DarkCyan);
			
			WriteInColor($"{prompt} ", Color.White);
			WriteInColor("\t[Y]es", Color.Green);
			WriteInColor("/", Color.White);
			WriteInColor("[N]o ", Color.Red);
		}
		
		public void Left(int left = 0) => Console.CursorLeft = left;
		public void Top(int top = 0) => Console.CursorTop = top;
		public void Position(int left, int top) => Console.SetCursorPosition(left, top);
		
		public void Line(char character, int length = 40) 
			=> Console.WriteLine(new string(character, length));

		public void LineWithPrefix(char character, char prefix, int length = 40) 
			=> Console.WriteLine($"{prefix}{new string(character, length)}");

		public void LineWithPrefixAndSuffix(char character, char prefix, char suffix, int length = 40) 
			=> Console.WriteLine($"{prefix}{new string(character, length)}{suffix}");

		public void LineWithInsert(char character, string insert, int length = 40)
		{
			Console.Write(new string(character, length));
			var center = length / 2;
			var targetFolderName = $"{insert}";
			var start = center - (targetFolderName.Length / 2);
			Console.CursorLeft = start;
			Console.WriteLine(targetFolderName);
			Console.CursorLeft = length + 1;
			Console.WriteLine();
		}
		
		public void WriteStatus(string text, Color color)
		{
			WriteInColor(" [", Color.Gray);
			WriteInColor($"{text}", color);
			WriteInColor("]\n", Color.Gray);
		}
	
		public void WriteError(DirectoryInfo directoryInfo, string message, string error, bool isMock = false)
		{
			ErrorBase(directoryInfo, message, isMock);
			WriteLineInColor($" ({error})", Color.Red);
		}

		public void WriteError(DirectoryInfo directoryInfo, string message, Exception ex, bool isMock = false)
		{
			ErrorBase(directoryInfo, message, isMock);
			WriteLineInColor($" {ex.Message}", Color.Red);
		}

		private void ErrorBase(DirectoryInfo directoryInfo, string message, bool isMock = false)
		{
			Left(0);
			if (isMock)
				WriteInColor("[MOCK] ", Color.DarkCyan);
			WriteInColor("ERROR: ", Color.Red);
			WriteInColor($"{message} ", Color.Gray);
			WriteInColor($"{directoryInfo.FullName}", Color.Yellow);
			WriteInColor(".", Color.Gray);
		}
		
		private static ConsoleColor ConvertToConsoleColor(Color color) 
			=> (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());
	}
}
