﻿using Discord;
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
    public class WorldStateAutomationModule : ModuleBase
    {
        [Command("wsa_go")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WSAGO(bool go)
        {
            await WorldStateAutomation.InitialiseWSA(Context);
            await WorldStateAutomation.AutoBuildWSA(Context, go);
        }




    }
}