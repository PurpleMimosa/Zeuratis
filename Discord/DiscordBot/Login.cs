namespace Zeuratis;

partial class DiscordBot
{
    private static DiscordSocketClient? s_Client;
    private static CommandService? s_Commands;
    private static IServiceProvider? s_Services;

    public static async Task Login()
    {
        s_Client = new();
        s_Commands = new();

        s_Services = new ServiceCollection()
            .AddSingleton(s_Client)
            .AddSingleton(s_Commands)
            .BuildServiceProvider();

        string Token = Credentials.BotToken;

        s_Client.Log += ClientLog;

        await RegisterCommandsAsync();

        await s_Client.LoginAsync(TokenType.Bot, Token);

        await s_Client.StartAsync();

        await Task.Delay(-1);
    }

    private static Task ClientLog(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    public static async Task RegisterCommandsAsync()
    {
        s_Client!.MessageReceived += HandleCommandAsync;
        await s_Commands!.AddModulesAsync(Assembly.GetEntryAssembly(), s_Services);
    }

    private static async Task HandleCommandAsync(SocketMessage arg)
    {
        var message = arg as SocketUserMessage;
        SocketCommandContext Context = new(s_Client, message);

        if (message!.Author.IsBot) 
            return;

        int argPos = 0;
        if (message.HasStringPrefix(">", ref argPos) && (Context.Channel.Name == Settings.GetId().ToString() || Context.Channel.Name == "zeuratis-setup"))
        {
            IResult result = await s_Commands!.ExecuteAsync(Context, argPos, s_Services);

            if (!result.IsSuccess) 
                Console.WriteLine(result.ErrorReason);

            if (result.Error.Equals(CommandError.UnmetPrecondition)) 
                await message.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
