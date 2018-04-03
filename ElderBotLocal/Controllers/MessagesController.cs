using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using ElderBotLocal.Dialogs;
using System.Linq;
using System;

namespace ElderBotLocal
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public int msgcount = 0;

        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                //Logging messages typed by the User to the Database
                LogMessageToDb.WriteToDatabase
                 (
                     conversationid: activity.Conversation.Id
                    , username: activity.From.Name
                    , channel: activity.ChannelId
                    , message: activity.Text
                 );

                //await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
                await Conversation.SendAsync(activity, () => new Dialogs.RootLuisDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    var welcome = "Hi!  My name is Elder Bot.  I can answer all manner of questions about the Church of Jesus Christ of Latter Day Saints also know as the Mormons.  What would you like to know?";
                    var reply = message.CreateReply(welcome);

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));
                    if (msgcount == 0)
                    {
                        LogMessageToDb.WriteToDatabase
                        (
                            conversationid: message.Conversation.Id
                            , username: "ElderBot"
                            , channel: message.ChannelId
                            , message: welcome
                        );
                        msgcount++;
                    }

                    //await connector.Conversations.ReplyToActivityAsync(reply);
                    connector.Conversations.ReplyToActivity(reply);
                }






            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}