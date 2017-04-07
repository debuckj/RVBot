﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public class Role
    {
        //outputs amount of users for all roles or a specific role
        public static async Task<string> GetRoleMembers(CommandContext context, string roleNames)
        {
            var roles = context.Guild.Roles.Where(x => roleNames == null || roleNames.Equals(x.Name.Replace("@", ""), StringComparison.CurrentCultureIgnoreCase)).OrderByDescending(x => x.Position).ToList();
            var users = await context.Guild.GetUsersAsync();

            var textTable = Temp.ToString((
                from role in roles
                select new
                {
                    Role = role.Name,
                    Online = users.Count(x => x.Status == UserStatus.Online && x.RoleIds.Any(y => y == role.Id)),
                    Idle = users.Count(x => x.Status == UserStatus.Idle && x.RoleIds.Any(y => y == role.Id)),
                    Total = users.Count(x => x.RoleIds.Any(y => y == role.Id)),
                }).ToList());
            return textTable;
        }

        //lists out the users for a specified role
        public static async Task<string> GetRoleUsers(CommandContext context, string roleName = null)
        {
            var users = await context.Guild.GetUsersAsync();

            if (roleName != null)
            {
                var role = context.Guild.Roles.FirstOrDefault(x => roleName.Equals(x.Name.Replace("@", ""), StringComparison.CurrentCultureIgnoreCase));
                if (role == null) { return ("ERROR_NOROLEFOUND"); }

                var textTable = Temp.ToString((
                    from user in users
                    where user.RoleIds.Contains(role.Id)
                    orderby user.Status ascending, (DateTime.Now - user.JoinedAt.Value.Date).Days descending
                    select new
                    {
                        Name = user.Nickname ?? user.Username,
                        Dis = user.Username + "#" + user.Discriminator,
                        Join = user.JoinedAt.Value.Date.ToShortDateString(),
                        Days = (DateTime.Now - user.JoinedAt.Value.Date).Days.ToString(),
                        Status = user.Status.ToString()

                    }).ToList());
                return textTable;
            }
            else
            {
                var textTable = Temp.ToString((
                from user in users

                select new
                {
                    Name = user.Nickname ?? user.Username,
                    Dis = user.Username + "#" + user.Discriminator,
                    Join = user.JoinedAt.Value.Date.ToShortDateString(),
                    Days = (DateTime.Now - user.JoinedAt.Value.Date).Days.ToString(),
                    Status = user.Status.ToString()

                }).ToList());
                return textTable;
            }
        }

        public static async Task<string> GetUnregisteredUsers(CommandContext context, string roleNames)
        {
            var statusMsg = await context.Channel.SendMessageAsync("Fetching unregistered users");
            List<IGuildUser> usersFound = new List<IGuildUser>();
            var users = await context.Guild.GetUsersAsync();
            foreach (IGuildUser user in users) { if (user.RoleIds.Count == 1) { usersFound.Add(user); } }

            switch (usersFound.Count)
            {
                case 0: await statusMsg.ModifyAsync(x => x.Content = "No unregistered users found"); return null;
                case 1: await statusMsg.ModifyAsync(x => x.Content = String.Format("Found {0} unregistered user", usersFound.Count.ToString())); break;
                default: await statusMsg.ModifyAsync(x => x.Content = String.Format("Found {0} unregistered users", usersFound.Count.ToString())); break;
            }

            var textTable = Temp.ToString((
            from user in usersFound
            orderby (DateTime.Now - user.JoinedAt.Value.Date).Days descending
            select new
            {
                Name = user.Nickname ?? user.Username,
                Dis = user.Username + "#" + user.Discriminator,
                Join = user.JoinedAt.Value.Date.ToShortDateString(),
                Days = (DateTime.Now - user.JoinedAt.Value.Date).Days.ToString(),
                Status = user.Status.ToString()
            }).ToList());
            return textTable;
        }

        //lists out the roles for a specified user
        public static async Task<string> GetUserRoles(CommandContext context, string userName = null)
        {
            var statusMsg = await context.Channel.SendMessageAsync(String.Format("Searching server for user {0}", userName));
            var users = await context.Guild.GetUsersAsync();
            IGuildUser _user = null;
            List<ulong> roleids = new List<ulong>();

            userName = userName.Replace("@", "");

            //await statusMsg.ModifyAsync(x => x.Content = String.Format("userlist check : {0}", users.Count.ToString()));

            foreach (IGuildUser user in users)
            {
                if (user.Nickname != null) { if (user.Nickname.Equals(userName, StringComparison.CurrentCultureIgnoreCase)) { _user = user; break; } }
                if ((user.ToUsernameDiscriminatorAndNickname().Equals(userName, StringComparison.CurrentCultureIgnoreCase)) ||
                    (user.Id.ToString().Equals(userName, StringComparison.CurrentCultureIgnoreCase)) ||
                    (user.Username.Equals(userName, StringComparison.CurrentCultureIgnoreCase)))
                { _user = user; break; }
            }

            //await statusMsg.ModifyAsync(x => x.Content = "user not found check");

            if (_user == null) { await statusMsg.ModifyAsync(x => x.Content = String.Format("Unable to find user {0}", userName)); return null; }

            roleids = _user.RoleIds.ToList();

            //await statusMsg.ModifyAsync(x => x.Content = "roles dumped check");

            string roles = "`"; int rolecount = 0;
            foreach (ulong roleid in roleids)
            {
                roles += context.Guild.Roles.FirstOrDefault(x => roleid.Equals(x.Id)).Name + " / "; rolecount++;
            }
            roles += "`";
            await statusMsg.ModifyAsync(x => x.Content = String.Format("User {0} ({1}) is assigned {2} roles:", userName, _user.ToUsernameDiscriminatorAndNickname(), rolecount));
            return roles;
        }
        
        public static IRole GetRole(CommandContext context, string rolename)
        {
            IRole role = context.Guild.Roles.FirstOrDefault(x => rolename.Equals(x.Name.Replace("@", ""), StringComparison.CurrentCultureIgnoreCase));
            if (role != null) { return role; }
            return null;
        }
        
        public static async Task VerifyMember(CommandContext context, string username)
        {
            IGuildUser user = await User.GetUser(context, username);
            IRole member = Role.GetRole(context, "Member");
            IRole pending = Role.GetRole(context, "Pending");
            await user.AddRoleAsync(member);
            await user.RemoveRoleAsync(pending);
            await Log.LogMessage(context, String.Format("verified user {0} - {1}", user.Nickname ?? user.Username, user.Id));
            await context.Channel.SendMessageAsync(String.Format("User {0} verified", user.Mention));
        }

        public static async Task AssignRole(CommandContext context, string rolename, string username)
        {
            IGuildUser user = await User.GetUser(context, username);
            IRole role = Role.GetRole(context, rolename);
            await user.AddRoleAsync(role);
        }

        public static async Task RevokeRole(CommandContext context, string rolename, string username)
        {
            IGuildUser user = await User.GetUser(context, username);
            IRole role = Role.GetRole(context, rolename);
            await user.RemoveRoleAsync(role);
        }

    }
}
