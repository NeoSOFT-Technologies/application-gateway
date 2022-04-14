using Newtonsoft.Json;

namespace ApplicationGateway.Api.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string method = context.Request.Method; // For Scope
            string actionName = context.Request.RouteValues["action"].ToString();
            string controllerName = context.Request.RouteValues["controller"].ToString(); // For Resource
            if (controllerName == "Auth")
            {
                await _next(context);
            }
            //TODO: Following condition should be based on helper's response
            if (controllerName != "Transformer") 
            {
                
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject("403 Not authorized");
                await context.Response.WriteAsync(result);
            }
        }
    }
}
