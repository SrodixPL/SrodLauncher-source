using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SrodLauncher_v2._0.Core;

namespace SrodLauncher_v2._0.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand SrodPackCommand { get; set; }
        public RelayCommand MinecraftCommand { get; set; }
        public RelayCommand SettingsCommand { get; set; }

        public SrodPackViewModel SrodPackViewModel { get; set; }
        public MinecraftViewModel MinecraftViewModel { get; set; }
        public SettingsViewModel SettingsViewModel { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; 
            OnPropertyChanged(); }
        }


        public MainViewModel()
        {
            SrodPackViewModel = new SrodPackViewModel();
            MinecraftViewModel = new MinecraftViewModel();
            SettingsViewModel = new SettingsViewModel();

            CurrentView = SrodPackViewModel;

            SrodPackCommand = new RelayCommand(o => { CurrentView = SrodPackViewModel; });
            MinecraftCommand = new RelayCommand(o => { CurrentView = MinecraftViewModel; });
            SettingsCommand = new RelayCommand(o => { CurrentView = SettingsViewModel; });
        }
    }
}
