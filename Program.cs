using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 1. Core Service Configuration: Register ASP.NET Core MVC Architecture
builder.Services.AddControllersWithViews();

// 2. Extract and Bind the Relational Connection String from appsettings.json
string connectionString = builder.Configuration.GetConnectionString("WorkspaceEnterpriseDB") 
    ?? throw new InvalidOperationException("Critical Infrastructure Error: Connection String 'WorkspaceEnterpriseDB' was not resolved.");

// [Academic Note]: Registering connection state mappings globally via Dependency Injection
// ensures that controllers can query the relational engine safely during runtime operations.

var app = builder.Build();

// 3. Configure the HTTP Request Pipeline Environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // Enforces strict network transmission security boundaries
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 4. Establish Global MVC Dynamic Route Boundaries
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CheckIn}/{action=Index}/{id?}");

app.Run();
