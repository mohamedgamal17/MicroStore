using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using MicroStore.AspNetCore.UI.Enums;

namespace MicroStore.AspNetCore.UI.TagHelpers
{

    [HtmlTargetElement(TARGET_NAME)]
    public class MicroLabelTagHelper : TagHelper
    {

        const string TARGET_NAME = "micro-label";

        const string FOR_ATTRIBUTE_NAME = "asp-for";

        const string DISPLAY_HINT_ATTRIBUTE_NAME = "asp-display-hint";

        const string HINT_ATTRIBUTE_NAME = "asp-hint";

        const string HINT_PLACEMENT_NAME = "asp-hint-placement";

        const string TEXT_SIZE = "text-size";



        [HtmlAttributeName(FOR_ATTRIBUTE_NAME)]
        public ModelExpression For { get; set; }


        [HtmlAttributeName(DISPLAY_HINT_ATTRIBUTE_NAME)]
        public bool DisplayHint { get; set; } = true;

        [HtmlAttributeName(HINT_ATTRIBUTE_NAME)]
        public string Hint { get; set; }

        [HtmlAttributeName(HINT_PLACEMENT_NAME)]
        public HintPlacement HintPlacement { get; set; }

        [HtmlAttributeName(TEXT_SIZE)]
        public TextSize TextSize { get; set; }
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
           
            var tagBuilder = Generator.GenerateLabel(ViewContext, For.ModelExplorer, For.Name, null, new { @class = ResolveTextSizeClass() });


            if (tagBuilder != null)
            {
                output.TagName = "div";

                output.TagMode = TagMode.StartTagAndEndTag;

                output.Attributes.Add(new TagHelperAttribute("class", "label-wrapper"));

                output.Content.SetHtmlContent(tagBuilder);

                var iconWrapperBuilder = new TagBuilder("div");

                iconWrapperBuilder.AddCssClass("d-inline");

                var iconBuilder = new TagBuilder("i");

                iconBuilder.AddCssClass("ml-2 fas fa-question-circle text-lightblue");

                if (DisplayHint)
                {
                    iconBuilder.Attributes.Add("data-toggle", "tooltip");
                    iconBuilder.Attributes.Add("data-placement", ResolveHintPlacement());
                    iconBuilder.Attributes.Add("title", Hint);
                }

                iconWrapperBuilder.InnerHtml.AppendHtml(iconBuilder);

                output.Content.AppendHtml(iconWrapperBuilder);
            }
        }


        private string ResolveTextSizeClass()
        {
            return TextSize switch
            {
                TextSize.Sm => "text-sm",
                TextSize.Lg => "text-lg",
                TextSize.Xl => "text-xl",
                _ => "text-md"
            };   
        }

        private string ResolveHintPlacement()
        {
            return HintPlacement switch
            {
                HintPlacement.Bottom => "bottom",
                HintPlacement.Top => "top",
                HintPlacement.Right => "right",
                _ => "left"
            };
        }

    }
}
