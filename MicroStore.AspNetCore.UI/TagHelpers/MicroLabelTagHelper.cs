using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
namespace MicroStore.AspNetCore.UI.TagHelpers
{

    [HtmlTargetElement(TARGET_NAME)]
    public class MicroLabelTagHelper : TagHelper
    {

        const string TARGET_NAME = "micro-label";

        const string FOR_ATTRIBUTE_NAME = "asp-for";

        const string DISPLAY_HINT_ATTRIBUTE_NAME = "asp-display-hint";



        [HtmlAttributeName(FOR_ATTRIBUTE_NAME)]
        public ModelExpression For { get; set; }


        [HtmlAttributeName(DISPLAY_HINT_ATTRIBUTE_NAME)]
        public bool DisplayHint { get; set; } = true;


        protected IHtmlGenerator Generator { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        private readonly ILogger<MicroLabelTagHelper> _logger;



        public MicroLabelTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            var tagBuilder = Generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, null, new { @class = "col-form-label" });


            if (tagBuilder != null)
            {
                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;
                output.Attributes.Add(new TagHelperAttribute("class", "label-wrapper"));

                output.Content.SetHtmlContent(tagBuilder);

                output.Content.AppendHtml("<div class='d-inline'> <i class='fas fa-question-circle text-lightblue '></i> </div>");
            }
        }

    }
}
