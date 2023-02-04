using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace LnzSoftware.Swashbuckle.GenericTypeResponseFilter
{
    public class GenericTypeResponseFilter : IOperationFilter
    {
        private readonly Type _genericType;
        public GenericTypeResponseFilter(Type genericType)
        {
            _genericType = genericType;
        }

        private static List<string> contentTypes = new List<string>
        {
            "text/plain",
            "application/json",
            "text/json"
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var returnType = context.MethodInfo.ReturnType;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                returnType = context.MethodInfo.ReturnType.GetTypeInfo().GenericTypeArguments[0];
            }

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == _genericType.GetGenericTypeDefinition())
            {
                var type = returnType.GenericTypeArguments[0];

                foreach (var response in operation.Responses)
                {
                    if (response.Key == StatusCodes.Status200OK.ToString())
                    {
                        var schema = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
                        foreach (var contentType in contentTypes)
                        {
                            operation.Responses[StatusCodes.Status200OK.ToString()].Content.Add(contentType, new OpenApiMediaType { Schema = schema });
                        }
                    }
                }
            }
        }
    }
}
