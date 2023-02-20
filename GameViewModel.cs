using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Chess
{
    class GameViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Cell> cells = new ObservableCollection<Cell>();
        private TimeSpan timer0;
        private TimeSpan timer1;
        private GameWindow GameWindow;
        private DispatcherTimer timer_ = new DispatcherTimer();
        private DispatcherTimer timer1_ = new DispatcherTimer();
        #region gettrs

        public ObservableCollection<Cell> Cells
        {
            get { return cells; }
            set { cells = value; }
        }
        public TimeSpan Timer
        {
            get { return timer0; }
            set
            { 
                timer0 = value; 
                OnPropertyChanged("Timer");
            }
        }
        public TimeSpan Timer1
        {
            get { return timer1; }
            set
            {
                timer1 = value;
                OnPropertyChanged("Timer1");
            }
        }
        #endregion
        public ICommand Move { get; set; }
        public ICommand Surrender1 { get; set; }
        public ICommand Surrender2 { get; set; }
        public ICommand Draw { get; set; }
        private async void Command_Execute_Surrender1(object parameter)
        {
            WinnerWindow winnerWindow=new WinnerWindow("Победа черных");
            winnerWindow.Show();
            GameWindow.Close();
        }
        private async void Command_Execute_Surrender2(object parameter)
        {
            WinnerWindow winnerWindow = new WinnerWindow("Победа белых");
            winnerWindow.Show();
            GameWindow.Close();
        }
        private bool IsMate(Cell CurrentCell)
        {
            bool isMate=false;
            for (int i = 0; i < cells.Count; i++)
            {
                if(CurrentCell.IsBlack!=cells[i].IsBlack)
                {
                    Vector[] posibleMoves = null;
                    if (((cells[i]._State == Cell.State.WhitePawn)))
                    {
                        posibleMoves = new Vector[2];
                        posibleMoves[0] = new Vector(cells[i].X + 1, cells[i].Y + 1);
                        posibleMoves[1] = new Vector(cells[i].X + 1, cells[i].Y - 1);
                    }
                    else if ((cells[i]._State == Cell.State.BlackPawn))
                    {
                        posibleMoves = new Vector[2];
                        posibleMoves[0] = new Vector(cells[i].X - 1, cells[i].Y + 1);
                        posibleMoves[1] = new Vector(cells[i].X - 1, cells[i].Y - 1);
                    }
                    else
                    {
                       posibleMoves = cells[i].posibleMoves(GetBarrriers());
                    }
                    if (posibleMoves != null)
                    {
                        for (int j = 0; j < posibleMoves.Length; j++)
                        {
                           Cell cell = new Cell((int)posibleMoves[j].X, (int)posibleMoves[j].Y, CurrentCell.Color, CurrentCell._State, Move, CurrentCell.IsBlack);
                            isMate = IsKingChecked(cell, cells[i]);
                            if (isMate == false)
                            {
                                return isMate;
                            }
                        }
                    }
                }
            }
            return isMate;
        }
        private Vector[] MovesForCastling(Cell cell)
        {
            Vector[] posibleMoves = cell.posibleMoves(GetBarrriers());
            if ((cell._State == Cell.State.BlackKing))
            {
                List<Vector> Moves = new List<Vector>();
                for (int i = 0; i < posibleMoves.Length; i++)
                {
                    Moves.Add(posibleMoves[i]);
                }
                bool IsRookRightMoved=true;
                bool IsRookLeftMoved= true;
                for (int i = 0; i < cells.Count; i++)
                {
                    if((cells[i]._State==Cell.State.BlackRook)&& (cells[i].Y==0))
                    {
                        IsRookLeftMoved = cells[i].IsMoved;
                    }
                    if ((cells[i]._State == Cell.State.BlackRook) && (cells[i].Y == 7))
                    {
                        IsRookRightMoved = cells[i].IsMoved;
                    }
                }
                if (!cell.IsMoved)
                {
                    if (!IsRookRightMoved)
                    {
                        bool IsBlocked=false;
                        Cell[] barriers = GetBarrriers();
                        for (int i = 0; i < barriers.Length; i++)
                        {
                            if ((barriers[i].X == 7) && (barriers[i].Y == 5))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            if ((barriers[i].X == 7) && (barriers[i].Y == 4))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            if ((barriers[i].X == 7) && (barriers[i].Y == 6))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            for (int j = 0; j  < cells.Count; j ++)
                            {
                                if (cell.IsBlack != cells[j].IsBlack)
                                {
                                    Vector[] Movess = cells[j].posibleMoves(barriers);
                                    if (Movess != null)
                                    {
                                        for (int k = 0; k < Movess.Length; k++)
                                        {
                                            if ((Movess[k].X == 7) && ((Movess[k].Y == 5) || (Movess[k].Y == 4)))
                                            {
                                                IsBlocked = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if(!IsBlocked) Moves.Add(new Vector(7, 5));
                    }
                    if (!IsRookLeftMoved)
                    {
                        bool IsBlocked = false;
                        Cell[] barriers = GetBarrriers();
                        for (int i = 0; i < barriers.Length; i++)
                        {
                            if ((barriers[i].X == 7) && (barriers[i].Y == 1))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            if ((barriers[i].X == 7) && (barriers[i].Y == 2))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            for (int j = 0; j < cells.Count; j++)
                            {
                                if (cell.IsBlack != cells[j].IsBlack)
                                {
                                    Vector[] Movess = cells[j].posibleMoves(barriers);
                                    if (Movess != null)
                                    {
                                        for (int k = 0; k < Movess.Length; k++)
                                        {
                                            if ((Movess[k].X == 7) && ((Movess[k].Y == 1) || (Movess[k].Y == 2)))
                                            {
                                                IsBlocked = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!IsBlocked) Moves.Add(new Vector(7, 1));
                    }

                    Vector[] posibleMovesPlus = new Vector[Moves.Count];
                    for (int i = 0; i < posibleMovesPlus.Length; i++)
                    {
                        posibleMovesPlus[i] = Moves[i];
                    }
                    return posibleMovesPlus;
                }
                else
                return posibleMoves;
            }
            else if ((cell._State == Cell.State.WhiteKing))
            {

                List<Vector> Moves = new List<Vector>();
                for (int i = 0; i < posibleMoves.Length; i++)
                {
                    Moves.Add(posibleMoves[i]);
                }
                bool IsRookRightMoved = true;
                bool IsRookLeftMoved = true;
                for (int i = 0; i < cells.Count; i++)
                {
                    if ((cells[i]._State == Cell.State.WhiteRook) && (cells[i].Y == 0))
                    {
                        IsRookLeftMoved = cells[i].IsMoved;
                    }
                    if ((cells[i]._State == Cell.State.WhiteRook) && (cells[i].Y == 7))
                    {
                        IsRookRightMoved = cells[i].IsMoved;
                    }
                }
                if (!cell.IsMoved)
                {
                    if (!IsRookRightMoved)
                    {
                        bool IsBlocked = false;
                        Cell[] barriers = GetBarrriers();
                        for (int i = 0; i < barriers.Length; i++)
                        {
                            if ((barriers[i].X == 0) && (barriers[i].Y == 5))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            if ((barriers[i].X == 0) && (barriers[i].Y == 4))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            if ((barriers[i].X == 0) && (barriers[i].Y == 6))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            for (int j = 0; j < cells.Count; j++)
                            {
                                if (cell.IsBlack != cells[j].IsBlack)
                                {
                                    Vector[] Movess = cells[j].posibleMoves(barriers);
                                    if (Movess != null)
                                    {
                                        for (int k = 0; k < Movess.Length; k++)
                                        {
                                            if ((Movess[k].X == 0) && ((Movess[k].Y == 5) || (Movess[k].Y == 4)))
                                            {
                                                IsBlocked = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!IsBlocked) Moves.Add(new Vector(0, 5));
                    }
                    if (!IsRookLeftMoved)
                    {
                        bool IsBlocked = false;
                        Cell[] barriers = GetBarrriers();
                        for (int i = 0; i < barriers.Length; i++)
                        {
                            if ((barriers[i].X == 0) && (barriers[i].Y == 1))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            if ((barriers[i].X == 0) && (barriers[i].Y == 2))
                            {
                                {
                                    IsBlocked = true;
                                }
                            }
                            for (int j = 0; j < cells.Count; j++)
                            {
                                if (cell.IsBlack != cells[j].IsBlack)
                                {
                                    Vector[] Movess = cells[j].posibleMoves(barriers);
                                    if (Movess != null)
                                    {
                                        for (int k = 0; k < Movess.Length; k++)
                                        {
                                            if ((Movess[k].X == 0) && ((Movess[k].Y == 1) || (Movess[k].Y == 2)))
                                            {
                                                IsBlocked = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!IsBlocked) Moves.Add(new Vector(0, 1));
                    }

                    Vector[] posibleMovesPlus = new Vector[Moves.Count];
                    for (int i = 0; i < posibleMovesPlus.Length; i++)
                    {
                        posibleMovesPlus[i] = Moves[i];
                    }
                    return posibleMovesPlus;
                }
                else
                    return posibleMoves;
            }
            else return posibleMoves;
        } 
        private async void Command_Execute_Draw(object parameter)
        {
            DrawWindow drawWin=new DrawWindow(GameWindow);
            drawWin.Show();
        } 
        private async void Command_Execute_Move(object parameter)
        {
            Cell cell = parameter as Cell;
            int SwapingCell = -1;
            int CurrentCell = -1;
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].Active == true)
                {
                    SwapingCell = i;
                }
                if (cells[i] == cell)
                {
                    CurrentCell = i;
                }
            }
            cell.Active = true;
            if ((SwapingCell >= 0))
            {
                Vector[] Moves = cells[SwapingCell].posibleMoves(GetBarrriers());
                if (cells[SwapingCell]._State == Cell.State.WhiteKing || cells[SwapingCell]._State == Cell.State.BlackKing)
                {
                   Moves = MovesForCastling(cells[SwapingCell]);
                }
                bool Imposible = true;
                if(Moves == null)
                {
                    cells[SwapingCell].Active = false;
                    cells[CurrentCell].Active = false;
                    return;
                }
                for (int i = 0; i < Moves.Length; i++)
                {
                    if ((Moves[i].X == cells[CurrentCell].X) && (Moves[i].Y == cells[CurrentCell].Y))
                    {
                        Imposible = false;
                    }
                }
                posibleMoves(cells[SwapingCell], false);
                if (Imposible)
                {
                    posibleMoves(cells[CurrentCell], false);
                    cells[SwapingCell].Active = false;
                    cells[CurrentCell].Active = false;
                    for (int i = 0; i < cells.Count; i++)
                    {
                        if (cells[i]._State != 0)
                        {
                            if (cells[SwapingCell].IsBlack != cells[i].IsBlack)
                            {
                                cells[i].Locked = false;
                            }
                        }
                    }
                }
                else
                {
                    if (cells[CurrentCell]._State == 0)
                    {
                        if (!IsKingChecked(cells[CurrentCell], cells[SwapingCell]))
                        {
                            if (cells[SwapingCell]._State == Cell.State.WhiteKing && cells[SwapingCell].IsMoved==false)
                            {
                                if ((cells[CurrentCell].X == 0) && (cells[CurrentCell].Y == 1))
                                {
                                    int LeftBlackRook = -1;
                                    int SwapLeftBlackRook = -1;
                                    for (int p = 0; p < cells.Count; p++)
                                    {
                                        if(cells[p].X==0 && cells[p].Y==0)
                                        {
                                            LeftBlackRook=p;
                                        }
                                        if (cells[p].X == 0 && cells[p].Y == 2)
                                        {
                                            SwapLeftBlackRook = p;
                                        }
                                    }
                                    Cell Cell00 = new Cell(0, 0, "", cells[LeftBlackRook]._State, Move, cells[LeftBlackRook].IsBlack);
                                    cells[LeftBlackRook]._State = cells[SwapLeftBlackRook]._State;
                                    cells[SwapLeftBlackRook]._State=Cell00._State;
                                    cells[SwapLeftBlackRook].IsBlack=Cell00.IsBlack;
                                    cells[LeftBlackRook].Active = false;
                                    cells[SwapLeftBlackRook].Active = false;
                                    cells[SwapLeftBlackRook].IsMoved = true;
                                    cells[LeftBlackRook].IsMoved = true;
                                }
                                else if ((cells[CurrentCell].X == 0) && (cells[CurrentCell].Y == 5))
                                {
                                    int RightBlackRook = -1;
                                    int SwapRightBlackRook = -1;
                                    for (int p = 0; p < cells.Count; p++)
                                    {
                                        if (cells[p].X == 0 && cells[p].Y == 7)
                                        {
                                            RightBlackRook = p;
                                        }
                                        if (cells[p].X == 0 && cells[p].Y == 4)
                                        {
                                            SwapRightBlackRook = p;
                                        }
                                    }
                                    Cell Cell00 = new Cell(0, 0, "", cells[RightBlackRook]._State, Move, cells[RightBlackRook].IsBlack);
                                    cells[RightBlackRook]._State = cells[SwapRightBlackRook]._State;
                                    cells[SwapRightBlackRook]._State = Cell00._State;
                                    cells[SwapRightBlackRook].IsBlack = Cell00.IsBlack;
                                    cells[RightBlackRook].Active = false;
                                    cells[SwapRightBlackRook].Active = false;
                                    cells[SwapRightBlackRook].IsMoved = true;
                                    cells[RightBlackRook].IsMoved = true;
                                }
                            }
                            else if (cells[SwapingCell]._State == Cell.State.BlackKing && cells[SwapingCell].IsMoved == false)
                            {
                                if ((cells[CurrentCell].X == 7) && (cells[CurrentCell].Y == 1))
                                {
                                    int LeftBlackRook = -1;
                                    int SwapLeftBlackRook = -1;
                                    for (int p = 0; p < cells.Count; p++)
                                    {
                                        if (cells[p].X == 7 && cells[p].Y == 0)
                                        {
                                            LeftBlackRook = p;
                                        }
                                        if (cells[p].X == 7 && cells[p].Y == 2)
                                        {
                                            SwapLeftBlackRook = p;
                                        }
                                    }
                                    Cell Cell00 = new Cell(0, 0, "", cells[LeftBlackRook]._State, Move, cells[LeftBlackRook].IsBlack);
                                    cells[LeftBlackRook]._State = cells[SwapLeftBlackRook]._State;
                                    cells[SwapLeftBlackRook]._State = Cell00._State;
                                    cells[SwapLeftBlackRook].IsBlack = Cell00.IsBlack;
                                    cells[LeftBlackRook].Active = false;
                                    cells[SwapLeftBlackRook].Active = false;
                                    cells[SwapLeftBlackRook].IsMoved = true;
                                    cells[LeftBlackRook].IsMoved = true;
                                }
                                else if ((cells[CurrentCell].X == 7) && (cells[CurrentCell].Y == 5))
                                {
                                    int RightBlackRook = -1;
                                    int SwapRightBlackRook = -1;
                                    for (int p = 0; p < cells.Count; p++)
                                    {
                                        if (cells[p].X == 7 && cells[p].Y == 7)
                                        {
                                            RightBlackRook = p;
                                        }
                                        if (cells[p].X == 7 && cells[p].Y == 4)
                                        {
                                            SwapRightBlackRook = p;
                                        }
                                    }
                                    Cell Cell00 = new Cell(0, 0, "", cells[RightBlackRook]._State, Move, cells[RightBlackRook].IsBlack);
                                    cells[RightBlackRook]._State = cells[SwapRightBlackRook]._State;
                                    cells[SwapRightBlackRook]._State = Cell00._State;
                                    cells[SwapRightBlackRook].IsBlack = Cell00.IsBlack;
                                    cells[RightBlackRook].Active = false;
                                    cells[SwapRightBlackRook].Active = false;
                                    cells[SwapRightBlackRook].IsMoved = true;
                                    cells[RightBlackRook].IsMoved = true;
                                }
                            }
                                Cell cell0 = new Cell(0, 0, "", cells[SwapingCell]._State, Move, cells[SwapingCell].IsBlack);
                            cells[SwapingCell]._State = cells[CurrentCell]._State;
                            cells[CurrentCell]._State = cell0._State;
                            cells[CurrentCell].IsBlack = cell0.IsBlack;
                            cells[SwapingCell].Active = false;
                            cells[CurrentCell].Active = false;
                            cells[CurrentCell].IsMoved = true;
                            for (int i = 0; i < cells.Count; i++)
                            {
                                if (cells[i]._State != 0)
                                {
                                    if (cells[SwapingCell].IsBlack == cells[i].IsBlack)
                                    {
                                        cells[i].Locked = false;
                                    }
                                }
                            }
                            if (cells[SwapingCell].IsBlack == false)
                            {
                                timer_.Stop();
                                timer1_.Start();
                            }
                            else
                            {
                                timer1_.Stop();
                                timer_.Start();
                            }
                            if (IsMate(cells[CurrentCell]))
                            {
                                string Winner = "";
                                if(cells[CurrentCell].IsBlack)
                                {
                                    Winner = "черных";
                                }
                                else
                                {
                                    Winner = "белых";
                                }
                                WinnerWindow winnerWindow = new WinnerWindow("Победа "+ Winner);
                                winnerWindow.Show();
                                GameWindow.Close();
                            }
                            if((cells[CurrentCell]._State==Cell.State.WhitePawn && cells[CurrentCell].X==7))
                            {
                                NewPiecesWindowForWhite newPiecesWindow = new NewPiecesWindowForWhite(cells[CurrentCell]);
                                newPiecesWindow.Show();
                            }
                            else if ((cells[CurrentCell]._State == Cell.State.BlackPawn)&&(cells[CurrentCell].X == 0))
                            {
                                NewPiecesWindow newPiecesWindow = new NewPiecesWindow(cells[CurrentCell]);
                                newPiecesWindow.Show();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < cells.Count; i++)
                            {
                                if (cells[i] == GetKing(cells[SwapingCell].IsBlack))
                                {
                                    string col = cells[i].Color;
                                    cells[i].Color = "Red";
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                    cells[i].Color=col;
                                }
                            }
                        }
                    }
                    else if (cells[CurrentCell].IsBlack != cells[SwapingCell].IsBlack)
                    {
                        if (!IsKingChecked(cells[CurrentCell],cells[SwapingCell]))
                        {
                            cells[CurrentCell]._State = cells[SwapingCell]._State;
                            cells[CurrentCell].IsBlack = cells[SwapingCell].IsBlack;
                            cells[SwapingCell]._State = 0;
                            cells[SwapingCell].Active = false;
                            cells[CurrentCell].Active = false;
                            cells[CurrentCell].IsMoved = true;
                            for (int i = 0; i < cells.Count; i++)
                            {
                                if (cells[i]._State != 0)
                                {
                                    if (cells[SwapingCell].IsBlack == cells[i].IsBlack)
                                    {
                                        cells[i].Locked = false;
                                    }
                                }
                            }
                            if (cells[SwapingCell].IsBlack == false)
                            {
                                timer_.Stop();
                                timer1_.Start();
                            }
                            else
                            {
                                timer1_.Stop();
                                timer_.Start();
                            }
                            if(IsMate(cells[CurrentCell]))
                            {
                                string Winner = "";
                                if (cells[CurrentCell].IsBlack)
                                {
                                    Winner = "черных";
                                }
                                else
                                {
                                    Winner = "белых";
                                }
                                WinnerWindow winnerWindow = new WinnerWindow("Победа "+Winner);
                                winnerWindow.Show();
                                GameWindow.Close();
                            }
                            if ((cells[CurrentCell]._State == Cell.State.WhitePawn && cells[CurrentCell].X == 7))
                            {
                                NewPiecesWindowForWhite newPiecesWindow = new NewPiecesWindowForWhite(cells[CurrentCell]);
                                newPiecesWindow.Show();
                            }
                            else if ((cells[CurrentCell]._State == Cell.State.BlackPawn) && (cells[CurrentCell].X == 0))
                            {
                                NewPiecesWindow newPiecesWindow = new NewPiecesWindow(cells[CurrentCell]);
                                newPiecesWindow.Show();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < cells.Count; i++)
                            {
                                if (cells[i] == GetKing(cells[SwapingCell].IsBlack))
                                {
                                    string col = cells[i].Color;
                                    cells[i].Color = "Red";
                                    await Task.Delay(TimeSpan.FromSeconds(1));
                                    cells[i].Color = col;
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                posibleMoves(cell, true);
                for (int i = 0; i < cells.Count; i++)
                {
                    if (cells[i]._State != 0)
                    {
                         cells[i].Locked = true;
                    }
                }
            }
            OnPropertyChanged("Cells");
        }
        
        private void posibleMoves(Cell cell, bool IsFirt)
        {
            Vector[] posibleMoves = cell.posibleMoves(GetBarrriers());
            if (cell._State == Cell.State.WhiteKing || cell._State == Cell.State.BlackKing)
            {
                posibleMoves = MovesForCastling(cell);
            }
            if (posibleMoves != null)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    for (int j = 0; j < posibleMoves.Length; j++)
                    {
                        if ((cells[i].X == posibleMoves[j].X) && (cells[i].Y == posibleMoves[j].Y))
                        {
                            if (IsFirt)
                            {
                                if(cells[i]._State!=0)
                                {
                                    cells[i].BorderBrushColor = "White";
                                }
                                else
                                cells[i].ImageSource = Path.GetFullPath("Circle.png");
                            }
                            else
                            {
                                if (cells[i]._State != 0)
                                {
                                    cells[i].BorderBrushColor = "#333333";
                                }
                                else
                                cells[i].ImageSource = Path.GetFullPath("cell.png");
                            }

                        }
                    }

                }
            }
        }
        private bool IsKingChecked(Cell CurrentCell,Cell SwapingCell)
        {
            Cell king = GetKing(SwapingCell.IsBlack);
            Vector[] posibleMoves = null;
            bool KingChecked = false;
            for (int i = 0; i < cells.Count; i++)
            {
                if (( (cells[i]._State == Cell.State.WhitePawn))&& (cells[i].IsBlack != king.IsBlack))
                {
                    posibleMoves=new Vector[2];
                    posibleMoves[0] = new Vector(cells[i].X + 1,cells[i].Y+1);
                    posibleMoves[1] = new Vector(cells[i].X + 1,cells[i].Y-1);
                }
                else if ((cells[i]._State == Cell.State.BlackPawn) && (cells[i].IsBlack != king.IsBlack))
                {
                    posibleMoves = new Vector[2];
                    posibleMoves[0] = new Vector(cells[i].X - 1, cells[i].Y + 1);
                    posibleMoves[1] = new Vector(cells[i].X -1, cells[i].Y - 1);
                }
                else if (cells[i].IsBlack!=king.IsBlack)
                {
                    posibleMoves = cells[i].posibleMoves(GetBarrriers());
                }
                else
                {
                    continue;
                }
                if (posibleMoves == null) continue;
                if (SwapingCell == king)
                {
                    for (int j = 0; j < posibleMoves.Length; j++)
                    {
                        if (CurrentCell._State != 0)
                        {
                            if (((posibleMoves[j].X == CurrentCell.X) && (posibleMoves[j].Y == CurrentCell.Y)) || (cells[i].X == CurrentCell.X && cells[i].Y == CurrentCell.Y))
                            {
                                for (int k = 0; k < cells.Count; k++)
                                {
                                    if (cells[k].IsBlack != king.IsBlack)
                                    {
                                        Vector[] posibleMovesProtect = null;
                                        if (((cells[i]._State == Cell.State.WhitePawn)))
                                        {
                                            posibleMovesProtect = new Vector[2];
                                            posibleMovesProtect[0] = new Vector(cells[i].X + 1, cells[i].Y + 1);
                                            posibleMovesProtect[1] = new Vector(cells[i].X + 1, cells[i].Y - 1);
                                        }
                                        else if ((cells[i]._State == Cell.State.BlackPawn))
                                        {
                                            posibleMovesProtect = new Vector[2];
                                            posibleMovesProtect[0] = new Vector(cells[i].X - 1, cells[i].Y + 1);
                                            posibleMovesProtect[1] = new Vector(cells[i].X - 1, cells[i].Y - 1);
                                        }
                                        else
                                        { 
                                        Cell[] Barrriers = GetBarrriers();
                                        for (int p = 0; p < Barrriers.Length; p++)
                                        {
                                            if (Barrriers[p]._State == CurrentCell._State)
                                            {
                                                Barrriers[p] = new Cell(-1, -1, SwapingCell.Color, SwapingCell._State, Move, SwapingCell.IsBlack);
                                            }
                                        }
                                        posibleMovesProtect = cells[k].posibleMoves(Barrriers);
                                         }
                                        if (posibleMovesProtect != null)
                                        {
                                            for (int z = 0; z < posibleMovesProtect.Length; z++)
                                            {
                                                if ((posibleMovesProtect[z].X == CurrentCell.X) && (posibleMovesProtect[z].Y == CurrentCell.Y))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int k = 0; k < posibleMoves.Length; k++)
                            {
                                if ((posibleMoves[k].X == CurrentCell.X) && (posibleMoves[k].Y == CurrentCell.Y))
                                {
                                    return true;
                                }
                            }
                            continue;
                                }
                    }
                     continue;
                }
                else
                {

                    for (int j = 0; j < posibleMoves.Length; j++)
                    {
                        if ((posibleMoves[j].X == king.X) && (posibleMoves[j].Y == king.Y))
                        {

                            KingChecked=true;
                        }
                    }
                    if(KingChecked)
                    {
                        if ((cells[i].X == CurrentCell.X) && (cells[i].Y == CurrentCell.Y))
                        {
                            KingChecked = false;
                            continue;
                        }
                            Cell[] Barrriers = new Cell[GetBarrriers().Length+1];
                        for (int k = 0; k < GetBarrriers().Length; k++)
                        {
                            Barrriers[k] = GetBarrriers()[k];
                        }
                        Barrriers[GetBarrriers().Length] = CurrentCell;

                        Vector[] posibleMovesPlus = cells[i].posibleMoves(Barrriers);
                        for (int j = 0; j < posibleMovesPlus.Length; j++)
                        {
                            if ((posibleMovesPlus[j].X == king.X) && (posibleMovesPlus[j].Y == king.Y))
                            {

                                return true;
                            }
                        }
                        KingChecked = false;
                        continue;
                    }
                    else
                    {
                        Cell[] Barrriers =GetBarrriers();
                        for (int k = 0; k < Barrriers.Length; k++)
                        {
                            if(Barrriers[k]==SwapingCell)
                            {
                                Barrriers[k] = new Cell(CurrentCell.X,CurrentCell.Y,SwapingCell.Color,SwapingCell._State,Move,SwapingCell.IsBlack);
                            }
                        }
                        posibleMoves = cells[i].posibleMoves(Barrriers);
                        for (int j = 0; j < posibleMoves.Length; j++)
                        {
                            if ((posibleMoves[j].X == king.X) && (posibleMoves[j].Y == king.Y))
                            {
                                if(CurrentCell.X==cells[i].X && CurrentCell.Y==cells[i].Y)
                                {
                                    continue;
                                }
                                else return true;
                            }
                        }
                        continue;
                    }
                }
            }
            return KingChecked;
        }
        private Cell GetKing(bool IsBlack)
        {
            Cell king = null;
            for (int i = 0; i < cells.Count; i++)
            {
                if (((IsBlack == true) && (cells[i]._State == Cell.State.BlackKing)) || ((IsBlack == false) && (cells[i]._State == Cell.State.WhiteKing)))
                {
                    king = cells[i];
                }
            }
            return king;
        }
        private Cell[] GetBarrriers()
        {
            List<Cell> barriers = new List<Cell>();
            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i]._State != 0)
                {
                    barriers.Add(cells[i]);
                }

            }
            Cell[] _barriers = new Cell[barriers.Count];
            for (int i = 0; i < barriers.Count; i++)
            {
                _barriers[i] = barriers[i];
            }
            return _barriers;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            double time = 0.02;
            Timer -= TimeSpan.FromHours(time);
        }
        private void timer_Tick1(object sender, EventArgs e)
        {
            double time = 0.02;
            Timer1 -= TimeSpan.FromHours(time);
        }

        public GameViewModel(double PlayTime, GameWindow gameWindow)
        {
            GameWindow = gameWindow;
            double time = PlayTime;
            timer0 = TimeSpan.FromHours(time);
            timer1 = TimeSpan.FromHours(time);
            timer_.Tick += new EventHandler(timer_Tick);
            timer_.Interval = new TimeSpan(0, 0, 1);
            timer1_.Tick += new EventHandler(timer_Tick1);
            timer1_.Interval = new TimeSpan(0, 0, 1);
            timer_.Start();

            Surrender1 = new RelayCommand(Command_Execute_Surrender1);
            Surrender2 = new RelayCommand(Command_Execute_Surrender2);
            Draw = new RelayCommand(Command_Execute_Draw);
            Move = new RelayCommand(Command_Execute_Move);
            string Color;
            string ImageSource;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i % 2 != 0)
                    {
                        if (j % 2 == 0)
                        {
                            Color = "#44944A";
                        }
                        else
                        {
                            Color = "#F5F5DC";
                        }
                    }
                    else
                    {
                        if (j % 2 != 0)
                        {
                            Color = "#44944A";
                        }
                        else
                        {
                            Color = "#F5F5DC";
                        }
                    }
                    if((i==0)&&((j==0)||(j==7)))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.WhiteRook, Move,false));
                    }
                    else if ((i == 7) && ((j == 0) || (j == 7)))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.BlackRook, Move,true));
                    }
                    else if ((i == 0) && ((j == 1) || (j == 6)))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.WhiteKnight, Move,false));
                    }
                    else if ((i == 7) && ((j == 1) || (j == 6)))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.BlackKnight, Move,true));
                    }
                    else if ((i == 0) && ((j == 2) || (j == 5)))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.WhiteBishop, Move,false));
                    }
                    else if ((i == 7) && ((j == 2) || (j == 5)))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.BlackBishop, Move,true));
                    }
                    else if ((i == 0) && (j == 3))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.WhiteKing, Move,false));
                    }
                    else if ((i == 7) && (j == 3))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.BlackKing, Move,true));
                    }
                    else if ((i == 0) && (j == 4))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.WhiteQueen, Move,false));
                    }
                    else if ((i == 7) && (j == 4))
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.BlackQueen, Move,true));
                    }
                    else if (i == 1)
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.WhitePawn, Move,false));
                    }
                    else if (i == 6)
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.BlackPawn, Move,true));
                    }
                    else
                    {
                        cells.Add(new Cell(i, j, Color, Cell.State.Empty, Move,false));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
