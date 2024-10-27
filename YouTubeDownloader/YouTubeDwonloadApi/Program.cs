using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YouTubeDwonloadApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(core => core.AddPolicy("AllowAll", policy => policy.
AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapGet("/", () =>"server now running if you wand to see endpoint docs insert to the url swagger");

app.MapPost("genlink", (IWebHostEnvironment env, [FromBody]string url,CancellationToken cancellation) =>
{
    var genLinkPath = Path.Combine(env.ContentRootPath, "ap", "genlink.exe");

    // בדיקה אם קובץ ה-ffmpeg קיים
    if (!File.Exists(genLinkPath))
    {
        return Results.NotFound("genLinkPath executable not found.");
    }

    string? result = GenereteYTLink.LinkVideo(genLinkPath, url);

    if(result == null)
    {
        return Results.NotFound("internal Error");
    }

    return Results.Ok(new {Result = result});
});

app.Run();

