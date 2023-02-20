using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows;
using System.IO;

namespace Chess
{
    public class Cell: INotifyPropertyChanged
    {
        private int x,y;
        private bool isBlack;
        private string imageSource;
        private string color;
        private string borderBrushColor;
        private bool active;
        private bool locked;
        private State state;
        private bool isMoved;
        public ICommand Move { set; get; }
        #region
        public string Color
        {
            get { return color; }
            set 
            { 
                color = value;
                OnPropertyChanged("Color");
            }
        }
        public string BorderBrushColor
        {
            get { return borderBrushColor; }
            set
            {
                borderBrushColor = value;
                OnPropertyChanged("BorderBrushColor");
            }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; }
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
        public bool IsBlack
        {
            get { return isBlack; }
            set { isBlack = value; }
        }
        public bool IsMoved
        {
            get { return isMoved; }
            set { isMoved = value; }
        }
        public bool Locked
        {
            get { return locked; }
            set
            { 
                locked = value;
                OnPropertyChanged("Locked");
            }
        }
        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public State _State
        {
            get { return state; }
            set 
            { 
                state = value;
                setImage();
                OnPropertyChanged("ImageSource");
            }
        }
        #endregion
        public Cell(int X, int Y, string color0, State state0, ICommand Command, bool isBlack0)
        {
            x = X;
            y = Y;
            state = state0;
            Move = Command;
            color = color0;
            active = false;
            isBlack = isBlack0;
            isMoved = false;
            if (state == 0)
            {
                locked = true;
            }
            else if (isBlack0==true)
            {
                locked=false;
            }
            else 
            {
                locked = true;
            }
            borderBrushColor = "#333333";
            setImage();
            
         }
        private void setImage()
        {
            switch(state)
            {
                case State.Empty:
                    imageSource = Path.GetFullPath("cell.png");
                    break;
                case State.WhitePawn:
                    imageSource = Path.GetFullPath("White_Pawn.png");
                    break ;
                case State.BlackPawn:
                    imageSource = Path.GetFullPath("Black_Pawn.png");
                    break;
                case State.WhiteKing:
                    imageSource = Path.GetFullPath("White_King.png");
                    break;
                case State.BlackKing:
                    imageSource = Path.GetFullPath("Black_King.png");
                    break;
                case State.WhiteQueen:
                    imageSource = Path.GetFullPath("White_Queen.png");
                    break;
                case State.BlackQueen:
                    imageSource = Path.GetFullPath("Black_Queen.png");
                    break;
                case State.WhiteRook:
                    imageSource = Path.GetFullPath("White_Rook.png");
                    break;
                case State.BlackRook:
                    imageSource = Path.GetFullPath("Black_Rook.png");
                    break;
                case State.WhiteBishop:
                    imageSource = Path.GetFullPath("White_Bishop.png");
                    break;
                case State.BlackBishop:
                    imageSource = Path.GetFullPath("Black_Bishop.png");
                    break;
                case State.WhiteKnight:
                    imageSource = Path.GetFullPath("White_Knight.png");
                    break;
                case State.BlackKnight:
                    imageSource = Path.GetFullPath("Black_Knight.png");
                    break;
            }
        }
        public Vector[] posibleMoves(Cell[] barriers)
        {
            switch (state)
            {
                case State.Empty:
                    return null;
                    break;
                case State.WhitePawn:
                    {
                        List<Vector> posibleMoves= new List<Vector>();
                        if (x == 1)
                        {
                            bool b = true;
                            bool b2 = true;
                            for (int i = 0; i < barriers.Length; i++)
                            {
                                if ((barriers[i].X == X + 1) && (barriers[i].Y == Y))
                                {
                                    b = false;
                                    b2 = false;
                                }
                                if ((barriers[i].X == X + 2) && (barriers[i].Y == Y))
                                {
                                    b2 = false;
                                }
                            }
                            if (b) posibleMoves.Add(new Vector(X + 1, Y));
                            if(b2) posibleMoves.Add(new Vector(X + 2, Y));
                        }
                        else
                        {
                            bool b = true;
                            for (int i = 0; i < barriers.Length; i++)
                            {
                                if ((barriers[i].X == X + 1) && (barriers[i].Y == Y))
                                {
                                    b = false;
                                }

                            }
                            if (b) posibleMoves.Add(new Vector(X + 1, Y));
                        }
                            for (int i = 0; i < barriers.Length; i++)
                        {
                            if((barriers[i].X == X + 1)&& (barriers[i].IsBlack != isBlack))
                            {
                                if(barriers[i].Y == Y + 1)
                                {
                                    posibleMoves.Add(new Vector(X + 1, Y+1));
                                }
                                else if(barriers[i].Y == Y - 1)
                                {
                                    posibleMoves.Add(new Vector(X + 1, Y - 1));
                                }
                            }
                        }
                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.BlackPawn:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        if (x == 6)
                        {
                            bool b = true;
                            bool b2 = true;
                            for (int i = 0; i < barriers.Length; i++)
                            {
                                if ((barriers[i].X == X - 1) && (barriers[i].Y == Y))
                                {
                                    b = false;
                                    b2 = false;
                                }
                                if ((barriers[i].X == X - 2) && (barriers[i].Y == Y))
                                {
                                    b2 = false;
                                }
                            }
                            if (b) posibleMoves.Add(new Vector(X - 1, Y));
                            if (b2) posibleMoves.Add(new Vector(X - 2, Y));
                        }
                        else
                        {
                            bool b = true;
                            for (int i = 0; i < barriers.Length; i++)
                            {
                                if ((barriers[i].X == X - 1) && (barriers[i].Y == Y))
                                {
                                    b = false;
                                }

                            }
                            if (b) posibleMoves.Add(new Vector(X - 1, Y));
                        }
                        for (int i = 0; i < barriers.Length; i++)
                        {
                            if ((barriers[i].X == X - 1) && (barriers[i].IsBlack != isBlack))
                            {
                                if (barriers[i].Y == Y + 1)
                                {
                                    posibleMoves.Add(new Vector(X - 1, Y + 1));
                                }
                                else if (barriers[i].Y == Y - 1)
                                {
                                    posibleMoves.Add(new Vector(X - 1, Y - 1));
                                }
                            }
                        }
                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.BlackKnight:
                    {
                        int n = 8;
                        Vector[] posibleMoves = new Vector[n];
                        posibleMoves[0] = new Vector(X + 2, Y + 1);
                        posibleMoves[1] = new Vector(X + 2, Y - 1);
                        posibleMoves[2] = new Vector(X - 2, Y + 1);
                        posibleMoves[3] = new Vector(X - 2, Y - 1);
                        posibleMoves[4] = new Vector(X + 1, Y + 2);
                        posibleMoves[5] = new Vector(X - 1, Y + 2);
                        posibleMoves[6] = new Vector(X + 1, Y - 2);
                        posibleMoves[7] = new Vector(X - 1, Y - 2);
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if((barriers[j].X==posibleMoves[i].X)&&(barriers[j].Y == posibleMoves[i].Y)&&(barriers[j].IsBlack==isBlack))
                                {
                                    posibleMoves[i] = new Vector(-1, -1);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            if ((posibleMoves[i].X > 7) || (posibleMoves[i].X < 0) || (posibleMoves[i].Y > 7) || (posibleMoves[i].Y < 0))
                            {
                                posibleMoves[i] = new Vector(-1, -1);
                            }
                        }

                        return posibleMoves;
                        break;

                    }
                case State.WhiteKnight:
                    {
                        int n = 8;
                        Vector[] posibleMoves = new Vector[n];
                        posibleMoves[0] = new Vector(X + 2, Y + 1);
                        posibleMoves[1] = new Vector(X + 2, Y - 1);
                        posibleMoves[2] = new Vector(X - 2, Y + 1);
                        posibleMoves[3] = new Vector(X - 2, Y - 1);
                        posibleMoves[4] = new Vector(X + 1, Y + 2);
                        posibleMoves[5] = new Vector(X - 1, Y + 2);
                        posibleMoves[6] = new Vector(X + 1, Y - 2);
                        posibleMoves[7] = new Vector(X - 1, Y - 2);
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == posibleMoves[i].X) && (barriers[j].Y == posibleMoves[i].Y) && (barriers[j].IsBlack == isBlack))
                                {
                                    posibleMoves[i] = new Vector(-1, -1);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            if ((posibleMoves[i].X > 7) || (posibleMoves[i].X < 0) || (posibleMoves[i].Y > 7) || (posibleMoves[i].Y < 0))
                            {
                                posibleMoves[i] = new Vector(-1, -1);
                            }
                        }

                        return posibleMoves;
                        break;

                    }
                case State.WhiteRook:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        int k = 1;
                        bool p = false;
                        while ((Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y + k))
                                {
                                    if(barriers[j].isBlack!=isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X,Y+k));
                                        for (int h = Y + k+1; h < 8;h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y + k; h < 8;h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                   
                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y + k));
                            k++; 
                        }
                        k = 1;
                        p = false;
                        while ((Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y ==Y-k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y - k));
                                        for (int h = Y - k-1; h >= 0;h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y - k; h >= 0;h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                       
                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y - k));
                            k++;
                        }
                        k = 1;
                        p= false;
                        while ((X + k < 8) && (X + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X+k) && (barriers[j].Y == Y))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y));
                                        for (int h = X + k + 1; h < 8;h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X + k; h < 8;h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X + k, Y));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y))
                                {

                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add( new Vector(X - k, Y));
                                        for (int h = X - k-1; h >= 0;h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X - k; h >= 0;h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X - k, Y ));
                            k++;
                        }
                       
                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.BlackRook:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        int k = 1;
                        bool p = false;
                        while ((Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y + k));
                                        for (int h = Y + k + 1; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y + k; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y - k));
                                        for (int h = Y - k - 1; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y - k; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y));
                                        for (int h = X + k + 1; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X + k; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X + k, Y));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y))
                                {

                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y));
                                        for (int h = X - k - 1; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X - k; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X - k, Y));
                            k++;
                        }

                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.BlackBishop:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        int k = 1;
                        bool p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add( new Vector(X + k, Y + k));
                                        posibleMoves.Add( new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y - k));
                            k++;
                        }
                        k = 1;
                        p=false;
                        while ((X - k < 8) && (X - k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y + k));
                                        posibleMoves.Add( new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y + k));
                            k++;
                        }
                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.WhiteBishop:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        int k = 1;
                        bool p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y + k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y + k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y + k));
                            k++;
                        }
                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.BlackQueen:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        int k = 1;
                        bool p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y + k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y + k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y + k));
                                        for (int h = Y + k + 1; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y + k; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y - k));
                                        for (int h = Y - k - 1; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y - k; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y));
                                        for (int h = X + k + 1; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X + k; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X + k, Y));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y))
                                {

                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y));
                                        for (int h = X - k - 1; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X - k; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X - k, Y));
                            k++;
                        }

                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.WhiteQueen:
                    {
                        List<Vector> posibleMoves = new List<Vector>();
                        int k = 1;
                        bool p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y + k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0) && (Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y - k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X + k, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0) && (Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y + k));
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                    else
                                    {
                                        posibleMoves.Add(new Vector(-1, -1));
                                        p = true;
                                    }
                                }
                            }
                            if (p == true) break;
                            posibleMoves.Add(new Vector(X - k, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((Y + k < 8) && (Y + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y + k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y + k));
                                        for (int h = Y + k + 1; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y + k; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y + k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((Y - k < 8) && (Y - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X) && (barriers[j].Y == Y - k))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X, Y - k));
                                        for (int h = Y - k - 1; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = Y - k; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X, Y - k));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X + k < 8) && (X + k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X + k) && (barriers[j].Y == Y))
                                {
                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X + k, Y));
                                        for (int h = X + k + 1; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X + k; h < 8; h++)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X + k, Y));
                            k++;
                        }
                        k = 1;
                        p = false;
                        while ((X - k < 8) && (X - k >= 0))
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == X - k) && (barriers[j].Y == Y))
                                {

                                    if (barriers[j].isBlack != isBlack)
                                    {
                                        posibleMoves.Add(new Vector(X - k, Y));
                                        for (int h = X - k - 1; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }
                                    else
                                    {
                                        for (int h = X - k; h >= 0; h--)
                                        {
                                            posibleMoves.Add(new Vector(-1, -1));
                                            p = true;
                                        }
                                    }

                                }
                            }
                            if (p) break;
                            posibleMoves.Add(new Vector(X - k, Y));
                            k++;
                        }

                        Vector[] _posibleMoves = new Vector[posibleMoves.Count];
                        for (int i = 0; i < _posibleMoves.Length; i++)
                        {
                            _posibleMoves[i] = posibleMoves[i];
                        }
                        return _posibleMoves;
                        break;
                    }
                case State.BlackKing:
                    {
                        int n = 8;
                        Vector[] posibleMoves = new Vector[n];
                        posibleMoves[0] = new Vector(X , Y + 1);
                        posibleMoves[1] = new Vector(X, Y - 1);
                        posibleMoves[2] = new Vector(X +1, Y);
                        posibleMoves[3] = new Vector(X-1, Y);
                        posibleMoves[4] = new Vector(X + 1, Y + 1);
                        posibleMoves[5] = new Vector(X - 1, Y -1);
                        posibleMoves[6] = new Vector(X + 1, Y - 1);
                        posibleMoves[7] = new Vector(X - 1, Y +1);
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == posibleMoves[i].X) && (barriers[j].Y == posibleMoves[i].Y) && (barriers[j].IsBlack == isBlack))
                                {
                                    posibleMoves[i] = new Vector(-1, -1);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            if ((posibleMoves[i].X > 7) || (posibleMoves[i].X < 0) || (posibleMoves[i].Y > 7) || (posibleMoves[i].Y < 0))
                            {
                                posibleMoves[i] = new Vector(-1, -1);
                            }
                        }
                        return posibleMoves;
                        break;
                    }
                case State.WhiteKing:
                    {
                        int n = 8;
                        Vector[] posibleMoves = new Vector[n];
                        posibleMoves[0] = new Vector(X, Y + 1);
                        posibleMoves[1] = new Vector(X, Y - 1);
                        posibleMoves[2] = new Vector(X + 1, Y);
                        posibleMoves[3] = new Vector(X - 1, Y);
                        posibleMoves[4] = new Vector(X + 1, Y + 1);
                        posibleMoves[5] = new Vector(X - 1, Y - 1);
                        posibleMoves[6] = new Vector(X + 1, Y - 1);
                        posibleMoves[7] = new Vector(X - 1, Y + 1);
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            for (int j = 0; j < barriers.Length; j++)
                            {
                                if ((barriers[j].X == posibleMoves[i].X) && (barriers[j].Y == posibleMoves[i].Y) && (barriers[j].IsBlack == isBlack))
                                {
                                    posibleMoves[i] = new Vector(-1, -1);
                                    break;
                                }
                            }
                        }
                        for (int i = 0; i < posibleMoves.Length; i++)
                        {
                            if ((posibleMoves[i].X > 7) || (posibleMoves[i].X < 0) || (posibleMoves[i].Y > 7) || (posibleMoves[i].Y < 0))
                            {
                                posibleMoves[i] = new Vector(-1, -1);
                            }
                        }
                        return posibleMoves;
                        break;
                    }
                default:
                    return null;
                    break;
            }
        }
        public enum State
        {
            Empty,       
            WhiteKing,   
            WhiteQueen,  
            WhiteRook,  
            WhiteKnight, 
            WhiteBishop, 
            WhitePawn,  
            BlackKing,
            BlackQueen,
            BlackRook,
            BlackKnight,
            BlackBishop,
            BlackPawn
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}