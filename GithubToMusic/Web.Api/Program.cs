using GithubCommitsToMusic;
using GithubCommitsToMusic.Extensions;
using GithubCommitsToMusic.Infrastructure;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Middlewares;
using GithubCommitsToMusic.Services;
using GithubCommitsToMusic.Time;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.AddInfrastructureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GitHubToMusic API",
        Description = "An ASP.NET Core Web API for generating Music items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ICommitService, CommitService>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Assembly>();
});
builder.Services.AddLogging();
builder.Services.AddMemoryCache();
builder.Services.Configure<KestrelServerOptions>(options => {
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.NoCertificate);
});

if (builder.Environment.ContentRootPath.Contains(@"\"))
    if (!Directory.Exists(builder.Environment.ContentRootPath + @"\wwwroot" + @"\Sheets"))
        Directory.CreateDirectory(builder.Environment.ContentRootPath + @"\wwwroot" + @"\Sheets");

if (builder.Environment.ContentRootPath.Contains(@"/"))
    if (!Directory.Exists(builder.Environment.ContentRootPath + "/wwwroot" + "/Sheets"))
        Directory.CreateDirectory(builder.Environment.ContentRootPath + "/wwwroot" + "/Sheets");

var app = builder.Build();

await app.InitialiseDatabaseAsync();
app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x
    .WithOrigins("https://localhost:3000", "http://localhost:3000", "https://commitstomusic.yunusozdemirr.com")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .SetIsOriginAllowed(origin => IsAllowedOrigin(origin)));

bool IsAllowedOrigin(string origin)
{
    return origin == "https://commitstomusic.yunusozdemirr.com" || origin == "https://localhost:3000" 
        || origin == "http://localhost:3000" ;
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwagger();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        builder.Environment.ContentRootPath + "/wwwroot" + "/Sheets"),
    //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")),
    RequestPath = new PathString("/Sheets"),
});

app.Run();
