using Microsoft.EntityFrameworkCore;
using Personal.DataContext;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//TODO: Read from environment variables
builder.Services.AddDbContext<WalletContext>(options => options.UseNpgsql(
"Host=localhost; Database=postgres; Username=postgres; Password=postgres; Port=5433"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
