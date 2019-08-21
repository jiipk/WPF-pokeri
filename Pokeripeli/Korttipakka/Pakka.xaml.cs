/*
 * Graafinen Korttipakka-komponentti, joka sisältää Pelikortti-komponentteja. 
 * Tarvitsee Pelikortti-komponentin toimiakseen.
 * 
 * 21.11.2012
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

namespace Korttipakka
{
    /// <summary>
    /// Graafinen korttipakka-komponentti, joka sisältää Pelikortti-komponentteja.
    /// Tarvitsee Pelikortti-komponentin toimiakseen.
    /// </summary>
    public partial class Pakka : UserControl
    {
        /// <summary>
        /// Delegaatti DropTapahtunut-eventille.
        /// </summary>
        /// <param name="Sender">Pakka, johon dropattiin</param>
        /// <param name="e">Asettaa ja palauttaa dropatun kortin</param>
        public delegate void DropTapahtunutHandler(Object Sender, KorttiEventArgs e);
        /// <summary>
        /// Eventti joka tapahtuu kun pakan päälle dropataan pelikortti.
        /// Dropattu kortti palautetaan KorttiEventArgs:in avulla.
        /// </summary>
        public event DropTapahtunutHandler DropTapahtunut;

        /// <summary>
        /// Delegaatti ClickTapahtunut-eventille.
        /// </summary>
        /// <param name="Sender">Pakka, jota klikattiin</param>
        /// <param name="e"></param>
        public delegate void ClickTapahtunutHandler(Object Sender, EventArgs e);
        /// <summary>
        /// Eventti joka tapahtuu kun pakkaa klikataan.
        /// </summary>
        public event ClickTapahtunutHandler ClickTapahtunut;

        /// <summary>
        /// Delegaatti PakkaTyhja-eventille.
        /// </summary>
        public delegate void PakkaTyhjaHandler();
        /// <summary>
        /// Eventti joka tapahtuu kun pakka on tyhjä.
        /// </summary>
        public event PakkaTyhjaHandler PakkaTyhja;

        /// <summary>
        /// Delegaatti NostetaanLiikaa-eventille.
        /// </summary>
        public delegate void NostetaanLiikaaHandler();
        /// <summary>
        /// Eventti joka tapahtuu kun pakasta yritetään nostaa enemmän kortteja kuin siinä on jäljellä.
        /// </summary>
        public event NostetaanLiikaaHandler NostetaanLiikaa;

        /// <summary>
        /// Delegaatti KorttiaKlikattu-eventille.
        /// </summary>
        /// <param name="Sender">Pakka, jonka korttia klikattiin</param>
        /// <param name="e">Asettaa ja palauttaa klikatun kortin</param>
        public delegate void KorttiaKlikattuHandler(Object Sender, KorttiEventArgs e);
        /// <summary>
        /// Eventti joka tapahtuu kun pakasta nostettua korttia klikataan.
        /// Klikattu kortti palautetaan KorttiEventArgs:in avulla.
        /// </summary>
        public event KorttiaKlikattuHandler KorttiaKlikattu;

        private List<Pelikortti.Kortti> pakanKortit = new List<Pelikortti.Kortti>();
        private List<Pelikortti.Kortti> nostetutKortit = new List<Pelikortti.Kortti>();
        private int korttejaJaljella = 0;
        private Boolean variVaihdettu = false;
        private Boolean kaannetty = false;
        private Boolean jokeritLisatty = false;
        private Boolean korttienRaahaus = false;
        private Boolean naytaLukittuTeksti = true;
        private Boolean naytaTyhjaTeksti = true;
        private String lukittuTeksti = "Lukittu";
        private int lukittuTekstiFonttiKoko = 20;
        private Color lukittuTekstiFonttiVari = Colors.Red;

        /// <summary>
        /// Asettaa pakan jokaisen kortin raahattavuuden päälle tai pois päältä.
        /// Jos true, niin korttien klikkaus ei ole päällä. Oletuksena false.
        /// </summary>
        public Boolean KorttienRaahaus
        {
            get { return korttienRaahaus; }
            set
            {
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    kortti.Raahaus = value;
                }
                korttienRaahaus = value;
            }
        }

        /// <summary>
        /// Asettaa pakan jokaisen kortin lukittutekstin 
        /// päälle tai pois päältä. Oletuksena true.
        /// </summary>
        public Boolean NaytaLukittuTeksti
        {
            get { return naytaLukittuTeksti; }
            set
            {
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    kortti.NaytaLukittuTeksti = value;
                }
                naytaLukittuTeksti = value;
            }
        }

        /// <summary>
        /// Asettaa pakan jokaisen kortin lukittutekstin.
        /// Oletuksena "Lukittu".
        /// </summary>
        public String LukittuTeksti
        {
            get { return lukittuTeksti; }
            set
            {
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    kortti.LukittuTeksti = value;
                }
                lukittuTeksti = value;
            }
        }


        /// <summary>
        /// Asettaa pakan jokaisen kortin lukittutekstin fonttikoon. 
        /// Oletuksena 20.
        /// </summary>
        public int LukittuTekstiFonttiKoko
        {
            get { return lukittuTekstiFonttiKoko; }
            set
            {
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    kortti.LukittuTekstiFonttiKoko = value;
                }
                lukittuTekstiFonttiKoko = value;
            }
        }

        /// <summary>
        /// Asettaa pakan jokaisen kortin lukittutekstin fontin värin. 
        /// Oletuksena "Red".
        /// </summary>
        public Color LukittuTekstiFonttiVari
        {
            get { return lukittuTekstiFonttiVari; }
            set
            {
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    kortti.LukittuTekstiFonttiVari = value;
                }
                lukittuTekstiFonttiVari = value;
            }
        }

        /// <summary>
        /// Näytetäänkö pakan päällä tyhjäteksti kun 
        /// pakka on tyhjä. Oletuksena true.
        /// </summary>
        public Boolean NaytaTyhjaTeksti
        {
            get { return naytaTyhjaTeksti; }
            set { naytaTyhjaTeksti = value; }
        }

        /// <summary>
        /// Kuinka paljon pakassa on kortteja jäljellä.
        /// </summary>
        public int KorttejaJaljella
        {
            get { return korttejaJaljella; }
        }

        /// <summary>
        /// Onko pakka tyhjä.
        /// </summary>
        public Boolean Tyhja
        {
            get 
            {
                if (korttejaJaljella == 0) return true;
                else return false;
            }
        }

        /// <summary>
        /// Onko pakka käännetty.
        /// Jos true niin pakan kortit on kuvapuoli ylöspäin.
        /// Oletuksena false.
        /// </summary>
        public Boolean Kaannetty
        {
            get { return kaannetty; }
            set { Kaanna(); }
        }

        /// <summary>
        /// Onko pakan taustaväri sininen.
        /// Jos true niin pakan taustaväri on sininen, muuten punainen.
        /// Asetettaessa vaihtaa myös kaikkien pakan korttien taustavärin
        /// samaksi kuin pakan taustaväri.
        /// Oletuksena false.
        /// </summary>
        public Boolean SininenTaustaVari
        {
            get { return variVaihdettu; }
            set { VaihdaTaustaVari(); }
        }

        /// <summary>
        /// Onko pakkaan lisätty jokerit.
        /// Oletuksena false.
        /// </summary>
        public Boolean Jokerit
        {
            get { return jokeritLisatty; }
            set
            {
                if (!jokeritLisatty)
                {
                    LisaaJokerit();
                }
                else PoistaJokerit();
            }
        }

        /// <summary>
        /// Pakasta nostetut kortit.
        /// </summary>
        public List<Pelikortti.Kortti> NostetutKortit
        {
            get { return nostetutKortit; }
        }

        /// <summary>
        /// Paremetriton konstruktori, joka luo pakan kortit (ei jokereita).
        /// </summary>
        public Pakka()
        {
            InitializeComponent();

            LuoKortit("H");
            LuoKortit("D");
            LuoKortit("S");
            LuoKortit("C");
        }

        /// <summary>
        /// Luo tietyn maan kortit ja lisää ne pakkaan.
        /// </summary>
        /// <param name="maa">Minkä maan kortit luodaan.</param>
        private void LuoKortit(String maa)
        {
            for (int i = 1; i < 10; i++)
            {
                Pelikortti.Kortti kortti = new Pelikortti.Kortti(i + maa);
                pakanKortit.Add(kortti);
                kortti.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);
            }
            Pelikortti.Kortti kortti2 = new Pelikortti.Kortti("A" + maa);
            Pelikortti.Kortti kortti3 = new Pelikortti.Kortti("J" + maa);
            Pelikortti.Kortti kortti4 = new Pelikortti.Kortti("Q" + maa);
            Pelikortti.Kortti kortti5 = new Pelikortti.Kortti("K" + maa);
            pakanKortit.Add(kortti2);
            pakanKortit.Add(kortti3);
            pakanKortit.Add(kortti4);
            pakanKortit.Add(kortti5);
            kortti2.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);
            kortti3.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);
            kortti4.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);
            kortti5.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);

            korttejaJaljella = pakanKortit.Count();
        }

        /// <summary>
        /// Vaihtaa pakan taustapuolen värin. Punainen tai sininen. Oletuksena punainen.
        /// Vaihtaa myös kaikkien pakan korttien taustavärin samaksi kuin pakan taustaväri.
        /// </summary>
        public void VaihdaTaustaVari()
        {
            if (variVaihdettu == false)
            {
                if (!kaannetty) AsetaKuva("pakka_sininen");
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    if (!kortti.SininenTaustaVari) kortti.VaihdaTaustaVari();
                }
                variVaihdettu = true;
            }
            else
            {
                if (!kaannetty) AsetaKuva("pakka_punainen");
                foreach (Pelikortti.Kortti kortti in pakanKortit)
                {
                    if (kortti.SininenTaustaVari) kortti.VaihdaTaustaVari();
                }
                variVaihdettu = false;
            }
        }

        /// <summary>
        /// Asettaa pakan kuvan.
        /// </summary>
        /// <param name="nimi">Kuvan nimi</param>
        private void AsetaKuva(String nimi)
        {
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            if (!kaannetty || Tyhja) bitImage.UriSource = new Uri("/Korttipakka;component/Images/" + nimi + ".png", UriKind.Relative);
            else if (kaannetty && !Tyhja) bitImage.UriSource = new Uri("/Pelikortti;component/Images/" + nimi + ".png", UriKind.Relative);
            bitImage.EndInit();

            if (!kaannetty || Tyhja)
            {
                imageKortti.Visibility = Visibility.Hidden;
                imagePakka.Stretch = Stretch.Fill;
                imagePakka.Source = bitImage;
            }
            else if (kaannetty && !Tyhja)
            {
                imageKortti.Visibility = Visibility.Visible;
                imageKortti.Stretch = Stretch.Fill;
                imageKortti.Source = bitImage;
            }
        }

        /// <summary>
        /// Kääntää pakan toisin päin.
        /// </summary>
        public void Kaanna()
        {
            pakanKortit.Reverse();
            foreach (Pelikortti.Kortti kortti in pakanKortit)
            {
                kortti.Kaanna();
            }
            if (!kaannetty)
            {
                kaannetty = true;
                AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count() - 1).KuvanNimi);
                imageKortti.Visibility = Visibility.Visible;
            }
            else
            {
                kaannetty = false;
                if (!variVaihdettu) AsetaKuva("pakka_punainen");
                else AsetaKuva("pakka_sininen");
                imageKortti.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Kääntää pakan korttien järjestyksen takaperin.
        /// </summary>
        public void KaannaKorttienJarjestys()
        {
            pakanKortit.Reverse();
            if (kaannetty) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);
        }

        /// <summary>
        /// Lisää pakkaan kaksi jokeria.
        /// </summary>
        public void LisaaJokerit()
        {
            if (!jokeritLisatty)
            {
                Pelikortti.Kortti kortti1 = new Pelikortti.Kortti("jokeri1");
                Pelikortti.Kortti kortti2 = new Pelikortti.Kortti("jokeri2");
                pakanKortit.Add(kortti1);
                pakanKortit.Add(kortti2);

                korttejaJaljella += 2;
                OnkoPakkaTyhja();

                if (kaannetty)
                {
                    AsetaKuva(kortti2.KuvanNimi);
                    kortti1.Kaanna();
                    kortti2.Kaanna();
                }

                if (variVaihdettu)
                {
                    if (!kortti1.SininenTaustaVari) kortti1.VaihdaTaustaVari();
                    if (!kortti2.SininenTaustaVari) kortti2.VaihdaTaustaVari();
                }

                kortti1.Raahaus = korttienRaahaus;
                kortti2.Raahaus = korttienRaahaus;
                kortti1.NaytaLukittuTeksti = naytaLukittuTeksti;
                kortti2.NaytaLukittuTeksti = naytaLukittuTeksti;
                kortti1.LukittuTeksti = lukittuTeksti;
                kortti2.LukittuTeksti = lukittuTeksti;
                kortti1.LukittuTekstiFonttiKoko = lukittuTekstiFonttiKoko;
                kortti2.LukittuTekstiFonttiKoko = lukittuTekstiFonttiKoko;
                kortti1.LukittuTekstiFonttiVari = lukittuTekstiFonttiVari;
                kortti2.LukittuTekstiFonttiVari = lukittuTekstiFonttiVari;

                kortti1.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);
                kortti2.ClickTapahtunut += new Pelikortti.Kortti.ClickTapahtunutHandler(KortinKlikkaus);

                jokeritLisatty = true;
            }
        }

        /// <summary>
        /// Poistaa pakasta jokerit.
        /// </summary>
        public void PoistaJokerit()
        {
            if (jokeritLisatty)
            {
                for (int i = 0; i < 2; i++)
                {
                    Pelikortti.Kortti jokeri = pakanKortit.Find(delegate(Pelikortti.Kortti k)
                    {
                        return k.Maa.Equals("jokeri");
                    });
                    pakanKortit.Remove(jokeri);
                }

                korttejaJaljella -= 2;
                OnkoPakkaTyhja();
                if (kaannetty && !Tyhja) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);

                jokeritLisatty = false;
            }
        }

        /// <summary>
        /// Poistaa lukituksen kaikista nostetuista korteista.
        /// </summary>
        public void PoistaLukituksetNostetuista()
        {
            foreach (Pelikortti.Kortti kortti in nostetutKortit)
            {
                kortti.Lukittu = false;
            }
        }

        /// <summary>
        /// Poistaa lukituksen kaikista pakan korteista.
        /// </summary>
        public void PoistaLukituksetPakasta()
        {
            foreach (Pelikortti.Kortti kortti in pakanKortit)
            {
                kortti.Lukittu = false;
            }
        }

        /// <summary>
        /// Kääntää kaikki pakan kortit pakan mukaiseksi.
        /// </summary>
        public void KaannaKortitOikeinpain()
        {
            foreach (Pelikortti.Kortti kortti in pakanKortit)
            {
                if (!kaannetty && kortti.Nakyvissa) kortti.Kaanna();
                if (kaannetty && !kortti.Nakyvissa) kortti.Kaanna();
            }
        }

        /// <summary>
        /// Palauttaa pakan päällimmäisen kortin.
        /// </summary>
        /// <returns>Pakan päällimmäinen kortti</returns>
        public Pelikortti.Kortti KorttiPaalta()
        {
            return HaeKortti(true);
        }

        /// <summary>
        /// Palauttaa pakan alimmaisen kortin.
        /// </summary>
        /// <returns>Pakan alimmainen kortti</returns>
        public Pelikortti.Kortti KorttiAlta()
        {
            return HaeKortti(false);
        }

        /// <summary>
        /// Hakee kortin joko pakan päältä tai alta.
        /// </summary>
        /// <param name="paalta">Haetaanko kortti pakan päältä</param>
        /// <returns>Kortti joko pakan päältä tai alta. Null, jos pakka on tyhjä.</returns>
        private Pelikortti.Kortti HaeKortti(Boolean paalta)
        {
            Pelikortti.Kortti kortti;
            if (OnkoPakkaTyhja())
            {
                if (NostetaanLiikaa != null) NostetaanLiikaa();
                return null;
            }

            if (paalta)
            {
                kortti = pakanKortit.ElementAt(pakanKortit.Count - 1);
                pakanKortit.RemoveAt(pakanKortit.Count - 1);
                korttejaJaljella--;
                if (kaannetty && !Tyhja) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);
            }
            else
            {
                kortti = pakanKortit.ElementAt(0);
                pakanKortit.RemoveAt(0);
                korttejaJaljella--;
            }

            nostetutKortit.Add(kortti);
            OnkoPakkaTyhja();

            return kortti;
        }

        /// <summary>
        /// Palauttaa listana parametrina annetun määrän kortteja pakan päältä.
        /// </summary>
        /// <param name="maara">Korttien määrä</param>
        /// <returns>Lista nostetuista korteista</returns>
        public List<Pelikortti.Kortti> KorttiPaalta(int maara)
        {
            return HaeKortit(maara, true);
        }

        /// <summary>
        /// Palauttaa listana parametrina annetun määrän kortteja pakan alta.
        /// </summary>
        /// <param name="maara">Korttien määrä</param>
        /// <returns>Lista nostetuista korteista</returns>
        public List<Pelikortti.Kortti> KorttiAlta(int maara)
        {
            return HaeKortit(maara, false);
        }

        /// <summary>
        /// Palauttaa listana parametrina annetun määrän kortteja
        /// joko pakan päältä tai alta.
        /// </summary>
        /// <param name="maara">Korttien määrä</param>
        /// <param name="paalta">Palautetaanko kortit pakan päältä</param>
        /// <returns>Lista nostetuista korteista tai null, jos pakka tyhjä tai 
        ///          yritetään nostaa liikaa kortteja</returns>
        private List<Pelikortti.Kortti> HaeKortit(int maara, Boolean paalta)
        {
            if (OnkoPakkaTyhja() || maara > korttejaJaljella)
            {
                if (NostetaanLiikaa != null) NostetaanLiikaa();
                return null;
            }

            List<Pelikortti.Kortti> nostettavatKortit = new List<Pelikortti.Kortti>();
            Pelikortti.Kortti kortti;

            if (paalta)
            {
                for (int i = 0; i < maara; i++)
                {
                    kortti = pakanKortit.ElementAt(pakanKortit.Count - 1);
                    nostetutKortit.Add(kortti);
                    pakanKortit.RemoveAt(pakanKortit.Count - 1);
                    nostettavatKortit.Add(kortti);
                    korttejaJaljella--;
                }
                if (!Tyhja && kaannetty) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);
            }
            else
            {
                for (int i = 0; i < maara; i++)
                {
                    kortti = pakanKortit.ElementAt(0);
                    nostetutKortit.Add(kortti);
                    pakanKortit.RemoveAt(0);
                    nostettavatKortit.Add(kortti);
                    korttejaJaljella--;
                }
            }

            OnkoPakkaTyhja();

            return nostettavatKortit;
        }

        /// <summary>
        /// Palauttaa pakasta parametrin mukaisen kortin tai null, jos korttia ei löydy tai
        /// pakka tyhjä.
        /// </summary>
        /// <param name="nimi">Palautettavan kortin nimi: Maa ja arvo, esim. "ruutu10", "pataA", "risti2" tai "herttaK".</param>
        /// <returns>Tietty kortti tai null, jos korttia ei löydy tai pakka on tyhjä</returns>
        public Pelikortti.Kortti AnnaKortti(String nimi)
        {
            if (OnkoPakkaTyhja())
            {
                if (NostetaanLiikaa != null) NostetaanLiikaa();
                return null;
            }

            Pelikortti.Kortti kortti = pakanKortit.Find(delegate(Pelikortti.Kortti k)
            {
                return k.KortinNimi.ToUpper().Equals(nimi.ToUpper());
            });

            if (kortti != null)
            {
                nostetutKortit.Add(kortti);
                pakanKortit.Remove(kortti);
                korttejaJaljella--;
                OnkoPakkaTyhja();
                if (kaannetty && !Tyhja) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);
            }

            return kortti;
        }

        /// <summary>
        /// Jos pakka on tyhjä niin synnytää eventin PakkaTyhja() 
        /// ja muuttaa pakan kuvan.
        /// </summary>
        /// <returns>Onko pakka tyhjä</returns>
        private Boolean OnkoPakkaTyhja()
        {
            if (Tyhja)
            {
                if (PakkaTyhja != null) PakkaTyhja();
               
                if (naytaTyhjaTeksti) labelTyhja.Visibility = Visibility.Visible;
                else labelTyhja.Visibility = Visibility.Hidden;
                if (!variVaihdettu) AsetaKuva("pakka_punainen");
                else AsetaKuva("pakka_sininen");
                return true;
            }

            labelTyhja.Visibility = Visibility.Hidden;
            return false;
        }

        /// <summary>
        /// Palauttaa kaikki pakasta nostetut kortit takaisin pakan päälle.
        /// Kääntää kortit pakan mukaiseksi jos ne on toisinpäin.
        /// Poistaa korttien lukitukset jos ne on lukittu.
        /// </summary>
        public void PalautaKaikkiNostetutPaalle()
        {
            PalautaKaikkiNostetut(true, true, true);
        }

        /// <summary>
        /// Palauttaa kaikki pakasta nostetut kortit takaisin pakan alle.
        /// Kääntää kortit pakan mukaiseksi jos ne on toisinpäin.
        /// Poistaa korttien lukitukset jos ne on lukittu.
        /// </summary>
        public void PalautaKaikkiNostetutAlle()
        {
            PalautaKaikkiNostetut(false, true, true);
        }

        /// <summary>
        /// Palauttaa kaikki pakasta nostetut kortit takaisin pakan päälle.
        /// Korttien kääntäminen valittavissa parametrin avulla.
        /// Lukitusten poistaminen valittavissa parametrin avulla.
        /// </summary>
        /// <param name="kaannetaan">Käännetäänkö kortti jos se on toisin päin kuin pakka</param>
        /// <param name="poistaLukitus">Poistetaanko lukitus jos kortti on lukittu</param>
        public void PalautaKaikkiNostetutPaalle(Boolean kaannetaan, Boolean poistaLukitus)
        {
            PalautaKaikkiNostetut(true, kaannetaan, poistaLukitus);
        }

        /// <summary>
        /// Palauttaa kaikki pakasta nostetut kortit takaisin pakan alle.
        /// Korttien kääntäminen valittavissa parametrin avulla.
        /// Lukitusten poistaminen valittavissa parametrin avulla.
        /// </summary>
        /// <param name="kaannetaan">Käännetäänkö kortti jos se on toisin päin kuin pakka</param>
        /// <param name="poistaLukitus">Poistetaanko lukitus jos kortti on lukittu</param>
        public void PalautaKaikkiNostetutAlle(Boolean kaannetaan, Boolean poistaLukitus)
        {
            PalautaKaikkiNostetut(false, kaannetaan, poistaLukitus);
        }

        /// <summary>
        /// Palauttaa kaikki pakasta nostetut kortit takaisin
        /// joko pakan päälle tai alle.
        /// Kääntää kortin pakan mukaiseksi jos se on toisinpäin kuin pakka ja kaannetaan=true.
        /// Poistaa kortin lukituksen, jos se on lukittu ja poistaLukitus=true.
        /// </summary>
        /// <param name="paalle">Palautetaanko kortit pakan päälle</param>
        /// <param name="kaannetaan">Käännetäänkö kortti jos se on toisin päin kuin pakka</param>
        /// <param name="poistaLukitus">Poistetaanko lukitus jos kortti on lukittu</param>
        private void PalautaKaikkiNostetut(Boolean paalle, Boolean kaannetaan, Boolean poistaLukitus)
        {
            if (nostetutKortit.Count == 0) return;

            Pelikortti.Kortti kortti = null;

            for (int i = 0; i < nostetutKortit.Count; i++)
            {
                kortti = nostetutKortit.ElementAt(i);
                if (kortti.Nakyvissa && !kaannetty && kaannetaan) kortti.Kaanna();
                if (!kortti.Nakyvissa && kaannetty && kaannetaan) kortti.Kaanna();
                if (kortti.Lukittu && poistaLukitus) kortti.Lukittu= false;

                if (paalle) pakanKortit.Add(kortti);
                else pakanKortit.Insert(0, kortti);
            }

            nostetutKortit.Clear();
            korttejaJaljella = pakanKortit.Count();
            OnkoPakkaTyhja();
            if (paalle && kaannetty) AsetaKuva(kortti.KuvanNimi);
        }

        /// <summary>
        /// Palauttaa tietyn pakasta nostetun kortin takaisin pakan päälle
        /// ja kääntää sen pakan mukaiseksi jos se on toisinpäin.
        /// Poistaa kortin lukituksen jos se on lukittu.
        /// </summary>
        /// <param name="kortti">Kortti joka palautetaan</param>
        public void PalautaNostettuPaalle(Pelikortti.Kortti kortti)
        {
            PalautaNostettu(kortti, true, true, true);
        }

        /// <summary>
        /// Palauttaa tietyn pakasta nostetun kortin takaisin pakan alle
        /// ja kääntää sen pakan mukaiseksi jos se on toisinpäin.
        /// Poistaa kortin lukituksen jos se on lukittu.
        /// </summary>
        /// <param name="kortti">Kortti joka palautetaan</param>
        public void PalautaNostettuAlle(Pelikortti.Kortti kortti)
        {
            PalautaNostettu(kortti, false, true, true);
        }

        /// <summary>
        /// Palauttaa tietyn pakasta nostetun kortin takaisin pakan päälle.
        /// Kortin kääntäminen valittavissa parametrin avulla.
        /// Lukituksen poistaminen valittavissa parametrin avulla.
        /// </summary>
        /// <param name="kortti">Kortti joka palautetaan</param>
        /// <param name="kaannetaan">Käännetäänkö kortti jos se on toisinpäin kuin pakka</param>
        /// <param name="poistaLukitus">Poistetaanko kortin lukitus jos se on lukittu</param>
        public void PalautaNostettuPaalle(Pelikortti.Kortti kortti, Boolean kaannetaan, Boolean poistaLukitus)
        {
            PalautaNostettu(kortti, true, kaannetaan, poistaLukitus);
        }

        /// <summary>
        /// Palauttaa tietyn pakasta nostetun kortin takaisin pakan alle
        /// Kortin kääntäminen valittavissa parametrin avulla.
        /// Lukituksen poistaminen valittavissa parametrin avulla.
        /// </summary>
        /// <param name="kortti">Kortti joka palautetaann</param>
        /// <param name="kaannetaan">Käännetäänkö kortti jos se on toisinpäin kuin pakka</param>
        /// <param name="poistaLukitus">Poistetaanko kortin lukitus jos se on lukittu</param>
        public void PalautaNostettuAlle(Pelikortti.Kortti kortti, Boolean kaannetaan, Boolean poistaLukitus)
        {
            PalautaNostettu(kortti, true, kaannetaan, poistaLukitus);
        }

        /// <summary>
        /// Palauttaa pakasta nostetun pelikortin pakkaan. 
        /// Kääntää kortin jos se on toisinpäin kuin pakka ja kaannetaan=true.
        /// Poistaa kortin lukituksen, jos se on lukittu ja poistaLukitus=true.
        /// </summary>
        /// <param name="kortti">Kortti joka palautetaan</param>
        /// <param name="paalle">Palautetaanko kortti pakan päälle</param>
        /// <param name="kaannetaan">Käännetäänkö kortti jos se on toisinpäin kuin pakka</param>
        /// <param name="poistaLukitus">Poistetaanko kortin lukitus jos se on lukittu</param>
        private void PalautaNostettu(Pelikortti.Kortti kortti, Boolean paalle, Boolean kaannetaan, Boolean poistaLukitus)
        {
            if (nostetutKortit.Count == 0) return;

            Pelikortti.Kortti palautettavaKortti = nostetutKortit.Find(delegate(Pelikortti.Kortti k)
            {
                return k.Arvo.Equals(kortti.Arvo) && k.Maa.Equals(kortti.Maa);
            });

            if (palautettavaKortti != null)
            {
                if (palautettavaKortti.Nakyvissa && !kaannetty && kaannetaan) palautettavaKortti.Kaanna();
                if (!palautettavaKortti.Nakyvissa && kaannetty && kaannetaan) palautettavaKortti.Kaanna();
                if (palautettavaKortti.Lukittu && poistaLukitus) palautettavaKortti.Lukittu = false;

                if (paalle)
                {
                    pakanKortit.Add(palautettavaKortti);
                    if (kaannetty) AsetaKuva(palautettavaKortti.KuvanNimi);
                }
                else pakanKortit.Insert(0, palautettavaKortti);

                nostetutKortit.Remove(palautettavaKortti);
                korttejaJaljella++;
                OnkoPakkaTyhja();
            }
        }

        /// <summary>
        /// Sekoittaa pakan.
        /// </summary>
        public void Sekoita()
        {
            Random rng = new Random();
            int n = pakanKortit.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Pelikortti.Kortti kortti = pakanKortit[k];
                pakanKortit[k] = pakanKortit[n];
                pakanKortit[n] = kortti;
            }
        }

        /// <summary>
        /// Järjestää pakan kortit korttien arvon mukaan.
        /// A:t, 2:t, 3:t,..., J:t, Q:t, K:t.
        /// Maiden järjestys: Hertta, Pata, Risti, Ruutu.
        /// </summary>
        public void JarjestaArvottain()
        {
            KorttiVertailijaArvot kv = new KorttiVertailijaArvot();
            pakanKortit.Sort(kv);
            if (kaannetty) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);
        }

        /// <summary>
        /// Järjestää pakan kortit korttien maan mukaan.
        /// Hertat, padat, ristit, ruudut.
        /// Pienimmästä isompaan: A,2,3,...,J,Q,K.
        /// </summary>
        public void JarjestaMaittain()
        {
            KorttiVertailijaMaat kv = new KorttiVertailijaMaat();
            pakanKortit.Sort(kv);
            if (kaannetty) AsetaKuva(pakanKortit.ElementAt(pakanKortit.Count - 1).KuvanNimi);
        }

        /// <summary>
        /// Aiheuttaa eventin kun pakan korttia klikataan.
        /// </summary>
        /// <param name="sender">Kortti jota klikattiin</param>
        /// <param name="e"></param>
        private void KortinKlikkaus(object sender, EventArgs e)
        {
            if (KorttiaKlikattu != null) KorttiaKlikattu(this, new KorttiEventArgs((Pelikortti.Kortti)sender));
        }

        /// <summary>
        /// Aiheuttaa eventin kun pakan päälle Dropataan pelikortti.
        /// </summary>
        /// <param name="sender">Kortti joka Dropattiin</param>
        /// <param name="e"></param>
        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("Pelikortti.Kortti"))
            {
                Pelikortti.Kortti kortti = e.Data.GetData("Pelikortti.Kortti") as Pelikortti.Kortti; 

                if (DropTapahtunut != null) DropTapahtunut(this, new KorttiEventArgs(kortti));
            }
        }

        /// <summary>
        /// Aiheuttaa eventin kun hiiren vasen painike nousee.
        /// </summary>
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickTapahtunut != null) ClickTapahtunut(this, e);
        }
    }

    /// <summary>
    /// Luokka joka asettaa ja palauttaa Pelikortin. 
    /// </summary>
    public class KorttiEventArgs : EventArgs
    {
        private Pelikortti.Kortti kortti = null;

        /// <summary>
        /// Asettaa tai palauttaa Pelikortin.
        /// </summary>
        public Pelikortti.Kortti Kortti
        {
            get { return kortti; }
            set { kortti = value; }
        }

        /// <summary>
        /// Asettaa kortille oletusarvon.
        /// </summary>
        /// <param name="oletus">Oletuskortti</param>
        public KorttiEventArgs(Pelikortti.Kortti oletus = null)
        {
            kortti = oletus;
        }
    }

    /// <summary>
    /// Luokka joka vertaa kahta korttia ensisijaisesti niiden arvojen perusteella. 
    /// </summary>
    public class KorttiVertailijaArvot : IComparer<Pelikortti.Kortti>
    {
        /// <summary>
        /// Vertaa kahta korttia ensisijaisesti niiden arvojen perusteella.
        /// </summary>
        /// <param name="x">Vertailtava kortti</param>
        /// <param name="y">Verrattava kortti</param>
        /// <returns>1 jos x isompi, -1 jos y isompi, 0 jos yhtäsuuret</returns>
        public int Compare(Pelikortti.Kortti x, Pelikortti.Kortti y)
        {
            String nimi1 = x.Arvo + x.Maa;
            String nimi2 = y.Arvo + y.Maa;

            if (x.Arvo.Equals("K")) nimi1 = "tt" + x.Maa;
            if (x.Arvo.Equals("A")) nimi1 = "1" + x.Maa;
            if (y.Arvo.Equals("K")) nimi2 = "tt" + y.Maa;
            if (y.Arvo.Equals("A")) nimi2 = "1" + y.Maa;
            if (x.Arvo.Equals("10")) nimi1 = "hh" + x.Maa;
            if (y.Arvo.Equals("10")) nimi2 = "hh" + y.Maa;

            if (x.Maa.Equals("jokeri")) nimi1 = "ww";
            if (y.Maa.Equals("jokeri")) nimi2 = "ww";

            return nimi1.CompareTo(nimi2);
        }
    }

    /// <summary>
    /// Luokka joka vertaa kahta korttia ensisijaisesti niiden maiden perusteella.
    /// </summary>
    public class KorttiVertailijaMaat : IComparer<Pelikortti.Kortti>
    {
        /// <summary>
        /// Vertaa kahta korttia ensisijaisesti niiden arvojen perusteella.
        /// </summary>
        /// <param name="x">Vertailtava kortti</param>
        /// <param name="y">Verrattava kortti</param>
        /// <returns>1 jos x isompi, -1 jos y isompi, 0 jos yhtäsuuret</returns>
        public int Compare(Pelikortti.Kortti x, Pelikortti.Kortti y)
        {
            String nimi1 = x.Maa + x.Arvo;
            String nimi2 = y.Maa + y.Arvo;
          
            if (x.Arvo.Equals("K")) nimi1 = x.Maa + "tt";
            if (x.Arvo.Equals("A")) nimi1 = x.Maa + "";
            if (y.Arvo.Equals("K")) nimi2 = y.Maa + "tt";
            if (y.Arvo.Equals("A")) nimi2 = y.Maa + "";
            if (x.Arvo.Equals("10")) nimi1 = x.Maa + "hh";
            if (y.Arvo.Equals("10")) nimi2 = y.Maa + "hh";

            if (x.Maa.Equals("jokeri")) nimi1 = "ww";
            if (y.Maa.Equals("jokeri")) nimi2 = "ww";

            return nimi1.CompareTo(nimi2);
        }
    }
}
