using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RVBot.Core
{
    public static class Automation
    {

        private static bool _bAutoBuildDailies;
        public static bool bAutoBuildDailies
        {
            get { return _bAutoBuildDailies; }
            set { _bAutoBuildDailies = value; }
        }

        private static Task AutoBuildDailiesBackgroundTask;


        public static async Task AutoBuildDailies(ICommandContext context, bool bBuildDailies)
        {
            _bAutoBuildDailies = bBuildDailies;
            await Log.LogMessage(context, bBuildDailies ? "AutoBuildDailies enabled" : "AutoBuildDailies disabled");
            AutoBuildDailiesBackgroundTask = Task.Run(async () =>
            {
                while (bAutoBuildDailies == true)
                {

                    // 24h int


                }
            });
        }
    }

}
