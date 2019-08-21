/*
 * Graafinen Pelikortti-komponentti. 
 * 
 * 19.11.2012
 */
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

namespace Pelikortti
{
    /// <summary>
    /// Graafinen pelikortti-komponentti.
    /// </summary>
    public partial class Kortti : UserControl
    {
        /// <summary>
        /// Delegaatti ClickTapahtunut-eventille.
        /// </summary>
        /// <param name="Sender">Kortti jota klikattiin</param>
        /// <param name="e"></param>
        public delegate void ClickTapahtunutHandler(Object Sender, EventArgs e);
        /// <summary>
        /// Eventti joka tapahtuu kun korttia on klikattu.
        /// </summary>
        public event ClickTapahtunutHandler ClickTapahtunut;

        private String kortinNimi = "";
        private String kuvanNimi = "";
        private String maa = "";
        private String arvo = "";
        private Boolean nakyvissa = false;
        private Boolean lukittu = false;
        private Boolean raahaus = false;
        private Boolean variVaihdettu = false;
        private Boolean naytaLukittuTeksti = true;
        private String lukittuTeksti= "Lukittu";
        private int lukittuTekstiFonttiKoko = 20;
        private Color lukittuTekstiFonttiVari = Colors.Red;

        /// <summary>
        /// Onko kortti lukittu. Oletuksena false.
        /// </summary>
        public Boolean Lukittu
        {
            get { return lukittu; }
            set 
            { 
                lukittu = value;
                if (naytaLukittuTeksti && lukittu) labelLukittu.Visibility = Visibility.Visible;
                else labelLukittu.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Näytetäänkö lukittuteksti kortin päällä kun 
        /// kortti on lukittu. Oletuksena true.
        /// </summary>
        public Boolean NaytaLukittuTeksti
        {
            get { return naytaLukittuTeksti; }
            set { naytaLukittuTeksti = value; }
        }

        /// <summary>
        /// Teksti joka näytetään kun kortti on lukittu
        /// ja NaytaLukittuTeksti=true. Oletuksena "Lukittu".
        /// </summary>
        public String LukittuTeksti
        {
            get { return lukittuTeksti; }
            set 
            { 
                lukittuTeksti = value;
                labelLukittu.Content = lukittuTeksti;
            }
        }

        /// <summary>
        /// Lukittutekstin fonttikoko. Oletuksena 20.
        /// </summary>
        public int LukittuTekstiFonttiKoko
        {
            get { return lukittuTekstiFonttiKoko; }
            set
            {
                lukittuTekstiFonttiKoko = value;
                labelLukittu.FontSize = lukittuTekstiFonttiKoko;
            }
        }

        /// <summary>
        /// Lukittutekstin fontin väri. Oletuksena "Red".
        /// </summary>
        public Color LukittuTekstiFonttiVari
        {
            get { return lukittuTekstiFonttiVari; }
            set
            {
                lukittuTekstiFonttiVari = value;
                labelLukittu.Foreground = new SolidColorBrush(lukittuTekstiFonttiVari);
            }
        }

        /// <summary>
        /// Onko kortti kuvapuoli ylöspäin. Oletuksena false.
        /// </summary>
        public Boolean Nakyvissa
        {
            get { return nakyvissa; }
        }

        /// <summary>
        /// Onko kortti raahattavissa.
        /// Jos true, niin kortin klikkaus ei ole päällä.
        /// Oletuksena false.
        /// </summary>
        public Boolean Raahaus
        {
            get { return raahaus; }
            set { raahaus = value; }
        }

        /// <summary>
        /// Onko kortin taustaväri sininen.
        /// Jos true niin kortin taustaväri on sininen, muuten punainen.
        /// Oletuksena false.
        /// </summary>
        public Boolean SininenTaustaVari
        {
            get { return variVaihdettu; }
        }

        /// <summary>
        /// Kortin arvo, esim. "K".
        /// </summary>
        public String Arvo
        {
            get { return arvo; }
        }

        /// <summary>
        /// Kortin maa, esim. "pata".
        /// </summary>
        public String Maa
        {
            get { return maa; }
        }

        /// <summary>
        /// Kortin nimi, esim. "pataK".
        /// </summary>
        public String KortinNimi
        {
            get { return kortinNimi; }
        }
        /// <summary>
        /// Kortin kuvapuolen kuvan nimi.
        /// </summary>
        public String KuvanNimi
        {
            get { return kuvanNimi; }
        }

        /// <summary>
        /// Konstruktori, joka luo kortin parametrina annetun merkkijonon perusteella.
        /// </summary>
        /// <param name="luotavaKortti">Kortin arvo ja maa merkkijonossa.</param>
        public Kortti(String luotavaKortti)
        {
            InitializeComponent();

            kuvanNimi = luotavaKortti;
            AsetaKortti(luotavaKortti);
            AsetaKuva("Red_Back");
        }

        /// <summary>
        /// Asettaa kortin maan, arvon ja kortinnimen.
        /// </summary>
        /// <param name="luotavakortti">Kortin arvo ja maa merkkijonossa.</param>
        private void AsetaKortti(String luotavakortti)
        {
            Char[] nimiKirjaimet = luotavakortti.ToCharArray();

            arvo = nimiKirjaimet[0].ToString();
            if (arvo.Equals("1")) arvo = "10";

            if (nimiKirjaimet[1].ToString().Equals("C")) maa = "risti";
            if (nimiKirjaimet[1].ToString().Equals("D")) maa = "ruutu";
            if (nimiKirjaimet[1].ToString().Equals("H")) maa = "hertta";
            if (nimiKirjaimet[1].ToString().Equals("S")) maa = "pata";

            if (arvo.Equals("j"))
            {
                maa = "jokeri";
                arvo = "jokeri";
            }

            kortinNimi = maa + arvo;
        }

        /// <summary>
        /// Kääntää kortin toisin päin.
        /// </summary>
        public void Kaanna()
        {
            if (!nakyvissa)
            {
                nakyvissa = true;
                AsetaKuva(kuvanNimi);
            }

            else
            {
                nakyvissa = false;
                if (variVaihdettu) AsetaKuva("Blue_Back");
                else AsetaKuva("Red_Back");
            }
        }

        /// <summary>
        /// Vaihtaa kortin taustapuolen värin. Punainen tai sininen. Oletuksena punainen.
        /// </summary>
        public void VaihdaTaustaVari()
        {
            if (!variVaihdettu)
            {
                AsetaKuva("Blue_Back");
                variVaihdettu = true;
            }
            else
            {
                AsetaKuva("Red_Back");
                variVaihdettu = false;
            }
        }

        /// <summary>
        /// Asettaa kortin kuvan.
        /// </summary>
        /// <param name="nimi">Kuvan nimi</param>
        private void AsetaKuva(String nimi)
        {
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri("/Pelikortti;component/Images/" + nimi + ".png", UriKind.Relative);
            bitImage.EndInit();
            imageKortti.Stretch = Stretch.Fill;
            imageKortti.Source = bitImage;
        }

        /// <summary>
        /// Aiheuttaa eventin kun hiiren vasen painike nousee.
        /// </summary>
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!raahaus)
                if (ClickTapahtunut != null) ClickTapahtunut(this, e);
        }

        /// <summary>
        /// Asettaa kortin raahattavaksi jos raahaus on päällä.
        /// </summary>
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (raahaus)
            {
                DataObject dragData = new DataObject("Pelikortti.Kortti", sender);
                DragDrop.DoDragDrop((Pelikortti.Kortti)sender, dragData, DragDropEffects.Move);
            }
        }
    }
}
