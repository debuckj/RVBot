using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RVBot.Core
{
    public class LogMessage
    {
        private string _timestamp;
        public string Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        private string _channel;
        public string Channel
        {
            get { return _channel; }
            set { _channel = value; }
        }

        private string _user;
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _command;
        public string Command
        {
            get { return _command; }
            set { _command = value; }
        }
    }



    public static class Log
    {
        //public enum LogFlag { Nom, Pos, Neg};

        private static string defaultLogChannel = "log-rvbot";
        private static int autoLogCleanLimit = 7;
        private static bool autoLogClean = false;

        private static ulong _logchannelid = 0;
        public static ulong LogChannelId
        {
            get { return _logchannelid; }
            set { _logchannelid = value; }
        }

        //private static Timer logTimer;
        private static Task backgroundTask;
        private static TimeSpan logTimerInterval = TimeSpan.FromHours(24);
        private static ICommandContext _commandcontext;

        public static async Task SetAutoLogClean(ICommandContext context, bool autoclean)
        {
            autoLogClean = autoclean;
            await Log.LogMessage(context, autoclean ? "Logging autocleaner enabled" : "Logging autocleaner disabled");
            backgroundTask = Task.Run(async () =>
            {
                while (autoLogClean == true)
                {
                    await CleanLog(_commandcontext);
                    await Task.Delay(logTimerInterval);
                }
            });
            //logTimer = new Timer(logTimerInterval);
            //logTimer.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            //logTimer.Enabled = autoclean;
            _commandcontext = context;
        }

        //private static async void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    await CleanLog(_commandcontext);
        //}



        // sets logging output to a specific channel
        public static async Task SetLogChannel(ICommandContext context, string channelname)
        {
            if (channelname == null) { await context.Channel.SendMessageAsync(String.Format("Please specify a channel")); }
            IMessageChannel logchannel = await Channel.GetChannel(context, channelname);
            if (logchannel != null) {
                LogChannelId = logchannel.Id;
                await context.Channel.SendMessageAsync(String.Format("Logging set to channel <#{0}>", LogChannelId)); return; }
            await context.Channel.SendMessageAsync(String.Format("Channel `{0}` not found", channelname));
        }
        //public static async Task SetLogChannel(ICommandContext context, IMessageChannel channel)
        //{
        //    LogChannelId = channel.Id;
        //    await context.Channel.SendMessageAsync(String.Format("Logging set to channel <#{0}>", LogChannelId)); return;
        //}



        // gets the predefined logging channel
        public static async Task<IMessageChannel> GetLogChannel(ICommandContext context)
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
        public static async Task LogMessage(ICommandContext context, string logmessage = "")
        {
            IMessageChannel logchannel = await Log.GetLogChannel(context);
            if (logchannel != null) { await logchannel.SendMessageAsync(Log.LogMessageStringBuilder(context, logmessage)); }
        }

        private static string LogMessageStringBuilder(ICommandContext context, string logmessage = "")
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
            string strUserSpacer = (intUserSpacer > strUser.Length) ? new string(' ', (intUserSpacer - strUser.Length)) : "";
            string strCommandSpacer = (intCommandSpacer > strCommand.Length) ? new string(' ', (intCommandSpacer - strCommand.Length)) : "";
            string strChannelSpacer = (intChannelSpacer > strChannel.Length) ? new string(' ', (intChannelSpacer - strChannel.Length)) : "";


            //returnmessage = "`" + strDate + " " + strUser + strUserSpacer + " " + strCommand + strCommandSpacer + " #" + strChannel + strChannelSpacer + " " + logmessage + "`";
            returnmessage = string.Format("`{0} #{5}{6} {1}{2} {3}{4}  {7}`", strDate, strUser, strUserSpacer, strCommand, strCommandSpacer, strChannel, strChannelSpacer, logmessage);
            return returnmessage;
        }




        public static async Task SetAutoLogCleanLimit(ICommandContext context, int days)
        {
            autoLogCleanLimit = days;
            await Log.LogMessage(context, String.Format("Logging autocleaner limit set to {0} days", autoLogCleanLimit));
        }

        public static async Task CleanLog(ICommandContext context)
        {
            IMessageChannel logchannel = await Channel.GetChannel(context, _logchannelid);
            if (logchannel == null) { await Log.LogMessage(context, "Unable to determine loggingchannel"); return; }

            var messages = await logchannel.GetMessagesAsync(100000).Flatten();

            int days = autoLogCleanLimit; int loopcounter = 0;
            await Log.LogMessage(context, String.Format("Cleaning logs older then {0} days", days));
            foreach (var msg in messages)
            {
                DateTime dateLimit = DateTime.UtcNow.Subtract(TimeSpan.FromDays(days));
                if (DateTime.Compare(msg.CreatedAt.DateTime, dateLimit) < 0) { loopcounter++; await msg.DeleteAsync(); }
            }
            await Log.LogMessage(context, String.Format("CleanLog finished - {0} messages deleted", loopcounter));
        }


        //outputs amount of users for all roles or a specific role
        public static async Task<string> Analyze(ICommandContext context, string searchparam)
        {
            IMessageChannel logchannel = await Log.GetLogChannel(context);
            if (logchannel == null) { await Log.LogMessage(context, "Unable to determine loggingchannel"); return null; }

            var messages = await logchannel.GetMessagesAsync(100000).Flatten();

            int days = 30; int loopcounter = 0;
            await Log.LogMessage(context, String.Format("Analyzing logs from the past {0} days", days));

            List<IMessage> logmessages = new List<IMessage>();

            foreach (var msg in messages)
            {
                DateTime dateLimit = DateTime.UtcNow.Subtract(TimeSpan.FromDays(days));
                if (DateTime.Compare(msg.CreatedAt.DateTime, dateLimit) > 0)
                {
                    loopcounter++;
                    logmessages.Add(msg);
                }          
            }

            List<LogMessage> output = new List<LogMessage>();

            foreach (var msg in logmessages)
            {
                string messagebody = msg.Content;
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                messagebody = regex.Replace(messagebody, " ");
                LogMessage logmessage = new LogMessage();
                logmessage.Timestamp = messagebody.Split(' ')[0];
                logmessage.Channel = messagebody.Split(' ')[1];
                logmessage.User = messagebody.Split(' ')[2];
                logmessage.Command = messagebody.Split(' ')[3];
                output.Add(logmessage);
            }

            if (searchparam != null)
            {
                IGuildUser usersearchparam = await User.GetUser(context, searchparam);
                if (usersearchparam != null) { output = output.FindAll(x => x.User.Contains(usersearchparam.ToString())); goto returnoutput; }
                output = output.FindAll(x => x.Command == searchparam);
            }

    returnoutput:
            var textTable = Temp.ToString((
            from msg in output        
            group msg by new { msg.Command, msg.User } into c
            orderby c.Key.Command ascending, c.Count() descending
            select new
            {
                Command = c.Key.Command,
                User = c.Key.User,
                Count = c.Count()
            }).ToList());
            
            return textTable;
        }
    }
}
