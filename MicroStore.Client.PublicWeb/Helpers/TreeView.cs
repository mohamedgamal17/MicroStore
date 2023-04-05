using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;

namespace MicroStore.Client.PublicWeb.Helpers
{
    public class TreeViewRoot
    {
        public IEnumerable<TreeViewNode> Nodes { get; set; }

        public TreeViewRoot(IEnumerable<TreeViewNode> nodes)
        {
            Nodes = nodes;
        }

        public TreeViewRoot()
        {

        }
    }

    public class TreeViewNode
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string? AnchorUrl { get; set; }
        public string? CssIcons { get; set; }
        public List<TreeViewNode> Children { get; } = new List<TreeViewNode>();
        public bool HasChildren => Children != null && Children.Any();

        public bool ContainsName(string name)
        {
            if(string.IsNullOrEmpty(name))
                return false;

            if (Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (name.StartsWith(Name))
                return true;


            if (HasChildren)
                return Children.Any(child => child.ContainsName(name));

            return false;
        }


       
    }

    public class TreeView : IHtmlContent
    {
        public TreeViewRoot Root { get;  }

        public  List<Func<TreeViewRoot, object>> RootHtmlAttributes;

        public  List<Func<TreeViewNode, object>> NodeHtmlAttributes;

        public  List<Func<TreeViewNode, object>> NestedListHtmlAttributes;

        public  List<Func<TreeViewNode, object>> AnchorHtmlAttributes;

        public  List<Func<TreeViewNode, object>> ParagrahpHtmlAttributes;


        public TreeView(TreeViewRoot root)
        {
            Root = root;
            RootHtmlAttributes = new();
            NodeHtmlAttributes = new();
            NestedListHtmlAttributes = new();
            AnchorHtmlAttributes = new();
            ParagrahpHtmlAttributes = new();
        }


        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var rootTagBuilder = new TagBuilder("ul");

            if (RootHtmlAttributes.Any())
            {
                ResolveRootElementHtmlAttributes(rootTagBuilder, Root, RootHtmlAttributes);
            }

            foreach (var node in Root.Nodes)
            {
                BuildNestedTag(node, rootTagBuilder);
            }

            rootTagBuilder.WriteTo(writer, encoder);
           
        }


        private void BuildNestedTag(TreeViewNode node , TagBuilder parentTag)
        {
            var nodeTagBuilder = BuildListItem(node);

          


            TagBuilder? nestedTagBuilder;

            if (node.Children != null && node.Children.Any())
            {
                nestedTagBuilder = new TagBuilder("ul");

                foreach (var child in node.Children)
                {
                    BuildNestedTag(child, nestedTagBuilder);
                }


                if(NestedListHtmlAttributes.Any())
                {
                    ResolveChildElementHtmlAttributes(nestedTagBuilder, node, NestedListHtmlAttributes);
                }


                nodeTagBuilder.InnerHtml.AppendHtml(nestedTagBuilder);
            }


            parentTag.InnerHtml.AppendHtml(nodeTagBuilder);


        }



        private TagBuilder BuildListItem(TreeViewNode node)
        {
            
            var nodeTagBuilder = new TagBuilder("li");

            TagBuilder? anchorTagBuilder = new TagBuilder("a");

            anchorTagBuilder.MergeAttribute("href", node.AnchorUrl ?? "#");

            if (AnchorHtmlAttributes.Any())
            {
                ResolveChildElementHtmlAttributes(anchorTagBuilder, node, AnchorHtmlAttributes);

            }


            if (!string.IsNullOrEmpty(node.CssIcons))
            {
                TagBuilder iconTag = new TagBuilder("i");

                iconTag.AddCssClass(node.CssIcons);

                anchorTagBuilder.InnerHtml.AppendHtml(iconTag);
            }

            var paragraphTagBuilder = new TagBuilder("P");

            paragraphTagBuilder.InnerHtml.Append(node.DisplayName);

            if (ParagrahpHtmlAttributes.Any())
            {
                ResolveChildElementHtmlAttributes(paragraphTagBuilder, node, ParagrahpHtmlAttributes);
            }

            anchorTagBuilder.InnerHtml.AppendHtml(paragraphTagBuilder);

            nodeTagBuilder.InnerHtml.AppendHtml(anchorTagBuilder);

            if (NodeHtmlAttributes.Any())
            {
                ResolveChildElementHtmlAttributes(nodeTagBuilder, node, NodeHtmlAttributes);
            }

            return nodeTagBuilder;

        }

        private void ResolveRootElementHtmlAttributes(TagBuilder tagBuilder, TreeViewRoot root, List<Func<TreeViewRoot, object>> callbacks)
        {
            callbacks.ForEach(callback => tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(callback(root))));

        }
        private void ResolveChildElementHtmlAttributes(TagBuilder tagBuilder,TreeViewNode node, List<Func<TreeViewNode , object>> callbacks)
        {
            callbacks.ForEach(callback => tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(callback(node))));
        }
    }


    public class TreeViewBuilder
    {

        private readonly TreeViewRoot _root;


        private readonly List<Func<TreeViewRoot, object>> _rootHtmlAttributes;

        private readonly List<Func<TreeViewNode, object>> _nodeHtmlAttributes;

        private readonly List<Func<TreeViewNode, object>> _nestedListHtmlAttributes;

        private readonly List<Func<TreeViewNode, object>> _anchorHtmlAttributes;

        private readonly List<Func<TreeViewNode, object>> _paragrahpHtmlAttributes;

        public TreeViewBuilder(TreeViewRoot root)
        {
            _root = root;
            _rootHtmlAttributes = new();
            _nodeHtmlAttributes = new();
            _nestedListHtmlAttributes = new();
            _anchorHtmlAttributes = new();
            _paragrahpHtmlAttributes = new();
        }

   

        public TreeViewBuilder RootHtmlAttributes(Func<TreeViewRoot, object> callBack)
        {
            if (callBack == null) throw new ArgumentNullException(nameof(callBack));

            _rootHtmlAttributes.Add(callBack);

            return this;
        }

        public TreeViewBuilder NodeHtmlAttributes(Func<TreeViewNode, object> callBack)
        {
            if (callBack == null) throw new ArgumentNullException(nameof(callBack));

            _nodeHtmlAttributes.Add(callBack);

            return this;
        }

        public TreeViewBuilder NestedListHtmlAttributes(Func<TreeViewNode, object> callBack)
        {
            if (callBack == null) throw new ArgumentNullException(nameof(callBack));

            _nestedListHtmlAttributes.Add(callBack);

            return this;
        }


        public TreeViewBuilder AnchorHtmlAttributes(Func<TreeViewNode, object> callBack)
        {
            if (callBack == null) throw new ArgumentNullException(nameof(callBack));

            _anchorHtmlAttributes.Add(callBack);

            return this;
        }

        public TreeViewBuilder ParagrahpHtmlAttributes(Func<TreeViewNode, object> callBack)
        {
            if (callBack == null) throw new ArgumentNullException(nameof(callBack));

            _paragrahpHtmlAttributes.Add(callBack);

            return this;
        }

        public TreeView Build()
        {
            return new TreeView(_root)
            {
                RootHtmlAttributes = _rootHtmlAttributes,
                NodeHtmlAttributes = _nodeHtmlAttributes,
                AnchorHtmlAttributes = _anchorHtmlAttributes,
                ParagrahpHtmlAttributes = _paragrahpHtmlAttributes,
                NestedListHtmlAttributes = _nestedListHtmlAttributes
            };
        }
    }
   
}
