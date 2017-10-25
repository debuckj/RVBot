﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WarframeNET;

namespace RVBot.Core
{
    public static class WSAutomation
    {
        private static Task AutoBuildBackgroundTask;
        private static TimeSpan AutoBuildInterval = TimeSpan.FromMinutes(5);

        private static string chanDefaultNews = "news";
        private static string chanDefaultSorties = "sorties";
        private static string chanDefaultFissures = "fissures";

 

        public static IMessageChannel chanNews { get; set; }
        public static IMessageChannel chanSorties { get; set; }
        public static IMessageChannel chanFissures { get; set; }

        private static bool _bAutoBuild;
        public static bool bAutoBuild
        {
            get { return _bAutoBuild; }
            set { _bAutoBuild = value; }
        }



        public static async Task InitialiseWSA(ICommandContext context)
        {
            chanNews = await Channel.GetChannel(context, chanDefaultNews); 
            chanSorties = await Channel.GetChannel(context, chanDefaultSorties);
            chanFissures = await Channel.GetChannel(context, chanDefaultFissures);
        }

        public static async Task AutoBuildWSA(ICommandContext context, bool AutoBuild)
        {
            _bAutoBuild = AutoBuild;
            await Log.LogMessage(context, bAutoBuild ? "AutoBuildWSA enabled" : "AutoBuildWSA disabled");

            await Channel.ClearChannel(chanNews);
            WSNewsStreamer newsStreamer = new WSNewsStreamer(chanNews);


            AutoBuildBackgroundTask = Task.Run(async () =>
            {
                while (bAutoBuild == true)
                {
                    WarframeClient wc = new WarframeClient();
                    WorldState ws = await wc.GetWorldStateAsync("pc/");

                    await newsStreamer.Refresh(ws);
                    await PopulateSorties(ws);
                    await PopulateFissures(ws);
                    
                    await Task.Delay(AutoBuildInterval);
                }
            });
        }

        public static async Task PopulateSorties(WorldState ws)
        {
            // populate sorties
            await Channel.ClearChannel(chanSorties);
            await chanSorties.SendMessageAsync("`This channel is automatically updated. To disable notifications, right-click the channel and enable mute`");
            await WSObjects.DisplaySorties(ws, chanSorties);
        }

        public static async Task PopulateFissures(WorldState ws)
        {
            // populate fissures
            await Channel.ClearChannel(chanFissures);
            await chanFissures.SendMessageAsync("`This channel is automatically updated. To disable notifications, right-click the channel and enable mute`");
            await WSObjects.DisplayFissures(ws, chanFissures);


        }





    }

}
