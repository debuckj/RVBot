using Discord;
using Discord.Commands;
using RVBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RVBot.Modules
{
    public class GiveAwayModule : ModuleBase
    {
        [Command("giveaway draw")]
        [Alias("draw")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Draws a user in a giveaway. ")]
        public async Task GetRoleSummary([Remainder] int amount=1)
        {
            await Log.LogMessage(Context);
            await Giveaway.Draw(Context, amount);
        }
        

    }
}
