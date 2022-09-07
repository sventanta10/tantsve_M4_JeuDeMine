using System;
using System.Collections.Generic;
using System.Linq;
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
            STOPPED, //lorsque la partie n'a pas débuté ou a été arrêté
            IN_PROGRESS, //lorsque la partie est en cours
            FINISH //lorsque la partie est terminée
        }

        //Attributs
        public int nbBomb { get; private set; }
        public int nbOpenedSquare { get; set; }
        private double bet { get; set; }
        private ENUM_GAME_STATUS status { get; set; }
        private Player player { get; set; }
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

        /// <summary>
        /// Démarre la partie
        /// </summary>
        public void start()
        {
            generateListOfSquare();
            dispatchBomb();
            status = ENUM_GAME_STATUS.IN_PROGRESS;

        }

        /// <summary>
        /// Termine la partie en cours
        /// </summary>
        private void end()
        {
            status = ENUM_GAME_STATUS.STOPPED;   
        }

        /// <summary>
        /// Termine le jeu complétement
        /// </summary>
        private void stop()
        {
            status = ENUM_GAME_STATUS.FINISH;
        }

        /// <summary>
        /// Génère tous tous les carrés cliquables
        /// </summary>
        private void generateListOfSquare()
        {
            for (int i = 0; i <=25; i++)
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

        public int calculateNextTile()
        {
            return -1;
        }
    }
}
