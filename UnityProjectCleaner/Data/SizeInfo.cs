namespace UnityProjectFolderCleaner.Data;

public readonly struct SizeInfo(long bytes)
{
	public long Bytes { get; } = bytes;

	public SizeInfo(DirectoryInfo directory) 
		: this(Calculate(directory)) { }

	public override string ToString()
	{
		const long oneKb = 1024;
		const long oneMb = oneKb * 1024;
		const long oneGb = oneMb * 1024;

		return Bytes switch
		{
			>= oneGb => $"{(double)Bytes / oneGb:F2} GB",
			>= oneMb => $"{(double)Bytes / oneMb:F2} MB",
			>= oneKb => $"{(double)Bytes / oneKb:F2} KB",
			_ => $"{Bytes} bytes"
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