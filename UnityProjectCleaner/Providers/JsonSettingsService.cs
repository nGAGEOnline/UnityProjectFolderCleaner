using Newtonsoft.Json;

namespace UnityProjectFolderCleaner.Providers;

public class JsonSettingsService
{
	private const string SettingsFilePath = "settings.json";

	public static bool SettingsExist() => File.Exists(SettingsFilePath);

	public static CleanerSettings? LoadSettings()
	{
		if (!SettingsExist()) return null;
		string json = File.ReadAllText(SettingsFilePath);
		return JsonConvert.DeserializeObject<CleanerSettings>(json);
	}

	public static void SaveSettings(CleanerSettings settings)
	{
		string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
		File.WriteAllText(SettingsFilePath, json);
	}
}
