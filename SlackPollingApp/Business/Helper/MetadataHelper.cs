namespace SlackPollingApp.Business.Helper
{
    public static class MetadataHelper
    {
        public static string FormatModalMetadata(string channelId) => $"channelId={channelId}";

        public static string GetChannelId(string metadata) => metadata.Replace("channelId=", "");
    }
}