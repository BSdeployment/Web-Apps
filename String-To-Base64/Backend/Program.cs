using ApiBase64.ConverterServices;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(pol => pol.AddPolicy("all", p => p.
AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod()));

var app = builder.Build();

app.UseCors("all");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();


app.MapGet("/", () =>
{
    return "server now runing if you want docs you can add swagger keyword to url";
});




app.MapGet("/string-to-base64/{yourString}", ([FromRoute]string yourString)=>
{
    var res = ConverToBase64Serviece.EncodeToBase64(yourString);

   return Results.Ok(res);
});




app.MapGet("/base64-to-string/{base64}", ([FromRoute]string base64) =>
{
    var res = ConverToBase64Serviece.DecodeFromBase64(base64);

    return Results.Ok(res);
});


app.Run();
