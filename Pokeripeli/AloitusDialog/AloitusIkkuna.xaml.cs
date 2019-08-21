using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

///<summary>Pokeripelin aloitusikkuna</summary>
///<datecreated>05.12.2012</datecreated>
namespace AloitusDialog
{
    /// <summary>
    /// Pokeripelin aloitusikkuna jossa kysytään pelin aloitusrahan määrä.
    /// </summary>
    public partial class AloitusIkkuna : Window
    {
        private double maara = 5;

        /// <summary>
        /// Pelin aloitusrahan määrä.
        /// </summary>
        public double Maara
        {
            get { return maara; }
        }

        /// <summary>
        /// Luo aloitusikkunan.
        /// </summary>
        public AloitusIkkuna()
        {
            InitializeComponent();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            maara = 2;
        }

        private void radioButton5_Checked(object sender, RoutedEventArgs e)
        {
            maara = 5;
        }

        private void radioButton10_Checked(object sender, RoutedEventArgs e)
        {
            maara = 10;
        }

        private void radioButton15_Checked(object sender, RoutedEventArgs e)
        {
            maara = 15;
        }

        private void radioButton20_Checked(object sender, RoutedEventArgs e)
        {
            maara = 20;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void buttonPeruuta_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
