using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;

namespace tantsve_M4_JeuDeMine
{
    internal class Player
    {
        //Constantes
        private const double DEFAULT_BALANCE = 1000;

        //Attributs
        public string username { get; set; }
        public double balance { get; private set; }

        /// <summary>
        /// Construit un joueur
        /// </summary>
        /// <param name="username">Nom d'utilisateur de ce joueur</param>
        public Player(string username)
        {
            this.username = username;
            balance = DEFAULT_BALANCE;
        }

        /// <summary>
        /// Permet de mettre à jour le solde du joueur
        /// </summary>
        /// <param name="value">solde à créditer / débiter</param>
        public void updateBalance(double value)
        {
            balance += value;
        }



    }
}
