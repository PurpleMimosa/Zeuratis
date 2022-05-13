namespace Zeuratis;

public class BotCommands : ModuleBase<SocketCommandContext>
{
    [Command("GetId")]
    public async Task GetBotId()
    {
        await ReplyAsync(Settings.GetId().ToString());
    }
}
