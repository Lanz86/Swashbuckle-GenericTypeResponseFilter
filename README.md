# https-github.com-Lanz86-Swashbuckle-GenericTypeResponseFilter
Declare Generic not buildable Swagger Response

If you have a generic not buildable type in response of api add in swagger configuration this filter with you generic concrate type
example:

<code>
 services.AddSwaggerGen(c =>
            {
                c.OperationFilter<GenericTypeResponseFilter>(typeof(ApiResult<>));
            }):
</code>