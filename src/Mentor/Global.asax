<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Routing" %>
<script RunAt="server">
    void Application_Start(Object sender, EventArgs args)
    {
        RegisterRoutes(RouteTable.Routes);
    }
    private static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
            );
    }
</script>
