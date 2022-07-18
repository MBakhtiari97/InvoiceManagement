using InvoiceManagement.Authorization;
using InvoiceManagement.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region SQL Server Configurations

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

#endregion

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#region Identity

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


#endregion

builder.Services.AddRazorPages();

#region Configure Identity

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
});


#endregion

#region Authorization

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

#endregion

#region IoC

builder.Services.AddScoped<IAuthorizationHandler, InvoiceCreatorAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, InvoiceManagerAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, InvoiceAdminAuthorizationHandler>();

#endregion


var app = builder.Build();

//Calling to seeding the data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //It will automatically run the migration process ,
    //so if we run the project migration and database update will happen
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    //Getting the password from secret manager
    var seedUserPass = builder.Configuration.GetValue<string>("SeedUserPass");
    
    //Initializing roles if there was not exists
    await SeedData.Initialize(services, seedUserPass);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
