using CMS1_Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

//Bundler and minifier services
#region 
builder.Services.AddWebOptimizer(options =>
{
    //CP bundler and minifier
    options.AddCssBundle("/css/sitecp.min.css",
                    "/css/bootstrap.min.css",
                    "/css/font-awesome.min.css",
                    "/css/custom.css"
                    );

    options.AddJavaScriptBundle("/js/sitecp.min.js",
                    "/js/jquery-3.4.1.min.js",
                    "/js/jquery.validate.min.js",
                    "/js/jquery.validate.unobtrusive.min.js",
                    "/js/jquery.unobtrusive-ajax.min.js",
                    "/js/bootstrap.bundle.min.js",
                    "/js/bootbox.min.js",
                    "/js/custom.min.js"
                    );

    //Login bundler and minifier
    options.AddCssBundle("/css/login.min.css",
                    "/css/bootstrap.min.css",
                    "/css/font-awesome.css",
                    "/css/site.css",
                    "/css/responsive.css",
                    "/css/swiper.min.css"
                    );

    options.AddJavaScriptBundle("/js/login.min.js",
                    "/js/jquery-3.4.1.min.js",
                    "/js/jquery.validate.min.js",
                    "/js/jquery.validate.unobtrusive.min.js",
                    "/js/jquery.unobtrusive-ajax.min.js",
                    "/js/bootstrap.bundle.min.js",
                    "/js/bootbox.min.js"
                    );

    //Change password bundler and minifier
    options.AddCssBundle("/css/changePassWord.min.css",
                    "/css/bootstrap.min.css",
                    "/css/font-awesome.min.css",
                    "/css/site.css",
                    "/css/responsive.css"
                    );

    options.AddJavaScriptBundle("/js/changePassWord.min.js",
                    "/js/jquery-3.4.1.min.js",
                    "/js/jquery.validate.min.js",
                    "/js/jquery.validate.unobtrusive.min.js",
                    "/js/jquery.unobtrusive-ajax.min.js",
                    "/js/bootstrap.bundle.min.js",
                    "/js/bootbox.min.js"
                    );

    //Site bundler and minifier
    options.AddCssBundle("/css/site.min.css",
                "/css/bootstrap.min.css",
                "/css/font-awesome.min.css",
                "/css/site.css",
                "/css/responsive.min.css",
                "/css/swiper.min.css"

                );

    options.AddJavaScriptBundle("/js/site.min.js",
                    "/js/jquery-3.4.1.min.js",
                    "/js/jquery.validate.min.js",
                    "/js/jquery.validate.unobtrusive.min.js",
                    "/js/jquery.unobtrusive-ajax.min.js",
                    "/js/bootstrap.bundle.min.js",
                    "/js/swiper.min.js"
                    );

    options.MinifyCssFiles();
    options.MinifyJsFiles();

});
#endregion


builder.Services.AddHttpContextAccessor();

//Add pagination service
builder.Services.AddCloudscribePagination();

//Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
});
//ConnectString
CMS1_Core.Helpers.Constants.webConnection = builder.Configuration.GetConnectionString("webConnection");
//UrlHost
CMS1_Core.Helpers.Constants.urlHost = builder.Configuration.GetSection("webConfig")["urlHost"];
CMS1_Core.Helpers.Constants.PlacementFB = builder.Configuration.GetSection("webConfig")["PlacementFB"];

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseDeveloperExceptionPage();

//User bundler and minifier
app.UseWebOptimizer();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admincp",
    pattern: "{area:exists}/{controller=admincp}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
