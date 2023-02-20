using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Chess
{
     class MainViewModel:INotifyPropertyChanged
    {
        public ICommand Play_1min{ set; get; }
        public ICommand Play_3min{ set; get; }
        public ICommand Play_10min{ set; get; }
        public ICommand Show { set; get; }
        private bool IsHidden;
        private string visibility;
        private string imageSource;
        public string Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChanged("Visibility");
            }
        }
        public string ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private async void Command_Execute_Play_1min(object parameter)
        {
           GameWindow gameWindow = new GameWindow(1.0);
            gameWindow.Show();
            Application.Current.MainWindow.Close();
        }
        private async void Command_Execute_Play_3min(object parameter)
        {
            GameWindow gameWindow = new GameWindow(3.0);
            gameWindow.Show();
            Application.Current.MainWindow.Close();
        }
        private async void Command_Execute_Play_10min(object parameter)
        {
            GameWindow gameWindow = new GameWindow(10.0);
            gameWindow.Show();
            Application.Current.MainWindow.Close();
        }
        private async void Command_Execute_Show(object parameter)
        {
            if (IsHidden)
            {
                IsHidden = false;
                Visibility = "Visible";
                ImageSource = Path.GetFullPath("Play_Button4.png");
            }
            else
            {
                IsHidden = true;
                Visibility = "Hidden";
                ImageSource = Path.GetFullPath("Play_Button3.png");
            }
        } 
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public MainViewModel()
        {
            ImageSource = Path.GetFullPath("Play_Button3.png");
            IsHidden = true;
            Visibility = "Hidden";
            Show = new RelayCommand(Command_Execute_Show);
            Play_1min = new RelayCommand(Command_Execute_Play_1min);
            Play_3min = new RelayCommand(Command_Execute_Play_3min);
            Play_10min = new RelayCommand(Command_Execute_Play_10min);
        }

    }
}
