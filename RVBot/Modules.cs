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
    public class Info : ModuleBase
    {

        [Command("arng"), Summary("creates random number based on range given")]
        public async Task ARNG([Summary("range indicator")] int range, [Remainder, Summary("sequencer")] int loop)
        {
            await Log.LogMessage(Context);
            var roleTable = await Util.Util.AnalyzeRNG(Context, range, loop);
            if (roleTable != null) { await Context.Channel.SendMessageSplitCodeblockAsync(roleTable); }

        }

        [Command("rng"), Summary("creates random number based on range given")]
        public async Task RNG([Remainder, Summary("get random number")] int range)
        {
            await ReplyAsync(Util.Util.GetCryptoRandom(range).ToString());
        }

    }

    // Create a module with the 'sample' prefix
    [Group("sample")]
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
        [Command("purge"), Summary("purge")]
        public async Task Purge()
        {
            await Log.LogMessage(Context);
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
        public async Task SendRoleUserPM([Remainder] string roleName = null)
        {
            await Log.LogMessage(Context);
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

                string succesMessage = "Send to: "; string failedMessage = "Failed to send to: ";
                var succesMsg = await Context.Channel.SendMessageAsync(succesMessage);
                var failedMsg = await Context.Channel.SendMessageAsync(failedMessage);

                foreach (IGuildUser user in usersFound)
                {
                    try
                    {
                        IDMChannel x = await user.CreateDMChannelAsync();
                        await x.SendMessageAsync("this is a test pm because you were assigned role " + role.Name);
                        succesMessage += (user.Nickname ?? user.Username + " ");
                        await succesMsg.ModifyAsync(y => y.Content = succesMessage);
                    }
                    catch
                    {
                        failedMessage += (user.Nickname ?? user.Username + " ");
                        await failedMsg.ModifyAsync(y => y.Content = failedMessage);
                    }
                }
            }
        }







    }
}
 