using ElectionWeb.Data;
using ElectionWeb.Models;
using ElectionWeb.Services;
using ElectionWeb.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUsers, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.ManageUsers, policy => policy.RequireAssertion(context => (
                context.User.HasClaim(c => c.Type == UserManagementClaims.ManageUsers && c.Value == "True")
    )));
	options.AddPolicy(Policies.ManageRoles, policy => policy.RequireAssertion(context => (
			   context.User.HasClaim(c => c.Type == RolesManagementClaims.ManageRoles && c.Value == "True")
   )));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
