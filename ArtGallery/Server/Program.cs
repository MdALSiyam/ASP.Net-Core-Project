using Microsoft.EntityFrameworkCore;
using Server.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CoreArtDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("con") ??
        throw new InvalidOperationException("Connection string not matched"));
});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAuthorization();
app.UseCors(policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
app.MapControllers();

app.Run();
