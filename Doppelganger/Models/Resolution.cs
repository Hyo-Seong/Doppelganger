using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static InputManager.MouseHook;

namespace Doppelganger.Models
{
    public class Resolution : BindableBase, ICloneable
    {
        public Resolution()
        {
            Rectangle rec = Screen.PrimaryScreen.Bounds;
            this.Width = rec.Width;
            this.Height = rec.Height;
        }

        private int _width;
        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        private int _height;
        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        public static bool CheckResolution(Resolution res1, Resolution res2)
        {
            if (res1.Width == res2.Width && res1.Height == res2.Height)
            {
                return true;
            }
            return false;
        }

        public static Resolution RectangleToResolution(Rectangle rectangle)
        {
            return new Resolution
            {
                Width = rectangle.Width,
                Height = rectangle.Height
            };
        }

        public static POINT ChangeResolution(Resolution desktopResolution, Resolution inputResolution, POINT point)
        {
            if(!CheckResolution(desktopResolution, inputResolution))
            {
                POINT point2 = new POINT
                {
                    x = AA(desktopResolution.Width, inputResolution.Width, point.x),
                    y = AA(desktopResolution.Width, inputResolution.Width, point.y)
                };
                Console.WriteLine("[" + inputResolution.Width + "," + inputResolution.Height + "] " + point.x + " " + point.y + " -> " + point2.x + " " + point2.y);
            }
            return point;
        }

        private static int AA(int res1, int res2, int point)
        {
            return (res1 / res2) * point;
        }

        public object Clone()
        {
            return new Resolution
            {
                Height = this.Height,
                Width = this.Width
            };
        }

        internal static Resolution GetDesktopResolution()
        {
            return RectangleToResolution(Screen.PrimaryScreen.Bounds);
        }
    }
}
