using Final.WebApp.Handlers;
using Final.WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";      
        options.LogoutPath = "/Account/Logout";    
        options.ExpireTimeSpan = TimeSpan.FromDays(7); 
        options.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HttpAuthHandler>();

builder.Services.AddHttpClient<IProductApiService, ProductApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:ProductApiUrl"]!);
}).AddHttpMessageHandler<HttpAuthHandler>();

builder.Services.AddHttpClient<IUserApiService, UserApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:UserApiUrl"]!);
}).AddHttpMessageHandler<HttpAuthHandler>();

builder.Services.AddHttpClient<ICartApiService, CartApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:UserApiUrl"]!);
}).AddHttpMessageHandler<HttpAuthHandler>();

builder.Services.AddHttpClient<IOrderApiService, OrderApiService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderApiUrl"]!);
}).AddHttpMessageHandler<HttpAuthHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
