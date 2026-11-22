using Microsoft.EntityFrameworkCore;
using MassTransit;
using OrdersAPI.Consumer;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("OrdersAPIContext") ?? throw new InvalidOperationException("Connection string 'OrdersAPIContext' not found.");

builder.Services.AddDbContext<OrdersAPIContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x => {
    x.AddConsumer<ProductCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) => {
        cfg.Host(new Uri("rabbitmq://localhost:4001"), h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("event-listener", e => {
            e.ConfigureConsumer<ProductCreatedConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
