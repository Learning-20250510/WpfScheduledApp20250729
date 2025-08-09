using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250729;

namespace WpfScheduledApp20250729.ViewModels
{
    internal class UpdateTaskViewModel : NotificationObject
    {
        public UpdateTaskViewModel(int task_id)
        {

        }

        public UpdateTaskViewModel(object taskEntity)
        {
            // ここでオブジェクトの型に応じて処理を分岐
            // taskEntityの型を確認して適切な処理を行う
        }
    }
}
