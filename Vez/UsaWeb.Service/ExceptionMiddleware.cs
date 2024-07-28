using System.Net.Mime;
using System.Net;
using System.Text.Json;
using UsaWeb.Service.Data;
using UsaWeb.Service.Models;

namespace UsaWeb.Service
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = string.Empty;
            if(ex.InnerException != null)
                message = ex.InnerException.Message;

            string url = context.Request.Host.Value + context.Request.Path.Value;
            using (Usaweb_DevContext db = new Usaweb_DevContext())
            {
                db.Log.Add(new Models.Log { 
                    Type = "API_ERROR",
                    ApplicationId = context.Response.StatusCode.ToString(),
                    Value1 = url,
                    Value2 = ex.Message,
                    Value3 = message,
                    MemberId = -1,
                    CreateTs = DateTime.Now,
                });
                db.SaveChanges();
            }


            var response = new CustomResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

    public class CustomResponse
    {
        public CustomResponse(int statusCode, string message, string details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }

}
