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
    /// Interaction logic for Customization.xaml
    /// </summary>
    public partial class Customization : Window
    {
        Gamevm _gamevm;
        public Customization(Gamevm gamevm)
        {
            _gamevm = gamevm;
            InitializeComponent();
        }

        private void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Data is converted here rather than the viewmodel, as the Slider class cannot be accessed there.
            Slider slider = e.Source as Slider;
            string name = (string)slider.Tag;
            int value = (int)slider.Value;
            _gamevm.UpdatePlayerSkills(name, value);
        }

        private void ContinueClicked(object sender, RoutedEventArgs e)
        {
            // Data is converted here rather than the viewmodel, as the ComboBoxItem class cannot be accessed there.
            ComboBoxItem cbi_weapon = (ComboBoxItem)cbox_Weapons.SelectedValue;
            string name = (string)cbi_weapon.Tag;
            _gamevm.UpdatePlayerWeapon(name);
            _gamevm.BeginGame(_gamevm);
            Close();
        }
    }
}
