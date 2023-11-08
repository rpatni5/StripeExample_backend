using Stripe;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<IStripeService, StripeService>();


StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(builder => builder
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowAnyOrigin()
    );

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
