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
        public ENUM_TYPE_SQUARE type { get; set; }
        public bool opened { get; set; }
        public Image image { get; set; }

        public Square()
        {
            type = ENUM_TYPE_SQUARE.STAR;
            opened = false;
            image = new Image();
            image.Width = 80;
            image.Height = 80;
            image.Margin = new Thickness(10);
            Uri URLsource = new Uri("/images/card_notopen.png", UriKind.Relative);
            image.Source = new BitmapImage(URLsource);


        }



    }
}
