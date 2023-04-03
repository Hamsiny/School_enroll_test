using System.Net;
using System.Text;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace UxtrataTask.Middleware;

public class HttpExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public HttpExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (HttpException httpException)
        {
            context.Response.StatusCode = httpException.StatusCode;
            context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            SystemMessage userMessage;
            userMessage = httpException.UserMessage ?? SystemMessage.GenericError();

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(new HttpExceptionResponse
                    {
                        Message = userMessage,
                        InnerMessage = httpException.InnerException != null
                            ? httpException.InnerException.Message
                            : httpException.ToString(),
                        StatusCode = httpException.StatusCode
                    },
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), Encoding.UTF8);
        }
        catch (Exception e)
        {
            const int statusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();

            await context.Response.WriteAsync(
                JsonConvert.SerializeObject(new HttpExceptionResponse
                    {
                        Message = SystemMessage.GenericError(),
                        InnerMessage = e.ToString(),
                        StatusCode = statusCode
                    },
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), Encoding.UTF8);
        }
    }
}