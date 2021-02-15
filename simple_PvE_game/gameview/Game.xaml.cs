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

namespace simple_PvE_game.gameview
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        Gamevm _gamevm;
        public Game(Gamevm gamevm)
        {
            _gamevm = gamevm;
            DataContext = gamevm;
            InitializeComponent();
        }

        private void ClickedMove(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            string name = (string)btn.Tag;
            _gamevm.Player_DetMove(name);       
        }
    }
}
