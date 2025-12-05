using BillByte.Data;
using BillByte.Hubs;
using BillByte.Interface;
using BillByte.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

// ✅ Add CORS BEFORE builder.Build()
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
          .WithOrigins("http://localhost:4200")   // EXACT origin (no wildcard)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();                    // allow credentials -> cannot use '*'
    });
});

builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add repository
builder.Services.AddScoped<PageRepository>();
builder.Services.AddScoped<FoodTypeRepository>();
builder.Services.AddScoped<IBillByteMenuRepository, BillByteMenuRepository>();
builder.Services.AddScoped<IBusinessUnitSettingRepository, BusinessUnitSettingRepository>();
builder.Services.AddSignalR();
builder.Services.AddScoped<TableOrderRepository>();
builder.Services.AddScoped<BillByteMenuRepository>();
builder.Services.AddScoped<ITableOrderRepository, TableOrderRepository>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Use CORS after Build
//app.UseCors("AllowAngular");
app.UseCors("CorsPolicy");

// Configure Swagger for Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<TableHub>("/tableHub");
//app.MapHub<TableHub>("/hubs/table");

app.MapControllers();

app.Run();
