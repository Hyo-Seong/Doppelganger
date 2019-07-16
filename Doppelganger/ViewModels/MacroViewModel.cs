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

        private bool flag2 = true;

        private void StopHooking()
        {
            _mouseHook.StopStopwatch();
            _keyboardHook.StopStopwatch();

            _mouseHook.Uninstall();
            _keyboardHook.Stop();
            //_keyboardHook.KeyDown -= _hook_KeyEvent;
            //_keyboardHook.KeyUp -= _hook_KeyEvent;
            Items.Add((Macro)macro.Clone());
            macro = null;
        }

        private void StartHooking()
        {
            macro = new Macro
            {
                Name = "MouseTemp"
            };
            _mouseHook.StartStopwatch();
            //_mouseHook.MouseHookReceived += new MouseHookCallback(mouseHook_MouseWheel);

            _mouseHook.Install();

            _keyboardHook.StartStopwatch();
            _keyboardHook.Start();

        }

        private void StartMouseHooking()
        {
            if (flag2)
            {
                macro = new Macro
                {
                    Name = "MouseTemp"
                };
                _mouseHook.StartStopwatch();
                _mouseHook.MouseHookReceived += new MouseHookCallback(mouseHook_MouseWheel);

                _mouseHook.Install();
            }
            else
            {
                _mouseHook.StopStopwatch();
                _mouseHook.Uninstall();
                Items.Add((Macro)macro.Clone());
                macro = null;
            }
            flag2 = !flag2;
        }

        void mouseHook_MouseWheel(MouseInput mouseInput)
        {
            macro.MouseInputs.Add(mouseInput);
        }

        private void _hook_KeyEvent(object sender, KeyboardInput e)
        {
            macro.KeyboardInputs.Add(e);
        }

        private bool stop = true;

        public void StartKeyboardHooking()
        {
            if (stop)
            {
                macro = new Macro
                {
                    Name = "KeyboardTemp"
                };
                _keyboardHook.StartStopwatch();
                _keyboardHook.Start();
                _keyboardHook.KeyboardStatusChanged += _hook_KeyEvent;
            }
            else
            {
                _keyboardHook.StopStopwatch();
                _keyboardHook.Stop();
                _keyboardHook.KeyboardStatusChanged -= _hook_KeyEvent;
                Items.Add((Macro)macro.Clone());
                macro = null;
            }
            stop = !stop;
        }

        public void ExcuteMouseInputs(List<MouseInput> mouseInputs)
        {
            if (mouseInputs != null && mouseInputs.Count != 0)
            {
                mouseInputs.ForEach(PressMouse);
            }
        }

        public T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
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
            Console.WriteLine(input.Key + " : " + input.KeyStatus.ToString());

            uint dwFlags = input.KeyStatus == KeyStatus.Down ? KEYDOWN : KEYDOWN | KEYUP;
            keybd_event(byte.Parse(((int)input.Key).ToString()), 0x45, dwFlags, UIntPtr.Zero);
        }

        public bool LoadMacros()
        {
            return true;
        }
    }
}
