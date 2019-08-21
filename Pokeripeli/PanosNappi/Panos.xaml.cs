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
using System.ComponentModel;

///<summary>Pokeripelin panosnappi</summary>
///<datecreated>11.12.2012</datecreated>
namespace PanosNappi
{
    /// <summary>
    /// Pokeripelin panosnappi, jossa näkyy kulloinkin käytössä oleva panos.
    /// </summary>
    public partial class Panos : UserControl
    {
        public delegate void ClickTapahtunutHandler();
        public event ClickTapahtunutHandler ClickTapahtunut;

        private double valittuPanos = 0.20;
        private Boolean klikattavissa = true;
        private double[] panokset = {0, 0.20, 0.40, 0.60, 0.80, 1.00}; 

        public static readonly DependencyProperty PanosArvoProperty =
 DependencyProperty.Register(
   "PanosArvo",
   typeof(String), // propertyn tietotyyppi
   typeof(Panos), // luokka jossa property sijaitsee
   new FrameworkPropertyMetadata("0.20",  // propertyn oletusarvo
        FrameworkPropertyMetadataOptions.AffectsRender, // vaikuttaa luokan ulkoasuun (textbox päivittyy)
         new PropertyChangedCallback(OnValueChanged),  // kutsutaan propertyn arvon muuttumisen jälkeen
        new CoerceValueCallback(MuutaPanosta))); // kutsutaan ennen propertyn arvon muutosta

        // seuraavat tehtävä juuri näin. Ei mitään tarkistuksien lisäämistä
        public String PanosArvo
        {
            get { return (String)GetValue(PanosArvoProperty); }
            set { SetValue(PanosArvoProperty, value); }
        }

        // tätä kutsutaan ennen laskurin muuttamista ja voidaan tässä vaiheessa
        // tehdä tarkistuksia ja muuttaa laskuriin asetettavaa arvoa
        private static object MuutaPanosta(DependencyObject element, object value)
        {
            String panos = (String)value;
            return panos;
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
        }

        /// <summary>
        /// Panos joka on valittuna.
        /// </summary>
        public double ValittuPanos
        {
            get { return Math.Round(valittuPanos, 2); }
            set 
            { 
                valittuPanos = value;
                String arvo = String.Format("{0:0.00}", valittuPanos);
                PanosArvo = arvo.Replace(',', '.');
            }
        }

        /// <summary>
        /// Onko panosnappi klikattavissa.
        /// </summary>
        public Boolean Klikattavissa
        {
            get { return Klikattavissa; }
            set 
            { 
                klikattavissa = value;
                if (klikattavissa) this.IsEnabled = true;
                else this.IsEnabled = false;
            }
        }

        /// <summary>
        /// Parametriton konstruktori.
        /// </summary>
        public Panos()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Asettaa panoksen parametrina tuodun ylärajan mukaan.
        /// </summary>
        /// <param name="ylaraja">Yläraja mitä suurempi panos ei saa olla</param>
        public void AsetaPanos(double ylaraja)
        {
            foreach (double summa in panokset)
            {
                if (Math.Round(ylaraja, 2) <= summa)
                {
                    ValittuPanos = summa;
                    if (ClickTapahtunut != null) ClickTapahtunut();
                    return;
                }
            }
        }

        /// <summary>
        /// Kasvattaa panosta kun nappia painetaan. 
        /// Aiheuttaa ClickTapahtunut-tapahtuman.
        /// Muuttaa PanosArvo-propertyn arvon.
        /// </summary>
        private void buttonPanos_Click(object sender, RoutedEventArgs e)
        {
            if (klikattavissa)
            {
                if (valittuPanos < 1) ValittuPanos += 0.20;
                else ValittuPanos = 0.20;

                if (ClickTapahtunut != null) ClickTapahtunut();
            }
        }
    }
       
}
