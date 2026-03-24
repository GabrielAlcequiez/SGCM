using SGCM.Domain.Exceptions;

namespace SGCM.API.Middleware
{
    public class ManejadorExcepcionesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;

        public ManejadorExcepcionesMiddleware(RequestDelegate next, ILogger<ManejadorExcepcionesMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ExcepcionDominio ex)
            {
                await EscribirRespuestaErrorAsync(context, ObtenerStatusCode(ex), ex.CodigoError, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no manejado: {Message}", ex.Message);
                await EscribirRespuestaErrorAsync(context, 500, "ERROR_INTERNO", "Ocurrió un error inesperado. Contacte al administrador.");
            }
        }

        private static int ObtenerStatusCode(ExcepcionDominio ex) => ex switch
        {
            ExcepcionNoEncontrado => 404,
            ExcepcionReglaNegocio or ExcepcionValidacion => 400,
            _ => 500
        };

        private static async Task EscribirRespuestaErrorAsync(HttpContext context, int statusCode, string codigo, string mensaje)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var respuesta = new
            {
                ok = false,
                codigo = codigo,
                mensaje = mensaje
            };

            await context.Response.WriteAsJsonAsync(respuesta);
        }
    }
}
