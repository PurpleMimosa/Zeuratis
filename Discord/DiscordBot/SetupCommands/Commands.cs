namespace Zeuratis;

public class SetupCommands : ModuleBase<SocketCommandContext>
{
    [Command(">BeginSetup")]
    public async Task BeginSetup(int Id, int defaultId = 0)
    {
        if ((defaultId == ZeuratisSetup.DefaultId || defaultId == 0) && !Settings.GetConfigureState(false))
        {
            await ReplyAsync($"Initializing setup: {Id}");
            await SetupDiscord(Id);

            if(DataMgr.ErrAlreadyThrown)
                DataMgr.ErrAlreadyThrown = false;
        }
    }

    [Command(">ScanUnconfigured")]
    public async Task ScanUnconfigured()
    {
        if (!Settings.GetConfigureState())
        {
            await ReplyAsync($"Not configured IP: {Net.GetPublicIP().Result}, DefaultId: {ZeuratisSetup.DefaultId}");
        }
    }

    [Command(">Ping")]
    public async Task Ping(int Id = 0, int defaultId = 0)
    {
        if (Id == Settings.GetId() || defaultId == ZeuratisSetup.DefaultId || (Id + defaultId) == 0)
        {
            await ReplyAsync($"Online: IP: {Net.GetPublicIP().Result}, ConfiguredState: {Settings.GetConfigureState()}");
        }
    }

    public async Task SetupDiscord(int Id)
    {
        ITextChannel Channel = await Context.Guild.CreateTextChannelAsync(Id.ToString());
        await Channel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new(viewChannel: PermValue.Deny));

        Settings.RegisterSettings(Id, true);
    }
}
