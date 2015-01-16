using System.Web.Mvc;

namespace Mentor
{
    public class DependencyResolverModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType.Namespace == "Mentor")
            {
                return DependencyResolver.Current.GetService(bindingContext.ModelType);
            }
            else
            {
                return base.BindModel(controllerContext, bindingContext);
            }
        }
    };
}
