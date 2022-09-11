using Microsoft.AspNetCore.Mvc;

namespace SlackPollingApp.Model.Slack.Request
{
    public class SlashCommandInvokedRequest
    {
        [BindProperty(Name = "token")]
        public string Token { get; set; }
        
        [BindProperty(Name = "team_id")]
        public string TeamId { get; set; }
        
        [BindProperty(Name = "text")]
        public string Text { get; set; }
        
        [BindProperty(Name = "response_url")]
        public string ResponseUrl { get; set; }
        
        [BindProperty(Name = "trigger_id")]
        public string TriggerId { get; set; }
        
        [BindProperty(Name = "user_id")]
        public string UserId { get; set; }
        
        [BindProperty(Name = "user_name")]
        public string UserName { get; set; }

        [BindProperty(Name = "enterprise_id")]
        public string EnterpriseId { get; set; }
        
        [BindProperty(Name = "channel_id")]
        public string ChannelId { get; set; }
        
        [BindProperty(Name = "api_app_id")]
        public string ApiAppId { get; set; }
    }
}