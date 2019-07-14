using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InputManager.MouseHook;

namespace Doppelganger.Models.Input
{
    public class MouseInput : BindableBase, ICloneable
    {
        private long _millis;
        public long Millis
        {
            get => _millis;
            set => SetProperty(ref _millis, value);
        }

        private MouseButtons _mouseStatus;
        public MouseButtons MouseStatus
        {
            get => _mouseStatus;
            set => SetProperty(ref _mouseStatus, value);
        }

        private POINT _point;
        public POINT Point
        {
            get => _point;
            set => SetProperty(ref _point, value);
        }

        public object Clone()
        {
            return new MouseInput
            {
                Millis = this.Millis,
                MouseStatus = this.MouseStatus,
                Point = this.Point
            };
        }
    }
    public enum MouseButtons
    {
        LeftDown = 2,
        LeftUp = 4,
        RightDown = 8,
        RightUp = 16,
        MiddleDown = 32,
        MiddleUp = 64,
        Wheel = 2048,
        Absolute = 32768,
        Move = 0
    }

}
