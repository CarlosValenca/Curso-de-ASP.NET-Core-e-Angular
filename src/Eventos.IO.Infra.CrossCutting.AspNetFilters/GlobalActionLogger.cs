using Elmah.Io.Client;
using Elmah.Io.Client.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventos.IO.Infra.CrossCutting.AspNetFilters
{
    public class GlobalActionLoggerr : IActionFilter
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostingEnvironment _hostingEnviroment;

        public GlobalActionLoggerr(ILogger<GlobalExceptionHandlingFilter> logger,
                                             IHostingEnvironment hostingEnviroment)
        {
            _logger = logger;
            _hostingEnviroment = hostingEnviroment;
        }

        // Estou registrando aqui erros de auditoria
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_hostingEnviroment.IsDevelopment())
            {
                // Objeto anonimo com algumas informações determinadas
                var data = new
                {
                    Version = "v1.0",
                    User = context.HttpContext.User.Identity.Name,
                    IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Hostname = context.HttpContext.Request.Host.ToString(),
                    AreaAccessed = context.HttpContext.Request.GetDisplayUrl(),
                    Action = context.ActionDescriptor.DisplayName,
                    TimeStamp = DateTime.Now
                };

                // Estou fazendo o log de erro e deixando de usar o recurso de logo de erro, este log não vai para o Elmah pois está sendo tratado aqui
                // ssbcvp - Por questões de performance estou desabilitando o log
                // _logger.LogInformation(1, "Log de Auditoria", data.ToString());
            }

            // ssbcvp - voltar aqui - não consegui pegar o log de auditoria no swagger
            if (_hostingEnviroment.IsProduction())
            {

                var message = new CreateMessage
                {
                    Version = "v1.0",
                    Application = "Eventos.IO",
                    Source = "GlobalActionLoggerFilter",
                    User = context.HttpContext.User.Identity.Name,
                    Hostname = context.HttpContext.Request.Host.Host,
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    DateTime = DateTime.Now,
                    Method = context.HttpContext.Request.Method,
                    StatusCode = context.HttpContext.Response.StatusCode,
                    Cookies = context.HttpContext.Request?.Cookies?.Keys.Select(k => new Item(k, context.HttpContext.Request.Cookies[k])).ToList(),
                    Form = Form(context.HttpContext),
                    ServerVariables = context.HttpContext.Request?.Headers?.Keys.Select(k => new Item(k, context.HttpContext.Request.Headers[k])).ToList(),
                    QueryString = context.HttpContext.Request?.Query?.Keys.Select(k => new Item(k, context.HttpContext.Request.Query[k])).ToList(),
                    Data = context.Exception?.ToDataList(),
                    Detail = JsonConvert.SerializeObject(new { DadoExtra = "Dados a mais", DadoInfo = "Pode ser um Json" })
                };

                // Fiz a autenticação no Elmah
                var client = ElmahioAPI.Create("ccde64921fbd4ef69bd01c1a097251af");
                client.Messages.Create(new Guid("809c3d10-450a-4395-bd4a-7fa23aaae94f").ToString(), message);
            }
        }

        private static List<Item> Form(HttpContext httpContext)
        {
            try
            {
                return httpContext.Request?.Form?.Keys.Select(k => new Item(k, httpContext.Request.Form[k])).ToList();
            }
            catch (InvalidOperationException)
            {
                // Request not a form POST or similar
            }

            return null;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
