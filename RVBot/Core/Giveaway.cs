using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public class Giveaway
    {

        // gets the predefined logging channel
        public static async Task Draw(CommandContext context, string giveawayRole, int amount=1)
        {
            var statusMsg = await context.Channel.SendMessageAsync("`Fetching giveaway participants`");
            // get users with role giveaway > refactor out plz wtf is this shit
            var users = await context.Guild.GetUsersAsync();

            var role = context.Guild.Roles.FirstOrDefault(x => giveawayRole.Equals(x.Name.Replace("@", ""), StringComparison.CurrentCultureIgnoreCase));
            if (role == null) { await statusMsg.ModifyAsync(x => x.Content = String.Format("`Unable to find role {0}`", giveawayRole)); }

            List<IGuildUser> userList = new List<IGuildUser>();

            foreach (IGuildUser user in users)
            {
                if (user.RoleIds.Contains(role.Id))
                {
                    userList.Add(user);
                }
            }

            switch (userList.Count)
            {
                case 0: await statusMsg.ModifyAsync(x => x.Content = String.Format("`No participants found`", userList.Count.ToString()));break;
                case 1: await statusMsg.ModifyAsync(x => x.Content = String.Format("`Found {0} participant`", userList.Count.ToString()));break;
                default: await statusMsg.ModifyAsync(x => x.Content = String.Format("`Found {0} participants`", userList.Count.ToString()));break;
            }

            if (amount > userList.Count) { await context.Channel.SendMessageAsync("`Unable to draw more then the amount of participants, dont be stupid m'kay?`"); return; }


            if (userList.Count >= 1)
            {
                List<IGuildUser> winnerList = new List<IGuildUser>();
                for (int x = 1; x <= amount; x++)
                {
                    IGuildUser drawUser = null;
                    do
                    {
                        int rngme = Util.Util.GetCryptoRandom(userList.Count);
                        //await context.Channel.SendMessageAsync(String.Format("`{0} - {1}`", x, rngme));
                        drawUser = userList.ElementAt(rngme);
                    }
                    while (winnerList.Contains(drawUser));
                    winnerList.Add(drawUser);
                }


                string winners = "";
                for (int w=1; w<=winnerList.Count; w++)
                {
                    winners += String.Format("<@{0}>", winnerList.ElementAt(w-1).Id);
                    if (winnerList.Count > 1)
                    {
                        if (w < winnerList.Count - 1)
                        {
                            winners += ", ";
                        }
                        else if (w < winnerList.Count)
                        {
                            winners += " and ";
                        }
                    }
                    
                }

                await context.Channel.SendMessageAsync(String.Format("Congratulations to {0}", winners));


                //await context.Channel.SendMessageAsync(String.Format("`{0}`", rngme));

                //int rngme = Util.Util.GetCryptoRandom(userList.Count);
                //IGuildUser drawUser = userList.ElementAt(rngme);
                //await context.Channel.SendMessageAsync(String.Format("Congratualtions to <@{0}>", drawUser.Id));

            }
        }

    }
}
