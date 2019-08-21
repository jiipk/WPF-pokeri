using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Globalization;

namespace Pokeripeli
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            String kieli = "fi-FI";
            if (Pokeripeli.Properties.Settings.Default.Kielivaihdettu) kieli = "en-US";

            Pokeripeli.Properties.Resources.Culture = new CultureInfo(kieli);
            PanosNappi.Properties.Resources.Culture = new CultureInfo(kieli);
            VoittoTaulukko.Properties.Resources.Culture = new CultureInfo(kieli);
            AloitusDialog.Properties.Resources.Culture = new CultureInfo(kieli);
            AboutDialog.Properties.Resources.Culture = new CultureInfo(kieli);
            TallennaDialog.Properties.Resources.Culture = new CultureInfo(kieli);
            EnnatyksetDialog.Properties.Resources.Culture = new CultureInfo(kieli);

            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo(kieli); ;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(kieli); ;
        }
    }
}
