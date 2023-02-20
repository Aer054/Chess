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
    class WinnerViewModel: INotifyPropertyChanged
    {
        private string winner;
        public string Winner
        {
            get { return winner; }
            set
            {
                winner = value;
                OnPropertyChanged("Winner");
            }
        }
        public WinnerViewModel(string WhoWin)
        {
            Winner = WhoWin;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
