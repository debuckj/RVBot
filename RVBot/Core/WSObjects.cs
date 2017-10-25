using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarframeNET;

namespace RVBot.Core
{
    public static class WSObjects
    {
        //private static List<Alert> cachedAlerts;


        public static async Task GetNews(WorldState ws)
        {


        }



        public static async Task DisplayNews(WorldState ws, IMessageChannel channel)
        {
            List<NewsArticle> news = ws.WS_News;

            foreach (NewsArticle na in news)
            {
                EmbedBuilder eb = new EmbedBuilder();             
                //eb.WithThumbnailUrl(na.ImageLink + "?width=80&height=80");
                eb.WithTitle(String.Format("{0} - {1} ago", na.Message, na.FormattedEta()));
                eb.WithDescription(na.Link);

                //eb.AddInlineField("Link", na.Link);
                //eb.AddInlineField("eta", na.ETA);
                eb.WithImageUrl(na.ImageLink);
                eb.WithColor(Color.Red);

                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.WithIconUrl("https://images-ext-1.discordapp.net/external/hAQVdE0EHRmMzbb965tL65KqD4wHXwj82tiI0C0_Mac/%3Fsize%3D128/https/cdn.discordapp.com/icons/166488311458824193/96a7f1b793bdf524165b0f5f62d32126.png?width=80&height=80");
                footer.WithText("Remnants of the Void");
                eb.WithFooter(footer);
                eb.WithTimestamp(new DateTimeOffset(DateTime.Now));
                await channel.SendMessageAsync("", false, eb);
            }
        }

        public static async Task DisplaySorties(WorldState ws, IMessageChannel channel)
        {
            Sortie s = ws.WS_Sortie;

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle(String.Format("{0} - {1}", s.ETA, s.Boss));

            string sr = "";

            foreach (SortieVariant sv in s.Variants)
            {
                sr += String.Format("**{0}** - {1}{2}", sv.MissionType, sv.Modifier, Environment.NewLine);
                sr += Environment.NewLine;
                //eb.AddField(sv.MissionType, sv.Modifier);
            }
            eb.WithDescription(sr);
            eb.WithColor(Color.Red);
            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.WithIconUrl("https://images-ext-1.discordapp.net/external/hAQVdE0EHRmMzbb965tL65KqD4wHXwj82tiI0C0_Mac/%3Fsize%3D128/https/cdn.discordapp.com/icons/166488311458824193/96a7f1b793bdf524165b0f5f62d32126.png?width=80&height=80");
            footer.WithText("Remnants of the Void");
            eb.WithFooter(footer);
            eb.WithTimestamp(new DateTimeOffset(DateTime.Now));
            await channel.SendMessageAsync("", false, eb);
        }

        public static async Task DisplayFissures(WorldState ws, IMessageChannel channel)
        {
            List<Fissure> fissures = ws.WS_Fissures;

            foreach (Fissure fis in fissures.OrderBy(x => x.TierNumber))
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithTitle(String.Format("{0} - {1}", fis.Tier, fis.MissionType));
                eb.WithDescription(String.Format("{0} - {1} - {2}", fis.eta, fis.Node, fis.Enemy));
                eb.WithColor(Color.Red);
                EmbedFooterBuilder footer = new EmbedFooterBuilder();
                footer.WithIconUrl("https://images-ext-1.discordapp.net/external/hAQVdE0EHRmMzbb965tL65KqD4wHXwj82tiI0C0_Mac/%3Fsize%3D128/https/cdn.discordapp.com/icons/166488311458824193/96a7f1b793bdf524165b0f5f62d32126.png?width=80&height=80");
                footer.WithText("Remnants of the Void");
                eb.WithFooter(footer);
                eb.WithTimestamp(new DateTimeOffset(DateTime.Now));
                await channel.SendMessageAsync("", false, eb);
            }
        }






    }
}
