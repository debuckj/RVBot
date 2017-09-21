using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RVBot.Core;

namespace RVBot.Modules
{
    public class ModerationModule : ModuleBase
    {



        [Command("kick")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Kicks a user.")]
        public async Task KickUser([Summary("The user to kick")] IGuildUser _user, [Remainder, Summary("The reason for the kick")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            //if (reason == null) { await ReplyAsync("Please specify a reason"); return; }
            if (await Permissions.IsOfficer(Context, _user)) { await ReplyAsync(String.Format("Unable to kick user {0}", _user.Mention)); return; }

            //await Context.Channel.SendFileAsync("Content/Images/KICK.gif", "https://www.youtube.com/watch?v=kvGMFBPDqmc");
      
            //string kickpmmessage = String.Format("You were kicked from RV discord by {0} for reason: {1}{2}{3}{4}", Context.User.Mention, reason, Environment.NewLine, Environment.NewLine, "https://www.youtube.com/watch?v=kvGMFBPDqmc");
            //await User.SendMessage(_user, kickpmmessage);

            await _user.KickAsync();
            
            await ReplyAsync(String.Format("user {0} kicked by {1} for reason: {2}", _user.Mention, Context.User.Mention, reason));
            await Log.LogMessage(Context, String.Format("user {0} kicked by {1} for reason: {2}", _user.ToUsernameDiscriminatorAndNickname(), Context.User.ToUsernameDiscriminatorAndNickname(), reason));
        }

        [Command("boot")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Ice cube kicks a user.")]
        public async Task BootUser([Summary("The user to kick")] IGuildUser _user, [Remainder, Summary("The reason for the kick")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            //if (user == null) { await ReplyAsync("Please specify a user"); return; }
            if (reason == null) { await ReplyAsync("Please specify a reason"); return; }

            //IGuildUser _user = await User.GetUser(Context, user);
            //if (_user == null) { await ReplyAsync(String.Format("Unable to find user {0}", user)); return; }
            if (await Permissions.IsOfficer(Context, _user)) { await ReplyAsync(String.Format("Unable to kick user {0}", _user.Mention)); return; }

            await Context.Channel.SendFileAsync("Content/Images/KICK.gif", "https://www.youtube.com/watch?v=kvGMFBPDqmc");

            string kickpmmessage = String.Format("You were kicked from RV discord by {0} for reason: {1}{2}{3}{4}", Context.User.Mention, reason, Environment.NewLine, Environment.NewLine, "https://www.youtube.com/watch?v=kvGMFBPDqmc");
            await User.SendMessage(_user, kickpmmessage);

            await _user.KickAsync();

            await ReplyAsync(String.Format("user {0} kicked by {1} for reason: {2}", _user.Mention, Context.User.Mention, reason));
            await Log.LogMessage(Context, String.Format("user {0} kicked by {1} for reason: {2}", _user.ToUsernameDiscriminatorAndNickname(), Context.User.ToUsernameDiscriminatorAndNickname(), reason));
        }


        [Command("ban")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Bans a user.")]
        public async Task BanUser([Summary("The user to ban")]  IGuildUser _user, [Remainder, Summary("The reason for the ban")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            //if (user == null) { await ReplyAsync("Please specify a user"); return; }
            if (reason == null) { await ReplyAsync("Please specify a reason"); return; }

            //IGuildUser _user = await User.GetUser(Context, user);
            //if (_user == null) { await ReplyAsync(String.Format("Unable to find user {0}", user)); return; }
            if (await Permissions.IsOfficer(Context, _user)) { await ReplyAsync(String.Format("Unable to ban user {0}", _user.Mention)); return; }

            await Context.Channel.SendFileAsync("Content/Images/BAN.gif", "https://www.youtube.com/watch?v=kvGMFBPDqmc");

            string banpmmessage = String.Format("You were banned from RV discord by {0} for reason: {1}{2}{3}{4}", Context.User.Mention, reason, Environment.NewLine, Environment.NewLine, "https://www.youtube.com/watch?v=kvGMFBPDqmc");
            await User.SendMessage(_user, banpmmessage);

            await Context.Guild.AddBanAsync(_user.Id, 1); await _user.KickAsync();

            await ReplyAsync(String.Format("user {0} banned by {1} for reason: {2}", _user.Mention, Context.User.Mention, reason));
            await Log.LogMessage(Context, String.Format("user {0} banned by {1} for reason: {2}", _user.ToUsernameDiscriminatorAndNickname(), Context.User.ToUsernameDiscriminatorAndNickname(), reason));
        }

        [Command("begonethot")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Kicks a thot.")]
        public async Task KickThot([Summary("The thot to kick")] IGuildUser _user, [Remainder, Summary("The reason for the kick")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            //if (reason == null) { await ReplyAsync("Please specify a reason"); return; }
            if (await Permissions.IsOfficer(Context, _user)) { await ReplyAsync(String.Format("Unable to kick user {0}", _user.Mention)); return; }

            await Context.Channel.SendMessageAsync("https://youtu.be/tyrKeThaEJM");

            string kickpmmessage = String.Format("You were kicked from RV discord by {0} for reason: {1}{2}{3}{4}", Context.User.Mention, reason, Environment.NewLine, Environment.NewLine, "https://youtu.be/tyrKeThaEJM");
            await User.SendMessage(_user, kickpmmessage);

            await _user.KickAsync();

            await ReplyAsync(String.Format("thot {0} kicked by {1} for reason: {2}", _user.Mention, Context.User.Mention, reason));
            await Log.LogMessage(Context, String.Format("thot {0} kicked by {1} for reason: {2}", _user.ToUsernameDiscriminatorAndNickname(), Context.User.ToUsernameDiscriminatorAndNickname(), reason));
        }



        


        [Command("mute")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("function not implemented. ")]
        public async Task Mute([Summary("The user to kick")] string user = null, [Remainder, Summary("The reason for the kick")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            await ReplyAsync(String.Format("function not implemented"));
        }





    }
}
