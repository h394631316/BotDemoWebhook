using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotDemoWebhook.Web
{
    public class WebhookSendData
    {
        public string SessionId { get; set; }
        public string CampaignId { get; set; }
        public string Question { get; set; }
        public string QuestionId { get; set; }
        public string IntentId { get; set; }
        public string BotId { get; set; }
        public string ChatId { get; set; }
        public string VisitorId { get; set; }
        public List<WebhookResponseFormValues> FormValues { get; set; }
        public StructVisitor Visitor { get; set; }
    }
    public class StructVisitor
    {
        public string ssoid { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
    public class WebhookResponseFormValues
    {
        public string id;
        public string label;
        public string value;
    }
    public class StructApiMessages
    {
        public List<StructApiMessage> messages { get; set; }
    }
    public class StructApiMessage
    {
        public string text;
    }
}