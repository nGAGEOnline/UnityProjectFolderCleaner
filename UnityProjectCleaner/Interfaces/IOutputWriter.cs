using UnityProjectFolderCleaner.Enums;

namespace UnityProjectFolderCleaner.Interfaces;

public interface IOutputWriter
{
	void WriteInColor(string text, Color color);
	void WriteLineInColor(string text, Color color);
	
	void Line(char character, int length = 40);
	void LineWithPrefix(char character, char prefix, int length = 40);
	void LineWithPrefixAndSuffix(char character, char prefix, char suffix, int length = 40);
	void LineWithInsert(char character, string insert, int length = 40);
	
	void NewLine(int count = 1);
	
	void Position(int left);
	void Position(int left, int top);
}
