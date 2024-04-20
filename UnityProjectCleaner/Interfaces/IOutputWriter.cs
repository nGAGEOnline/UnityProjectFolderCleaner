using UnityProjectFolderCleaner.Enums;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IOutputWriter
{
	void NewLine(int count = 1);
	void Clear();

	bool Confirm(string prompt);
	
	void Left(int left);
	void Top(int top = 0);
	void Position(int left, int top);
	
	void WriteStatus(string text, Color color);
	void WriteError(DirectoryInfo directoryInfo, string message, string error, bool isMock = false);
	void WriteError(DirectoryInfo directoryInfo, string message, Exception ex, bool isMock = false);
	
	void WriteInColor(string text, Color color);
	void WriteLineInColor(string text, Color color);
	
	void Line(char character, int length = 40);
	void LineWithPrefix(char character, char prefix, int length = 40);
	void LineWithPrefixAndSuffix(char character, char prefix, char suffix, int length = 40);
	void LineWithInsert(char character, string insert, int length = 40);
}
