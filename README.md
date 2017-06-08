# AspNetCoreAreaSample
## エリア属性付きコントローラ
```cs
....
namespace AspNetCoreAreaSample.Area1.Controllers
{
    [Area("area1")]
    public class HomeController : Controller
    {
        ....
    }
}
```

## ファイルツリー構造
```text
Project/
    Controllers/
        HomeController.cs
    Views/
        Home/
            Index.cshtml
            About.cshtml
    Areas/
        area1/
            Controllers/
                HomeController.cs
            Views/
                Home/
                    Index.cshtml
                    About.cshtml
        area2
            Controllers/
                HomeController.cs
            Views/
                Home/
                    Index.cshtml
                    About.cshtml
```

## エリア付きルート設定
```cs
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
    // ★ここを追加
    routes.MapRoute(
        name: "area-route",
        template: "{area}/{controller=Home}/{action=Index}/{id?}");
});
```

## ドメインによるエリアルート設定
```cs
app.MapWhen(
    (HttpContext context) =>
    {
        return context.Request.Host.Value.StartsWith("area1.");
    },
    (IApplicationBuilder builder) =>
    {
        builder.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "area1route",
                template: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "area1" }
            );
        });
    }
);
app.MapWhen(
    (HttpContext context) =>
    {
        return context.Request.Host.Value.StartsWith("area2.");
    },
    (IApplicationBuilder builder) =>
    {
        builder.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "area2route",
                template: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { area = "area2" }
            );
        });
    }
);
```
