<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Mentor" %>
<%@ Import Namespace="SimpleInjector" %>
<%@ Import Namespace="SimpleInjector.Integration.Web.Mvc" %>

<script RunAt="server">
    void Application_Start(Object sender, EventArgs args)
    {
        ConfigureContainer();
        ConfigureRoutes(RouteTable.Routes);
        ConfigureBundles(BundleTable.Bundles);
    }

    private static void ConfigureContainer()
    {
        var container = new Container();
        container.RegisterMvcControllers(typeof(HomeController).Assembly);
        container.RegisterMvcIntegratedFilterProvider();
        container.RegisterPerWebRequest<MentorDb>();
        container.Verify();

        DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
    }

    private static void ConfigureRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute("Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
    }

    private static void ConfigureBundles(BundleCollection bundles)
    {
        bundles.Add(new StyleBundle("~/content/css/bootstrap").Include(
            "~/Content/bootstrap/bootstrap.min.css",
            "~/Content/dashboard.css",
            "~/Content/style.css"));

        bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
            "~/Content/jquery/jquery-2.1.1.min.js",
            "~/Content/bootstrap/bootstrap.min.js",
            "~/Content/scripts.js"));
    }
</script>
