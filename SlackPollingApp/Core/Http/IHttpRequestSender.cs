using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SlackPollingApp.Core.Config;

namespace SlackPollingApp.Core.Http
{
    public interface IHttpRequestSender
    {
        Task<string> PostToSlackAsync(string path, object body);
    }
}