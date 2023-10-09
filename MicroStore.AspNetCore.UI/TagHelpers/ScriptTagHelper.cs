using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace MicroStore.AspNetCore.UI.TagHelpers
{
    [HtmlTargetElement("script",Attributes = "target-zone")]
    public class ScriptTagHelper  : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }


        [HtmlAttributeName("target-zone")]
        public string TargetZone { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();

            string attributes = output.Attributes.Select(x => $"{x.Name}=\"{x.Value}\"").JoinAsString(" ");

            var javaScript = content.GetContent();
            
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("<script {0}>", attributes));

            sb.AppendLine(javaScript);

            sb.AppendLine("</script>");
            var scripts = ViewContext.HttpContext.Items.ContainsKey(TargetZone)
             ? (List<StringBuilder>)ViewContext.HttpContext.Items[TargetZone]! 
             : new List<StringBuilder>();
            
            scripts.Add(sb);

            ViewContext.HttpContext.Items[TargetZone] = scripts;

            output.SuppressOutput();
        }
    }
}
