using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RVBot.Core;


namespace RVBot.Modules
{
    public class HelpModule : ModuleBase
    {
        [Command("help")]
        [Summary("Command to take over the world")]
        public async Task Help()
        {
            if (await Permissions.IsServerStaff(Context) == false) { return; }

            var sb = new StringBuilder();      

            sb.AppendLine($"Prefix for all commands is `rv!` or {Context.Client.CurrentUser.Mention}");

            sb.AppendLine();

            CommandService _commandService = RVCommandService.Service;

            foreach (var module in _commandService.Modules.OrderBy(x => x.Name).ThenBy(x => x.Summary))
            { 

                //sb.Append($"{module.Name}");
                //if (!string.IsNullOrWhiteSpace(module.Summary))
                //{
                //    sb.Append($" - {module.Summary}");
                //}

                if (module.Preconditions.Any())
                {
                    Func<PreconditionAttribute, string> getDesc = x =>
                    {
                        if (x is RequireBotPermissionAttribute)
                        {
                            return $"`I (the bot) need to have {((RequireBotPermissionAttribute)x).GuildPermission } to enable this`";
                        }
                        if (x is RequireContextAttribute)
                        {
                            return $"`only useable in {((RequireContextAttribute)x).Contexts }`";
                        }
  
                        return x.GetType().Name;
                    };

                    sb.Append($" (Requires: {string.Join(" and ", module.Preconditions.OrderBy(x => x.GetType().Name).Select(getDesc))}");

                    sb.Append(")");
                }

                sb.AppendLine();
                foreach (var command in module.Commands.OrderBy(x => x.Name).ThenBy(x => x.Summary))
                {
                    sb.Append($"   {string.Join(" or ", command.Aliases.Select(x => $"`{x}`"))}");

                    if (!string.IsNullOrWhiteSpace(command.Summary))
                    {
                        sb.Append($" - {command.Summary}");
                    }
                    sb.AppendLine();
                }
            }
            var msg = sb.ToString();
            await Context.Channel.SendMessageSplitAsync(msg);
        }





        //[Command("help")]
        //[Summary("Command to take over the world")]
        //public async Task Help()
        //{
        //    var sb = new StringBuilder();

        //    sb.AppendLine($"Prefix for all commands is `/` or `!` or {Context.Client.CurrentUser.Mention}");

        //    sb.AppendLine();

        //    CommandService _commandService = RVCommandService.Service;

        //    foreach (var module in _commandService.Modules.OrderBy(x => x.Name).ThenBy(x => x.Summary))
        //    {
        //        var isRsb = await RequireWFRSB.CheckPermissionsHelper(Context);

        //        if (module.Preconditions.Any(x => x is HelpIgnore) && isRsb.IsSuccess)
        //        {
        //            var guildUser = Context.User as IGuildUser;
        //            if (!guildUser.RoleIds.Contains<ulong>(WarframeRaidSchoolBusModTools.ModId))
        //            {
        //                continue;
        //            }
        //        }
        //        if (module.Preconditions.Any(x => x is RequireWFRSB) && !isRsb.IsSuccess)
        //        {
        //            continue;
        //        }
        //        sb.Append($"{module.Name}");
        //        if (!string.IsNullOrWhiteSpace(module.Summary))
        //        {
        //            sb.Append($" - {module.Summary}");
        //        }

        //        if (module.Preconditions.Any())
        //        {
        //            Func<PreconditionAttribute, string> getDesc = x =>
        //            {
        //                if (x is RequireWFRSB)
        //                {
        //                    return $"`Only usable in WFRSB`";
        //                }
        //                if (x is RequireBotPermissionAttribute)
        //                {
        //                    return $"`I (the bot) need to have {((RequireBotPermissionAttribute)x).GuildPermission } to enable this`";
        //                }
        //                if (x is RequireContextAttribute)
        //                {
        //                    return $"`only useable in {((RequireContextAttribute)x).Contexts }`";
        //                }
        //                //if (x is RequireP)
        //                //{
        //                //    var attr = (RequirePermissionAttribute)x;


        //                //    var perms = string.Join(" and ",
        //                //        (new object[] { attr.ChannelPermission, attr.GuildPermission }).Where(y => y != null));


        //                //    return $"`Requires invoking user to have {perms}`";
        //                //}
        //                return x.GetType().Name;
        //            };

        //            sb.Append($" (Requires: {string.Join(" and ", module.Preconditions.OrderBy(x => x.GetType().Name).Select(getDesc))}");

        //            sb.Append(")");
        //        }

        //        sb.AppendLine();
        //        foreach (var command in module.Commands.OrderBy(x => x.Name).ThenBy(x => x.Summary))
        //        {
        //            if (command.Preconditions.Any(x => x is HelpIgnore))
        //            {
        //                continue;
        //            }
        //            //sb.Append($"   {command.Name} (Aliases: {string.Join("/", command.Aliases)}).");

        //            sb.Append($"   {string.Join(" or ", command.Aliases.Select(x => $"`{x}`"))}");

        //            if (!string.IsNullOrWhiteSpace(command.Summary))
        //            {
        //                sb.Append($" - {command.Summary}");
        //            }

        //            sb.AppendLine();
        //        }
        //    }

        //    var msg = sb.ToString();
        //    await Context.Channel.SendMessageSplitAsync(msg);
        //}

    }
}
