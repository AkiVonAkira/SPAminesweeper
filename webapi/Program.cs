using SPAminesweepersweeper.Hubs;

var builder = WebApplication.CreateBuilder(args);



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
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");

//app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
