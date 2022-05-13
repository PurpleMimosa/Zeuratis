namespace Zeuratis;

public class DiscWebHook
{
    private static readonly HttpClient s_webHook = new();

    private readonly String _webHookToken;

    public DiscWebHook(String whToken)
    {
        _webHookToken = whToken;
    }

    public async Task SendMessageAsync(String username, String description, String title, String color, bool withTimeStamp = false)
    {
        description += withTimeStamp ?
                    "\n\n TimeStamp: " + DateTime.Now.ToString() : "";

        String json = JsonConvert.SerializeObject(new
        {
            username,
            embeds = new[]
            {
                new
                {
                    description,
                    title,
                    color
                }
            }
        });

        StringContent webHookData = new(json);

        webHookData.Headers.ContentType = new("application/json");
        webHookData.Headers.ContentLength = json.Length;

        await s_webHook.PostAsync(_webHookToken, webHookData).ConfigureAwait(false);
    }

    public async Task SendFileAsync(String filePath)
    {
        MultipartFormDataContent form = new();
        ByteArrayContent fileContent = new(await File.ReadAllBytesAsync(filePath));

        fileContent.Headers.ContentType = new("multipart/form-data");
        form.Add(fileContent, "file", Path.GetFileName(filePath));

        await s_webHook.PostAsync(_webHookToken, form).ConfigureAwait(false);
    }
}
