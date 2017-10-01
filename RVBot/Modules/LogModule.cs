using Discord;
using Discord.Commands;
using RVBot.Core;
using System;
using System.Threading.Tasks;


namespace RVBot
{
    [Group("log")]
    public class LogModule : ModuleBase
    {
        [Command("set"), Summary("Sets the channel to log to.")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetLogChannelCommand([Remainder, Summary("Sets the channel to log to")] string channelname = null)
        {
            if (await Permissions.IsServerStaff(Context) == false) { throw new UnauthorizedAccessException(); }
            if (channelname == null) { await Context.Channel.SendMessageAsync("Please provide a channel"); return; }
            await Log.SetLogChannel(Context, channelname);
        }

        [Command("send"), Summary("Logs a message to the predefined channel.")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogMessageCommand([Remainder, Summary("The message to log")] string logmessage = null)
        {
            if (await Permissions.IsServerStaff(Context) == false) { throw new UnauthorizedAccessException(); }
            if (logmessage == null) { await Context.Channel.SendMessageAsync("Please provide something to log"); return; }
            await Log.LogMessage(Context, logmessage);
        }

        [Command("setauto"), Summary("Automaticly cleans the logchannel of older messages.")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetAutoLogCleanCommand([Remainder, Summary("Sets the channel to log to")] string yesno = null)
        {
            if (await Permissions.IsServerStaff(Context) == false) { throw new UnauthorizedAccessException(); }
            bool byesno;
            if ((yesno == null) || (!bool.TryParse(yesno, out byesno))) { await Context.Channel.SendMessageAsync("Please provide yes or no"); return; }
            await Log.SetAutoLogClean(Context, byesno);
        }

        [Command("setautodays"), Summary("Sets the limit for the autocleaner.")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetAutoLogCleanLimitCommand([Remainder, Summary("Sets the amount of days to keep logs off")] int days = 0)
        {
            if (await Permissions.IsServerStaff(Context) == false) { throw new UnauthorizedAccessException(); }
            if (days>=14) { await Context.Channel.SendMessageAsync("Please provide a value under 14 days"); return; }
            await Log.SetAutoLogCleanLimit(Context, days);
        }

        [Command("clean"), Summary("Automaticly cleans the logchannel.")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CleanLogCommand()
        {
            if (await Permissions.IsServerStaff(Context) == false) { throw new UnauthorizedAccessException(); }
            await Log.CleanLog(Context);
        }
    }
}
