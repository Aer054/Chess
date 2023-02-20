using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Логика взаимодействия для DrawWindow.xaml
    /// </summary>
    public partial class DrawWindow : Window
    {
        public DrawWindow(GameWindow GameWindow)
        {
            InitializeComponent();
            DataContext=new DrawViewModel(this, GameWindow);
        }
    }
}
