using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;

namespace JamesBot;

public class Bot
{
    public DiscordClient? Client { get; private set; }
    public CommandsNextExtension? Commands { get; private set; }
    private string _token, _prefix;
    public Bot(string token, string prefix = "") 
    {
        _token = token;
        _prefix = prefix;
    }

    public async Task RunAsync()
    {
        var config = new DiscordConfiguration
        {
            MinimumLogLevel = LogLevel.Debug,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            Token = _token,
        };

        Client = new DiscordClient(config);
        Client.Ready += OnClientReady;

        var commandsConfig = new CommandsNextConfiguration
        {
            StringPrefixes = new string[] { _prefix },
            EnableMentionPrefix = true,
            EnableDms = true,
            DmHelp = true,
        };

        Commands = Client.UseCommandsNext(commandsConfig);
        Commands.RegisterCommands<JamesBot.Commands.Base>();
        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private Task? OnClientReady(DiscordClient o, ReadyEventArgs e)
    {                
        return Task.CompletedTask;
    }
}