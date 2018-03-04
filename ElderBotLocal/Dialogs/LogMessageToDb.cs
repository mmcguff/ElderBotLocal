using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElderBotLocal.Dialogs
{
    public class LogMessageToDb
    {
        public static void WriteToDatabase(string conversationid, string username, string channel, string message)
        {
            //Instantiate entity frameowork connection
            Models.ElderBotLocalDbEntities db = new Models.ElderBotLocalDbEntities();

            //Create a new MessageLog Object
            Models.MessageLog NewMsgLog = new Models.MessageLog
            {
                // Set the properties on the UserLog object
                ConversationId = conversationid,
                UserName = username,
                Channel = channel,
                Message = message,//Need to truncate to 500 chracters but doesn't work right now. 
                Created = DateTime.UtcNow
            };

            //Add the NewMsgLog Object to the actual Database
            db.MessageLogs.Add(NewMsgLog);

            db.SaveChanges();
        }
    }
}