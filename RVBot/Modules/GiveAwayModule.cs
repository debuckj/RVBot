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
        public async Task Draw([Remainder] int amount=1)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            await Giveaway.Draw(Context, "Giveaway Participant", amount);
        }

        [Command("giveaway drawmember")]
        [Alias("drawmember")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageRoles)]
        [Summary("Draws a user in a giveaway. ")]
        public async Task DrawMember([Remainder] int amount = 1)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            await Giveaway.Draw(Context, "Member", amount);
        }


        [Command("arng"), Summary("analyze rng procedure based on range and iterations given")]
        public async Task ARNG([Summary("range indicator")] int range, [Remainder, Summary("sequencer")] int loop)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            var roleTable = await Util.Util.AnalyzeRNG(Context, range, loop);
            if (roleTable != null) { await Context.Channel.SendMessageSplitCodeblockAsync(roleTable); }

        }

        [Command("rng"), Summary("creates random number based on range given")]
        public async Task RNG([Remainder,] int range)
        {
            await Log.LogMessage(Context);
            if (await Permissions.IsServerStaff(Context) == false) { await ReplyAsync("You are not authorised to use this command"); return; }
            await ReplyAsync(Util.Util.GetCryptoRandom(range).ToString());
        }

    }
}
