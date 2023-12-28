using AppSemTemplate.Data;
using AppSemTemplate.Extensions;
using AppSemTemplate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using AppSemTemplate.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddGlobalizationConfig()
    .AddMvcConfiguration()
    .AddElmahConfiguration()
    .AddIdentityConfiguration()
    .AddDependencyInjectionConfiguration();

var app = builder.Build();

app.UseMvcConfiguration();

app.Run();

//site.com/meus-pedidos/pedidos-cancelados