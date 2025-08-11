using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.ViewModels
{
    internal class NavigationMenuViewModel : NotificationObject
    {
        private ObservableCollection<NavigationMenuItem> _menuItems = new();
        public ObservableCollection<NavigationMenuItem> MenuItems
        {
            get => _menuItems;
            set => SetProperty(ref _menuItems, value);
        }

        private NavigationMenuItem? _selectedMenuItem;
        public NavigationMenuItem? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                if (SetProperty(ref _selectedMenuItem, value) && value != null)
                {
                    OnMenuItemSelected(value);
                }
            }
        }

        public event Action<NavigationMenuItem>? MenuItemSelected;

        public NavigationMenuViewModel()
        {
            InitializeDefaultMenuItems();
        }

        private void InitializeDefaultMenuItems()
        {
        }

        public void AddMenuItem(NavigationMenuItem menuItem)
        {
            MenuItems.Add(menuItem);
        }

        public void AddMenuItemWithSubItems(string displayName, NavigationMenuItem[] subItems, string? toolTip = null, string? icon = null)
        {
            var parentMenuItem = new NavigationMenuItem
            {
                DisplayName = displayName,
                ToolTip = toolTip,
                Icon = icon
            };

            foreach (var subItem in subItems)
            {
                parentMenuItem.SubItems.Add(subItem);
            }

            MenuItems.Add(parentMenuItem);
        }

        public void AddSubMenuItem(NavigationMenuItem parentItem, NavigationMenuItem subItem)
        {
            parentItem.SubItems.Add(subItem);
        }

        public void RemoveMenuItem(NavigationMenuItem menuItem)
        {
            MenuItems.Remove(menuItem);
        }

        public void ClearMenuItems()
        {
            MenuItems.Clear();
        }

        private void OnMenuItemSelected(NavigationMenuItem menuItem)
        {
            if (menuItem.Command?.CanExecute(menuItem.CommandParameter) == true)
            {
                menuItem.Command.Execute(menuItem.CommandParameter);
            }
            MenuItemSelected?.Invoke(menuItem);
        }
    }
}