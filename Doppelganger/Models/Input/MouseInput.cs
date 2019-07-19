using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InputManager.Mouse;
using static InputManager.MouseHook;

namespace Doppelganger.Models.Input
{
    public class MouseInput : BindableBase, ICloneable
    {
        private Resolution _resolution;
        public Resolution Resolution
        {
            get => _resolution;
            set => SetProperty(ref _resolution, value);
        }

        private long _millis;
        public long Millis
        {
            get => _millis;
            set => SetProperty(ref _millis, value);
        }

        private MouseButtons _mouseStatus = MouseButtons.Absolute;
        public MouseButtons MouseStatus
        {
            get => _mouseStatus;
            set => SetProperty(ref _mouseStatus, value);
        }

        private uint _mouseData;
        public uint MouseData
        {
            get => _mouseData;
            set => SetProperty(ref _mouseData, value);
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
                Resolution = this.Resolution,
                Millis = this.Millis,
                MouseData = this.MouseData,
                MouseStatus = this.MouseStatus,
                Point = this.Point
            };
        }
    }

}
