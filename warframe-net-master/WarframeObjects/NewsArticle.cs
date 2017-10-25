﻿using Newtonsoft.Json;
using System;

namespace WarframeNET
{
    /// <summary>
    /// An article of the current game news.
    /// </summary>
    public class NewsArticle
    {
        /// <summary>
        /// Id of the article.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Message of the article.
        /// </summary>
        public string Message { get; set; }
 
        /// <summary>
        /// Estimate Time of Arrival of the article.
        /// </summary>
        [JsonProperty("eta")]
        public string ETA { get; set; }

        /// <summary>
        /// Link to the article online.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Is this article important?
        /// </summary>
        public bool Priority { get; set; }

        /// <summary>
        /// Date of the article.
        /// </summary>
        public DateTime Date { get; set; }


        /// <summary>
        /// Link to an image if any.
        /// </summary>
        public string ImageLink { get; set; }

        public string FormattedEta()
        {
            string x1 = ETA.Split(' ')[0];

            if (x1.EndsWith("d"))
            {
                return x1;
            }
            else if(x1.EndsWith("h"))
            {
                string x2 = ETA.Split(' ')[1];
                return x1 + " " + x2;
            }
            else
            {
                return x1;
            }

        }

        internal NewsArticle() { }
    }
}