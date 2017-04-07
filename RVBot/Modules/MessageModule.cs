using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using RVBot.Core;


namespace RVBot.Modules
{
    public class MessageModule : ModuleBase
    {
        [Command("postinfo")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Post a new message in the info channel ")]
        public async Task PostInfoMessage(string title = null, [Remainder] string message = null)
        {
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            await Log.LogMessage(Context);
        }

        [Command("editinfo")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Post a new message in the info channel ")]
        public async Task EditInfoMessage(string title = null, [Remainder] string message = null)
        {
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            await Log.LogMessage(Context);
        }

        [Command("postannouncement")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Post a new message in the info channel ")]
        public async Task PostAnnouncement(string title = null, [Remainder] string message = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (title == null) { await ReplyAsync("Please provide a title"); return; }
            if (message == null) { await ReplyAsync("Please provide a message"); return; }
            IMessageChannel channel = await Channel.GetChannel(Context, "Announcements");
            await channel.SendMessageAsync(await Message.FormatAnnouncementMessage(Context, title, message));          
        }

        [Command("editannouncement")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Post a new message in the info channel ")]
        public async Task EditAnnouncement(string title = null, [Remainder] string message = null)
        {
            await ReplyAsync("<:whitespace:298909396644265984>");
            await ReplyAsync("<:rv:298926353632198657>");
            await ReplyAsync(":whitespace:");
            await ReplyAsync(":rv:");
        }


    }
}
