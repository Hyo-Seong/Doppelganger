using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.Models
{
    public class InputValue : BindableBase, ICloneable
    {
        private InputType _inputType;
        public InputType InputType
        {
            get => _inputType;
            set => SetProperty(ref _inputType, value);
        }

        private int _millis;
        public int Millis
        {
            get => _millis;
            set => SetProperty(ref _millis, value);
        }

        public object Clone()
        {
            return new InputValue
            {
                InputType = this.InputType,
                Millis = this.Millis
            };
        }
    }

    public enum InputType
    {
        Keyboard, Mouse
    }
}
