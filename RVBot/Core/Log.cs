using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RVBot.Core
{
    public static class Log
    {
        //public enum LogFlag { Nom, Pos, Neg};
            
        private static string defaultLogChannel = "log-rvbot";
        private static bool autoLogClean = false;
        private static int autoLogCleanLimit = 7;

        private static ulong _logchannelid = 0;
        public static ulong LogChannelId
        {
            get { return _logchannelid; }
            set { _logchannelid = value; }
        }

        // sets logging output to a specific channel
        public static async Task SetLogChannel(CommandContext context, string channelname)
        {
            if (channelname == null) { await context.Channel.SendMessageAsync(String.Format("Please specify a channel")); }
            IMessageChannel logchannel = await Channel.GetChannel(context, channelname);
            if (logchannel != null) {
                LogChannelId = logchannel.Id;
                await context.Channel.SendMessageAsync(String.Format("Logging set to channel <#{0}>", LogChannelId)); return; }
            await context.Channel.SendMessageAsync(String.Format("Channel `{0}` not found", channelname));
        }

        // gets the predefined logging channel
        public static async Task<IMessageChannel> GetLogChannel(CommandContext context)
        {
            if (LogChannelId == 0) {
                var chans = await context.Guild.GetTextChannelsAsync();
                foreach (IMessageChannel chan in chans) {
                    if (chan.Name.Equals(defaultLogChannel)) {
                        LogChannelId = chan.Id; return chan; } } }
            if (LogChannelId != 0) {
                var chans = await context.Guild.GetTextChannelsAsync();
                foreach (IMessageChannel chan in chans) {
                    if (chan.Id.Equals(LogChannelId)) {
                        return chan; } } }
            return null;
        }

        // logs a message to the predefined logging channel
        public static async Task LogMessage(CommandContext context, string logmessage = "")
        {
            IMessageChannel logchannel = await Log.GetLogChannel(context);
            if (logchannel != null) { await logchannel.SendMessageAsync(Log.LogMessageStringBuilder(context, logmessage)); }
        }

        private static string LogMessageStringBuilder(CommandContext context, string logmessage = "")
        {
            string returnmessage; int intUserSpacer = 12; int intCommandSpacer = 12; int intChannelSpacer = 12;
            //string strUserSpacer = ""; string strCommandSpacer = ""; string strChannelSpacer = "";

            string strDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
            string strUser = context.User.ToUsernameDiscriminatorAndNickname();
            string strCommand = context.Message.ToString();
            string strChannel = context.Channel.Name;

            //if (intUserSpacer > strUser.Length) { strUserSpacer = new string(' ', (intUserSpacer - strUser.Length)); }
            //if (intCommandSpacer > strCommand.Length) { strCommandSpacer = new string(' ', (intCommandSpacer - strCommand.Length)); }
            //if (intChannelSpacer > strChannel.Length) { strChannelSpacer = new string(' ', (intChannelSpacer - strChannel.Length)); }
            string strUserSpacer = (intUserSpacer > strUser.Length) ? new string(' ', (intUserSpacer - strUser.Length)): "";
            string strCommandSpacer = (intCommandSpacer > strCommand.Length) ? new string(' ', (intCommandSpacer - strCommand.Length)) : "";
            string strChannelSpacer = (intChannelSpacer > strChannel.Length) ? new string(' ', (intChannelSpacer - strChannel.Length)) : ""; 


            //returnmessage = "`" + strDate + " " + strUser + strUserSpacer + " " + strCommand + strCommandSpacer + " #" + strChannel + strChannelSpacer + " " + logmessage + "`";
            returnmessage = string.Format("`{0} #{5}{6} {1}{2} {3}{4}  {7}`", strDate, strUser, strUserSpacer, strCommand, strCommandSpacer, strChannel, strChannelSpacer, logmessage);
            return returnmessage;
        }



        public static async Task SetAutoLogClean(CommandContext context, bool autoclean)
        {
            autoLogClean = autoclean;
            await Log.LogMessage(context, "Logging autocleaner enabled");
        }

        public static async Task SetAutoLogCleanLimit(CommandContext context, int days)
        {
            autoLogCleanLimit = days;
            await Log.LogMessage(context, String.Format("Logging autocleaner limit set to {0} days", autoLogCleanLimit));
        }

        public static async Task CleanLog(CommandContext context)
        {
            IMessageChannel logchannel = await Channel.GetChannel(context, _logchannelid);
            if (logchannel==null) { await Log.LogMessage(context,"Unable to determine loggingchannel"); return; }

            var messages = await logchannel.GetMessagesAsync(100000).Flatten();

            int days = 7; int loopcounter = 0;
            await Log.LogMessage(context, String.Format("Cleaning logs older then {0} days", days));
            foreach (var msg in messages)
            {
                DateTime dateLimit = DateTime.UtcNow.Subtract(TimeSpan.FromDays(days));
                if (DateTime.Compare(msg.CreatedAt.DateTime, dateLimit) < 0) { loopcounter++; await msg.DeleteAsync();  }
            }
            await Log.LogMessage(context, String.Format("CleanLog finished - {0} messages deleted", loopcounter));
        }     
    }
}
