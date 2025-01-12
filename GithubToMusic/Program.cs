using GithubCommitsToMusic;
using GithubCommitsToMusic.Infrastructure;
using GithubCommitsToMusic.Interfaces;
using GithubCommitsToMusic.Middlewares;
using GithubCommitsToMusic.Services;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
builder.Services.AddScoped<ICommitService, CommitService>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<Assembly>();
});
builder.Services.AddLogging();
builder.Services.AddMemoryCache();


if (builder.Environment.ContentRootPath.Contains(@"\"))
    if (!Directory.Exists(builder.Environment.ContentRootPath + @"\wwwroot" + @"\Uploads"))
        Directory.CreateDirectory(builder.Environment.ContentRootPath + @"\wwwroot" + @"\Uploads");

if (builder.Environment.ContentRootPath.Contains(@"/"))
    if (!Directory.Exists(builder.Environment.ContentRootPath + "/wwwroot" + "/Uploads"))
        Directory.CreateDirectory(builder.Environment.ContentRootPath + "/wwwroot" + "/Uploads");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        builder.Environment.ContentRootPath + "/wwwroot" + "/Sheets"),
    //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")),
    RequestPath = new PathString("/Sheets"),
});

app.Run();
