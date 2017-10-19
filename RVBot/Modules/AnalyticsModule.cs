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
    public class AnalyticsModule : ModuleBase
    {

        [Command("analyzelog")]
        //[Alias("unassigned")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Analyze bot log.")]
        public async Task AnalyzeLog(string searchparam = null)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { throw new UnauthorizedAccessException(); }

            var roleTable = await Log.Analyze(Context, searchparam);
            await Context.Channel.SendMessageSplitCodeblockAsync(roleTable);
        }
    }
}
