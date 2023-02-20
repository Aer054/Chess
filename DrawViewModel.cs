using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess
{
    internal class DrawViewModel: INotifyPropertyChanged
    {
        public ICommand Accept { get; set; }
        public ICommand Cancel { get; set; }
        private DrawWindow DrawWindow;
        private GameWindow GameWindow;
        private async void Command_Execute_Accept(object parameter)
        {
            WinnerWindow winnerWindow = new WinnerWindow("Ничья");
            winnerWindow.Show();
            DrawWindow.Close();
            GameWindow.Close();


        }
        private async void Command_Execute_Cancel(object parameter)
        {
            DrawWindow.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public DrawViewModel(DrawWindow drawWindow, GameWindow gameWindow)
        {
            GameWindow=gameWindow;
            DrawWindow = drawWindow;
            Cancel = new RelayCommand(Command_Execute_Cancel);
            Accept = new RelayCommand(Command_Execute_Accept);
        }
    }
}
