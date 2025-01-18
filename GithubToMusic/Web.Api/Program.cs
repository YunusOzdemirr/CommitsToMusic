using GithubCommitsToMusic;
using GithubCommitsToMusic.Extensions;
using GithubCommitsToMusic.Infrastructure;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Middlewares;
using GithubCommitsToMusic.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ICommitService, CommitService>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Assembly>();
});
builder.Services.AddLogging();
builder.Services.AddMemoryCache();
builder.Services.AddInfrastructure(builder.Configuration);


if (builder.Environment.ContentRootPath.Contains(@"\"))
    if (!Directory.Exists(builder.Environment.ContentRootPath + @"\wwwroot" + @"\Sheets"))
        Directory.CreateDirectory(builder.Environment.ContentRootPath + @"\wwwroot" + @"\Sheets");

if (builder.Environment.ContentRootPath.Contains(@"/"))
    if (!Directory.Exists(builder.Environment.ContentRootPath + "/wwwroot" + "/Sheets"))
        Directory.CreateDirectory(builder.Environment.ContentRootPath + "/wwwroot" + "/Sheets");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.ApplyMigrations();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionMiddleware>();
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
