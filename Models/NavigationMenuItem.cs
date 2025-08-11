using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WpfScheduledApp20250729.Models
{
    public class NavigationMenuItem
    {
        public string DisplayName { get; set; } = string.Empty;
        public ICommand? Command { get; set; }
        public object? CommandParameter { get; set; }
        public string? Icon { get; set; }
        public bool IsEnabled { get; set; } = true;
        public string? ToolTip { get; set; }
        public ObservableCollection<NavigationMenuItem> SubItems { get; set; } = new ObservableCollection<NavigationMenuItem>();
        public bool HasSubItems => SubItems.Count > 0;
    }
}