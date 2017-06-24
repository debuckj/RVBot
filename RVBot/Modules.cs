using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Text;
using RVBot.Core;
using RVBot.Util;


namespace RVBot
{


    // Create a module with the 'sample' prefix
    public class Sample : ModuleBase
    {
        [Command("userinfo"), Summary("Returns info about the current user, or the user parameter, if one passed.")]
        [Alias("user", "whois")]
        public async Task UserInfo([Summary("The (optional) user to get info for")] IUser user = null)
        {
            var userInfo = user ?? Context.Client.CurrentUser;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }
    }


    public class UtilModule : ModuleBase
    {
        [Command("purge"), Summary("purges all messages from channel")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Purge()
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            string statusMessage;
            int messageCount = 0; int loopcounter = 0; int statusUpdateDelay = 10; int statusDelay = 0;

            // update statusmsg fetching messages
            statusMessage = "```Fetching messages - calculating ```";
            var statusMsg = await Context.Channel.SendMessageAsync(statusMessage);

            //  fetching messages
            var messages = await Context.Channel.GetMessagesAsync(10000).Flatten();

            // get messagecounter
            foreach (var msg in messages)
            {
                messageCount++;
            }

            // update statusmsg with results
            statusMessage = "```Fetching messages - " + messageCount + " Messages Found```";
            await statusMsg.ModifyAsync(x => x.Content = statusMessage);

            statusMessage = "```Deleted messages - " + loopcounter.ToString() + " / " + messageCount + " ```";
            statusMsg = await Context.Channel.SendMessageAsync(statusMessage);

            foreach (var msg in messages)
            {
                loopcounter++;
                await msg.DeleteAsync();

                statusDelay++;
                if (statusDelay >= statusUpdateDelay)
                {
                    statusMessage = "```Deleted messages - " + loopcounter.ToString() + " / " + messageCount + " ```";
                    await statusMsg.ModifyAsync(x => x.Content = statusMessage);
                    statusDelay = 0;
                }

            }
            await Task.Delay(500);
            await statusMsg.DeleteAsync();
        }

      
       
        [Command("privatemessageroleusers")]
        [Alias("pmru")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Sends a pm to all users in the given role.")]
        public async Task SendRoleUserPM(string roleName = null, [Remainder] string message = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }

            if (roleName == null) { await Context.Channel.SendMessageAsync("Please provide a role"); return; }
            var statusMsg = await Context.Channel.SendMessageAsync("Fetching users");

            IGuild guild = Context.Guild;
            var users = await guild.GetUsersAsync();

            if (roleName != null)
            {
                var role = guild.Roles.FirstOrDefault(x => roleName.Equals(x.Name.Replace("@", ""), StringComparison.CurrentCultureIgnoreCase));
                if (role == null) { await statusMsg.ModifyAsync(x => x.Content = "No role found with name " + roleName); return; }

                List<IGuildUser> usersFound = new List<IGuildUser>();

                foreach (IGuildUser user in users)
                {
                    if (user.RoleIds.Contains(role.Id))
                    {
                        usersFound.Add(user);
                    }
                }

                if (usersFound.Count == 0) { await statusMsg.ModifyAsync(x => x.Content = "No users found with role " + roleName); return; }
                await statusMsg.ModifyAsync(x => x.Content = "Found " + usersFound.Count.ToString() + " users with role " + roleName + ", sending pm");

                List<IGuildUser> usersSucces = new List<IGuildUser>();
                List<IGuildUser> usersFailed = new List<IGuildUser>();

                foreach (IGuildUser user in usersFound)
                {
                    try
                    {
                        IDMChannel x = await user.CreateDMChannelAsync();
                        string messageTitle = "Message send by " + Context.User.ToUsernameDiscriminatorAndNickname() + " to all <" + role.Name + ">";
                        await x.SendMessageAsync(String.Format("{0}{1}{2}{3}", messageTitle, Environment.NewLine, Environment.NewLine, message));
                        usersSucces.Add(user);
                    }
                    catch
                    {
                        usersFailed.Add(user);
                    }
                }

                string succesMessage = "Send to: "; string failedMessage = "Failed to send to: ";

                if (usersSucces.Count !=0)
                { 
                    foreach (IGuildUser user in usersSucces)
                    {
                        succesMessage += user.Nickname ?? user.Username;
                        if (!user.Equals(usersSucces.Last())) { succesMessage += " / "; }
                    }
                    await Context.Channel.SendMessageAsync(succesMessage);
                }

                if (usersFailed.Count != 0)
                {
                    foreach (IGuildUser user in usersFailed)
                    {
                        succesMessage += user.Nickname ?? user.Username;
                        if (!user.Equals(usersFailed.Last())) { succesMessage += " / "; }
                    }

                    await Context.Channel.SendMessageAsync(failedMessage);
                }
            }
        }







    }
}
 