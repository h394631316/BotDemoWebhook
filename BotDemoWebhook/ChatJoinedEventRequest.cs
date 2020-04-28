using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotDemoWebhook.Web
{
    public class ChatJoinedEventRequest
    {
        public EnumThirdEventType eventType { get; set; }
        public string chatId { get; set; }
        public int campaignId { get; set; }
        public ThirdPartyVisitorInfo visitorInfo { get; set; }
    }
    public class ThirdPartyVisitorInfo
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public long? id { get; set; }
        public string ssoId { get; set; }
        public List<int> segments { get; set; }
    }
    public enum EnumThirdEventType
    {
        questionAsked,
        intentClicked,
        locationShared,
        formSubmitted,
        chatJoined
    }
}