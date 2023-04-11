using Microsoft.AspNetCore.Mvc;
using MicroStore.AspNetCore.UI.HtmlHelpers;
using MicroStore.Client.PublicWeb.Menus;
using Volo.Abp.UI.Navigation;

namespace MicroStore.Client.PublicWeb.Areas.Administration.Components
{
    [ViewComponent]
    public class SidebarViewComponent : ViewComponent
    {

        private readonly IMenuManager _menuManager;

        private readonly ILogger<SidebarViewComponent> _logger;
        public SidebarViewComponent(IMenuManager menuManager, ILogger<SidebarViewComponent> logger)
        {
            _menuManager = menuManager;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var rootMenu = await _menuManager
                 .GetAsync(ApplicationMenusDefaults.BackEnd);

            _logger.LogInformation($"Sync View Component : {rootMenu.Items.Count}");

            return View(BuildMenuTree(rootMenu));
        }



        private TreeViewRoot BuildMenuTree(ApplicationMenu applicationMenu)
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            applicationMenu.Items.ForEach(item => nodes.Add(BuildNode(item)));

            return new TreeViewRoot(nodes);
        }


        private TreeViewNode BuildNode(ApplicationMenuItem menuItem)
        {
            var node = new TreeViewNode
            {
                Name = menuItem.Name,
                DisplayName = menuItem.DisplayName,
                AnchorUrl = menuItem.Url,
                CssIcons = menuItem.Icon
            };

            if (menuItem.Items.Any())
            {
                menuItem.Items.ForEach((child) => node.Children.Add(BuildNode(child)));              
            }

            return node;
        }
    }
}
