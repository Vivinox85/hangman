using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HangManZ2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string pfad = "wortliste1.txt";  // PFAD DER WORTLISTE! BITTE ANPASSEN!
        private StreamReader sr = new StreamReader(pfad, System.Text.Encoding.Default); // Zum Auslesen der Wortlisten-Textdatei
        private List<string> wortliste = new List<string>();    // Die Wortliste im Programm
        private String bufferString;                            // String zum Zwischenspeichern
        private String aktuellesRatewort;                       // Das momentan zu ratende Wort
        private Random zufallszahl = new Random();
        private int aktPosi;                                    // Position des momentan zu ratenden Wortes in der Wortliste
        private string ratefortschritt;                         // String der ausgegeben wird z.B.: _ _ _ F _ G _ _
        private string eingabe;                                 // Tipp des Users
        private int fehlerzahl = 0;                             // Counter für falsche Tipps
        private List<char> bereitsGeraten = new List<char>();   // Liste mit bereits getippten Buchstaben

        // Zeichen Objekte
        private Line hangBoden = new Line();                    
        private Line hangBalkenSenkrecht = new Line();
        private Line hangBalkenWaagrecht = new Line();
        private Line hangSeil = new Line();
        private Line hangKoerper = new Line();
        private Line hangArmLinks = new Line();
        private Line hangArmRechts = new Line();
        private Line hangBeinLinks = new Line();
        private Line hangBeinRechts = new Line();
        private Ellipse hangKopf = new Ellipse();

        public MainWindow()
        {
            InitializeComponent();
            zeichnungVorbereiten();
        }

        // Click des Absenden Buttons
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WortPruefen();    
        }

        // Erste Initalisierung
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Auslesen der Datei und einfügen in die List wortliste
            while((bufferString = sr.ReadLine()) != null)
            {
                wortliste.Add(bufferString.ToUpper());
            }
            sr.Close();
            // UI vorbereiten
            txtAusgabe.FontSize = 35;
            txtAusgabe.TextAlignment = TextAlignment.Center;
            NewGame();
            txtEingabe.Focus();
        }

        // Vorbereitung der Zeichen-Elemente
        public void zeichnungVorbereiten() {
            // Boden-Linie
            hangBoden.StrokeThickness = 5;
            hangBoden.Stroke = Brushes.Black;
            hangBoden.X1 = 120;
            hangBoden.X2 = 600;
            hangBoden.Y1 = 280;
            hangBoden.Y2 = 280;

            hangBalkenSenkrecht.StrokeThickness = 3;
            hangBalkenSenkrecht.Stroke = Brushes.Brown;
            hangBalkenSenkrecht.X1 = (hangBoden.X2 / 2) - 50;
            hangBalkenSenkrecht.X2 = hangBalkenSenkrecht.X1;
            hangBalkenSenkrecht.Y1 = hangBoden.Y1;
            hangBalkenSenkrecht.Y2 = hangBoden.Y1 - 200;

            hangBalkenWaagrecht.StrokeThickness = hangBalkenSenkrecht.StrokeThickness;
            hangBalkenWaagrecht.Stroke = hangBalkenSenkrecht.Stroke;
            hangBalkenWaagrecht.X1 = hangBalkenSenkrecht.X1;
            hangBalkenWaagrecht.X2 = hangBalkenWaagrecht.X1 + 75;
            hangBalkenWaagrecht.Y1 = hangBalkenSenkrecht.Y2;
            hangBalkenWaagrecht.Y2 = hangBalkenSenkrecht.Y2;

            hangSeil.StrokeThickness = 2;
            hangSeil.Stroke = Brushes.SandyBrown;
            hangSeil.X1 = hangBalkenWaagrecht.X2;
            hangSeil.X2 = hangSeil.X1;
            hangSeil.Y1 = hangBalkenWaagrecht.Y1;
            hangSeil.Y2 = hangSeil.Y1 + 50;

            hangKopf.Stroke = Brushes.Black;
            hangKopf.StrokeThickness = 2;
            hangKopf.Width = 35;
            hangKopf.Height = hangKopf.Width;

            hangKoerper.StrokeThickness = 2;
            hangKoerper.Stroke = Brushes.Black;
            hangKoerper.X1 = hangSeil.X1;
            hangKoerper.X2 = hangKoerper.X1;
            hangKoerper.Y1 = hangSeil.Y2 + hangKopf.Height;
            hangKoerper.Y2 = hangKoerper.Y1 + 50;

            hangArmLinks.StrokeThickness = 2;
            hangArmLinks.Stroke = Brushes.Black;
            hangArmLinks.X1 = hangKoerper.X1;
            hangArmLinks.X2 = hangArmLinks.X1 - 20;
            hangArmLinks.Y1 = hangKoerper.Y2 - 25;
            hangArmLinks.Y2 = hangArmLinks.Y1 - 20;

            hangArmRechts.StrokeThickness = 2;
            hangArmRechts.Stroke = Brushes.Black;
            hangArmRechts.X1 = hangKoerper.X1;
            hangArmRechts.X2 = hangArmRechts.X1 + 20;
            hangArmRechts.Y1 = hangKoerper.Y2 - 25;
            hangArmRechts.Y2 = hangArmRechts.Y1 - 20;

            hangBeinLinks.StrokeThickness = 2;
            hangBeinLinks.Stroke = Brushes.Black;
            hangBeinLinks.X1 = hangKoerper.X1;
            hangBeinLinks.X2 = hangBeinLinks.X1 - 20;
            hangBeinLinks.Y1 = hangKoerper.Y2;
            hangBeinLinks.Y2 = hangBeinLinks.Y1 + 20;

            hangBeinRechts.StrokeThickness = 2;
            hangBeinRechts.Stroke = Brushes.Black;
            hangBeinRechts.X1 = hangKoerper.X1;
            hangBeinRechts.X2 = hangBeinRechts.X1 + 20;
            hangBeinRechts.Y1 = hangKoerper.Y2;
            hangBeinRechts.Y2 = hangBeinRechts.Y1 + 20;
        }

        // Methode, die aufgerufen wird wenn ein falscher Tipp abgegeben wurde
        public void FalschGeraten()
        {
            switch (fehlerzahl)
            {
                case 0:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangBalkenSenkrecht);
                    break;

                case 1:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangBalkenWaagrecht);
                    break;

                case 2:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangSeil);
                    break;

                case 3:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangKopf);
                    Canvas.SetTop(hangKopf, hangSeil.Y2);
                    Canvas.SetLeft(hangKopf, hangSeil.X1-(hangKopf.Width/2));
                    break;

                case 4:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangKoerper);
                    break;

                case 5:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangArmLinks);
                    break;

                case 6:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangArmRechts);
                    break;

                case 7:
                    fehlerzahl++;
                    canvPaint.Children.Add(hangBeinLinks);
                    break;

                case 8:
                    canvPaint.Children.Add(hangBeinRechts);
                    btnAbsenden.IsEnabled = false;
                    txtEingabe.IsEnabled = false;
                    LoesungZeigen();
                    MessageBox.Show("Leider verloren! Starten Sie ein neues Spiel!");
                    break;
            }
        }
        // Gibt die Lösung im Ausgabefenster aus.
        public void LoesungZeigen()
        {
            StringBuilder sbX = new StringBuilder(aktuellesRatewort);
            for (int i = 1; i < (aktuellesRatewort.Length * 2); i += 2)
            {
                sbX.Insert(i, " ");
            }
            txtAusgabe.Text = sbX.ToString();
        }
        // Methode, die das GUI im Falle eines Gewinns vorbereitet
        public void Gewonnen()
        {
            LoesungZeigen();            
            MessageBox.Show("Richtig! Sie haben gewonnen!");
            btnAbsenden.IsEnabled = false;
            txtEingabe.IsEnabled = false;
        }

        // Methode, die ein neues Spiel initiiert
        public void NewGame()
        {
            ratefortschritt = "";
            // Zufälliges Wort aus der Liste heraussuchen
            aktPosi = zufallszahl.Next(0, wortliste.Count);
            aktuellesRatewort = wortliste[aktPosi];
            // Ausgabe-String vorbereiten
            for (int i = 0; i < aktuellesRatewort.Length; i++)
            {
                ratefortschritt += "_";
            }
            txtAusgabe.Text = AusgabeAuflockern();
            btnAbsenden.IsEnabled = true;
            txtEingabe.IsEnabled = true;
            // Clear Canvas
            canvPaint.Children.RemoveRange(0, 10);
            canvPaint.Children.Add(hangBoden);
            fehlerzahl = 0;
            bereitsGeraten.Clear();
            txtEingabe.Focus();
        }

        // Methode die das eingegebene Wort / den Buchstaben auf Richtigkeit prüft
        public void WortPruefen()
        {
            eingabe = txtEingabe.Text.ToUpper();
            // Wenn richtiges Wort getippt wurde
            if (eingabe == aktuellesRatewort)
            {
                Gewonnen();
            }
            // Wenn nur 1 Buchstabe getippt wurde
            else if(eingabe.Length == 1)
            {
                // Wenn der Buchstabe bereits getippt wurde
                if (bereitsGeraten.Contains(eingabe[0]))
                {
                    FalschGeraten();
                }
                // Wenn richtig getippt wurde, also der Buchstabe im Wort vorkommt
                else if (aktuellesRatewort.Contains(eingabe))
                {
                    StringBuilder sb = new StringBuilder(ratefortschritt);
                    int verglPosi = 0;
                    while((verglPosi = aktuellesRatewort.IndexOf(eingabe, verglPosi))!= -1)
                    {
                        sb[verglPosi] = eingabe[0];
                        ratefortschritt = sb.ToString();
                        verglPosi++;
                    }
                    txtAusgabe.Text = AusgabeAuflockern();
                    bereitsGeraten.Add(eingabe[0]);
                    if(ratefortschritt == aktuellesRatewort)
                    {
                        Gewonnen();
                    }
                }
                // Sonst falsch geraten
                else
                {
                    FalschGeraten();
                    bereitsGeraten.Add(eingabe[0]);
                }
            }
            // Sonst falsch geraten
            else
            {
                FalschGeraten();
            }
            txtEingabe.Text = "";
            txtEingabe.Focus();
        }

        // 1 Leerzeichen nach jedem Buchstaben für eine sichtbarere Teilung der Buchstaben in der Ausgabe
        private string AusgabeAuflockern()
        {
            string ausgabe;
            StringBuilder sbA = new StringBuilder(ratefortschritt);

            for(int i=1; i<(ratefortschritt.Length*2); i += 2)
            {
                sbA.Insert(i, " ");
            }
            ausgabe = sbA.ToString();

            return ausgabe;
        }
        // Absenden des Tipps via Enter-Taste
        private void txtEingabe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                WortPruefen();
            }
        }

        private void miSpielNeu_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }
    }
}

// TODO:
// Leerzeichen ermöglichen
// Neues Spiel Methode schreiben & im Menü verlinken.
