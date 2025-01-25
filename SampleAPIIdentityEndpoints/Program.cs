using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SampleAPIASPIdentityEndpoint.Database;
using Microsoft.AspNetCore.Identity.UI;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(Options =>
{
    Options.Password.RequireDigit = true;
    Options.Password.RequireLowercase = true;
    Options.Password.RequireNonAlphanumeric = true;
    Options.Password.RequireUppercase = true;
    Options.Password.RequiredLength = 8;
    Options.SignIn.RequireConfirmedEmail = true;
    Options.User.RequireUniqueEmail = true;
})
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

app.MapControllers();

app.Run();