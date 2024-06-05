var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
//pattern: "{controller=Profile}/{action=InsertProfile_View}/{id?}");
//pattern: "{controller=Registration}/{action=Registration_View}/{id?}");
//pattern: "{controller=Authentication}/{action=Authentication_View}/{id?}");
//pattern: "{controller=VerifyCode}/{action=VerifyCode}/{id?}");
//pattern: "{controller=Account}/{action=AccountDetail_View}/{id?}");
//pattern: "{controller=Profile}/{action=ViewProfile}/{id?}");
pattern: "{controller=Login}/{action=Login_View}/{id?}");



//pattern: "{controller=Login}/{action=Test}/{id?}");
//pattern: "{controller=Profile}/{action=ProfileHomeView}/{id?}");
//pattern: "{controller=Home}/{action=Index}/{id?}");
//pattern: "{controller=Home}/{action=Home_View}/{id?}");
// pattern: "{controller=Home}/{action=Index}/{id?}"); original line of code

app.Run();
