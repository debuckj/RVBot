using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public static class User
    {
        public static async Task<IGuildUser> GetUser(ICommandContext context, IUser user)
        {
            return await GetUser(context, user.Id);
        }
        public static async Task<IGuildUser> GetUser(ICommandContext context, IGuildUser user)
        {
            return await GetUser(context, user.Id);
        }
        public static async Task<IGuildUser> GetUser(ICommandContext context, ulong userid)
        {
            var users = await context.Guild.GetUsersAsync();
            foreach (IGuildUser user in users) {
                if (user.Id.Equals(userid)) {
                    return user; } }
            return null;
        }
        public static async Task<IGuildUser> GetUser(ICommandContext context, string username)
        {
            string _username = username.Trim('<', '@', '!', '>');
            var users = await context.Guild.GetUsersAsync();
            foreach (IGuildUser user in users)
            {
                if (user.Nickname != null) { if (user.Nickname.Equals(_username, StringComparison.CurrentCultureIgnoreCase)) { return user; } }
                if ((user.ToUsernameDiscriminatorAndNickname().Equals(_username, StringComparison.CurrentCultureIgnoreCase)) ||
                    (user.Id.ToString().Equals(_username, StringComparison.CurrentCultureIgnoreCase)) ||
                    (user.Username.Equals(_username, StringComparison.CurrentCultureIgnoreCase)))
                { return user; }
            }
            return null;
        }

        public static bool HasRole(IGuildUser user, IRole role)
        {
            return user.RoleIds.Contains(role.Id);
        }

        public static async Task ChangeName(IGuildUser user, string newname)
        {
            if ((user == null) || (newname == null)) { return; }
            await user.ModifyAsync(x => x.Nickname = newname);
        }

        public static async Task SendMessage(IGuildUser user, string message)
        {
            IDMChannel x = await user.GetOrCreateDMChannelAsync();
            await x.SendMessageAsync(message);
        }
    }
}
