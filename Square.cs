/*  
 * Auteur : Sven Tantardini
 * Organisation : École des métiers techniques - EST
 * Date : 02.11.2022
 * Description : Classe qui représente une carte de jeu, une case.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace tantsve_M4_JeuDeMine
{
    internal class Square
    {
        //Énumération
        public enum ENUM_TYPE_SQUARE
        {
            BOMB,
            STAR
        }

        //Attributs
        public ENUM_TYPE_SQUARE Type { get; set; }
        public bool Opened { get; set; }
        public Image Image { get; set; }

        public Square()
        {
            Type = ENUM_TYPE_SQUARE.STAR;
            Opened = false;
            Image = new Image();
            Image.Width = 80;
            Image.Height = 80;
            Image.Margin = new Thickness(10);
            Uri URLsource = new Uri("/images/card_notopen.png", UriKind.Relative);
            Image.Source = new BitmapImage(URLsource);


        }



    }
}
