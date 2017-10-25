using Discord;
using Discord.Commands;
using RVBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarframeNET;

namespace RVBot.Modules
{
    public class WorldStateModule : ModuleBase
    {
        [Command("poe")]
        [Alias("cetus")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Plains of Eidolon / Cetus cycle")]
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

            eb.WithTitle("Plains of Eidolon / Cetus cycle info");
            eb.WithDescription(output);
            eb.WithColor(Color.Red);
            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.WithIconUrl("https://images-ext-1.discordapp.net/external/hAQVdE0EHRmMzbb965tL65KqD4wHXwj82tiI0C0_Mac/%3Fsize%3D128/https/cdn.discordapp.com/icons/166488311458824193/96a7f1b793bdf524165b0f5f62d32126.png?width=80&height=80");
            footer.WithText("Remnants of the Void");
            eb.WithFooter(footer);
            eb.WithTimestamp(new DateTimeOffset(DateTime.Now));
            await Context.Channel.SendMessageAsync("", false, eb);
        }

        [Command("earth")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Earth cycle")]
        public async Task GetEarthInfo()
        {
            EmbedBuilder eb = new EmbedBuilder();
            string output = "";

            WarframeClient wc = new WarframeClient();
            WorldState ws = await wc.GetWorldStateAsync("pc/");

            EarthCycle ec = ws.WS_EarthCycle;
            output = String.Format("It's currently **{0}time** on Earth for another **{1}**", ec.TimeOfDay(), ec.timeLeft);

            eb.WithTitle("Earth cycle info");
            eb.WithDescription(output);
            eb.WithColor(Color.Red);
            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.WithIconUrl("https://images-ext-1.discordapp.net/external/hAQVdE0EHRmMzbb965tL65KqD4wHXwj82tiI0C0_Mac/%3Fsize%3D128/https/cdn.discordapp.com/icons/166488311458824193/96a7f1b793bdf524165b0f5f62d32126.png?width=80&height=80");
            footer.WithText("Remnants of the Void");
            eb.WithFooter(footer);
            eb.WithTimestamp(new DateTimeOffset(DateTime.Now));
            await Context.Channel.SendMessageAsync("", false, eb);
        }

        [Command("baro")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.Administrator)]
        public async Task x(string channelname = null)
        {
            EmbedBuilder eb = new EmbedBuilder();
            string output = "";

            WarframeClient wc = new WarframeClient();
            VoidTrader vt = await wc.GetVoidTraderAsync ("pc/");

            if (!vt.active)
            {
                output = String.Format("baro arrives in {0}", vt.etaStart);

            }
            else
            {
                output = String.Format("baro leaves in {0}", vt.etaLeave);
                output += Environment.NewLine;
                output += "```";

                int intItemMax = 40; int intDucatMax = 5; int intCreditMax = 8;
 
                foreach (VoidTraderItem vti in vt.Inventory)
                {
                    string strItemSpacer = (intItemMax > vti.Item.Length) ? new string(' ', (intItemMax - vti.Item.Length)) : " ";
                    string strDucatSpacer = (intDucatMax > vti.Ducats.ToString().Length) ? new string(' ', (intDucatMax - vti.Ducats.ToString().Length)) : " ";
                    string strCreditSpacer = (intCreditMax > vti.Credits.ToString().Length) ? new string(' ', (intCreditMax - vti.Credits.ToString().Length)) : " ";

                    output += Environment.NewLine;
                    output += String.Format("{0}{1}{2}{3}{4}{5}", vti.Item, strItemSpacer, strDucatSpacer, vti.Ducats, strCreditSpacer, vti.Credits);
                }

                output += "```";
            }

            eb.WithTitle("Baro info");
            eb.WithDescription(output);
            eb.WithColor(Color.Red);
            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.WithIconUrl("https://images-ext-1.discordapp.net/external/hAQVdE0EHRmMzbb965tL65KqD4wHXwj82tiI0C0_Mac/%3Fsize%3D128/https/cdn.discordapp.com/icons/166488311458824193/96a7f1b793bdf524165b0f5f62d32126.png?width=80&height=80");
            footer.WithText("Remnants of the Void");
            eb.WithFooter(footer);
            eb.WithTimestamp(new DateTimeOffset(DateTime.Now));
            await Context.Channel.SendMessageAsync("", false, eb);
        }

        



        // WS TEST STUFF



        [Command("ws_test")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_Test()
        {
            WarframeClient wc = new WarframeClient();
            DateTime dt = await wc.GetTimestampAsync("pc/");
            await Context.Channel.SendMessageAsync(String.Format("worldstatefile updated at : {0}", dt.ToString()));
        }




        [Command("ws_sorties")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_Sorties()
        {
            WarframeClient wc = new WarframeClient();
            WorldState ws = await wc.GetWorldStateAsync("pc/");
            await WSObjects.DisplaySorties(ws, Context.Channel);
        }

        [Command("ws_news")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_News()
        {
            WarframeClient wc = new WarframeClient();
            WorldState ws = await wc.GetWorldStateAsync("pc/");
            await WSObjects.DisplayNews(ws, Context.Channel);
        }

        [Command("ws_fissures")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_Fissures()
        {
            WarframeClient wc = new WarframeClient();
            WorldState ws = await wc.GetWorldStateAsync("pc/");
            await WSObjects.DisplayFissures(ws, Context.Channel);
        }


    }
}
