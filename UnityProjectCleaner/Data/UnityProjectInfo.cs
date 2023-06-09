﻿namespace UnityProjectFolderCleaner.Data;

public class UnityProjectInfo
{
	public string Name => _directoryInfo.Name;
	public string UnityVersion { get; private set; }

	private readonly string[] _protectedFolders = { "Assets", "Packages", "ProjectSettings", "UserSettings" };
	private readonly DirectoryInfo _directoryInfo;

	public UnityProjectInfo(string path)
	{
		_directoryInfo = new DirectoryInfo(path);
		UnityVersion = GetUnityVersion();
	}

	public bool IsProtected(string folderName) 
		=> _protectedFolders.Any(x => string.Equals(x, folderName, StringComparison.CurrentCultureIgnoreCase));

	public IEnumerable<DirectoryInfo> GetDirectories() 
		=> _directoryInfo.GetDirectories();

	public bool HasSolutionFiles() 
		=> File.Exists(Path.Combine(_directoryInfo.FullName, $"{_directoryInfo.Name}.sln"));

	private string GetUnityVersion()
	{
		var version = "Unknown";
		var projectVersionFilePath = Path.Combine(_directoryInfo.FullName, "ProjectSettings", "ProjectVersion.txt");

		if (!File.Exists(projectVersionFilePath))
			return version;
		
		var lines = File.ReadAllLines(projectVersionFilePath);
		foreach (var line in lines)
		{
			if (!line.StartsWith("m_EditorVersion:"))
				continue;
				
			version = line["m_EditorVersion:".Length..].Trim();
			break;
		}
		return version;
	}
}