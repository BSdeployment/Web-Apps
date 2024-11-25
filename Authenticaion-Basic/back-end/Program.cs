using ApiWuthHush.ServicesCol;
using ApiWuthHush.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.MyServices();

//builder.Services.AddCors(cor => cor.AddPolicy("GetAll", policy => policy.AllowAnyHeader()
//.AllowAnyMethod()
//.WithOrigins("https://localhost:7193")
//.AllowCredentials()));

builder.Services.AddCors(cor => cor.AddPolicy("GetAll", policy => policy.AllowAnyHeader()
.AllowAnyMethod()
.AllowAnyOrigin()
));

builder.Services.AddMyAutentication();

builder.Services.AddAuthorization();





var app = builder.Build();


app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"].ToString();

    if (string.IsNullOrEmpty(authHeader))
    {
        // אם לא נשלח טוקן בכותרת, נסה לקבל אותו מה-Cookie
        var token = context.Request.Cookies["AuthToken"]; // מקבל את ה-JWT מה-Cookie

        if (!string.IsNullOrEmpty(token))
        {
            // הוסף את הטוקן לכותרת Authorization
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }
    }

    
    await next(); // ממשיך לבקשה הבאה
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("GetAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
