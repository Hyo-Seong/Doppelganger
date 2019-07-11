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

        private MouseMessages _mouseStatus;
        public MouseMessages MouseStatus
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

    public enum MouseMessages
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,

        // unused value
        //WM_LBUTTONDBLCLK = 0x0203,
    }
}
