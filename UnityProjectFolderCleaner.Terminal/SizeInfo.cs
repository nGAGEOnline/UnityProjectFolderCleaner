namespace UnityProjectFolderCleaner.Terminal;

public class SizeInfo
{
	public long Size { get; }

	public SizeInfo(long size) => Size = size;

	public override string ToString()
	{
		const long oneKb = 1024;
		const long oneMb = oneKb * 1024;
		const long oneGb = oneMb * 1024;

		return Size switch
		{
			>= oneGb => $"{(double)Size / oneGb:F2} GB",
			>= oneMb => $"{(double)Size / oneMb:F2} MB",
			>= oneKb => $"{(double)Size / oneKb:F2} KB",
			_ => $"{Size} bytes"
		};
	}
	
	public static long Calculate(DirectoryInfo directory)
	{
		var files = directory.GetFiles();
		var subDirectories = directory.GetDirectories();

		var size = files.Sum(file => file.Length);
		size += subDirectories.Sum(Calculate);

		return size;
	}
}