using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using CADR.Administrations.Api.Controllers;
using CADR.Administrations.Api.Infrastructures;
using CADR.Api.DI;
using CADR.Api.Infrastructures;
using CADR.Common.Mvc.Extensions;
using CADR.Common.Mvc.Filters;
using CADR.Context;
using CADR.Context.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);
if (args.Length > 1 && args.Contains(CadrDesignTimeContextFactory.MigrateDatabaseKey))
{
    await CadrContextFactory.Migrate(args);
    return;
}
builder.Services.AddHealthChecks();
builder.Services.AddCadrLogger(builder.Configuration, "CADR.Api");
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddCadrContext(builder.Environment.IsDevelopment());

#region Controllers

var accountAssembly = typeof(AccountController).GetTypeInfo().Assembly;
var controllers = builder.Services.AddControllers(config =>
{
    config.Conventions.Add(new ApiExplorerGroupConvention());
    config.Filters.Add<ApiExceptionFilter>();
    config.Filters.Add<AdministrationExceptionFilter>();
    config.Filters.Add<CryptographicExceptionFilter>();
})
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()))
    .AddApplicationPart(accountAssembly);

if (builder.Environment.IsEnvironment(ApiConstants.IntegrationgTestingEnvironment))
{
    controllers.AddControllersAsServices();
}

#endregion

#region Documentations

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version"));
})
    .AddApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Заголовок авторизации JWT с использованием схемы Bearer. \r\n\r\n Введите ваш токен в текстовое поле ниже.\r\n\r\nПример: \"eyJhbGciOiJI**********Eomy5nEqws\"",
        });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = JwtBearerDefaults.AuthenticationScheme, },
                Scheme = "oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });

    // Загружаем всю документацию и шаманим с enum, чтобы в схеме были видны summary
    // и тестировщики больше не ругались, что они не понимают что всё это значит
    var dir = new DirectoryInfo(AppContext.BaseDirectory);
    foreach (var fi in dir.EnumerateFiles("*.xml"))
    {
        var doc = XDocument.Load(fi.FullName);
        options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
        options.SchemaFilter<DescribeEnumMembers>(doc);
        options.OperationFilter<DocumentationIgnoreFilter>();
    }

    // Правильно отслеживать nullable
    options.UseAllOfToExtendReferenceSchemas();
    // Не ставить nullable для reference-типов, которые не nullable
    options.SupportNonNullableReferenceTypes();

    options.MapType<FileContentResult>(() => new OpenApiSchema { Type = "file" });
    options.OperationFilter<FormFileFilter>();
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations();
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

#endregion

builder.Services.AddCors(options =>
{
    var section = builder.Configuration.GetSection("CorsPolicyOrigins");
    var origins = section.Get<string[]>() ?? Array.Empty<string>();
    options.AddPolicy(ApiConstants.DebugCorsPolicyName, config =>
    {
        config.WithOrigins(origins)
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddDataProtection();
builder.Services.RegisterModule<ApiModule>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseDocumentation(apiVersionDescriptionProvider);
    app.UseCors(ApiConstants.DebugCorsPolicyName);
}
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();


/// <summary>
/// Маркерный класс для тестирования зависимостей
/// </summary>
public partial class Program
{
}
