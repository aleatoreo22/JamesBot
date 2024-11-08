using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace JamesBot;

public class Bot
{
    public DiscordClient? Client { get; private set; }
    public CommandsNextExtension? Commands { get; private set; }
    private readonly string _token, _prefix, _path;

    public Bot(string token, string prefix, string path)
    {
        _token = token;
        _prefix = prefix;
        _path = path;
    }

    public async Task RunAsync()
    {
        var config = new DiscordConfiguration
        {
            MinimumLogLevel = LogLevel.Debug,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            Token = _token,
            Intents = DiscordIntents.All
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
        Commands.RegisterCommands<Commands.Base>();
        Client.MessageCreated += OnMessageCreated;
        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private async Task OnMessageCreated(DiscordClient sender, MessageCreateEventArgs e)
    {
        if (e.Author.IsBot) return;
        if (e.Message.Content.StartsWith(_prefix))
        {
            var comandText = e.Message.Content[1..];
            if (comandText.Contains(' '))
                comandText = comandText[..comandText.IndexOf(" ")];
            var commands = sender.GetCommandsNext();
            var command = commands.FindCommand(comandText, out _);
            if (command != null)
                return;
            var luaFile = _path + "/LuaCommands/" + comandText + ".lua";
            if (!File.Exists(luaFile))
            {
                await e.Message.RespondAsync("Comando nao reconhecido! `'-'`");
                return;
            }
            var paramters = new List<string>();
            if (e.Message.Content.Length > comandText.Length + 2)
                paramters = e.Message.Content[(comandText.Length + 2)..].Split(' ').ToList();
            paramters.RemoveAll(x => x == "");
            var response = RunLuaScript(luaFile, paramters);
            await e.Message.RespondAsync(response);
        }
    }

    private string RunLuaScript(string luaFile, List<string> paramters)
    {
        try
        {
            string luaPath = "/usr/bin/lua"; 
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = luaPath,
                    Arguments = luaFile + " " + string.Join(' ', paramters),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (!string.IsNullOrEmpty(error))
               throw new Exception(error); 
            return output;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private Task? OnClientReady(DiscordClient o, ReadyEventArgs e)
    {
        return Task.CompletedTask;
    }
}
