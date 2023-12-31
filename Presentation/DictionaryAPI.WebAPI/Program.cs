﻿using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.Abstracts.Security.JWT;
using DictionaryAPI.Application.Abstracts.Security.TwoFactorAuth;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Infastructure.Security.Hash;
using DictionaryAPI.Infastructure.Security.Jwt;
using DictionaryAPI.Infastructure.Security.TwoFactorAuth;
using DictionaryAPI.Infastructure.Services.EmailService;
using DictionaryAPI.Infastructure.Utils;
using DictionaryAPI.Persistence.Concretes.Business;
using DictionaryAPI.Persistence.Concretes.DAL;
using DictionaryAPI.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using DictionaryAPI.Application.Abstracts.Services.StorageService;
using DictionaryAPI.Infastructure.Services.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//IoC
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUserDal, UserDal>();
builder.Services.AddSingleton<ITitleService, TitleService>();
builder.Services.AddSingleton<ITitleDal, TitleDal>();
builder.Services.AddSingleton<IEntryService, EntryService>();
builder.Services.AddSingleton<IEntryDal, EntryDal>();
builder.Services.AddSingleton<IEntryFavoriteService, EntryFavoriteService>();
builder.Services.AddSingleton<IEntryFavoriteDal, EntryFavoriteDal>();
builder.Services.AddSingleton<IEntryVoteService, EntryVoteService>();
builder.Services.AddSingleton<IEntryVoteDal, EntryVoteDal>();
builder.Services.AddSingleton<IUtilService, UtilService>();
builder.Services.AddSingleton<IHashHelper, HashHelper>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<DictionaryContext, DictionaryContext>();
builder.Services.AddSingleton<IJwtHelper, JwtHelper>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ITwoFactorAuthHelper, TwoFactorAuthHelper>();
builder.Services.AddSingleton<IStorageService, StorageService>();


//Cors
//AddCors servisi, cors politikalarını ayarlamamızı sağlayan bir servis.
builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(policy => {

        policy.WithOrigins("http://localhost:3000", "https://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();

    });

});


//JWT validation configuration. From Microsoft.AspNetCore.AuthenticationJwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecurityKey")))
            
        };

    });

//JSON
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//wwwroot - static files
app.UseStaticFiles();

//Cors
app.UseCors();

app.UseHttpsRedirection();

//Jwt middleware
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
