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
            List<NewsArticle> news = ws.WS_News;
            bool found;
            List<IMessage> markedForDeleteMessages = new List<IMessage>();

            foreach (WSNewsBlock nb in cachedNews)
            {
                found = false;

                foreach (NewsArticle na in news)
                {
                    if(na.Id == nb.newsArticle.Id)
                    {
                        found = true;
                        news.Remove(na);
                        break;
                    }
                }

                if (!found)
                {
                    markedForDeleteMessages.Add(nb.newsMessage);
                    cachedNews.Remove(nb);
                }
            }

            // delete outdated messages
            await channelNews.DeleteMessagesAsync(markedForDeleteMessages);

            foreach (NewsArticle na in news)
            {
                WSNewsBlock wsnb = new WSNewsBlock();
                cachedNews.Add(wsnb);
                await wsnb.Write(channelNews, na);
            }
        }
    }
}
