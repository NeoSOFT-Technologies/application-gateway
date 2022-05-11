﻿using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

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
            var permissionClaims = context.User.Claims.Where(x => x.Type == "groups").ToList();
            //var token = context.Request.Headers["Authorization"].ToString();
            //    token = token.Substring(7);
            //var handler = new JwtSecurityTokenHandler();
            //var jwt = handler.ReadJwtToken(token);
            //var groups = jwt.Claims.First(claim => claim.Type == "groups").Value;

            List<string> permissions = new List<string>();
            foreach (var claim in permissionClaims)
            {
                permissions.Add(claim.Value);
            }
            var roleClaims = context.User.Claims.Where(x => x.Type == "realm_access").ToList();
            string requiredPermission = "";
            switch (context.Request.Method)
            {
                case "GET":
                    requiredPermission = "view";
                    break;
                case "POST":
                    requiredPermission = "create";
                    break;
                case "PUT":
                    requiredPermission = "edit";
                    break;
                case "DELETE":
                    requiredPermission = "delete";
                    break;
                default:
                    break;
            }
            if (permissions.Contains(requiredPermission) || requiredPermission == "")
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
