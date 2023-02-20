using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chess
{
    internal class NewPiecesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Cell> cells = new ObservableCollection<Cell>();
        private Cell _cell;
        private NewPiecesWindow window;
        public ObservableCollection<Cell> Cells
        {
            get { return cells; }
            set { cells = value; }
        }
        public ICommand VotePieces { get; set; }
        private async void Command_Execute_VotePieces(object parameter)
        {
            Cell VotedCell = parameter as Cell;
            _cell._State = VotedCell._State;
            window.Close();
        }
        public NewPiecesViewModel(Cell cell, NewPiecesWindow win)
        {
            window = win;
            _cell = cell;
            VotePieces = new RelayCommand(Command_Execute_VotePieces);
            cells.Add(new Cell(-1, -1, "#44944A", Cell.State.BlackQueen, VotePieces, true));
            cells.Add(new Cell(-1, -1, "#F5F5DC", Cell.State.BlackKnight, VotePieces, true));
            cells.Add(new Cell(-1, -1, "#44944A", Cell.State.BlackRook, VotePieces, true));
            cells.Add(new Cell(-1, -1, "#F5F5DC", Cell.State.BlackBishop, VotePieces, true));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
