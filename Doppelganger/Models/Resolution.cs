using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.Models
{
    public class Resolution : BindableBase, ICloneable
    {
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

        public object Clone()
        {
            return new Resolution
            {
                Height = this.Height,
                Width = this.Width
            };
        }
    }
}
