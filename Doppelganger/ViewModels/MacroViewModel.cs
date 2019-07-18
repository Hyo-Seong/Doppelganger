﻿using Doppelganger.Models;
using Doppelganger.Hook;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Doppelganger.Models.Input;
using InputManager;
using static InputManager.Mouse;
using MouseHook = Doppelganger.Hook.MouseHook;
using static Doppelganger.Hook.MouseHook;
using System.Windows;

namespace Doppelganger.ViewModels
{
    public class MacroViewModel : BindableBase
    {
        #region DllImport
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        #endregion

        #region 
        private readonly uint WHEEL_DOWN = 4287102976;
        private readonly uint WHEEL_UP = 7864320;

        private readonly uint KEYDOWN = 0x1;
        private readonly uint KEYUP = 0x2;

        private MouseHook _mouseHook = new MouseHook();

        private WindowState _windowState = WindowState.Normal;
        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }

        private Hook.KeyboardHook _keyboardHook = new Hook.KeyboardHook();

        private ObservableCollection<Macro> _items;
        public ObservableCollection<Macro> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private Macro macro = new Macro();
        #endregion

        #region Command
        public DelegateCommand StartRecordingCommand { get; private set; }
        public DelegateCommand StopeRecordingCommand { get; private set; }


        #endregion

        public MacroViewModel()
        {
            _keyboardHook = new Hook.KeyboardHook(false);
            Items = new ObservableCollection<Macro>();
            StartRecordingCommand = new DelegateCommand(StartHooking);
            StopeRecordingCommand = new DelegateCommand(StopHooking);

            _mouseHook.MouseHookReceived += new MouseHookCallback(mouseHook_MouseWheel);
            _keyboardHook.KeyboardStatusChanged += _hook_KeyEvent;
        }
        private void StartHooking()
        {
            WindowState = WindowState.Minimized;
            macro = new Macro
            {
                Name = "MouseTemp"
            };
            _mouseHook.StartStopwatch();
            _keyboardHook.StartStopwatch();

            _mouseHook.StartHooking();
            _keyboardHook.StartHooking();
            
        }

        private void StopHooking()
        {
            _mouseHook.StopStopwatch();
            _keyboardHook.StopStopwatch();

            _mouseHook.StopHooking();
            _keyboardHook.StopHooking();

            Items.Add((Macro)macro.Clone());
            macro = null;
        }


        void mouseHook_MouseWheel(MouseInput mouseInput)
        {
            macro.MouseInputs.Add(mouseInput);
        }

        private void _hook_KeyEvent(object sender, KeyboardInput e)
        {
            macro.KeyboardInputs.Add(e);
        }

        public void ExcuteInputs(Macro macro)
        {
            StartKeyboard(macro.KeyboardInputs);
            StartMouse(macro.MouseInputs);
        }

        public Thread StartKeyboard(List<KeyboardInput> keyboardInputs)
        {
            var t = new Thread(() => ExcuteKeyboard(keyboardInputs));
            t.Start();
            return t;
        }

        public Thread StartMouse(List<MouseInput> mouseInputs)
        {
            var t = new Thread(() => ExcuteMouse(mouseInputs));
            t.Start();
            return t;
        }
        private void ExcuteKeyboard(List<KeyboardInput> keyboardInputs)
        {
            if (keyboardInputs != null && keyboardInputs.Count != 0)
            {
                keyboardInputs.ForEach(PressKeyboard);
            }
        }
        private void ExcuteMouse(List<MouseInput> mouseInputs)
        {
            if (mouseInputs != null && mouseInputs.Count != 0)
            {
                mouseInputs.ForEach(PressMouse);
            }
        }


        private void PressMouse(MouseInput mouseInput)
        {
            Thread.Sleep((int)mouseInput.Millis);
            
            if(mouseInput.MouseStatus == MouseButtons.Wheel)
            {
                Mouse.Scroll(CheckWheelStatus(mouseInput.MouseData));
            }
            else if(mouseInput.MouseStatus == 0)
            {
                MovePosition(mouseInput.Point);
            } else
            {
                Mouse.SendButton(mouseInput.MouseStatus);
            }
        }

        private Mouse.ScrollDirection CheckWheelStatus(uint mouseData)
        {
            Mouse.ScrollDirection scrollDirection = Mouse.ScrollDirection.Down;
            if (mouseData == WHEEL_DOWN)
            {
                scrollDirection = Mouse.ScrollDirection.Down;
            }
            else if (mouseData == WHEEL_UP)
            {
                scrollDirection = Mouse.ScrollDirection.Up;
            }
            return scrollDirection;
        }

        private void MovePosition(InputManager.MouseHook.POINT point)
        {
            Mouse.Move(point.x, point.y);
        }

        public void ExcuteKeyboardInputs(List<KeyboardInput> keyboardInputs)
        {
            if(keyboardInputs != null && keyboardInputs.Count != 0)
            {
                keyboardInputs.ForEach(PressKeyboard);
            }
        }

        void PressKeyboard(KeyboardInput input)
        {
            Thread.Sleep((int)input.Millis);

            uint dwFlags = input.KeyStatus == KeyStatus.Down ? KEYDOWN : KEYDOWN | KEYUP;
            keybd_event(byte.Parse(((int)input.Key).ToString()), 0x45, dwFlags, UIntPtr.Zero);
        }

        public bool LoadMacros()
        {
            return true;
        }
    }
}
