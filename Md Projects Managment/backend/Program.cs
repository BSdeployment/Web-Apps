using Backend.Data;
using Backend.Repositories;
using Backend.Services;
using Backend.Utils;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

var builder = WebApplication.CreateBuilder(args);


#if DEBUG == false
// Use production appsettings in production
var serverConfig = builder.Configuration.GetSection("ServerConfig");
var desiredPort = serverConfig.GetValue<int>("Port", 8080);
var openBrowser = serverConfig.GetValue<bool>("OpenBrowserOnStart", true);
var port = FindAvailablePort(desiredPort);
if (port != desiredPort)
    Console.WriteLine($"Port {desiredPort} is taken, using port {port} instead.");
builder.WebHost.UseUrls($"http://localhost:{port}");
#endif


// 📁 AppData paths
var appDataRoot = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "MdWriterApp" // 🔁 שנה לשם האפליקציה שלך
);

Directory.CreateDirectory(appDataRoot);

var dbPath = Path.Combine(appDataRoot, "writer.db");
var storagePath = Path.Combine(appDataRoot, "Storage");

Directory.CreateDirectory(storagePath);




builder.Services.AddCors(p => p.AddPolicy("allow",policy =>
{
 policy.AllowAnyHeader();
    policy.AllowAnyMethod();
    policy.AllowAnyOrigin();
}));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("Storage"));


// 📦 StorageSettings דינמי
builder.Services.Configure<StorageSettings>(options =>
{
    options.RootPath = storagePath;
});





//builder.Services.AddDbContext<WritingDbContext>(options =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//    options.UseSqlite(connectionString);
//});

// 🗄️ SQLite בתוך AppData
builder.Services.AddDbContext<WritingDbContext>(options =>
{
    options.UseSqlite($"Data Source={dbPath}");
});




builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ILinkRepository, LinkRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<INoteService, NoteService>();

var app = builder.Build();


app.UseCors("allow");

//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<WritingDbContext>();
//    db.Database.Migrate();

//    var storage = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<StorageSettings>>().Value;
//    Directory.CreateDirectory(Path.GetFullPath(storage.RootPath));
//}

// 🧱 DB + Storage init
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<WritingDbContext>();
    db.Database.Migrate();

    Directory.CreateDirectory(storagePath);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


#if DEBUG == false
app.UseDefaultFiles(new DefaultFilesOptions {
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "frontend"))
});
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "frontend"))
});
app.MapFallbackToFile("index.html", new StaticFileOptions {
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(AppContext.BaseDirectory, "frontend"))
});

if (openBrowser)
    Process.Start(new ProcessStartInfo($"http://localhost:{port}") { UseShellExecute = true });
#endif

app.Run();

static int FindAvailablePort(int startPort)
{
    for (int p = startPort; p < startPort + 100; p++)
    {
        var listener = new TcpListener(IPAddress.Loopback, p);
        try { listener.Start(); listener.Stop(); return p; }
        catch { }
    }
    throw new Exception("No available port found!");
}
