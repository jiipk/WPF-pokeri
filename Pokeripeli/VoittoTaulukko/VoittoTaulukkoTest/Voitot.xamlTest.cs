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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoittoTaulukko;

namespace TestVoittoTaulukko
{
	[TestClass()]
	public  class TestVoitot
	{
		[TestMethod()]
		public  void testOnkoVoitto154()
		{
			VoittoTaulukko.Voitot voitot = new VoittoTaulukko.Voitot();
			Pelikortti.Kortti hertta3 = new Pelikortti.Kortti(3 + "H");
			Pelikortti.Kortti pata3 = new Pelikortti.Kortti(3 + "S");
			Pelikortti.Kortti hertta2 = new Pelikortti.Kortti(2 + "H");
			Pelikortti.Kortti pata2 = new Pelikortti.Kortti(2 + "S");
			Pelikortti.Kortti ruutu2 = new Pelikortti.Kortti(2 + "D");
			Pelikortti.Kortti risti2 = new Pelikortti.Kortti(2 + "C");
			Pelikortti.Kortti hertta6 = new Pelikortti.Kortti(6 + "H");
			Pelikortti.Kortti risti5 = new Pelikortti.Kortti(5 + "C");
			Pelikortti.Kortti risti6 = new Pelikortti.Kortti(6 + "C");
			Pelikortti.Kortti ruutu4 = new Pelikortti.Kortti(4 + "D");
			Pelikortti.Kortti hertta4 = new Pelikortti.Kortti(4 + "H");
			Pelikortti.Kortti hertta5 = new Pelikortti.Kortti(5 + "H");
			Pelikortti.Kortti jokeri = new Pelikortti.Kortti("jokeri1");
			List<Pelikortti.Kortti> kortit = new List<Pelikortti.Kortti>(){hertta3,hertta2,pata2,risti6,ruutu4};
			String[] voitto = new String[] {"", "0"};
			String[] tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 173");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 174");
			kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta2,pata2,risti6,ruutu4};
			voitto = new String[] {"Kolmoset", "0,4"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 179");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 180");
			kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta2,pata2,hertta4,ruutu4};
			voitto = new String[] {"Täyskäsi", "1,6"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 185");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 186");
			kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta3,pata3,hertta4,ruutu2};
			voitto = new String[] {"Kaksiparia", "0,4"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 191");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 192");
			kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta3,pata3,jokeri,ruutu2};
			voitto = new String[] {"Täyskäsi", "1,6"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 197");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 198");
			kortit = new List<Pelikortti.Kortti>(){ruutu2,hertta3,hertta6,jokeri,ruutu2};
			voitto = new String[] {"Kolmoset", "0,4"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 203");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 204");
			kortit = new List<Pelikortti.Kortti>(){hertta2,hertta3,hertta6,jokeri,hertta4};
			voitto = new String[] {"Värisuora", "6"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 209");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 210");
			kortit = new List<Pelikortti.Kortti>(){hertta2,risti6,hertta6,jokeri,hertta4};
			voitto = new String[] {"Kolmoset", "0,4"};
			tulos = voitot.OnkoVoitto(kortit);
			Assert.AreEqual( tulos[0], voitto[0] , "in method OnkoVoitto, line 215");
			Assert.AreEqual( tulos[1], voitto[1] , "in method OnkoVoitto, line 216");
		}
	}
}

