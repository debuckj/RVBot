using Discord;
using Discord.Commands;
using RVBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WarframeNET;

namespace RVBot.Modules
{

    public class LolModule : ModuleBase
    {
        [Command("meow")]
        [Alias("givpussy")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get random cat pic to make u smile. ")]
        public async Task ShowCat()
        {
            await Log.LogMessage(Context);

            //string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //string catPath = appPath + "\\Content\\ImagesCats";


            //catPath = catPath.Replace("\\", "/");
            //catPath = catPath.Replace("file:/", "");

            var catPath = Path.Combine("Content", "ImagesCats");

            string[] cats = Directory.GetFiles(catPath, "*", SearchOption.AllDirectories);


            Random rand = new Random();
            int randindex = rand.Next(cats.Length);
            string randomCat = cats[randindex];

            await Context.Channel.SendFileAsync(randomCat);
        }

        [Command("woof")]
        [Alias("givbitch")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get random cat pic to make u smile. ")]
        public async Task ShowDog()
        {
            await Log.LogMessage(Context);

            //string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //string dogPath = appPath + "\\Content\\ImagesDogs";


            //dogPath = dogPath.Replace("\\", "/");
            //dogPath = dogPath.Replace("file:/", "");

            var dogPath = Path.Combine("Content", "ImagesDogs");

            string[] dogs = Directory.GetFiles(dogPath, "*", SearchOption.AllDirectories);


            Random rand = new Random();
            int randindex = rand.Next(dogs.Length);
            string randomDog = dogs[randindex];

            await Context.Channel.SendFileAsync(randomDog);
        }


        [Command("raidtime")]
        [RequireContext(ContextType.Guild)]
        [Summary("Alert raidtime. ")]
        public async Task ShowRaidTime()
        {
            await Log.LogMessage(Context);
            IRole role = Role.GetRole(Context, "RaidTime");
            await Context.Channel.SendFileAsync("Content/Images/RAIDTIME.jpg", role.Mention);
        }



        [Command("master")]
        [Alias("dizzy")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get dizzy. ")]
        public async Task ShowDiZzY()
        {
            await Log.LogMessage(Context);
            await Context.Channel.SendFileAsync("Content/Images/dizzy.jpg");
        }

        [Command("maniac")]
        //[Alias("dizzy")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get a maniac. ")]
        public async Task ShowManiac()
        {
            await Log.LogMessage(Context);
            await Context.Channel.SendFileAsync("Content/Images/maniac.jpg");
        }

        [Command("hawki")]
        //[Alias("dizzy")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get a hawki. ")]
        public async Task ShowHawki()
        {
            await Log.LogMessage(Context);
            await Context.Channel.SendFileAsync("Content/Images/hawki.jpg");
        }

        [Command("ohshit")]
        //[Alias("dizzy")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get a hawki. ")]
        public async Task ShowSteve()
        {
            await Log.LogMessage(Context);
            await Context.Channel.SendFileAsync("Content/Images/steve.png", "`[DE]Steve testing Plains of Eidolon 3 hours before release`");
        }



        // CETUS TEST STUFF



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

        [Command("ws_cetus")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_Cetus()
        {
            WarframeClient wc = new WarframeClient();
            CetusCycle cc = await wc.GetCetuscycleAsync("pc/");

            string output = String.Format("It's currently {0} in Cetus for another {1}", cc.TimeOfDay(), cc.timeLeft);



            await Context.Channel.SendMessageAsync(output);
        }


        [Command("ws_ostrons")]
        [RequireContext(ContextType.Guild)]
        //[RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_Ostrons()
        {
            WarframeClient wc = new WarframeClient();
            List<SyndicateMission> missions = await wc.GetSyndicateMissionsAsync("pc/");

            SyndicateMission mission = missions.FirstOrDefault(x => x.Syndicate.Equals("Ostrons"));

            if (mission != null)
            {
                await Context.Channel.SendMessageAsync(String.Format("Ostrons reset in {0}", mission.eta));
            }
        }

        [Command("ws_sorties")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_Sorties()
        {
            WarframeClient wc = new WarframeClient();
            WorldState ws = await wc.GetWorldStateAsync("pc/");

            Sortie s = ws.WS_Sortie;        

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithTitle(String.Format("{0} - {1}", s.ETA, s.Boss));

            string sr="";

            foreach (SortieVariant sv in s.Variants)
            {
                sr += String.Format("**{0}** - {1}{2}", sv.MissionType, sv.Modifier, Environment.NewLine);              
                //eb.AddField(sv.MissionType, sv.Modifier);
            }
            eb.WithDescription(sr);
            eb.WithColor(Color.Red);
            await Context.Channel.SendMessageAsync("",false, eb);

        }


        [Command("ws_news")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Test ")]
        public async Task WS_News()
        {
            WarframeClient wc = new WarframeClient();
            List<NewsArticle> news = await wc.GetNewsAsync("pc/");

            foreach (NewsArticle na in news)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithTitle(na.Message);
                eb.AddInlineField("Link", na.Link);
                eb.AddInlineField("eta", na.ETA);
                eb.WithThumbnailUrl(na.ImageLink);
                eb.WithColor(Color.Red);
                await Context.Channel.SendMessageAsync("", false, eb);
            }  
        }

    }
}
