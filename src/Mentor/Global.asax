<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Mentor" %>
<%@ Import Namespace="SimpleInjector" %>
<%@ Import Namespace="SimpleInjector.Integration.Web.Mvc" %>

<script runat="server">
    void Application_Start(Object sender, EventArgs args)
    {
        ConfigureContainer();
        ConfigureBundles(BundleTable.Bundles);
        ConfigureFilters(GlobalFilters.Filters);
        ConfigureRoutes(RouteTable.Routes);
    }

    protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    {
        var principal = UserService.GetPrincipal();
        if (principal == null)
            return;
        
        Context.User = principal;
    }

    private static void ConfigureBundles(BundleCollection bundles)
    {
        bundles.Add(new StyleBundle("~/content/css/bootstrap").Include(
            "~/Content/bootstrap/bootstrap.min.css",
            "~/Content/style.css"));

        bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
            "~/Content/jquery/jquery-2.1.1.min.js",
            "~/Content/bootstrap/bootstrap.min.js",
            "~/Content/bootstrap/respond.js",
            "~/Content/scripts.js"));
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

    private static void ConfigureFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
    }

    private static void ConfigureRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute("Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
    }
</script>
