using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarframeNET;

namespace RVBot.Core
{
    public class WSNewsStreamer
    {
        private List<WSNewsBlock> cachedNews = new List<WSNewsBlock>();
        private IMessageChannel channelNews;

        public WSNewsStreamer(IMessageChannel channel)
        {
            channelNews = channel;
        }


        public async Task Refresh(WorldState ws)
        {
            List<WSNewsBlock> markedForDelete = new List<WSNewsBlock>();
            List<NewsArticle> news = ws.WS_News;
            bool found;

            foreach (WSNewsBlock nb in cachedNews)
            {
                found = false;

                foreach (NewsArticle na in news)
                {
                    if(na.Id == nb.newsArticle.Id)
                    {
                        found = true;
                        news.Remove(na);
                    }
                }

                if (!found)
                {
                    markedForDelete.Add(nb);
                    cachedNews.Remove(nb);
                }
            }

            List<IMessage> markedForDeleteMessages = new List<IMessage>();

            foreach (WSNewsBlock nb in markedForDelete)
            {
                markedForDeleteMessages.Add(nb.newsMessage);
            }

            // delete outdated messages
            await channelNews.DeleteMessagesAsync(markedForDeleteMessages);

            foreach (NewsArticle na in news)
            {
                IMessage message = await WriteNewsBlock(na);
                cachedNews.Add(new WSNewsBlock(na, message));
            }
        }


        public async Task<IMessage> WriteNewsBlock(NewsArticle na)
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
            return await channelNews.SendMessageAsync("", false, eb);
        }

    }
}
