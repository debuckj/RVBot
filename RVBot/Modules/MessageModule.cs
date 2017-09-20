using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using RVBot.Core;
using System.Collections.Generic;

namespace RVBot.Modules
{
    public class MessageModule : ModuleBase
    {
        [Command("postinfo")]
        [Alias("pi")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Post a new message in the info channel ")]
        public async Task PostInfoMessage(string title = null, [Remainder] string message = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (title == null) { await ReplyAsync("Please provide a title, make sure your title is between quotes (\")"); return; }
            if (message == null) { await ReplyAsync("Please provide a message"); return; }
            IMessageChannel channel = await Channel.GetChannel(Context, "Info");
            await channel.SendMessageAsync(Message.FormatInfoMessage(Context, title, message));
            await ReplyAsync("New info posted, please verify the result"); return;
        }

        [Command("editinfo")]
        [Alias("ei")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Edit a message in the info channel (default: last message in the channel)")]
        public async Task EditInfoMessage(string title = null, string message = null, [Remainder, Summary("Optional message id to edit")] ulong id = 0)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (title == null) { await ReplyAsync("Please provide a title"); return; }
            if (message == null) { await ReplyAsync("Please provide a message"); return; }
            IMessageChannel channel = await Channel.GetChannel(Context, "Info");
            IUserMessage umessage = null;

            if (id == 0)
            { 
                IEnumerable<IMessage> messages = await channel.GetMessagesAsync(1).Flatten();
                foreach (IMessage msg in messages) { umessage = (IUserMessage)await channel.GetMessageAsync(msg.Id); break; }
            }
            else { umessage = (IUserMessage)await channel.GetMessageAsync(id); }

            if (umessage != null)
            {
                await umessage.ModifyAsync(x => x.Content = Message.FormatAnnouncementMessage(Context, title, message));
                await ReplyAsync("Info modified, please verify the result"); return;
            }




        }

        [Command("postannouncement")]
        [Alias("pa")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Post a new message in the announcement channel ")]
        public async Task PostAnnouncement(string title = null, [Remainder] string message = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (title == null) { await ReplyAsync("Please provide a title, make sure your title is between quotes (\")"); return; }
            //if (message == null) { await ReplyAsync("Please provide a message"); return; }
            IMessageChannel channel = await Channel.GetChannel(Context, "Announcements");
            await channel.SendMessageAsync(Message.FormatAnnouncementMessage(Context, title, message));
            await ReplyAsync("New announcement posted, please verify the result"); return;
        }

        [Command("editannouncement")]
        [Alias("ea")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Edit a message in the announcement channel ")]
        public async Task EditAnnouncement(string title = null, string message = null, [Remainder, Summary("Optional message id to edit")] ulong id = 0)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsWarlord(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (title == null) { await ReplyAsync("Please provide a title"); return; }
            if (message == null) { await ReplyAsync("Please provide a message"); return; }

            IMessageChannel channel = await Channel.GetChannel(Context, "announcements");
            IUserMessage umessage = null;

            if (id == 0)
            {
                IEnumerable<IMessage> messages = await channel.GetMessagesAsync(1).Flatten();          
                foreach (IMessage msg in messages) { umessage = (IUserMessage)await channel.GetMessageAsync(msg.Id); break; }
            }
            else { umessage = (IUserMessage)await channel.GetMessageAsync(id); }

            if (umessage != null)
            {
                await umessage.ModifyAsync(x => x.Content = Message.FormatAnnouncementMessage(Context, title, message));
                await ReplyAsync("Announcement modified, please verify the result"); return;
            }
        }
    }
}
