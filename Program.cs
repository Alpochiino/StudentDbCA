using StudentWebApp.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<StudentController>(client =>
{
    client.BaseAddress = new Uri("https://databaseca-f0bmfzchfccuasg2.northeurope-01.azurewebsites.net/");
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Student/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Student}/{action=Index}/{id?}");

app.Run();
