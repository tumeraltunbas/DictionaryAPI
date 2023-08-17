using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Infastructure.Security.Hash;
using DictionaryAPI.Infastructure.Services.EmailService;
using DictionaryAPI.Infastructure.Utils;
using DictionaryAPI.Persistence.Concretes.Business;
using DictionaryAPI.Persistence.Concretes.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//IoC
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUtilService, UtilService>();
builder.Services.AddSingleton<IUserDal, UserDal>();
builder.Services.AddSingleton<IHashHelper, HashHelper>();
builder.Services.AddSingleton<IEmailService, EmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
