<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script RunAt="server">
    void Application_Start(Object sender, EventArgs args)
    {
        RegisterRoutes(RouteTable.Routes);
        RegisterBundles(BundleTable.Bundles);
    }
    private static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
    }

    private static void RegisterBundles(BundleCollection bundles)
    {
        bundles.Add(new StyleBundle("~/content/css/bootstrap").Include(
            "~/content/bootstrap.min.css",
            "~/content/bootstrap-theme.min.css"));
        
        bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
            "~/Scripts/jquery-2.1.1.min.js",
            "~/Scripts/bootstrap.min.js"));
    }
</script>