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
    public class RoleModule : ModuleBase
    {
        [Command("grs")]
        [Alias("getrolesummary")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Shows how many users there are in all roles or in a specified role. ")]
        public async Task GetRoleSummary([Remainder] string role = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            var roleTable = await Role.GetRoleMembers(Context, role);
            await Context.Channel.SendMessageSplitCodeblockAsync(roleTable);
        }

        [Command("gru")]
        [Alias("getroleusers")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Lists the users in a specified role.")]
        public async Task GetRoleUserSummary([Remainder] string role = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (role == null) { await ReplyAsync("Please specify a role to query"); return; }
            var userTable = await Role.GetRoleUsers(Context, role);
            if (userTable == "ERROR_NOROLEFOUND") { await ReplyAsync("No role found with name " + role); return; }
            await Context.Channel.SendMessageSplitCodeblockAsync(userTable);
        }

        [Command("guu")]
        [Alias("getunregisteredusers")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Lists the users that have no roles assigned")]
        public async Task GetUnregisteredUsersSummary([Remainder] string role = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            var roleTable = await Role.GetUnregisteredUsers(Context, role);
            if (roleTable != null) { await Context.Channel.SendMessageSplitCodeblockAsync(roleTable); }
        }

        [Command("gur")]
        [Alias("getuserroles")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Lists the roles of a specified user.")]
        public async Task GetUserRoleSummary([Remainder] string username = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (username == null) { await ReplyAsync("Please specify a user to query"); return; }
            var roleTable = await Role.GetUserRoles(Context, username);
            //if (userTable == "ERROR_NOUSERFOUND") { await ReplyAsync("No user found with name " + username); return; }
            await ReplyAsync(roleTable);
        }

        [Command("vrv")]
        [Alias("verifyrv")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Verifies a user as a member.")]
        public async Task VerifyRV([Remainder] IGuildUser _user)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            //if (username == null) { await ReplyAsync("Please specify a user to verify"); return; }
            await Role.VerifyRV(Context, _user);
        }

        [Command("vpv")]
        [Alias("verifypv")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Verifies a user as a member.")]
        public async Task VerifyPV([Remainder] IGuildUser _user)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsOfficer(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            //if (username == null) { await ReplyAsync("Please specify a user to verify"); return; }
            await Role.VerifyPV(Context, _user);
        }

        [Command("+role")]
        [Alias("assignrole")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Assign a role to a user.")]
        public async Task AssignRole(string rolename, [Remainder] string username = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (rolename == null) { await ReplyAsync("Please specify a role"); return; }
            if (username == null) { await ReplyAsync("Please specify a user"); return; }
            await Role.AssignRole(Context, rolename, username);
        }
        [Command("-role")]
        [Alias("revokerole")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Revoke a role from a user.")]
        public async Task RevokeRole(string rolename, [Remainder] string username = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            if (username == null) { await ReplyAsync("Please specify a user to verify"); return; }
            //try
            //{
                await Role.RevokeRole(Context, rolename, username);
                //await ReplyAsync("id:" + Context.Message.Id);
                //await Task.Delay(500);
    
                //await Context.Message.AddReactionAsync("<:white_check_mark:327429986472689664>");
                //await Context.Message.AddReactionAsync("<:white_check_mark:327429986472689664:>");
                //await Context.Message.AddReactionAsync(":white_check_mark:327429986472689664");
                //await Context.Message.AddReactionAsync(":white_check_mark:327429986472689664:");
                //await Context.Message.AddReactionAsync(":white_check_mark:");
                //await Context.Message.AddReactionAsync("<:white_check_mark:>");

            //}
            //catch
            //{
            //    await Context.Message.AddReactionAsync(":x:327434356916355074");
            //}
        }
        





    }
}
