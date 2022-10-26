using System;
using System.Collections.Generic;
using System.Linq;
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
        private Game Game { get; set; }


        public MainWindow()
        {

            InitializeComponent();
            Game = new Game(new Player("Sven"));
            Label_Username.Content += Game.Player.Username;

        }
        /// <summary>
        /// Permet d'afficher les carrés 
        /// </summary>
        public void DisplaySquare()
        {
            WrapPanel_Squares.Children.Clear();
            for (int i = 0; i < Game.ListOfSquare.Count; i++)
            {

                Game.ListOfSquare[i].Image.Tag = i;
                WrapPanel_Squares.Children.Add(Game.ListOfSquare[i].Image);
                Game.ListOfSquare[i].Image.MouseLeftButtonDown += EventClickSquare;

            }
            UpdateOpenedSquare();
        }

        /// <summary>
        /// Clic sur une des cases
        /// </summary>
        /// <param name="sender">objet</param>
        /// <param name="e">Evenement</param>
        public void EventClickSquare(object sender, MouseButtonEventArgs e)
        {

            Image img = (Image)sender;

            Square mySquare = this.Game.ListOfSquare[(int)img.Tag];

            Uri URLsource = null;

            mySquare.Opened = true;
            mySquare.Image.IsEnabled = false;
            Game.NbOpenedSquare++;


            //Affiche la bonne image d'après le type.
            if (mySquare.Type == Square.ENUM_TYPE_SQUARE.BOMB)
            {
                URLsource = new Uri("images/card_bomb.png", UriKind.Relative);
                TurnAllSquares();
                Game.Status = Game.ENUM_GAME_STATUS.FINISH;


            }
            else
            {
                URLsource = new Uri("images/card_star.png", UriKind.Relative);
            }
            DisplayGameStatut();
            mySquare.Image.Source = new BitmapImage(URLsource);
            UpdateOpenedSquare();
            UpdateLabelButtonRetrieveBenefice();
            Game.CalculateNextTile(this);

            //Vérifie si il reste du solde, dans le cas contraire, demander à l'utilisateur de recommencer
            WantPlayerContinueToPlay();

        }

        /// <summary>
        /// Vérifie si la partie est terminée et propose au joueur de recommencer.
        /// </summary>
        private void WantPlayerContinueToPlay()
        {
            if (Game.Player.Balance == 0 && Game.Status == Game.ENUM_GAME_STATUS.FINISH)
            {
                MessageBoxResult result = MessageBox.Show($"Malheureusement votre n'avez plus de solde.\nSouhaitez-vous commencer une nouvelle partie ?\n\nSolde de départ : {Player.DEFAULT_BALANCE} $",
                                      "Partie terminée", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Game.Player.UpdateBalance(Player.DEFAULT_BALANCE);
                        UpdateBalanceLabel();
                        break;
                    case MessageBoxResult.No:
                        this.Close();
                        break;
                }
            }
        }

        /// <summary>
        /// Met à jour le label avec l'information du solde du joueur
        /// </summary>
        public void UpdateBalanceLabel()
        {
            Label_Balance.Content = $"Solde : {Game.Player.Balance} $";
        }

        /// <summary>
        /// Met à jour le bouton pour récupérer la mise
        /// </summary>
        private void UpdateLabelButtonRetrieveBenefice()
        {
            if (Game.NbOpenedSquare != 0)
                Button_End.Content = $"RÉCUPÉRER : {Game.CalculateBeneficeNow(this)} $";
            else
                Button_End.Content = $"RÉCUPÉRER : {Game.Bet} $";
        }

        /// <summary>
        /// Permet de mettre à jour les éléments suivants
        ///     - Nombre de case ouvertes
        ///     - Prochain gain
        /// </summary>
        private void UpdateOpenedSquare()
        {
            Label_NbOpenedSquare.Content = $"{Game.NbOpenedSquare} / {25 - Game.NbBomb}";

        }

        /// <summary>
        /// Retourne toute les cases en affichant leurs images
        /// </summary>
        private void TurnAllSquares()
        {
          
            for (int i = 0; i < 25; i++)
            {
                Square mySquare = Game.ListOfSquare[i];
                if (mySquare.Opened == false)
                {
                    Uri URLsource = null;
                    if (mySquare.Type == Square.ENUM_TYPE_SQUARE.BOMB)
                    {
                        URLsource = new Uri("images/card_bomb.png", UriKind.Relative);
                        Game.Stop(this);
                        Game.Status = Game.ENUM_GAME_STATUS.FINISH;
                    }
                    else
                    {
                        URLsource = new Uri("images/card_star.png", UriKind.Relative);
                    }

                    mySquare.Image.Opacity = 0.6;
                    mySquare.Image.IsEnabled = false;
                    mySquare.Image.Source = new BitmapImage(URLsource);

                }
            }

        }

        /// <summary>
        /// Augmente le paris d'une unité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncreaseBetClick(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            double bet = Convert.ToDouble(label_betAmount.Text.Trim());
            bet += Convert.ToDouble(myButton.Tag);
            label_betAmount.Text = bet.ToString();
        }

        /// <summary>
        /// Réduit le pari d'une unité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecreaseBetClick(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            double bet = Convert.ToDouble(label_betAmount.Text.Trim());
            if (bet > 0)
            {
                bet -= Convert.ToDouble(myButton.Tag);
                label_betAmount.Text = bet.ToString();
            }
        }

        /// <summary>
        /// Vérifie l'entrée du paris
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regex regex = new Regex("^[0-9]+,[0-9]+");
            //e.Handled = regex.IsMatch(e.Text);
            //MessageBox.Show("Allo");
            //return e.Handled;
        }

        /// <summary>
        /// Affiche le texte correspondant au statut de la partie
        /// </summary>
        public void DisplayGameStatut()
        {
            switch (Game.Status)
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
        private int RetrieveNumberBomb()
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
        private double RetrieveBetAmount()
        {
            return Double.Parse(label_betAmount.Text);
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
            if (label_betAmount.Text != "0" && label_betAmount.Text != "" && Double.Parse(label_betAmount.Text) <= Game.Player.Balance)
            {
                //lancement de la partie
                
                Game.NbBomb = RetrieveNumberBomb();
                Game.Bet = RetrieveBetAmount();
                //game.nbOpenedSquare = 0;
                Game.GameInit();
                //MessageBox.Show("La partie va commencer !");
                Game.Start(this);
                Game.CalculateNextTile(this);
                UpdateLabelButtonRetrieveBenefice();

            }
            else
            {
                //message d'erreur de lancement de partie 
                MessageBox.Show("Le montant du pari est incorrect.");
            }

        }

        /// <summary>
        /// Permet de terminer la partie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClickEnd(object sender, RoutedEventArgs e)
        {

            TurnAllSquares();
            Game.Status = Game.ENUM_GAME_STATUS.FINISH;
            DisplayGameStatut();
            if (Game.NbOpenedSquare == 0)
                Game.Player.UpdateBalance(Game.Bet);
            else
                Game.Player.UpdateBalance(Game.CalculateBeneficeNow(this));
            Game.Stop(this);
        }

        /// <summary>
        /// Permet d'afficher ou non la sélection des bombes
        /// </summary>
        /// <param name="display"></param>
        public void DisplayBetBomb(bool display)
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

            if (Game.Status == Game.ENUM_GAME_STATUS.IN_PROGRESS)
            {
                Button_End.Visibility = Visibility.Visible;
            }
            else
            {
                Button_End.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Mise à jour du label avec le montant de la prochaine étoile.
        /// </summary>
        /// <param name="amount">montant</param>
        public void UpdateNextTileLabel(double amount)
        {
            Label_NextTile.Content = $"Étoile suivante : {amount:F2}$";
        }

        /// <summary>
        /// Mise du solde entier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllInBetClick(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;
            double bet = Game.Player.Balance; //Convert.ToInt32(label_betAmount.Text.Trim());

            label_betAmount.Text = bet.ToString();
        }

        /// <summary>
        /// Permet de vérifier l'entrée du montant du paris
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_betAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox mySender = (TextBox)sender;    
            Regex regex = new Regex(@"^[0-9]+(,)?[0-9]*$");
            bool match = regex.IsMatch(mySender.Text);
            if (!match)
                label_betAmount.Text = "0";
        }
    }
}
