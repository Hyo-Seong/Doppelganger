using Doppelganger.Hook;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.Models.Input
{
    public class KeyboardInput : BindableBase, ICloneable
    {
        private long _millis;
        public long Millis
        {
            get => _millis;
            set => SetProperty(ref _millis, value);
        }

        private KeyStatus _keyStatus;
        public KeyStatus KeyStatus
        {
            get => _keyStatus;
            set => SetProperty(ref _keyStatus, value);
        }

        private Keys _key;
        public Keys Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        public object Clone()
        {
            return new KeyboardInput
            {
                Key = this.Key,
                KeyStatus = this.KeyStatus,
                Millis = this.Millis
            };
        }

    }

    public enum KeyStatus
    {
        Down = 0x1,
        Up = 0x2
    }
}
