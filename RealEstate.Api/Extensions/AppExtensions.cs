using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace RealEstate.Api.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app,
                                       IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(c =>
            {
                c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
            });


            app.UseSwaggerUI(c =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint(
                       $"/swagger/{desc.GroupName}/swagger.json",
                       $"RealEstate API {desc.GroupName.ToUpperInvariant()}");
                    c.DefaultModelRendering(ModelRendering.Model);
                }
            });
        }

    }
}
