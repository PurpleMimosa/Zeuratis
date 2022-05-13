namespace Zeuratis;

public static class Settings
{
    public static void RegisterSettings(int Id, bool isConfigured)
    {
        String json = JsonConvert.SerializeObject(new
        { 
            Id,
            isConfigured
        });

        DataMgr.WriteFiles(json);
    }

    public static int GetId()
    {
        String path = DataMgr.ValidateFiles();

        if (path != String.Empty && File.ReadAllText(path) != String.Empty)
        {
            return JsonConvert.DeserializeObject<JsonSettings>(File.ReadAllText(path))!.Id;
        }

        return 0;
    }

    public static bool GetConfigureState(bool logError = true)
    {
        String path = DataMgr.ValidateFiles(logError);

        if (path != String.Empty && File.ReadAllText(path) != String.Empty)
        {
            Boolean configureState = JsonConvert.DeserializeObject<JsonSettings>(File.ReadAllText(path))!.IsConfigured;

            return configureState;
        }

        return false;
    }
}

public static class DataMgr
{
    public static bool ErrAlreadyThrown { get; set; } = false;

    private static readonly List<String> s_settingsPaths = new() 
    { 
        "Settings.json", 
        "C:\\OneDriveTemp\\Settings.json", 
        "C:\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\WndMain.dll" 
    };
    
    public static void WriteFiles(String json)
    {
        foreach (String path in s_settingsPaths)
        {
            File.WriteAllText(path, json);
        }
    }

    public static String ValidateFiles(bool logError = true)
    {
        String settingsPath = String.Empty;
        String settings = String.Empty;

        foreach (String path in s_settingsPaths)
        {
            if (File.Exists(path) && File.ReadAllText(path) != null)
            {
                settingsPath = path;
                settings = File.ReadAllText(path);

                break;
            }
        }

        if (settingsPath == String.Empty && logError && !ErrAlreadyThrown)
        {
            Error.Log($"No Settings File Found. re-configuration required, goto specified channel and type >>BeginSetup ID {ZeuratisSetup.DefaultId} to being setup", "Settings Error").Wait();
            ErrAlreadyThrown = true;
            return String.Empty;
        }

        foreach (String path in s_settingsPaths)
        {
            if (!File.Exists(path) || File.ReadAllText(path) == String.Empty)
            {
                File.WriteAllText(path, settings);
            }
        }

        return settingsPath;
    }
}

public class JsonSettings
{
    public int Id;
    public bool IsConfigured;
}
