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
        [Summary("Shows how many users there are in all roles or in a specified role. ")]
        public async Task KickUser([Summary("The user to kick")] string user = null, [Remainder, Summary("The reason for the kick")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            if (user == null) { await ReplyAsync("Please specify a user"); return; }
            if (reason == null) { await ReplyAsync("Please specify a reason"); return; }

            IGuildUser _user = await User.GetUser(Context, user);
            if (_user == null) { await ReplyAsync(String.Format("Unable to find user {0}", user)); return; }
            if (await Permissions.IsOfficer(Context, _user)) { await ReplyAsync(String.Format("Unable to kick user {0}", user)); return; }

            await _user.KickAsync();
            await ReplyAsync(String.Format("user {0} kicked by {1} for reason: {2}", _user.Mention, Context.User.Mention, reason));
            await Log.LogMessage(Context, String.Format("user {0} kicked by {1} for reason: {2}", _user.ToUsernameDiscriminatorAndNickname(), Context.User.ToUsernameDiscriminatorAndNickname(), reason));
        }

        [Command("ban")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Shows how many users there are in all roles or in a specified role. ")]
        public async Task BanUser([Summary("The user to ban")] string user = null, [Remainder, Summary("The reason for the ban")] string reason = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            if (user == null) { await ReplyAsync("Please specify a user"); return; }
            if (reason == null) { await ReplyAsync("Please specify a reason"); return; }

            IGuildUser _user = await User.GetUser(Context, user);
            if (_user == null) { await ReplyAsync(String.Format("Unable to find user {0}", user)); return; }
            if (await Permissions.IsOfficer(Context, _user)) { await ReplyAsync(String.Format("Unable to ban user {0}", user)); return; }

            await _user.KickAsync(); await Context.Guild.AddBanAsync(_user.Id, 1);
            await ReplyAsync(String.Format("user {0} banned by {1} for reason: {2}", _user.Mention, Context.User.Mention, reason));
            await Log.LogMessage(Context, String.Format("user {0} banned by {1} for reason: {2}", _user.ToUsernameDiscriminatorAndNickname(), Context.User.ToUsernameDiscriminatorAndNickname(), reason));
        }


        [Command("mute")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("function not implemented. ")]
        public async Task Mute([Summary("The user to kick")] string user = null, [Remainder, Summary("The reason for the kick")] string reason = null)
        {
            await ReplyAsync(String.Format("function not implemented"));
        }





    }
}
