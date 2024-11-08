using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JamesBot.Zork;
using Newtonsoft.Json;

namespace JamesBot.Commands;

public class Base : BaseCommandModule
{

    [Command("zork")]
    public async Task Zork(CommandContext commandContext, params string[] args)
    {
        if (args[0] == "help")
        {
            await commandContext.Channel.SendMessageAsync("");
            return;
        }

        if (args[0] == "action")
        {
            if (args.Length < 2)
            {
                return;
            }
            var userdata = await ZorkClient.Action(args[1], args[2], string.Join(' ', args.Skip(2)));
            await commandContext.Channel.SendMessageAsync("```" + Environment.NewLine +
                userdata?.userProfile.userEmail + Environment.NewLine +
                userdata?.userProfile.lastGame + Environment.NewLine +
                // Environment.NewLine +
                // userdata.titleInfo + Environment.NewLine +
                Environment.NewLine +
                userdata?.cmdOutput + Environment.NewLine +
                userdata?.lookOutput.firstLine + Environment.NewLine
            + "```");
            return;
        }
        if (args[0] == "start")
        {
            if (args.Length < 2)
            {
                return;
            }
            var userdata = await ZorkClient.Start(args[1], args[2]);
            await commandContext.Channel.SendMessageAsync("```" + Environment.NewLine +
                userdata?.userProfile.userEmail + Environment.NewLine +
                userdata?.userProfile.lastGame + Environment.NewLine +
                // Environment.NewLine +
                // userdata.titleInfo + Environment.NewLine +
                Environment.NewLine +
                userdata?.firstLine + Environment.NewLine
            + "```");
            return;
        }

        if (args[0] == "user")
        {
            if (args.Length < 2)
            {
                return;
            }
            var userdata = await ZorkClient.User(args[1]);
            await commandContext.Channel.SendMessageAsync("```" +
                JsonConvert.SerializeObject(userdata, formatting: Formatting.Indented)
                + "```"
            );
            return;
        }
    }


    [Command("start")]
    public async Task Start(CommandContext commandContext)
    {
        var users = (await commandContext.Guild.GetAllMembersAsync()).ToList();
        users.RemoveAll(x => x.DisplayName == "JamesBot");
        while (true)
        {
            foreach (var item in users)
            {
                //var probability = new System.Random().Next(100);
                /*if(probability > 25)
                    continue;*/
                var msg = await new DiscordMessageBuilder()
                    .WithContent(item.Mention).WithTTS(true)
                    .SendAsync(commandContext.Channel);
                Thread.Sleep(120000);
            }
        }
    }

    [Command("teste")]
    public async Task Teste(CommandContext commandContext)
    {
        DateTime? date = null;
        var a = (await commandContext.Guild.GetChannelsAsync()).ToList();
        var users = (await commandContext.Guild.GetAllMembersAsync()).ToList();
        var b = a.Find(x => x.Name == commandContext.Channel.Name);
        if (b == null)
            return;
        while (true)
        {
            var c = (await b.GetMessagesAsync()).ToList().OrderBy(x => x.Timestamp.UtcDateTime);
            if (date == null)
                date = DateTime.Parse(c.Last().Timestamp.UtcDateTime.ToString()).AddSeconds(10);
            if (date < c.Last().Timestamp.UtcDateTime)
            {
                date = DateTime.Parse(c.Last().Timestamp.UtcDateTime.ToString()).AddSeconds(10);
                await commandContext.Channel.SendMessageAsync("Novo");
            }
        }
    }
}