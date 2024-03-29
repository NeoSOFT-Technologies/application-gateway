﻿using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 

namespace ApplicationGateway.Api.Extensions
{
 
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/application-gateway/swagger/v1/swagger.json", "CleanArchitecture.WebApi");
            });
 
        } 
    }
}
