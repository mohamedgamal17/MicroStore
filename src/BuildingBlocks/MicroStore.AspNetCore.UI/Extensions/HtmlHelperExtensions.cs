using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MicroStore.AspNetCore.UI.HtmlHelpers;

namespace MicroStore.AspNetCore.UI.Extensions
{
    public static class HtmlHelperExtensions
    {

        public static TreeViewBuilder CreateMenutItems(this IHtmlHelper htmlHelper, TreeViewRoot root)
        {
            return new TreeViewBuilder(root);
        }

        private static IDictionary<string, object>? ResolveAnonymousObjectToHtmlAttribute(object? obj)
        {
            if (obj == null)
            {
                return null;
            }
            
            return HtmlHelper.AnonymousObjectToHtmlAttributes(obj);
        }




    }
}
