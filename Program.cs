using Microsoft.EntityFrameworkCore;
using ReminderList.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:80");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure the in-memory database
builder.Services.AddDbContext<TodoContext>(options =>
	options.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
