using Discord;
using Discord.Commands;
using RVBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RVBot.Modules
{

    public class CatModule : ModuleBase
    {
        [Command("meow")]
        [Alias("givpussy")]
        [RequireContext(ContextType.Guild)]
        [Summary("Get random cat pic to make u smile. ")]
        public async Task ShowCat()
        {
            await Log.LogMessage(Context);

            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            string catPath = appPath + "\\Content\\ImagesCats";

            
            catPath = catPath.Replace("\\", "/");
            catPath = catPath.Replace("file:/", "");

            string[] cats = Directory.GetFiles(catPath, "*", SearchOption.AllDirectories);


            Random rand = new Random();
            int randindex = rand.Next(cats.Length);
            string randomCat = cats[randindex];

            await Context.Channel.SendFileAsync(randomCat);
        }
    }
}
