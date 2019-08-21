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

namespace TallennaDialog
{
    /// <summary>
    /// Dialogi joka kysyy käyttäjältä nimimerkin ja tallentaa ennätyksen.
    /// </summary>
    public partial class TallennaIkkuna : Window
    {
        private double voitot = 0;
        private double aloitus = 0;
        private Boolean virhe = true;

        /// <summary>
        /// DependencyProperty nimimerkille.
        /// </summary>
        public static readonly DependencyProperty NimimerkkiProperty = DependencyProperty.Register(
           "Nimimerkki",
            typeof(String), // propertyn tietotyyppi
            typeof(TallennaIkkuna), // luokka jossa property sijaitsee
            new FrameworkPropertyMetadata("",  // propertyn oletusarvo
            FrameworkPropertyMetadataOptions.AffectsRender)); // vaikuttaa luokan ulkoasuun (textbox päivittyy)

        /// <summary>
        /// Nimimerkki, joka tallennetaan.
        /// </summary>
        public String Nimimerkki
        {
            get { return (String)GetValue(NimimerkkiProperty); }
            set { SetValue(NimimerkkiProperty, value); }
        }

        /// <summary>
        /// Luo TallennaIkkunan.
        /// </summary>
        public TallennaIkkuna()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Konstruktori, jolle tuodaan parametrina tallennettava summa.
        /// </summary>
        /// <param name="summa">Voittosumma joka ennätykseen tallennetaan</param>
        /// <param name="summa">Pelaajan alussa valitsema aloitusraha</param>
        public TallennaIkkuna(double summa, double alkusumma)
        {
            InitializeComponent();

            voitot = summa;
            aloitus = alkusumma;

            AsetaVoitot();
            textBoxNimimerkki.Focus();
        }

        /// <summary>
        /// Asettaa voittosumman ja aloitusrahan näkyviin ikkunan labeleihin.
        /// </summary>
        private void AsetaVoitot()
        {
            String voitotTeksti = String.Format("{0:0.00}", voitot);
            labelVoitotRuutu.Content = voitotTeksti.Replace(',', '.');

            String alkurahaTeksti = String.Format("{0:0.00}", aloitus);
            labelAlkurahaRuutu.Content = alkurahaTeksti.Replace(',', '.');
        }

        /// <summary>
        /// Sulkee ikkunan ja asettaa DialogResult = true jos nimimerkissä ei virheitä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTallenna_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxNimimerkki.Text.Length == 0) labelVirhe.Content = Properties.Resources.labelVirheTyhja;
            if (virhe)
            {
                textBoxNimimerkki.Focus();
                return;
            }

            DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Sulkee ikkunan ja asettaa DialogResult = false.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPeruuta_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Tarkistaa onko nimimerkki syötetty oikein.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxNimimerkki_TextChanged(object sender, TextChangedEventArgs e)
        {
            labelVirhe.Content = "";
            virhe = false;

            if (textBoxNimimerkki.Text.Length == 0)
            {
                labelVirhe.Content = Properties.Resources.labelVirheTyhja;
                virhe = true;
                return;
            }

            Char[] merkit = textBoxNimimerkki.Text.ToCharArray();
            foreach (char merkki in merkit)
            {
                if (!Char.IsLetter(merkki))
                {
                    labelVirhe.Content = Properties.Resources.labelVirheVaarin;
                    virhe = true;
                }
            }
        }
    }

    /// <summary>
    /// Luokka nimimerkin oikeellisuustarkistuksille.
    /// </summary>
    class NimimerkkiRule : ValidationRule
    {
        /// <summary>
        /// Tarkistaa onko nimimerkki oikeanlainen.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns>Onko syötetty oikeanlainen nimimerkki</returns>
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            String teksti = value.ToString();
            Char[] merkit = teksti.ToCharArray();

            if (teksti.Length == 0) return new ValidationResult(false, "Nimi ei saa olla tyhjä!");

            foreach (char merkki in merkit)
            {
                if (!Char.IsLetter(merkki)) return new ValidationResult(false, "Virheellinen merkki. Vain aakkoset kelpaavat!"); ;
            }

            return ValidationResult.ValidResult;
        }
    }
}
