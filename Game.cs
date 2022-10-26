using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace tantsve_M4_JeuDeMine
{
    
    internal class Game
    {
        Random rnd = new Random();

        //Énumération
        public enum ENUM_GAME_STATUS
        {
            STOPPED , //lorsque la partie n'a pas débuté ou a été arrêté
            IN_PROGRESS, //lorsque la partie est en cours
            FINISH //lorsque la partie est terminée
        }

        //Attributs
        public int NbBomb { get;  set; }
        public int NbOpenedSquare { get; set; }
        public double Bet { get; set; }
        public ENUM_GAME_STATUS Status { get; set; }
        public Player Player { get; set; }
        public List<Square> ListOfSquare {  get; private set; }
        private double LastMulti { get; set; }

        /// <summary>
        /// Construit une partie
        /// </summary>
        /// <param name="player"></param>
        public Game( Player player)
        {
            this.NbBomb = 0;
            this.Bet = 0;
            this.Player = player;
            this.Status = ENUM_GAME_STATUS.STOPPED;
            ListOfSquare = new List<Square>();
            this.NbOpenedSquare = 0;
            LastMulti = 0;

        }
        /// <summary>
        /// Initialisation d'une partie
        /// </summary>
        public void GameInit() {
            ListOfSquare = new List<Square>();
            this.NbOpenedSquare = 0;
        }

        /// <summary>
        /// Démarre la partie
        /// </summary>
        public void Start(MainWindow window)
        {
            GenerateListOfSquare();
            DispatchBomb();
            window.DisplaySquare();
            Status = ENUM_GAME_STATUS.IN_PROGRESS;
            window.DisplayGameStatut();
            window.DisplayBetBomb(false);
            this.Player.UpdateBalance(-Bet);
            
            window.UpdateBalanceLabel();


        }

        /// <summary>
        /// Termine la partie en cours
        /// </summary>
        public void Stop(MainWindow window)
        {
            Status = ENUM_GAME_STATUS.STOPPED;
            window.DisplayBetBomb(true);
            window.UpdateBalanceLabel();
        }

        /// <summary>
        /// Génère tous tous les carrés cliquables
        /// </summary>
        private void GenerateListOfSquare()
        {
            for (int i = 0; i <25; i++)
            {
                Square mySquare = new Square();
                ListOfSquare.Add(mySquare);
            }

        }
        /// <summary>
        /// Réparti les bombes dans les différentes cases
        /// </summary>
        private void DispatchBomb()
        {
            int nbBombDraw = 0;
            
            do
            {
                int randomNb = rnd.Next(25);

                if (ListOfSquare[randomNb].Type != Square.ENUM_TYPE_SQUARE.BOMB) { 
                    ListOfSquare[randomNb].Type = Square.ENUM_TYPE_SQUARE.BOMB;
                    nbBombDraw++;
                }

            } while (nbBombDraw < NbBomb);

        }
        /// <summary>
        /// Permet de calculer le gain actuel
        /// </summary>
        /// <param name="window">Fenêtre</param>
        /// <returns>gain actuel</returns>
        public double CalculateBeneficeNow(MainWindow window)
        {
            //int openedSquare = open;
            double nextMulti = 1;
            double nextTile = 0;

            for (int i = 0; i <= NbOpenedSquare - 1; i++)
            {
                nextMulti = nextMulti + nextMulti * ((double)NbBomb / (25.0 - i));
                nextTile = nextMulti * Bet;
                //Console.WriteLine($"{i}\t:\t{nextMulti * bet}");
            }
            window.UpdateNextTileLabel(nextTile);
            //window.Button_End.Content = Math.Round(nextTile, 2);
            
            return Math.Round(nextTile, 2);
        }
        
        /// <summary>
        /// Permet de calculer le gain de la prochaine étoile
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public double CalculateNextTile(MainWindow window)
        {
            //int openedSquare = open;
            double nextMulti = 1;
            double nextTile = 0;

            for (int i = 0; i <= NbOpenedSquare; i++)
            {
                nextMulti = nextMulti + nextMulti * ((double)NbBomb / (25.0 - i));
                nextTile = nextMulti * Bet;
                //Console.WriteLine($"{i}\t:\t{nextMulti * bet}");
            }
            window.UpdateNextTileLabel(nextTile);
            //window.Button_End.Content = Math.Round(nextTile, 2);
            return Math.Round(nextTile,2);
        }
    }
}
