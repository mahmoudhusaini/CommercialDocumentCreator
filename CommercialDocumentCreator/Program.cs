using CommercialDocumentCreator.Classes.Data;
using CommercialDocumentCreator.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1.
builder.Services.AddControllers();

// 2. DI Container Service
builder.Services.AddTransient<InvoiceHelper>();
builder.Services.AddTransient<QuotationHelper>();
builder.Services.AddTransient<ReceiptHelper>();
builder.Services.AddTransient<TradeTallyHelper>();
builder.Services.AddTransient<StatementHelper>();
builder.Services.AddTransient<CategoryHelper>();
builder.Services.AddTransient<SharedUtilitiesHelper>();



// 3.AppDbContext Service
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(connectionString);

//}, ServiceLifetime.Transient);




var app = builder.Build();
app.UseStaticFiles();
// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();



app.MapFallbackToFile("clientapp/home/home.html");
app.MapControllers();

app.Run();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

app.UseCors("AllowAll");


