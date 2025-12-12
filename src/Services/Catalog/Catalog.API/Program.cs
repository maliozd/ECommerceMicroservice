var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString")!;
builder.Services.AddNpgsqlDataSource(connectionString);

builder.Services.AddMarten(options =>
{
    options.DatabaseSchemaName = "public";
})
.UseLightweightSessions()
.UseNpgsqlDataSource();

var app = builder.Build();

app.MapCarter();
app.Run();