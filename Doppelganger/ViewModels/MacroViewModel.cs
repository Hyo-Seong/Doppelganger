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
using static RamGecTools.MouseHook;
using Doppelganger.Models.Input;
using RamGecTools;

namespace Doppelganger.ViewModels
{
    public class MacroViewModel : BindableBase
    {
        #region DllImport
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        #endregion

        #region 
        private readonly uint KEYDOWN = 0x1;
        private readonly uint KEYUP = 0x2;

        MouseHook _mouseHook = new MouseHook();

        private UserActivityHook _keyboardHook;

        private Stopwatch stopwatch = new Stopwatch();

        private ObservableCollection<Macro> _items;
        public ObservableCollection<Macro> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private Macro macro = new Macro();
        #endregion

        #region Command
        public DelegateCommand StartKeyboardRecordingCommand { get; private set; }
        public DelegateCommand StartMouseRecordingCommand { get; private set; }


        #endregion

        public MacroViewModel()
        {
            _keyboardHook = new UserActivityHook(true, true);
            Items = new ObservableCollection<Macro>();
            StartKeyboardRecordingCommand = new DelegateCommand(StartKeyboardHooking);
            StartMouseRecordingCommand = new DelegateCommand(StartMouseHooking);
        }

        private bool flag2 = true;

        private void StartMouseHooking()
        {
            if (flag2)
            {
                _mouseHook.MouseHookReceived += new MouseHookCallback(mouseHook_MouseWheel);

                _mouseHook.Install();
            }
            else
            {
                _mouseHook.Uninstall();
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
                    Name = "Temp1"
                };
                _keyboardHook.StartStopwatch();
                _keyboardHook.KeyDown += _hook_KeyEvent;
                _keyboardHook.KeyUp += _hook_KeyEvent;
            }
            else
            {
                _keyboardHook.StopStopwatch();
                _keyboardHook.KeyDown -= _hook_KeyEvent;
                _keyboardHook.KeyUp -= _hook_KeyEvent;

                macro.KeyboardInputs.Add(new KeyboardInput
                {
                    Key = Keys.None,
                    KeyStatus = KeyStatus.Down,
                    Millis = stopwatch.ElapsedMilliseconds
                });
                Items.Add((Macro)macro.Clone());
                macro = null;
            }
            stop = !stop;
        }

        public void ExcuteMacro(List<KeyboardInput> keyboardInputs)
        {
            if(keyboardInputs != null && keyboardInputs.Count != 0)
            {
                keyboardInputs.ForEach(PressKey);
            }
        }

        void PressKey(KeyboardInput input)
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
