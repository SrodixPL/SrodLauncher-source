using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SrodLauncher_v2._0.MVVM.ViewModel
{
    internal class MinecraftViewModel
    {
        public ObservableCollection<Version> Versions { get; set; } = new ObservableCollection<Version>();
    }
}
