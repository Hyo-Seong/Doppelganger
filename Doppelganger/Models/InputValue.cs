﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.Models
{
    public class InputValue : BindableBase, IInput
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

    }


}