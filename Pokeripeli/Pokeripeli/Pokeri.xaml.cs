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
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

///<summary>Pokeripeli</summary>
///<datecreated>16.12.2012</datecreated>
namespace Pokeripeli
{
    /// <summary>
    /// Pokeripeli, joka toimii kuten kolikkopeli.
    /// </summary>
    public partial class Pokeri : Window
    {
        private List<Pelikortti.Kortti> lukitsemattomatKortit = new List<Pelikortti.Kortti>();
        private List<Pelikortti.Kortti> uudetKortit = new List<Pelikortti.Kortti>();
        private List<Pelikortti.Kortti> tuplausKortit = new List<Pelikortti.Kortti>();
        private List<Pelikortti.Kortti> pelinKortit = new List<Pelikortti.Kortti>();
        private Pelikortti.Kortti korttiTuplaus;
        List<int> poistetut = new List<int>();
        private Boolean peliAloitettu = false;
        private double voittoSumma = 0;
        private String voittoKasi = "";
        private double voitotYhteensa = 0;
        private double omatRahat = 0;
        private double alkurahat = 0;
        private int asetettavaKortti = 0;
        private int kortinPaikka = 1;
        private DispatcherTimer ajastinAsetaKortit;
        private DispatcherTimer ajastinKaannaKortit;
        private DispatcherTimer ajastinOdota;
        private Boolean odotetaan = true;
        private Boolean tuplaus = false;
        private Boolean saaKlikataPelaa = true;
        private Boolean kielenVaihtoKlikattu = false;
        private String nimimerkki = "";
        private Storyboard sekoitusStoryboard;

        /// <summary>
        /// Parametriton konstruktori, joka luo korteille tapahtumankäsittelijät, 
        /// sekoitusanimaation ja ajastimet.
        /// </summary>
        public Pokeri()
        {
            InitializeComponent();

            // Luodaan tapahtumankäsitelijät korteille kun
            // hiiren nappi painettuna.
            // HUOM! Olisi voinut tämänkin tehdä tuohon pakka-komponenttiin.
            List<Pelikortti.Kortti> kortit = pakkaPokeri.KorttiPaalta(pakkaPokeri.KorttejaJaljella);
            foreach (Pelikortti.Kortti kortti in kortit)
            {
                kortti.MouseLeftButtonDown += new MouseButtonEventHandler(nappiAlhaalla);
            }
            pakkaPokeri.PalautaKaikkiNostetutPaalle();

            // Ajastin joka asettaa kortit.
            ajastinAsetaKortit = new DispatcherTimer();
            ajastinAsetaKortit.Tick += new EventHandler(ajastinAsetaKortit_Tick);
            ajastinAsetaKortit.Interval = new TimeSpan(0, 0, 0, 0, 150);

            // Ajastin joka kääntää kortit.
            ajastinKaannaKortit = new DispatcherTimer();
            ajastinKaannaKortit.Tick += new EventHandler(ajastinKaannaKortit_Tick);
            ajastinKaannaKortit.Interval = new TimeSpan(0, 0, 0, 0, 150);

            // Ajastin joka odottaa sekoittamisen ajan ennen korttien asettamista.
            ajastinOdota = new DispatcherTimer();
            ajastinOdota.Tick += new EventHandler(ajastinOdota_Tick);
            ajastinOdota.Interval = new TimeSpan(0, 0, 0, 0, 500);

            LuoSekoitusAnimaatio();

            pakkaPokeri.LukittuTeksti = Properties.Resources.korttiLukittuTeksti;
        }

        /// <summary>
        /// Luo sekoitusanimaation.
        /// </summary>
        private void LuoSekoitusAnimaatio()
        {
            NameScope.SetNameScope(this, new NameScope());
            this.RegisterName(imageKortti1.Name, imageKortti1);
            this.RegisterName(imageKortti2.Name, imageKortti2);
            this.RegisterName(imageKortti3.Name, imageKortti3);
            this.RegisterName(imageKortti4.Name, imageKortti4);

            ThicknessAnimation sekoitusAnimaatio1 = new ThicknessAnimation();
            sekoitusAnimaatio1.From = new Thickness(100, 0, 0, 10);
            sekoitusAnimaatio1.To = new Thickness(150, 0, 0, 10);
            sekoitusAnimaatio1.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            sekoitusAnimaatio1.AutoReverse = true;
            sekoitusAnimaatio1.RepeatBehavior = new RepeatBehavior(4);

            ThicknessAnimation sekoitusAnimaatio2 = new ThicknessAnimation();
            sekoitusAnimaatio2.From = new Thickness(100, 0, 0, 10);
            sekoitusAnimaatio2.To = new Thickness(50, 0, 0, 10);
            sekoitusAnimaatio2.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            sekoitusAnimaatio2.AutoReverse = true;
            sekoitusAnimaatio2.RepeatBehavior = new RepeatBehavior(4);

            ThicknessAnimation sekoitusAnimaatio3 = new ThicknessAnimation();
            sekoitusAnimaatio3.From = new Thickness(100, 0, 0, 10);
            sekoitusAnimaatio3.To = new Thickness(150, 0, 0, 10);
            sekoitusAnimaatio3.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            sekoitusAnimaatio3.AutoReverse = true;
            sekoitusAnimaatio3.RepeatBehavior = new RepeatBehavior(2);

            ThicknessAnimation sekoitusAnimaatio4 = new ThicknessAnimation();
            sekoitusAnimaatio4.From = new Thickness(100, 0, 0, 10);
            sekoitusAnimaatio4.To = new Thickness(50, 0, 0, 10);
            sekoitusAnimaatio4.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            sekoitusAnimaatio4.AutoReverse = true;
            sekoitusAnimaatio4.RepeatBehavior = new RepeatBehavior(2);

            sekoitusStoryboard = new Storyboard();
            sekoitusStoryboard.Children.Add(sekoitusAnimaatio1);
            sekoitusStoryboard.Children.Add(sekoitusAnimaatio2);
            sekoitusStoryboard.Children.Add(sekoitusAnimaatio3);
            sekoitusStoryboard.Children.Add(sekoitusAnimaatio4);

            Storyboard.SetTargetName(sekoitusAnimaatio1, imageKortti1.Name);
            Storyboard.SetTargetProperty(sekoitusAnimaatio1, new PropertyPath(Image.MarginProperty));

            Storyboard.SetTargetName(sekoitusAnimaatio2, imageKortti2.Name);
            Storyboard.SetTargetProperty(sekoitusAnimaatio2, new PropertyPath(Image.MarginProperty));

            Storyboard.SetTargetName(sekoitusAnimaatio3, imageKortti3.Name);
            Storyboard.SetTargetProperty(sekoitusAnimaatio3, new PropertyPath(Image.MarginProperty));

            Storyboard.SetTargetName(sekoitusAnimaatio4, imageKortti4.Name);
            Storyboard.SetTargetProperty(sekoitusAnimaatio4, new PropertyPath(Image.MarginProperty));
        }

        /// <summary>
        /// Asettaa sekoittamisen jälkeen kortit pöydälle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ajastinOdota_Tick(object sender, EventArgs e)
        {
            // Jos pitää odottaa sekoittamisen ajan.
            if (odotetaan)
            {
                odotetaan = false;
                return;
            }
            // Jos ei ole tuplaustilanne niin asetetaan viisi korttia peliin.
            else if (!tuplaus)
            {
                imageKortti1.Visibility = Visibility.Hidden;
                imageKortti2.Visibility = Visibility.Hidden;
                imageKortti3.Visibility = Visibility.Hidden;
                imageKortti4.Visibility = Visibility.Hidden;
                peliAloitettu = true;

                voittoKasi = "";
                voittoSumma = 0;
                pakkaPokeri.PalautaKaikkiNostetutPaalle();

                // Poistetaan ylimääräinen jokeri. 
                // HUOM! Olisi pitänyt ottaa huomioon komponentissa jos haluaa vain yhden jokerin.
                // Jokereilla olisi voinut olla parempi nimi, ja eri nimi molemmille jokereille.
                pakkaPokeri.AnnaKortti("jokerijokeri");

                pakkaPokeri.Sekoita();
                pelinKortit = pakkaPokeri.KorttiPaalta(5);

                gridKortit.Children.Clear();
                ajastinAsetaKortit.Start();

                ajastinOdota.Stop();
                odotetaan = true;
            }
            // Jos on tuplaustilanne niin asetetaan tuplauskortti.
            else if (tuplaus && !odotetaan)
            {
                buttonVoitot.Visibility = Visibility.Hidden;
                buttonTuplaa.Visibility = Visibility.Hidden;
                buttonPieni.Visibility = Visibility.Visible;
                buttonIso.Visibility = Visibility.Visible;

                gridKortit.Children.Clear();

                pakkaPokeri.PalautaKaikkiNostetutPaalle();

                pakkaPokeri.AnnaKortti("jokerijokeri");
                pakkaPokeri.Sekoita();
                korttiTuplaus = pakkaPokeri.KorttiPaalta();
                tuplausKortit.Add(korttiTuplaus);

                Grid.SetRow(korttiTuplaus, 0);
                Grid.SetColumn(korttiTuplaus, 5);
                gridKortit.Children.Add(korttiTuplaus);

                korttiTuplaus.IsEnabled = false;

                AsetaAvustusTeksti(Properties.Resources.avustusTuplaus1, Properties.Resources.avustusTuplaus2);

                ajastinOdota.Stop();
                odotetaan = true;
                tuplaus = false;
            }
        }

        /// <summary>
        /// Asettaa kortit pöydälle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ajastinAsetaKortit_Tick(object sender, EventArgs e)
        {
            // Kun peli aloitetaan.
            if (peliAloitettu)
            {
                //Asetetaan 5 korttia peliin.
                if (asetettavaKortti < 5)
                {
                    Grid.SetRow(pelinKortit[asetettavaKortti], 0);
                    Grid.SetColumn(pelinKortit[asetettavaKortti], kortinPaikka);
                    pelinKortit[asetettavaKortti].Name = "k" + kortinPaikka;
                    gridKortit.Children.Add(pelinKortit[asetettavaKortti]);
                    lukitsemattomatKortit.Add(pelinKortit[asetettavaKortti]);

                    asetettavaKortti++;
                    kortinPaikka += 2;
                }
                // Pysaytetään ajastin kun viisi korttia on asetettu ja käännetään kortit.
                else
                {
                    ajastinAsetaKortit.Stop();
                    ajastinKaannaKortit.Start();
                    asetettavaKortti = 0;
                    kortinPaikka = 1;
                }
            }
            // Kun peli on jo käynnissä.
            else
            {
                // Asetetaan kortit pois käytöstä (ettei voi klikata).
                foreach (Pelikortti.Kortti kortti in pelinKortit)
                {
                    kortti.IsEnabled = false;
                }
                pakkaPokeri.PoistaLukituksetNostetuista();
                // Asetetaan uudet kortit poistettujen tilalle.
                if (asetettavaKortti < poistetut.Count)
                {
                    Grid.SetRow(uudetKortit[asetettavaKortti], 0);
                    Grid.SetColumn(uudetKortit[asetettavaKortti], poistetut[asetettavaKortti]);
                    uudetKortit[asetettavaKortti].Name = "k" + poistetut[asetettavaKortti];
                    gridKortit.Children.Add(uudetKortit[asetettavaKortti]);
                    pelinKortit.Add(uudetKortit[asetettavaKortti]);

                    asetettavaKortti++;
                }
                //Pysaytetään ajastin kun uudet kortit on asetettu ja käännetään kortit.
                else
                {
                    asetettavaKortti = 0;
                    ajastinAsetaKortit.Stop();
                    ajastinKaannaKortit.Start();
                }
            }
        }

        /// <summary>
        /// Kääntää asetetut kortit.
        /// Tarkistaa onko voittoa ja toteuttaa tuplauksen voiton tullessa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ajastinKaannaKortit_Tick(object sender, EventArgs e)
        {
            // Käännetään pelin viisi korttia.
            if (peliAloitettu)
            {
                if (asetettavaKortti < 5)
                {
                    pelinKortit[asetettavaKortti].Kaanna();
                    asetettavaKortti++;
                }
                    //Pysäytetään ajastin kun kortit on käännetty.
                else
                {
                    saaKlikataPelaa = true;
                    asetettavaKortti = 0;
                    ajastinKaannaKortit.Stop();
                    ajastinKaannaKortit.Interval = new TimeSpan(0, 0, 0, 0, 300);

                    AsetaAvustusTeksti(Properties.Resources.avustusValitsekortit1, Properties.Resources.avustusValitsekortit2);
                }
            }
            // Käännetään pelin uudet kortit.
            else
            {
                if (asetettavaKortti < poistetut.Count)
                {
                    uudetKortit[asetettavaKortti].Kaanna();
                    asetettavaKortti++;
                }
                // Pysäytetään ajastin kun uudet kortit on käännetty
                // ja tarkistetaan onko voittoa.
                else
                {
                    asetettavaKortti = 0;
                    ajastinKaannaKortit.Stop();

                    lukitsemattomatKortit.Clear();
                    uudetKortit.Clear();
                    poistetut.Clear();
                    ajastinKaannaKortit.Interval = new TimeSpan(0, 0, 0, 0, 150);

                    TarkistaVoitto();
                    // Jos voitto niin kysytään haluaako tuplata.
                    if (voittoSumma != 0) KysyTuplaus(false);
                    // Muuten ilmoitetaan ettei voittoa.
                    else
                    {
                        labelVoittoIlmoitus.Content = Properties.Resources.ilmoitusEivoittoa;
                        AsetaAvustusTeksti(Properties.Resources.avustusAlkutilanne);
                        // Pienennetään panos jos se on liian suuri.
                        if (omatRahat + voitotYhteensa > 0 && omatRahat + voitotYhteensa < panosPokeri.ValittuPanos) panosPokeri.AsetaPanos(omatRahat + voitotYhteensa);
                        TarkistaOnkoRahaa(false);
                    }
                    panosPokeri.Klikattavissa = true;
                    saaKlikataPelaa = true;
                }
            }
        }

        /// <summary>
        /// Aloittaa pelin tai asettaa uudet kortit ja tarkistaa voiton sen
        /// mukaan mikä vaihe pelistä on käynnissä.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPelaa_Click(object sender, RoutedEventArgs e)
        {
            // Jos pelin kortteja ei ole vielä jaettu.
            if (!peliAloitettu && saaKlikataPelaa)
            {
                if (!TarkistaOnkoRahaa(true)) return;
                if (!PaivitaRahaTilanne()) return;

                // Asetetaan kaikki käytöstä poistetut kortit käytettäviksi.
                foreach (Pelikortti.Kortti kortti in pelinKortit)
                {
                    kortti.IsEnabled = true;
                }
                foreach (Pelikortti.Kortti kortti2 in tuplausKortit)
                {
                    kortti2.IsEnabled = true;
                }
                tuplausKortit.Clear();

                saaKlikataPelaa = false;
                panosPokeri.Klikattavissa = false;
                gridKortit.Children.Clear();
                labelVoittoIlmoitus.Content = "";
                AsetaAvustusTeksti("");

                imageKortti1.Visibility = Visibility.Visible;
                imageKortti2.Visibility = Visibility.Visible;
                imageKortti3.Visibility = Visibility.Visible;
                imageKortti4.Visibility = Visibility.Visible;
                sekoitusStoryboard.Begin(this);
                ajastinOdota.Start();
            }
            // Kun pelin kortin on jaettu ja lukittu haluamat kortit.
            else if (saaKlikataPelaa)
            {
                // Jos ei ole lukittu yhtään korttia.
                if (lukitsemattomatKortit.Count == 5)
                {
                    labelVoittoIlmoitus.Content = "Valitse ainakin yksi kortti!";
                    return;
                }
                labelVoittoIlmoitus.Content = "";
                AsetaAvustusTeksti("");
                peliAloitettu = false;
                saaKlikataPelaa = false;

                PoistaLukitsemattomatKortit();
                ajastinAsetaKortit.Start();
            }
        }

        /// <summary>
        /// Tarkistaa onko rahaa jäljellä ja ilmoittaa jos ne on loppu.
        /// </summary>
        /// <param name="tyhjennetaan">Poistetaanko kortit pöydältä ja ilmoitetaanko 
        /// voittoilmoituksessa rahojen loppumisesta</param>
        /// <returns>Onko rahaa jäljellä</returns>
        private Boolean TarkistaOnkoRahaa(Boolean tyhjennetaan)
        {
            if (Math.Round(omatRahat, 2) == 0 && Math.Round(voitotYhteensa, 2) == 0)
            {
                if (tyhjennetaan)
                {
                    labelVoittoIlmoitus.Content = Properties.Resources.ilmoitusRahatloppu;
                    AsetaAvustusTeksti(Properties.Resources.avustusUusipeli);
                    gridKortit.Children.Clear();
                    return false;
                }
                else
                {
                    AsetaAvustusTeksti(Properties.Resources.avustusRahatLoppu);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Pienentää omia rahoja ja voittoja panoksen mukaan.
        /// Ilmoittaa jos panos on liian suuri.
        /// </summary>        
        /// <returns>False jos liian suuri panos, muuten true</returns>
        private Boolean PaivitaRahaTilanne()
        {
            if (Math.Round(omatRahat, 2) >= panosPokeri.ValittuPanos)
            {
                omatRahat -= panosPokeri.ValittuPanos;
                if (Math.Round(omatRahat, 2) == 0) labelOmatRahatRuutu.Content = "0";
                else
                {
                    String rahat = String.Format("{0:0.00}", omatRahat);
                    labelOmatRahatRuutu.Content = rahat.ToString().Replace(',', '.');
                }
                return true;
            }
            else if (Math.Round(omatRahat, 2) + Math.Round(voitotYhteensa, 2) >= panosPokeri.ValittuPanos)
            {
                voitotYhteensa -= panosPokeri.ValittuPanos - omatRahat;
                omatRahat = 0;
                labelOmatRahatRuutu.Content = "0";

                if (Math.Round(voitotYhteensa, 2) == 0) labelVoitotRuutu.Content = "0";
                else
                {
                    String rahat = String.Format("{0:0.00}", voitotYhteensa);
                    labelVoitotRuutu.Content = rahat.ToString().Replace(',', '.');
                }
                return true;
            }
            else
            {
                labelVoittoIlmoitus.Content = "Pienennä panosta";
                return false;
            }            
        }

        /// <summary>
        /// Poistaa pelistä kortit joita ei ole lukittu
        /// ja asettaa uudet kortin listaan pakan päältä.
        /// </summary>
        private void PoistaLukitsemattomatKortit()
        {
            foreach (Pelikortti.Kortti kortti in lukitsemattomatKortit)
            {
                Char[] nimikirjaimet = kortti.Name.ToCharArray();
                int kohta = Convert.ToInt32(nimikirjaimet[1].ToString());

                gridKortit.Children.Remove(kortti);
                pelinKortit.Remove(kortti);
               
                poistetut.Add(kohta);
                uudetKortit.Add(pakkaPokeri.KorttiPaalta());
            }
            poistetut.Sort(); // Järjestetään lista, jotta poistettujen korttien kohdat ovat järjestyksessä.
        }

        /// <summary>
        /// Tarkistaa onko tullut voitto ja asettaa voittosumman ja voittokäden.
        /// </summary>
        private void TarkistaVoitto()
        {
            String[] voittoTieto = voitotPokeri.OnkoVoitto(pelinKortit);
            voittoKasi = voittoTieto[0];
            voittoSumma = Convert.ToDouble(voittoTieto[1]);
        }

        /// <summary>
        /// Kysyy voittotilanteessa tai tuplauksen onnistuttua 
        /// haluaako pelaaja ottaa voitot vai tuplata.
        /// </summary>
        /// <param name="tuplausJatkuu">Jatkuuko tuplaus oikean tuplauksen jälkeen</param>
        private void KysyTuplaus(Boolean tuplausJatkuu)
        {
            buttonPelaa.Visibility = Visibility.Hidden;
            panosPokeri.Visibility = Visibility.Hidden;
            buttonIso.Visibility = Visibility.Hidden;
            buttonPieni.Visibility = Visibility.Hidden;
            buttonVoitot.Visibility = Visibility.Visible;
            buttonTuplaa.Visibility = Visibility.Visible;

            foreach (Pelikortti.Kortti kortti in pelinKortit)
            {
                kortti.IsEnabled = false;
            }

            String voitot = String.Format("{0:0.00}", voittoSumma);

            if (!tuplausJatkuu) labelVoittoIlmoitus.Content = voittoKasi + Properties.Resources.ilmoitusTuplausEijatku + voitot.Replace(',', '.') + "€.";
            else labelVoittoIlmoitus.Content = Properties.Resources.ilmoitusVoitto + voitot.Replace(',', '.') + "€.";
            AsetaAvustusTeksti(Properties.Resources.avustusVoittotilanne);
        }

        /// <summary>
        /// Lukitsee tai poistaa lukituksen kortilta korttia klikatessa. 
        /// </summary>
        /// <param name="Sender">Kortti joka lukitaan</param>
        /// <param name="e"></param>
        private void LukitseKortti(object Sender, EventArgs e)
        {
            Pelikortti.Kortti kortti = (Pelikortti.Kortti)Sender;
            
            if (!kortti.Lukittu)
            {
                kortti.Lukittu = true;
                lukitsemattomatKortit.Remove(kortti);
            }
            else
            {
                kortti.Lukittu = false;
                lukitsemattomatKortit.Add(kortti);
            }
        }
        
        /// <summary>
        /// Asettaa voittosummat kun panosta vaihdetaan.
        /// </summary>
        private void panos1_ClickTapahtunut()
        {
            AsetaVoitot();
        }

        /// <summary>
        /// Asettaa voittosummat eri voittokäsille.
        /// </summary>
        private void AsetaVoitot()
        {
            voitotPokeri.Kaksiparia = panosPokeri.ValittuPanos * 2;
            voitotPokeri.Kolmoset = panosPokeri.ValittuPanos * 2;
            voitotPokeri.Suora = panosPokeri.ValittuPanos * 3;
            voitotPokeri.Vari = panosPokeri.ValittuPanos * 4;
            voitotPokeri.Tayskasi = panosPokeri.ValittuPanos * 8;
            voitotPokeri.Neloset = panosPokeri.ValittuPanos * 15;
            voitotPokeri.Varisuora = panosPokeri.ValittuPanos * 30;
            voitotPokeri.Viitoset = panosPokeri.ValittuPanos * 60;
        }

        /// <summary>
        /// Lukitsee tai poistaa lukituksen kortilta kun sitä klikataan.
        /// </summary>
        /// <param name="Sender">Pakka jota klikattiin</param>
        /// <param name="e">Palauttaa klikatun kortin</param>
        private void pakkaPokeri_KorttiaKlikattu(object Sender, Korttipakka.KorttiEventArgs e)
        {
            Pelikortti.Kortti kortti = e.Kortti;
            kortti.Margin = new Thickness(0, 0, 0, 0); // Poistetaan marginaali joka tulee kun hiiren nappi alhaalla.

            if (!kortti.Lukittu)
            {
                kortti.Lukittu = true;
                lukitsemattomatKortit.Remove(kortti);
            }
            else
            {
                kortti.Lukittu = false;
                lukitsemattomatKortit.Add(kortti);
            }
        }

        /// <summary>
        /// Lisää kortin marginaalia kun hiiren nappi alhaalla.
        /// </summary>
        /// <param name="sender">Mikä kortti kyseessä</param>
        /// <param name="e"></param>
        public void nappiAlhaalla(object sender, EventArgs e)
        {
            Pelikortti.Kortti kortti = (Pelikortti.Kortti)sender;
            kortti.Margin = new Thickness(2, 2, 2, 2);
        }

        /// <summary>
        /// Lisää voitot ja päivittää ne labeliin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVoitot_Click(object sender, RoutedEventArgs e)
        {
            voitotYhteensa += voittoSumma;
            String voitot = String.Format("{0:0.00}", voitotYhteensa);
            labelVoitotRuutu.Content = voitot.Replace(',','.');

            gridKortit.Children.Clear();
            AsetaAlkuTilanne("");
        }

        /// <summary>
        /// Toteuttaa tuplaustilanteen kun tuplausnappia painetaan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTuplaa_Click(object sender, RoutedEventArgs e)
        {
            gridKortit.Children.Clear();
            AsetaAvustusTeksti("");

            String voitot = String.Format("{0:0.00}", voittoSumma*2);
            labelVoittoIlmoitus.Content = Properties.Resources.ilmoitusTuplaus + voitot.Replace(',', '.') + " €";

            buttonVoitot.Visibility = Visibility.Hidden;
            buttonTuplaa.Visibility = Visibility.Hidden;

            imageKortti1.Visibility = Visibility.Visible;
            imageKortti2.Visibility = Visibility.Visible;
            imageKortti3.Visibility = Visibility.Visible;
            imageKortti4.Visibility = Visibility.Visible;
            sekoitusStoryboard.Begin(this);
            tuplaus = true;
            ajastinOdota.Start();
        }

        /// <summary>
        /// Tarkistaa menikö tuplaus oikein kun Iso-nappia painetaan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonIso_Click(object sender, RoutedEventArgs e)
        {
            korttiTuplaus.Kaanna();

            String arvo = korttiTuplaus.Arvo;
            if (arvo.Equals("K")) arvo = "13";
            if (arvo.Equals("Q")) arvo = "12";
            if (arvo.Equals("J")) arvo = "11";
            if (arvo.Equals("A")) arvo = "1";
            if (arvo.Equals("jokeri")) arvo = "0";

            if (Convert.ToDouble(arvo) > 7 || Convert.ToDouble(arvo) == 0)
            {
                voittoSumma = voittoSumma * 2;
                KysyTuplaus(true);
            }
            else
            {
                AsetaAlkuTilanne(Properties.Resources.ilmoitusVaaraArvaus);
                // Pienennetään panos jos se on liian suuri.
                if (omatRahat + voitotYhteensa > 0 && omatRahat + voitotYhteensa < panosPokeri.ValittuPanos) panosPokeri.AsetaPanos(omatRahat + voitotYhteensa);
                TarkistaOnkoRahaa(false);
            }
        }

        /// <summary>
        /// Tarkistaa menikö tuplaus oikein kun Pieni-nappia painetaan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPieni_Click(object sender, RoutedEventArgs e)
        {
            korttiTuplaus.Kaanna();

            String arvo = korttiTuplaus.Arvo;
            if (arvo.Equals("K")) arvo = "13";
            if (arvo.Equals("Q")) arvo = "12";
            if (arvo.Equals("J")) arvo = "11";
            if (arvo.Equals("A")) arvo = "1";
            if (arvo.Equals("jokeri")) arvo = "0";

            if (Convert.ToDouble(arvo) < 7 || Convert.ToDouble(arvo) == 0)
            {
                voittoSumma = voittoSumma * 2;
                KysyTuplaus(true);
            }
            else
            {
                AsetaAlkuTilanne(Properties.Resources.ilmoitusVaaraArvaus);
                // Pienennetään panos jos se on liian suuri.
                if (omatRahat + voitotYhteensa > 0 && omatRahat + voitotYhteensa < panosPokeri.ValittuPanos) panosPokeri.AsetaPanos(omatRahat + voitotYhteensa);
                TarkistaOnkoRahaa(false);
            }
        }

        /// <summary>
        /// Asettaa pelin tilanteeseen jossa valitaan panos ja aloitetaan peli.
        /// </summary>
        private void AsetaAlkuTilanne(String teksti)
        {
            buttonPelaa.Visibility = Visibility.Visible;
            panosPokeri.Visibility = Visibility.Visible;
            buttonPieni.Visibility = Visibility.Hidden;
            buttonIso.Visibility = Visibility.Hidden;
            buttonTuplaa.Visibility = Visibility.Hidden;
            buttonVoitot.Visibility = Visibility.Hidden;

            AsetaAvustusTeksti(Properties.Resources.avustusAlkutilanne);
            labelVoittoIlmoitus.Content = teksti;
        }

        /// <summary>
        /// Asettaa avustustekstin jossa yksi rivi.
        /// </summary>
        /// <param name="teksti">Teksti joka asetetaan</param>
        private void AsetaAvustusTeksti(String teksti)
        {
            textBlockOhjeet.Inlines.Clear();
            textBlockOhjeet.Inlines.Add(new Run(teksti));
        }

        /// <summary>
        /// Asettaa avustustekstin jossa kaksi riviä.
        /// </summary>
        /// <param name="teksti1">Ylempi teksti</param>
        /// <param name="teksti2">Alempi teksti</param>
        private void AsetaAvustusTeksti(String teksti1, String teksti2)
        {
            textBlockOhjeet.Inlines.Clear();
            textBlockOhjeet.Inlines.Add(new Run(teksti1));
            textBlockOhjeet.Inlines.Add(new LineBreak());
            textBlockOhjeet.Inlines.Add(new Run(teksti2));
        }

        /// <summary>
        /// Aloittaa uuden pelin kysymällä aloitusrahan määrän.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uusiPeli_Click(object sender, RoutedEventArgs e)
        {
            NaytaAloitusDialog(true);
        }

        /// <summary>
        /// Kysyy aloitusrahan määrän kun ikkuna on ladattu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowPokeri_Loaded(object sender, RoutedEventArgs e)
        {
            NaytaAloitusDialog(false);
        }

        /// <summary>
        /// Nayttaa aloitusdialogin jossa kysytään pelirahan määrää.
        /// </summary>
        /// <param name="uusiPeli">Aloitetaanko uusipeli kesken käynnissä olevan pelin</param>
        private void NaytaAloitusDialog(Boolean uusiPeli)
        {
            AloitusDialog.AloitusIkkuna dlg = new AloitusDialog.AloitusIkkuna();
            dlg.ShowDialog();

            if (dlg.DialogResult == true)
            {
                omatRahat = dlg.Maara;
                alkurahat = omatRahat;

                String rahat = String.Format("{0:0.00}", omatRahat);
                labelOmatRahatRuutu.Content = rahat.ToString().Replace(',', '.');
                AsetaAvustusTeksti(Properties.Resources.avustusAlkutilanne);

                if (uusiPeli)
                {
                    labelVoitotRuutu.Content = "0";

                    gridKortit.Children.Clear();
                    peliAloitettu = false;
                    AsetaAlkuTilanne("");
                    panosPokeri.ValittuPanos = 0.20;
                    panosPokeri.Klikattavissa = true;
                    AsetaVoitot();
                }
            }
            else if (dlg.DialogResult == false && !uusiPeli)
            {
                AsetaAvustusTeksti(Properties.Resources.avustusUusipeli);
            }
        }

        /// <summary>
        /// Näyttää About-dialogin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuTietoja_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog.AboutIkkuna dialogTietoja = new AboutDialog.AboutIkkuna();
            dialogTietoja.ShowDialog();
        }

        /// <summary>
        /// Avaa tallennusdialogin jos voitot riittävät uuteen ennätykseen.
        /// Tallentaa ennätyksen jos on painettu Tallenna.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuTallenna_Click(object sender, RoutedEventArgs e)
        {
            EnnatyksetDialog.EnnatyksetIkkuna dialogEnnatykset = new EnnatyksetDialog.EnnatyksetIkkuna();
            if (dialogEnnatykset.OnkoEnnatys(voitotYhteensa))
            {
                TallennaDialog.TallennaIkkuna dialogTallenna = new TallennaDialog.TallennaIkkuna(voitotYhteensa, alkurahat);
                dialogTallenna.ShowDialog();

                // Jos on painettu Tallenna.
                if (dialogTallenna.DialogResult == true)
                {
                    nimimerkki = dialogTallenna.Nimimerkki;

                    if (dialogEnnatykset.LaskeEnnatykset() < 10) TallennaEnnatys(); // Jos alle 10 tallennettua ennätystä niin tallennetaan uusi.

                    else // Jos ennätyksiä on jo 10 niin poistetaan viimeinen ja tallennetaan uusi.
                    {
                        dialogEnnatykset.PoistaViimeinen();
                        TallennaEnnatys();
                    }
                }
            }
            else MessageBox.Show(Properties.Resources.messageEiEnnatysta, Properties.Resources.messageEiEnnatystaOtsikko, MessageBoxButton.OK);
            dialogEnnatykset.Close();
        }

        /// <summary>
        /// Tallentaa ennätyksen tekstitiedostoon.
        /// </summary>
        private void TallennaEnnatys()
        {
            String voitotTeksti = String.Format("{0:0.00}", voitotYhteensa);
            if (!File.Exists("ennatykset.txt"))
            {
                File.Create("ennatykset.txt");
            }
            System.IO.StreamWriter tiedosto = new System.IO.StreamWriter(@"ennatykset.txt", true);

            tiedosto.WriteLine(nimimerkki + ": " + voitotTeksti.Replace(',', '.') + " € (aloitusraha " + alkurahat.ToString() + " €)");
            tiedosto.Close();
        }

        /// <summary>
        /// Poistaa ennätykset tekstitiedostosta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuTyhjenna_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.messageEnnatyspoisto, Properties.Resources.messageEnnatyspoistoOtsikko, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                System.IO.File.WriteAllText(@"ennatykset.txt", String.Empty);
            }
        }

        /// <summary>
        /// Aukaisee messageboxin jolla voi vaihtaa ohjelman kielen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuVaihdaKieli_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.messageVaihdakieli, Properties.Resources.messageVaihdakieliOtsikko, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (!kielenVaihtoKlikattu)
                {
                    if (Properties.Settings.Default.Kielivaihdettu) Properties.Settings.Default.Kielivaihdettu = false;
                    else Properties.Settings.Default.Kielivaihdettu = true;

                    kielenVaihtoKlikattu = true;
                    Properties.Settings.Default.Save();
                }
            }
            // Jos klikataan ei niin perutaan mahdollisesti muutettu kieli.
            else 
            {
                if (kielenVaihtoKlikattu)
                {
                    if (Properties.Settings.Default.Kielivaihdettu) Properties.Settings.Default.Kielivaihdettu = false;
                    else Properties.Settings.Default.Kielivaihdettu = true;

                    kielenVaihtoKlikattu = false;
                    Properties.Settings.Default.Save();
                }
            } 
        }

        /// <summary>
        /// Aukaisee dialogin joka näyttää tallennetut ennätykset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuNaytaEnnatykset_Click(object sender, RoutedEventArgs e)
        {
            EnnatyksetDialog.EnnatyksetIkkuna dialogEnnatykset = new EnnatyksetDialog.EnnatyksetIkkuna();

            dialogEnnatykset.ShowDialog();
        }

        /// <summary>
        /// Avaa avustuksen selaimeen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAvustus_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://trac.cc.jyu.fi/projects/gko/wiki/2012/jopekork/avustus/");
        }

        /// <summary>
        /// Avaa messageboxin jolla varmistetaan haluaako käyttäjä varmasti sulkea pelin.
        /// Jos vastaa kyllä niin peli suljetaan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLopeta_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Properties.Resources.messageLopetus, Properties.Resources.messageLopetusOtsikko, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
      
        /// <summary>
        /// Testimetodi pakan DropTapahtunut-tapahtumalle.
        /// </summary>
        /// <param name="Sender">Pakka johon dropattiin</param>
        /// <param name="e">Palauttaa kortin joka dropattiin</param>
        private void pakkaPokeri_DropTapahtunut(object Sender, Korttipakka.KorttiEventArgs e)
        {
            Pelikortti.Kortti kortti = e.Kortti;

            pakkaPokeri.PalautaNostettuAlle(kortti);
            pakkaPokeri.Kaanna();
            AsetaAvustusTeksti("");
        }
    }
}
