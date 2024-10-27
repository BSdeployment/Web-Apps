using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "convert video to audio api by bs dotnet"}));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");


app.MapGet("hello", () =>
{
    return Results.Ok("hello");
});






app.MapPost("/convert", async (IWebHostEnvironment env, IFormFile videoFile) =>
{
    // תיקיית ffmpeg - שימוש בנתיב יחסי לשורש האתר
    var ffmpegPath = Path.Combine(env.ContentRootPath, "ffmpeg", "ffmpeg.exe");

    // בדיקה אם קובץ ה-ffmpeg קיים
    if (!File.Exists(ffmpegPath))
    {
        return Results.NotFound("FFmpeg executable not found.");
    }

    // הגדרת תיקייה לשמירת הקבצים שהועלו - שימוש בנתיב יחסי לשורש האתר
    var uploadsPath = Path.Combine(env.ContentRootPath, "UploadedFiles");

    // יצירת התיקייה אם היא לא קיימת
    if (!Directory.Exists(uploadsPath))
    {
        Directory.CreateDirectory(uploadsPath);
    }

    // שמירת הקובץ שהועלה
    var uploadedFilePath = Path.Combine(uploadsPath, videoFile.FileName);
    using (var stream = new FileStream(uploadedFilePath, FileMode.Create))
    {
        await videoFile.CopyToAsync(stream);
    }
    var outputFilePath = Path.Combine(uploadsPath, Path.GetFileNameWithoutExtension(videoFile.FileName) + ".mp3");
    try
    {
        // הגדרת נתיב לקובץ הפלט (אודיו)
       

        // הפעלת FFmpeg להמרת וידאו לאודיו
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = $"-i \"{uploadedFilePath}\" \"{outputFilePath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        await process.WaitForExitAsync();

        // החזרת קובץ הפלט שהומר
        if (File.Exists(outputFilePath))
        {
            //var streamOutput = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read);
            var audioBytes = await File.ReadAllBytesAsync(outputFilePath);
            return Results.File(audioBytes, "audio/mpeg", Path.GetFileName(outputFilePath));
        }
        else
        {
            return Results.BadRequest("faild 500");
        }
    }
    
   finally
    {
        if (File.Exists(uploadedFilePath))
            File.Delete(uploadedFilePath);
        if (File.Exists(outputFilePath))
            File.Delete(outputFilePath);
    }
}).DisableAntiforgery();




app.Run();





internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
