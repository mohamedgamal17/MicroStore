using Volo.Abp.DependencyInjection;

namespace MicroStore.Client.PublicWeb.Infrastructure
{
    public class UIMenuNavigationManager : IScopedDependency
    {

        private  string _currentMenu;

        public UIMenuNavigationManager()
        {
            _currentMenu = string.Empty;
        }

        public void SetCurrentMenu(string name)
        {
            _currentMenu = name;
        }

        public string GetCurrentMenu() => _currentMenu;
    }
}
