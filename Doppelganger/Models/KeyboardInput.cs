using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Doppelganger.Models
{
    public class KeyboardInput : InputValue
    {
        private KeyStatus _keyStatus;
        public KeyStatus KeyStatus
        {
            get => _keyStatus;
            set => SetProperty(ref _keyStatus, value);
        }

        private Key _key;
        public Key Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }


    }

    public enum KeyStatus
    {
        Down, Up
    }
}
