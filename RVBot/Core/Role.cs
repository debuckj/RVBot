using Discord;
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
        public static async Task<string> GetRoleMembers(ICommandContext context, string roleNames)
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
        public static async Task<string> GetRoleUsers(ICommandContext context, string roleName = null)
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
                        Join = user.JoinedAt.Value.Date.ToString("yyyy-MM-dd"),
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
                    Join = user.JoinedAt.Value.Date.ToString("yyyy-MM-dd"),
                    Days = (DateTime.Now - user.JoinedAt.Value.Date).Days.ToString(),
                    Status = user.Status.ToString()

                }).ToList());
                return textTable;
            }
        }

        public static async Task<string> GetUnregisteredUsers(ICommandContext context, string roleNames)
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
                Join = user.JoinedAt.Value.Date.ToString("yyyy-MM-dd"),
                Days = (DateTime.Now - user.JoinedAt.Value.Date).Days.ToString(),
                Status = user.Status.ToString()
            }).ToList());
            return textTable;
        }

        //lists out the roles for a specified user
        public static async Task<string> GetUserRoles(ICommandContext context, IGuildUser _user)
        {
            var statusMsg = await context.Channel.SendMessageAsync(String.Format("Query server for user {0}", _user));
            //IGuildUser _user = null;
            List<ulong> roleids = new List<ulong>();
            //_user = await User.GetUser(context, userName);

            if (_user == null) { await statusMsg.ModifyAsync(x => x.Content = String.Format("Unable to find user {0}", _user)); return null; }

            roleids = _user.RoleIds.ToList();

            string roles = "`"; int rolecount = 0;
            foreach (ulong roleid in roleids)
            {
                roles += context.Guild.Roles.FirstOrDefault(x => roleid.Equals(x.Id)).Name;
                if(!roleid.Equals(roleids.Last())) { roles += " / "; }
                rolecount++;
            }
            roles += "`";
            await statusMsg.ModifyAsync(x => x.Content = String.Format("User {0} ({1}) is assigned {2} roles:", _user, _user.ToUsernameDiscriminatorAndNickname(), rolecount));
            return roles;
        }

        public static IRole GetRole(ICommandContext context, string rolename)
        {
            IRole role = context.Guild.Roles.FirstOrDefault(x => rolename.Equals(x.Name.Replace("@", ""), StringComparison.CurrentCultureIgnoreCase));
            if (role != null) { return role; }
            return null;
        }

        public static async Task VerifyRV(ICommandContext context, IGuildUser user)
        {
            //IGuildUser user = await User.GetUser(context, username);
            IRole member = GetRole(context, "RV");
            IRole pending = GetRole(context, "Pending");

            await Task.Delay(100);

            await user.AddRoleAsync(member);
            await user.RemoveRoleAsync(pending);
            await Log.LogMessage(context, String.Format("verified user {0} - {1}", user.Nickname ?? user.Username, user.Id));
            await context.Channel.SendMessageAsync(String.Format("User {0} verified", user.Mention));
        }

        public static async Task VerifyPV(ICommandContext context, IGuildUser user)
        {
            //IGuildUser user = await User.GetUser(context, username);
            IRole member = Role.GetRole(context, "PV");
            IRole pending = Role.GetRole(context, "Pending");

            await Task.Delay(100);

            await user.AddRoleAsync(member);
            await user.RemoveRoleAsync(pending);
            await Log.LogMessage(context, String.Format("verified user {0} - {1}", user.Nickname ?? user.Username, user.Id));
            await context.Channel.SendMessageAsync(String.Format("User {0} verified", user.Mention));
        }




        public static async Task AssignRole(ICommandContext context, IRole _role, IGuildUser _user)
        {
            //IGuildUser user = await User.GetUser(context, _user);
            //IRole role = Role.GetRole(context, rolename);
            await _user.AddRoleAsync(_role);
        }

        public static async Task RevokeRole(ICommandContext context, IRole _role, IGuildUser _user)
        {
            //IGuildUser user = await User.GetUser(context, _user);
            //IRole role = Role.GetRole(context, rolename);
            await _user.RemoveRoleAsync(_role);
        }

    }
}
