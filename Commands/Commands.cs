using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace JamesBot.Commands;

public class Base : BaseCommandModule
{
    [Command("start")]
    public static async Task Start(CommandContext commandContext)
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
    public static async Task Teste(CommandContext commandContext)
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