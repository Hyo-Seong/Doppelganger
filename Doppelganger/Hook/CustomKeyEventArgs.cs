using Doppelganger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Doppelganger.Hook
{
    public class CustomKeyEventArgs : EventArgs
    {
        public Keys Key { get; private set; }

        public bool Handled { get; private set; }

        public KeyStatus KeyStatus { get; private set; }

        public CustomKeyEventArgs(Keys key, KeyStatus keyStatus)
        {
            Key = key;
            KeyStatus = keyStatus;
        }
    }
}
