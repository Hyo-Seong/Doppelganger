using Doppelganger.Models;
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
using Shell32;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Drawing;
using MouseButtons = InputManager.Mouse.MouseButtons;

namespace Doppelganger.ViewModels
{
    public class MacroViewModel : BindableBase
    {
        #region DllImport
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(int hwnd, int id);

        #endregion

        #region 
        private readonly uint WHEEL_DOWN = 4287102976;
        private readonly uint WHEEL_UP = 7864320;

        private readonly uint KEYDOWN = 0x1;
        private readonly uint KEYUP = 0x2;

        public static Resolution DesktopResolution = new Resolution();

        private MouseHook _mouseHook = new MouseHook();

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
        public DelegateCommand StopRecordingCommand { get; private set; }


        #endregion

        public MacroViewModel()
        {
            _keyboardHook = new Hook.KeyboardHook(false);
            Items = new ObservableCollection<Macro>();
            SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            StartRecordingCommand = new DelegateCommand(StartHooking);
            StopRecordingCommand = new DelegateCommand(StopHooking);

            _mouseHook.MouseStatusChanged += _hook_MouseEvent;
            _keyboardHook.KeyboardStatusChanged += _hook_KeyboardEvent;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            SetResolution();
        }

        private void SetResolution()
        {
            DesktopResolution = Resolution.GetDesktopResolution();
        }

        private void MinimizeAll()
        {
            Shell shell = new Shell();
            shell.MinimizeAll();
        }

        private void StartHooking()
        {
            MinimizeAll();
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
        }

        void _hook_MouseEvent(MouseInput mouseInput)
        {
            macro.MouseInputs.Add(mouseInput);
        }

        private void _hook_KeyboardEvent(object sender, KeyboardInput e)
        {
            macro.KeyboardInputs.Add(e);
        }

        public void ExcuteInputs(Macro macro)
        {
            MinimizeAll();

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
                MovePosition(Resolution.ChangeResolution(DesktopResolution, mouseInput.Resolution, mouseInput.Point));
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
            // TODO - hyoseong : 여기서 확인 한번 해야함(해상도)
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
