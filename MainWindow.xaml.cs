using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;
using Image = System.Windows.Controls.Image;

namespace tantsve_M4_JeuDeMine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game { get; set; }
        const string DIRECTORY_IMAGE = "C:\\Users\\tantsve\\OneDrive - DIVTEC\\Partage Tantardini-Ribeaud\\Module 4\\" +
                                        "Projet M4\\Sven\\2-Programme\\tantsve_M4_JeuDeMine\\img\\";
        public MainWindow()
        {
            
            InitializeComponent();
            Player p1 = new Player("coucou");

            game = new Game(10, 100, p1);

            game.start();
            displaySquare();
            

        }
        /// <summary>
        /// Permet d'afficher les carrés 
        /// </summary>
        private void displaySquare()
        {
            WrapPanel_Squares.Children.Clear();
            for (int i = 0; i < game.listOfSquare.Count; i++)
            {
                
                game.listOfSquare[i].image.Tag = i;
                WrapPanel_Squares.Children.Add(game.listOfSquare[i].image);
                game.listOfSquare[i].image.MouseLeftButtonDown += EventClickSquare;
               
            }

        }

        /// <summary>
        /// Clic sur une des cases
        /// </summary>
        /// <param name="sender">objet</param>
        /// <param name="e">Evenement</param>
        public void EventClickSquare(object sender, MouseButtonEventArgs e)
        {
            
            Image img = (Image)sender;
            
            Square mySquare = this.game.listOfSquare[(int)img.Tag];

            Uri URLsource = null;

            mySquare.opened = true;
            mySquare.image.IsEnabled = false;
            game.nbOpenedSquare++;


            //Affiche la bonne image d'après le type.
            if (mySquare.type == Square.ENUM_TYPE_SQUARE.BOMB) { 
                URLsource = new Uri($"{DIRECTORY_IMAGE}card_bomb.png", UriKind.Absolute);
                turnAllSquares();
            }
            else { 
                URLsource = new Uri($"{DIRECTORY_IMAGE}card_star.png", UriKind.Absolute);
            }


            mySquare.image.Source = new BitmapImage(URLsource);
            updateOpenedSquare();

        }

        /// <summary>
        /// Permet de mettre à jour les éléments suivants
        ///     - Nombre de case ouvertes
        ///     - Prochain gain
        /// </summary>
        private void updateOpenedSquare()
        {
            Label_NbOpenedSquare.Content = $"{game.nbOpenedSquare} / 25";

        }

        /// <summary>
        /// Retourne toute les cases en affichant leurs images
        /// </summary>
        private void turnAllSquares()
        {
            #region Retournement des cartes
            for (int i = 0; i < 25; i++)
            {
                Square mySquare = game.listOfSquare[i];
                if (mySquare.opened == false)
                {
                    Uri URLsource = null;
                    if (mySquare.type == Square.ENUM_TYPE_SQUARE.BOMB)
                    {
                        URLsource = new Uri($"{DIRECTORY_IMAGE}card_bomb.png", UriKind.Absolute);
                        
                    }
                    else
                    {
                        URLsource = new Uri($"{DIRECTORY_IMAGE}card_star.png", UriKind.Absolute);
                    }
                    
                    mySquare.image.Opacity = 0.6;
                    mySquare.image.IsEnabled = false;
                    mySquare.image.Source = new BitmapImage(URLsource);
                    

                }
            }
            #endregion
                      
        }

    }
}
