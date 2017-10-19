using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarframeNET;


namespace RVBot.Modules
{
    public class CetusModule : ModuleBase
    {
        [Command("cetus")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Get Cetus info")]
        public async Task GetCetusInfo()
        {
            EmbedBuilder eb = new EmbedBuilder();
            string output = "";

            WarframeClient wc = new WarframeClient();
            WorldState ws = await wc.GetWorldStateAsync("pc/");

            CetusCycle cc = ws.WS_CetusCycle;
            output = String.Format("It's currently **{0}time** in Cetus for another **{1}**", cc.TimeOfDay(), cc.timeLeft);

            List<SyndicateMission> missions = ws.WS_SyndicateMissions;
            SyndicateMission mission = missions.FirstOrDefault(x => x.Syndicate.Equals("Ostrons"));
            if (mission != null)
            {
                output += Environment.NewLine;
                output += String.Format("**Bounties** reset in **{0}**", mission.eta);
            }

            eb.WithTitle("Cetus info");
            eb.WithDescription(output);
            eb.WithColor(Color.Red);
            await Context.Channel.SendMessageAsync("", false, eb);
        }

    }
}
