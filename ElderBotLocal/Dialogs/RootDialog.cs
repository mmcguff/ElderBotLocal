using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace ElderBotLocal.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            //Lets use something like this to prevent over long input.  
            int length = (activity.Text ?? string.Empty).Length;  


            //var response = "ElderBot is responding";
            //This will call an method that will pull from the database.  

            var topic = activity.Text;

            var response = GetBotResponse.Response(topic);


            // return bot reply to the user
            await context.PostAsync($"{response}");
            
            //write bot replay to MessageLogDatabase
            LogMessageToDb.WriteToDatabase
            (
                   conversationid: activity.Conversation.Id
                 , username: "ElderBot"
                 , channel: activity.ChannelId
                 , message: response
            );

            context.Wait(MessageReceivedAsync);
        }
    }
}