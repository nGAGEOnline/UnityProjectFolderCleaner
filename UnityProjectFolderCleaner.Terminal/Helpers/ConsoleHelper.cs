using UnityProjectFolderCleaner.Terminal.Enums;

namespace UnityProjectFolderCleaner.Terminal.Helpers
{
	public class ConsoleOutputWriter : IOutputWriter
	{
		public void Write(string text) => Console.Write(text);
		public void WriteLine(string text) => Console.WriteLine(text);
		public void WriteLine() => Console.WriteLine();
        
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
		private static ConsoleColor ConvertToConsoleColor(Color color) 
			=> (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color.ToString());

		public void NewLine(int count = 1)
		{
			for (var i = 0; i < count; i++)
				Console.WriteLine();
		}

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
	}
}
