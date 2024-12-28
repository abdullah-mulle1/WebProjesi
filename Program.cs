    using Berber.Data;
    using Berber.Models;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    // Configure DbContext
    builder.Services.AddDbContext<BerberContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("VarsayilanBaglanti")));

    // Configure Identity
    builder.Services.AddIdentity<Kullanici, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<BerberContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
 .AddCookie(options =>
 {
     options.LoginPath = "/Hesap/GirisYap"; // „”«—  ”ÃÌ· «·œŒÊ·
     options.LogoutPath = "/Hesap/CikisYap"; // „”«—  ”ÃÌ· «·Œ—ÊÃ
 });

// Add Session service
builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(1); // „œ… «‰ Â«¡ «·Ã·”…
        options.Cookie.HttpOnly = true; // «·Õ„«Ì… „‰ «·ÂÃ„«  ⁄»— «·Ã«›«”ﬂ—Ì» 
        options.Cookie.IsEssential = true; // «·”„«Õ »«·ﬂÊﬂÌ Õ Ï „⁄ ﬁÊ«⁄œ GDPR
    });




    var app = builder.Build();

    // Initialize roles and admin user
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        await KullaniciRolu.InitializeAsync(services);
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseSession();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
