using Microsoft.AspNetCore.Http;
using ServerLib.Utill;
using Share.Const;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    /// <summary>
    /// 응답을 보낼때 종합적인 처리를 하는 미들웨어 
    /// </summary>
    public class ResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public class ErrorDetails
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public override string ToString()
            {
                return JsonSerializer.Serialize(this);
            }
        }

        public ResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                httpContext.Response.ContentType = "application/json";
                if (!httpContext.Request.Headers.TryGetValue("IsNewUser", out _))
                {
                    var playerId = PlayerDataUtill.GetPlayerId(httpContext.Request.Headers);
                    httpContext.Response.Headers.Add(HeaderConst.HeaderPlayerId, playerId.ToString());
                }

                await _next(httpContext);
            }
            catch (BadHttpRequestException ex)
            {
                Console.WriteLine($"BadHttpRequestException: {ex}");
                httpContext.Response.StatusCode = ex.StatusCode;

                /*await httpContext.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = ex.Message
                }.ToString());*/
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"NotSupportedException: {ex}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ArgumentException: {ex}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.ResetContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex.Message}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
    }
}
