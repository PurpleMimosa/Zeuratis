namespace Zeuratis;

class Error
{
    public static async Task Log(String msg, String title)
    {
        DiscWebHook DiscWH = new(Credentials.ErrorWHToken);
        await DiscWH.SendMessageAsync("Error", msg, title, "4000000", true);
    }
}
