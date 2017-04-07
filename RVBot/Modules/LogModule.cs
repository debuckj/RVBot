using Discord;
using Discord.Commands;
using RVBot.Core;
using System.Threading.Tasks;


namespace RVBot
{
    [Group("log")]
    public class LogModule : ModuleBase
    {
        [Command("set"), Summary("Sets the channel to log to.")]
        public async Task SetLogChannelCommand([Remainder, Summary("Sets the channel to log to")] string channelname = null)
        {
            if (channelname == null) { await Context.Channel.SendMessageAsync("Please provide a channel"); return; }
            await Log.SetLogChannel(Context, channelname);
        }

        [Command("send"), Summary("Logs a message to the predefined channel.")]
        public async Task LogMessageCommand([Remainder, Summary("The message to log")] string logmessage = null)
        {
            if (logmessage == null) { await Context.Channel.SendMessageAsync("Please provide something to log"); return; } 
            await Log.LogMessage(Context, logmessage);
        }

        [Command("setauto"), Summary("Automaticly cleans the logchannel of older messages.")]
        public async Task SetAutoLogCleanCommand([Remainder, Summary("Sets the channel to log to")] string yesno = null)
        {
            bool byesno;
            if ((yesno == null) || (!bool.TryParse(yesno, out byesno))) { await Context.Channel.SendMessageAsync("Please provide yes or no"); return; }
            await Log.SetAutoLogClean(Context, byesno); 
        }

        [Command("setautodays"), Summary("Sets the limit for the autocleaner.")]
        public async Task SetAutoLogCleanLimitCommand([Remainder, Summary("Sets the amount of days to keep logs off")] int days = 0)
        {
            if (days>=14) { await Context.Channel.SendMessageAsync("Please provide a value under 14 days"); return; }
            await Log.SetAutoLogCleanLimit(Context, days);
        }

        [Command("clean"), Summary("Automaticly cleans the logchannel.")]
        public async Task CleanLogCommand()
        {
            await Log.CleanLog(Context);
        }
    }
}
