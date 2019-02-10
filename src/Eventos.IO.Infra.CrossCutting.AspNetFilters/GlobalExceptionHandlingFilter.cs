using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace Eventos.IO.Infra.CrossCutting.AspNetFilters
{
    public class GlobalExceptionHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostingEnvironment _hostingEnviroment;

        public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger,
                                             IHostingEnvironment hostingEnviroment)
        {
            _logger = logger;
            _hostingEnviroment = hostingEnviroment;
        }


        // Captura e faz o log do erro
        public void OnException(ExceptionContext context)
        {
            if(_hostingEnviroment.IsProduction())
            {
                // Estou fazendo o log de erro e deixando de usar o recurso de log de erro da controller ErrosController
                _logger.LogError(1, context.Exception, context.Exception.Message);
            }

            var result = new ViewResult { ViewName = "Error" };
            var modelData = new EmptyModelMetadataProvider();
            result.ViewData = new ViewDataDictionary(modelData, context.ModelState)
            {
                {"MensagemErro", context.Exception.Message}
            };

            context.ExceptionHandled = true;
            context.Result = result;
        }
    }
}
