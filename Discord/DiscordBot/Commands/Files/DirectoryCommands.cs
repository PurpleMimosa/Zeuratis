namespace Zeuratis;

[SupportedOSPlatform("windows")]
public class DirectoryCommands : ModuleBase<SocketCommandContext>
{
    [Command("CopyDir")]
    public async Task CopyDir(String path, String desPath)
    {
        if (!Directory.Exists(desPath))
            Directory.CreateDirectory(desPath);

        String[] files = Directory.GetFiles(path);

        foreach (String file in files)
        {
            String name = Path.GetFileName(file);
            String dest = Path.Combine(desPath, name);
            File.Copy(file, dest);
        }

        String[] dirs = Directory.GetDirectories(path);

        foreach (String dir in dirs)
        {
            String name = Path.GetFileName(dir);
            String dest = Path.Combine(desPath, name);
            await CopyDir(dir, dest);
        }
    }

    [Command("MoveDir")]
    public async Task MoveDir(String path, String desPath)
    {
        Directory.Move(path, desPath);
        await ReplyAsync("Folder was moved");
    }

    [Command("DeleteDir")]
    public async Task DeleteDir(String path)
    {
        Directory.Delete(path);
        await ReplyAsync($"Deleted Directory: {path}");
    }
}
