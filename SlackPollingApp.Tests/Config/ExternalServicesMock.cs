using System.Reflection;
using Moq;
using SlackPollingApp.Core.Http;

namespace SlackPollingApp.Tests.Config;

public class ExternalServicesMock
{
    public Mock<IHttpRequestSender> HttpRequestSender { get; }

    public ExternalServicesMock()
    {
        HttpRequestSender = new Mock<IHttpRequestSender>();
    }

    public IEnumerable<(Type, object)> GetMocks()
    {
        return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x =>
            {
                var underlyingType = x.PropertyType.GetGenericArguments()[0];
                var value = x.GetValue(this) as Mock;

                return (underlyingType, value.Object);
            })
            .ToArray();
    }
}