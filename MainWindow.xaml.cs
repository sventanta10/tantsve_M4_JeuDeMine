using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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

        /*
        const string DIRECTORY_IMAGE = "C:\\Users\\svent\\OneDrive - DIVTEC\\Partage Tantardini-Ribeaud\\Module 4\\" +
                                        "Projet M4\\Sven\\Git\\tantsve_M4_JeuDeMine\\img\\";
        */

        public MainWindow()
        {

            InitializeComponent();
            game = new Game(-1, -1.1, new Player("Sven"));
            Label_Username.Content += game.player.username;

        }
        /// <summary>
        /// Permet d'afficher les carrés 
        /// </summary>
        public void displaySquare()
        {
            WrapPanel_Squares.Children.Clear();
            for (int i = 0; i < game.listOfSquare.Count; i++)
            {

                game.listOfSquare[i].image.Tag = i;
                WrapPanel_Squares.Children.Add(game.listOfSquare[i].image);
                game.listOfSquare[i].image.MouseLeftButtonDown += EventClickSquare;

            }
            updateOpenedSquare();
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
            if (mySquare.type == Square.ENUM_TYPE_SQUARE.BOMB)
            {
                URLsource = new Uri($"{DIRECTORY_IMAGE}card_bomb.png", UriKind.Absolute);
                turnAllSquares();
                game.status = Game.ENUM_GAME_STATUS.FINISH;
                mySquare.image.Source = new BitmapImage(URLsource);
                //Vérifie si il reste du solde, dans le cas contraire, demander à l'utilisateur de recommencer
                canPlayerContinueToPlay();
            }
            else
            {
                URLsource = new Uri($"{DIRECTORY_IMAGE}card_star.png", UriKind.Absolute);
                mySquare.image.Source = new BitmapImage(URLsource);
            }
            displayGameStatut();
            updateOpenedSquare();
            updateLabelButtonRetrieveBenefice();



        }

        private void canPlayerContinueToPlay()
        {
            if (game.player.balance == 0 && game.status == Game.ENUM_GAME_STATUS.FINISH)
            {
                MessageBoxResult result = MessageBox.Show($"Malheureusement votre n'avez plus de solde.\nSouhaitez-vous commencer une nouvelle partie ?\n\nSolde de départ : {Player.DEFAULT_BALANCE} $",
                                      "Partie terminée", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        game.player.updateBalance(Player.DEFAULT_BALANCE);
                        updateBalanceLabel();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        public void updateBalanceLabel()
        {
            Label_Balance.Content = $"Solde : {game.player.balance} $";
        }

        private void updateLabelButtonRetrieveBenefice()
        {
            if (game.nbOpenedSquare != 0)
                Button_End.Content = $"RÉCUPÉRER : {game.bet * game.nbOpenedSquare} $";
            else
                Button_End.Content = $"RÉCUPÉRER : {game.bet} $";
        }

        /// <summary>
        /// Permet de mettre à jour les éléments suivants
        ///     - Nombre de case ouvertes
        ///     - Prochain gain
        /// </summary>
        private void updateOpenedSquare()
        {
            Label_NbOpenedSquare.Content = $"{game.nbOpenedSquare} / {25 - game.nbBomb}";

        }

        /// <summary>
        /// Retourne toute les cases en affichant leurs images
        /// </summary>
        private void turnAllSquares()
        {

            for (int i = 0; i < 25; i++)
            {
                Square mySquare = game.listOfSquare[i];
                if (mySquare.opened == false)
                {
                    Uri URLsource = null;
                    if (mySquare.type == Square.ENUM_TYPE_SQUARE.BOMB)
                    {
                        URLsource = new Uri($"{DIRECTORY_IMAGE}card_bomb.png", UriKind.Absolute);
                        game.stop(this);
                        game.status = Game.ENUM_GAME_STATUS.FINISH;
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

        }

        /// <summary>
        /// Augmente le paris d'une unité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void increaseBetClick(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            int bet = Convert.ToInt32(label_betAmount.Text.Trim());
            bet += Convert.ToInt32(myButton.Tag);
            label_betAmount.Text = bet.ToString();
        }

        /// <summary>
        /// Réduit le pari d'une unité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void decreaseBetClick(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            int bet = Convert.ToInt32(label_betAmount.Text.Trim());
            if (bet > 0)
            {
                bet -= Convert.ToInt32(myButton.Tag);
                label_betAmount.Text = bet.ToString();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Affiche le texte correspondant au statut de la partie
        /// </summary>
        public void displayGameStatut()
        {
            switch (game.status)
            {
                case Game.ENUM_GAME_STATUS.IN_PROGRESS:
                    label_gameStatut.Content = "Partie en cours";
                    // code block
                    break;
                case Game.ENUM_GAME_STATUS.STOPPED:
                    label_gameStatut.Content = "Choisissez votre mise et le nombre de bombe";
                    break;
                case Game.ENUM_GAME_STATUS.FINISH:
                    label_gameStatut.Content = "Partie terminée";
                    // code block
                    break;

            }
        }



        /// <summary>
        /// Récupère le nombre de bombe sélectionné
        /// </summary>
        /// <returns>nombre de bombe</returns>
        private int retrieveNumberBomb()
        {
            int numberChecked = -1;
            foreach (UIElement el in Grid_BombSection.Children)
            {
                RadioButton radio = (RadioButton)el;
                if (radio.IsChecked == true)
                    numberChecked = int.Parse((string)radio.Tag);

            }
            return numberChecked;

        }

        /// <summary>
        /// Récupère la valeur du pari
        /// </summary>
        /// <returns></returns>
        private int retrieveBetAmount()
        {
            return Int32.Parse(label_betAmount.Text);
        }

        /// <summary>
        /// Permet de récupéérer l'évenement click sur un bouton radio des minues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickRadioButtonBomb(object sender, RoutedEventArgs e)
        {
            Button_Start.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Click sur le bouton "JOUER"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickStart(object sender, RoutedEventArgs e)
        {
            //Vérifie les conditions pour lancer la partie
            if (label_betAmount.Text != "0" && label_betAmount.Text != "" && Int32.Parse(label_betAmount.Text) <= game.player.balance)
            {
                //lancement de la partie

                game.nbBomb = retrieveNumberBomb();
                game.bet = (double)retrieveBetAmount();
                //game.nbOpenedSquare = 0;
                game.gameInit();
                //MessageBox.Show("La partie va commencer !");
                game.start(this);
                updateLabelButtonRetrieveBenefice();

            }
            else
            {
                //message d'erreur de lancement de partie 
                MessageBox.Show("Le montant du pari est incorrect.");
            }

        }

        private void ButtonClickEnd(object sender, RoutedEventArgs e)
        {


            turnAllSquares();
            game.status = Game.ENUM_GAME_STATUS.FINISH;
            displayGameStatut();
            game.player.updateBalance(game.calculateBeneficeNow());
            game.stop(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        public void displayBetBomb(bool display)
        {
            bool enabled = false;
            Visibility visible = Visibility.Hidden;
            if (display)
            {
                enabled = true;
                visible = Visibility.Visible;
            }

            label_instructionsBetBomb.IsEnabled = enabled;
            Grid_BetSection.IsEnabled = enabled;
            Grid_BombSectionGeneral.IsEnabled = enabled;
            Button_Start.Visibility = visible;

            if (game.status == Game.ENUM_GAME_STATUS.IN_PROGRESS)
            {
                Button_End.Visibility = Visibility.Visible;
            }
            else
            {
                Button_End.Visibility = Visibility.Hidden;
            }
        }
    }
}
