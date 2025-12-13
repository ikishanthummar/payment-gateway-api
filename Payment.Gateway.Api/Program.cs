using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Api.Middleware;
using Payment.Gateway.Data;
using Payment.Gateway.Services.Concrete;
using Payment.Gateway.Services.Interface;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
                .AddJsonOptions(o =>
                  o.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter(namingPolicy: null, allowIntegerValues: false)
                  ));

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IWebhookService, WebhookService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<WebhookHmacVerificationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
