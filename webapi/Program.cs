using Microsoft.EntityFrameworkCore;
using SPAminesweepersweeper.Hubs;
using webapi.Areas.Identity.Data;
using webapi.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("webapiContextConnection") ?? throw new InvalidOperationException("Connection string 'webapiContextConnection' not found.");

builder.Services.AddDbContext<webapiContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<webapiUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    //options.Password.RequireNonAlphanumeric = false;
    //options.Password.RequireLowercase = false;
    //options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<webapiContext>();

//builder.Services.AddAuthentication()
//    .AddIdentityServerJwt();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthorization();

//app.UseAuthentication();

//app.UseIdentityServer();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

//app.MapFallbackToFile("index.html");
app.Run();
