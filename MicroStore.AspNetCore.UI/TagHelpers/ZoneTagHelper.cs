
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace MicroStore.AspNetCore.UI.TagHelpers
{
    [HtmlTargetElement("zone", Attributes ="name")]
    public class ZoneTagHelper : TagHelper
    {

        [HtmlAttributeName("name")]
        public string Name { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.HttpContext.Items.ContainsKey(Name))
            {
                var scripts = (List<StringBuilder>)ViewContext.HttpContext.Items[Name]!;

                foreach (var script in scripts)
                {
                    if (output.Content.IsEmptyOrWhiteSpace)
                    {
                        output.Content.SetHtmlContent(script.ToString());
                    }
                    else
                    {

                        output.PostContent.AppendHtml(script.ToString());
                    }
                  
                }

                output.TagName = string.Empty;
           
            }
            else
            {

                output.SuppressOutput();
            }          
        }
    }
}
