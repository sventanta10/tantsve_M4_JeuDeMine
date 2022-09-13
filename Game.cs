using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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
        public int nbBomb { get;  set; }
        public int nbOpenedSquare { get; set; }
        public double bet { get; set; }
        public ENUM_GAME_STATUS status { get; set; }
        public Player player { get; set; }
        public List<Square> listOfSquare {  get; private set; }

        public Game(int nbMine, double bet, Player player)
        {
            this.nbBomb = nbMine;
            this.bet = bet;
            this.player = player;
            this.status = ENUM_GAME_STATUS.STOPPED;
            listOfSquare = new List<Square>();
            this.nbOpenedSquare = 0;

        }

        public void gameInit() {
            listOfSquare = new List<Square>();
            this.nbOpenedSquare = 0;
        }

        /// <summary>
        /// Démarre la partie
        /// </summary>
        public void start(MainWindow window)
        {
            generateListOfSquare();
            dispatchBomb();
            window.displaySquare();
            status = ENUM_GAME_STATUS.IN_PROGRESS;
            window.displayGameStatut();
            window.displayBetBomb(false);
            this.player.updateBalance(-bet);
            
            window.updateBalanceLabel();


        }

        /// <summary>
        /// Termine la partie en cours
        /// </summary>
        public void stop(MainWindow window)
        {
            status = ENUM_GAME_STATUS.STOPPED;
            window.displayBetBomb(true);
            window.updateBalanceLabel();
        }

        /// <summary>
        /// Génère tous tous les carrés cliquables
        /// </summary>
        private void generateListOfSquare()
        {
            for (int i = 0; i <25; i++)
            {
                Square mySquare = new Square();
                listOfSquare.Add(mySquare);
            }

        }
        /// <summary>
        /// Réparti les bombes dans les différentes cases
        /// </summary>
        private void dispatchBomb()
        {
            int nbBombDraw = 0;
            
            do
            {
                int randomNb = rnd.Next(25);

                if (listOfSquare[randomNb].type != Square.ENUM_TYPE_SQUARE.BOMB) { 
                    listOfSquare[randomNb].type = Square.ENUM_TYPE_SQUARE.BOMB;
                    nbBombDraw++;
                }

            } while (nbBombDraw < nbBomb);

        }

        public double calculateBeneficeNow()
        {
            double benefice = 0.0;
            if (nbOpenedSquare == 0)
                benefice = bet;
            else
                benefice = bet * nbOpenedSquare;

            return benefice;
        }
        
        public int calculateNextTile()
        {
            return -1;
        }
    }
}
