using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EnnatyksetDialog
{
    /// <summary>
    /// Dialogi joka näyttää ennätykset.
    /// </summary>
    public partial class EnnatyksetIkkuna : Window
    {
        private List<String> ennatykset = new List<string>();

        /// <summary>
        /// Luo EnnätyksetIkkunan ja asettaa ennätykset tiedostosta textblockiin.
        /// </summary>
        public EnnatyksetIkkuna()
        {
            InitializeComponent();

            AsetaEnnatykset();
        }

        /// <summary>
        /// Lukee ennätykset tiedostosta listaan ja järjestää ne voittojen 
        /// mukaan suuruusjärjestykseen (suurin ensin).
        /// </summary>
        public void LueEnnatykset()
        {
            EnnatysVertailija vertailija = new EnnatysVertailija();
            ennatykset.Clear();

            if (File.Exists("ennatykset.txt"))
            {
                String rivi = "";
                System.IO.StreamReader tiedosto = new System.IO.StreamReader("ennatykset.txt");
                while ((rivi = tiedosto.ReadLine()) != null)
                {
                    ennatykset.Add(rivi);
                }
                tiedosto.Close();
            }

            ennatykset.Sort(vertailija);
            ennatykset.Reverse();
        }

        /// <summary>
        /// Asettaa ennätykset textblockiin.
        /// </summary>
        private void AsetaEnnatykset()
        {
            LueEnnatykset();
            textBlockEnnatykset.Inlines.Clear();

            if (ennatykset.Count == 0)
            {
                textBlockEnnatykset.Inlines.Add(Properties.Resources.labelEiEnnatyksia);
                return;
            }

            int numero = 1;
            foreach (String ennatys in ennatykset)
            {
                Bold sijoitus = new Bold(new Run(numero + ". "));
                textBlockEnnatykset.Inlines.Add(sijoitus);

                String[] sanat = ennatys.Split(' ');
                textBlockEnnatykset.Inlines.Add(new Run(sanat[0] + " "));
                Bold tummennus = new Bold(new Run(sanat[1] + " " + sanat[2] + " "));
                textBlockEnnatykset.Inlines.Add(tummennus);
                textBlockEnnatykset.Inlines.Add(new Run(sanat[3] + " "));
                textBlockEnnatykset.Inlines.Add(new Run(sanat[4] + " "));
                textBlockEnnatykset.Inlines.Add(new Run(sanat[5]));

                textBlockEnnatykset.Inlines.Add(new LineBreak());
                numero++;
            }
        }

        /// <summary>
        /// Tarkistaa onko voittosumma uusi ennätys.
        /// </summary>
        /// <param name="summa">Voitot jotka tarkistetaan</param>
        /// <returns>Onko voittosumma uusi ennätys</returns>
        public Boolean OnkoEnnatys(double summa)
        {
            LueEnnatykset();

            if (ennatykset.Count < 10) return true; // Jos ei vielä 10 ennätystä.

            String[] ennatys = ennatykset[9].Split(' ');
            double arvo = Convert.ToDouble(ennatys[1].Replace('.', ','));

            if (summa <= arvo) return false; // Jos ei uusi ennätys.

            else return true;
        }

        /// <summary>
        /// Poistaa viimeisen ennätyksen.
        /// </summary>
        public void PoistaViimeinen()
        {
            LueEnnatykset();

            ennatykset.Remove(ennatykset[9]);
            System.IO.File.WriteAllText(@"ennatykset.txt", String.Empty);

            System.IO.StreamWriter tiedosto = new System.IO.StreamWriter(@"ennatykset.txt", true);
            foreach (String rivi in ennatykset)
            {
                tiedosto.WriteLine(rivi);
            }

            tiedosto.Close();
        }

        /// <summary>
        /// Laskee tallennettujen ennätysten määrän.
        /// </summary>
        /// <returns>Tallennettujen ennätysten määrä</returns>
        public int LaskeEnnatykset()
        {
            ennatykset.Clear();


            if (File.Exists("ennatykset.txt"))
            {
                String rivi = "";
                System.IO.StreamReader tiedosto = new System.IO.StreamReader("ennatykset.txt");
                while ((rivi = tiedosto.ReadLine()) != null)
                {
                    ennatykset.Add(rivi);
                }
                tiedosto.Close();
            }
            return ennatykset.Count;
        }

        /// <summary>
        /// Sulkee ikkunan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSulje_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// Luokka joka vertaa kahta ennätystä.
    /// </summary>
    public class EnnatysVertailija : IComparer<String>
    {
        /// <summary>
        /// Vertaa kahta ennätystä niiden voittojen perusteella.
        /// </summary>
        /// <param name="x">Vertailtava ennätys</param>
        /// <param name="y">Verrattava ennätys</param>
        /// <returns>1 jos x isompi, -1 jos y isompi, 0 jos yhtäsuuret</returns>
        public int Compare(String x, String y)
        {
            String[] ennatys1 = x.Split(' ');
            String[] ennatys2 = y.Split(' ');

            double arvo1 = Convert.ToDouble(ennatys1[1].Replace('.', ','));
            double arvo2 = Convert.ToDouble(ennatys2[1].Replace('.', ','));

            return arvo1.CompareTo(arvo2);
        }
    }
}
