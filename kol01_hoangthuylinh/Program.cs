using kol01_hoangthuylinh;
using kol01_hoangthuylinh.Middleware;

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
                        "/css/swiper.min.css",
                        "/css/site.css",
                        "/css/responsive.css"
                   );

    options.AddJavaScriptBundle("/js/site.min.js",
                    "/js/jquery-3.4.1.min.js",
                    "/js/jquery.validate.min.js",
                    "/js/jquery.validate.unobtrusive.min.js",
                    "/js/jquery.unobtrusive-ajax.min.js",
                    "/js/bootstrap.bundle.min.js",
                    "/js/swiper-8.3.2.min.js"
                    );
    // options.MinifyCssFiles();
    // options.MinifyJsFiles();

});
#endregion

//Add kanang detection
builder.Services.AddDetection();

builder.Services.AddHttpContextAccessor();

//Add pagination service
builder.Services.AddCloudscribePagination();

//Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(300);
});
//ConnectString
kol01_hoangthuylinh.Helpers.Constants.webConnection = builder.Configuration.GetConnectionString("webConnection");
//UrlHost
kol01_hoangthuylinh.Helpers.Constants.urlHost = builder.Configuration.GetSection("webConfig")["urlHost"];
kol01_hoangthuylinh.Helpers.Constants.PlacementFB = builder.Configuration.GetSection("webConfig")["PlacementFB"];
//Enable redis cached
kol01_hoangthuylinh.Helpers.Constants.isRedisCachedEnable = builder.Configuration.GetSection("webConfig")["RedisCached"] == "0" ? false : true;

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

app.UseDetection();
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

//Register route

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "Bài viết video",
    pattern: "/bai-viet-video",
    defaults: new { controller = "Video", action = "Index" }
);

app.MapControllerRoute(
    name: "Bài viết",
    pattern: "/bai-viet",
    defaults: new { controller = "News", action = "Index" }
);

app.MapControllerRoute(
    name: "Bộ sưu tập",
    pattern: "/bo-suu-tap",
    defaults: new { controller = "Gallery", action = "Index" }
);

app.MapControllerRoute(
    name: "Đọc bài",
    pattern: "{url}-{id}.html",
    defaults: new { controller = "Read", action = "Index" }
);

 app.MapControllerRoute(
    name: "Tìm kiếm",
    pattern: "tim-kiem/q={name}",
    defaults: new { controller = "Search", action = "Index" }
);

app.Run();
