using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250629;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Views;

namespace WpfScheduledApp20250729.ViewModels
{
    class ReadTasksViewModel : NotificationObject
    {
        private readonly IWindowService _windowService;

        public ReadTasksViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            _windowService.ShowAddTaskWindow();
        }


    }
}
