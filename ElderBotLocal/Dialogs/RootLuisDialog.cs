﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace ElderBotLocal.Dialogs
{
    [LuisModel("ac704d12-bddc-4e78-b0bd-73c8d161dc9c", "f30e1c3ce6f64868b20e95f28109d045")]
    [Serializable]
    public class RootLuisDialog : LuisDialog<object>
    {
       
        dynamic t;

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
           
            t = result.TopScoringIntent.Intent;
            string topic = t;

            string message = GetBotResponse.Response(topic);

            //write to database
            LogMessageToDb.WriteToDatabase
            (
                conversationid: context.Activity.Conversation.Id
                , username: "ElderBot"
                , channel: context.Activity.ChannelId
                , message: message
            );

            await context.PostAsync(message);
            
            context.Wait(this.MessageReceived);
        }

    }
}