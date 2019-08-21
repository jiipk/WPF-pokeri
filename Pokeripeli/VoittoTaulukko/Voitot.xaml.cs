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

///<summary>Pokeripelin voittotaulukko</summary>
///<datecreated>12.12.2012</datecreated>
namespace VoittoTaulukko
{
    /// <summary>
    /// Pokeripelin voittotaulukko, joka myös tarkistaa voittokädet ja ilmoittaa voittosummat.
    /// </summary>
    public partial class Voitot : UserControl
    {
        private double viitoset = 10.00;
        private double varisuora = 6.00;
        private double neloset = 3.00;
        private double tayskasi = 1.60;
        private double vari = 0.80;
        private double suora = 0.60;
        private double kolmoset = 0.40;
        private double kaksiparia = 0.40;

        /// <summary>
        /// Palauttaa tai asettaa viitosten voittosumman.
        /// </summary>
        public double Viitoset
        {
            get { return viitoset; }
            set
            {
                viitoset = value;
                String arvo = String.Format("{0:0.00}", value);
                labelViitosetArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa värisuoran voittosumman.
        /// </summary>
        public double Varisuora
        {
            get { return varisuora; }
            set
            {
                varisuora = value;
                String arvo = String.Format("{0:0.00}", value);
                labelVarisuoraArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa nelosten voittosumman.
        /// </summary>
        public double Neloset
        {
            get{ return neloset; }
            set
            {
                neloset = value;
                String arvo = String.Format("{0:0.00}", value);
                labelNelosetArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa täyskäden voittosumman.
        /// </summary>
        public double Tayskasi
        {
            get { return tayskasi; }
            set
            {
                tayskasi = value;
                String arvo = String.Format("{0:0.00}", value);
                labelTayskasiArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa värin voittosumman.
        /// </summary>
        public double Vari
        {
            get { return vari; }
            set
            {
                vari = value;
                String arvo = String.Format("{0:0.00}", value);
                labelVariArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa suoran voittosumman.
        /// </summary>
        public double Suora
        {
            get { return suora; }
            set
            {
                suora = value;
                String arvo = String.Format("{0:0.00}", value);
                labelSuoraArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kolmosten voittosumman.
        /// </summary>
        public double Kolmoset
        {
            get { return kolmoset; }
            set
            {
                kolmoset = value;
                String arvo = String.Format("{0:0.00}", value);
                labelKolmosetArvo.Content = arvo.Replace(',', '.');
            }
        }
        /// <summary>
        /// Palauttaa tai asettaa kahdenparin voittosumman.
        /// </summary>
        public double Kaksiparia
        {
            get { return kaksiparia; }
            set
            {
                kaksiparia = value;
                String arvo = String.Format("{0:0.00}", value);
                labelKaksipariaArvo.Content = arvo.Replace(',', '.');
            }
        }

        /// <summary>
        /// Luo VoittoTaulukon.
        /// </summary>
        public Voitot()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tarkistaa onko viiden kortin listassa pokerikäsi ja palauttaa voittosumman ja voittokäden.
        /// Tarkistuksissa otetaan huomioon yksi jokeri.
        /// </summary>
        /// <param name="kortit">Kortit josta käsiä tarkistetaan</param>
        /// <returns>Voittosumman tai 0 jos ei voittoa</returns>
		/// <example>
		/// <pre name="test">
		/// VoittoTaulukko.Voitot voitot = new VoittoTaulukko.Voitot();
		/// Pelikortti.Kortti hertta3 = new Pelikortti.Kortti(3 + "H"); 
		/// Pelikortti.Kortti pata3 = new Pelikortti.Kortti(3 + "S"); 
		/// Pelikortti.Kortti hertta2 = new Pelikortti.Kortti(2 + "H"); 
		/// Pelikortti.Kortti pata2 = new Pelikortti.Kortti(2 + "S"); 
		/// Pelikortti.Kortti ruutu2 = new Pelikortti.Kortti(2 + "D");
		/// Pelikortti.Kortti risti2 = new Pelikortti.Kortti(2 + "C");
		/// Pelikortti.Kortti hertta6 = new Pelikortti.Kortti(6 + "H");
		/// Pelikortti.Kortti risti5 = new Pelikortti.Kortti(5 + "C");
		/// Pelikortti.Kortti risti6 = new Pelikortti.Kortti(6 + "C");
		/// Pelikortti.Kortti ruutu4 = new Pelikortti.Kortti(4 + "D");
		/// Pelikortti.Kortti hertta4 = new Pelikortti.Kortti(4 + "H");
		/// Pelikortti.Kortti hertta5 = new Pelikortti.Kortti(5 + "H");
		/// Pelikortti.Kortti jokeri = new Pelikortti.Kortti("jokeri1");
		/// 
		/// List<Pelikortti.Kortti> kortit = new List<Pelikortti.Kortti>(){hertta3,hertta2,pata2,risti6,ruutu4};
		/// String[] voitto = new String[] {"", "0"};
		/// String[] tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta2,pata2,risti6,ruutu4};
		/// voitto = new String[] {"Kolmoset", "0,4"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta2,pata2,hertta4,ruutu4};
		/// voitto = new String[] {"Täyskäsi", "1,6"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta3,pata3,hertta4,ruutu2};
		/// voitto = new String[] {"Kaksiparia", "0,4"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1]
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta3,pata3,jokeri,ruutu2};
		/// voitto = new String[] {"Täyskäsi", "1,6"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta3,hertta6,jokeri,ruutu2};
		/// voitto = new String[] {"Kolmoset", "0,4"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){hertta2,hertta3,hertta6,jokeri,hertta4};
		/// voitto = new String[] {"Värisuora", "6"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// 
		/// kortit = new List<Pelikortti.Kortti>(){hertta2,risti6,hertta6,jokeri,hertta4};
		/// voitto = new String[] {"Kolmoset", "0,4"};
		/// tulos = voitot.OnkoVoitto(kortit);
		/// voitto[0] === tulos[0];
		/// voitto[1] === tulos[1];
		/// </pre>
		/// </example>		
        public String[] OnkoVoitto(List<Pelikortti.Kortti> kortit)
        {
            List<int> arvot = new List<int>();
            List<String> maat = new List<String>();
            String[] kaikkiMaat = { "hertta", "pata", "risti", "ruutu" };
            String[] palautus = {"",""};
            String arvo;
            Boolean kolmoset = false;
            Boolean pari = false;
            Boolean kaksiParia = false;
            Boolean kolmeparia = false;
            Boolean jokeriKaytetty = false;
            Boolean vari = false;
            Boolean suora = true;

            foreach (Pelikortti.Kortti kortti in kortit)
            {
                arvo = kortti.Arvo;
                if (arvo.Equals("K")) arvo = "13";
                if (arvo.Equals("Q")) arvo = "12";
                if (arvo.Equals("J")) arvo = "11";
                if (arvo.Equals("A")) arvo = "1";
                if (arvo.Equals("jokeri")) arvo = "0";

                arvot.Add(Convert.ToInt32(arvo));
                maat.Add(kortti.Maa);
            }

            arvot.Sort();

            // Muutetaan ässän arvo suoran tarkistusta varten jos 
            // korteissa on kuningas.
            if (arvot.IndexOf(1) != -1 && arvot.IndexOf(13) != -1)
            {
                arvot.Remove(1);
                arvot.Add(14);
            }

            // Tarkistetaan onko väri.
            foreach (String maa in kaikkiMaat)
            {
                List<String> loydetyt = maat.FindAll(delegate(String nimi)
                {
                    return nimi.Equals(maa) || nimi.Equals("jokeri");
                });

                if (loydetyt.Count == 5) // Jos viisi samaa maata.
                {
                    vari = true;
                }
            }

            // Tarkistetaan onko suora.
            if (arvot[0] != 0) // Jos korteissa ei jokeria.
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!(arvot[i + 1] == arvot[i] + 1)) suora = false;
                }
            }
            else if (arvot[0] == 0) // Jos korteissa jokeri.
            {
                for (int i = 1; i < 4; i++)
                {
                    if (!(arvot[i + 1] == arvot[i] + 1 || (arvot[i + 1] == arvot[i] + 2) && !jokeriKaytetty)) suora = false;
                    if (arvot[i + 1] == arvot[i] + 2) jokeriKaytetty = true;
                }
            }
            jokeriKaytetty = false;
            if (suora)
            {
                if (vari) // Onko värisuora.
                {
                    palautus[0] = Properties.Resources.labelVarisuora;
                    palautus[1] = Varisuora.ToString();
                }
                else
                {
                    palautus[0] = Properties.Resources.labelSuora;
                    palautus[1] = Suora.ToString();
                }
                return palautus;
            }
            else if (vari)
            {
                palautus[0] = Properties.Resources.labelVari;
                palautus[1] = Vari.ToString();
                return palautus;
            }

            // Tarkistetaan muut voitot.
            for (int i = 1; i < 14; i++)
            {
                List<int> loydetyt = arvot.FindAll(delegate(int luku)
                {
                    if (luku == 0) jokeriKaytetty = true;
                    return luku == i || luku == 0;
                });

                if (loydetyt.Count == 5) // Jos viisi samaa arvoa.
                {
                    palautus[0] = Properties.Resources.labelViitoset;
                    palautus[1] = Viitoset.ToString();
                    return palautus;
                }
                else if (loydetyt.Count == 4) // Jos neljä samaa arvoa.
                {
                    palautus[0] = Properties.Resources.labelNeloset;
                    palautus[1] = Neloset.ToString();
                    return palautus;
                }
                if (loydetyt.Count == 3 && !kolmoset) // Jos kolme samaa arvoa eikä vielä kolmosia niin kolmoset.
                {
                    kolmoset = true;
                }
                else if (loydetyt.Count == 3 && kolmoset) // Jos kolme samaa arvoa ja kolmoset jo löydetty niin täyskäsi (Jokeri huomioitu).
                {
                    palautus[0] = Properties.Resources.labelTayskasi;
                    palautus[1] = Tayskasi.ToString();
                    return palautus;
                }
                else if (loydetyt.Count == 2 && !pari) // Jos kaksi samaa arvoa ja ei vielä paria.
                {
                    if (kolmoset && !jokeriKaytetty) // Täyskäsi, voidaan lopettaa tarkistus.
                    {
                        palautus[0] = Properties.Resources.labelTayskasi;
                        palautus[1] = Tayskasi.ToString();
                        return palautus;
                    }
                    pari = true;
                }
                else if (loydetyt.Count == 2 && kaksiParia) // Huomioidaan jokeri.
                {
                    kolmeparia = true;
                    kaksiParia = false;
                }
                else if (loydetyt.Count == 2 && pari && !kolmoset && !kolmeparia) // Jos kaksi samaa arvoa ja pari jo löydetty.
                {
                    kaksiParia = true;
                }
            }

            if (kolmoset && pari && !jokeriKaytetty) // Jos kolmoset ja pari eikä jokeria niin täyskäsi.
            {
                palautus[0] = Properties.Resources.labelTayskasi;
                palautus[1] = Tayskasi.ToString();
                return palautus;
            }
            else if (kolmoset)
            {
                palautus[0] = Properties.Resources.labelKolmoset;
                palautus[1] = Kolmoset.ToString();
                return palautus;
            }
            else if (kaksiParia)
            {
                palautus[0] = Properties.Resources.labelKaksiparia;
                palautus[1] = Kaksiparia.ToString();
                return palautus;
            }
            else // Ei voittoa.
            {
                palautus[0] = "";
                palautus[1] = "0";
                return palautus;
            }
        }
    }
}
