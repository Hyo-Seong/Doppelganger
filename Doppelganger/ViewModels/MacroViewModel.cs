using Doppelganger.Models;
using Doppelganger.Util;
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

namespace Doppelganger.ViewModels
{
    public class MacroViewModel : BindableBase
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        #region 
        private UserActivityHook _hook;

        private Stopwatch stopwatch = new Stopwatch();

        private ObservableCollection<Macro> _items;
        public ObservableCollection<Macro> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private Macro macro = new Macro();
        #endregion

        public DelegateCommand StartRecordingCommand { get; private set; }

        void _hook_KeyDown(object sender, CustomKeyEventArgs e)
        {
            stopwatch.Stop();
            macro.InputValues.Add(new InputValue
            {
                InputType = InputType.Keyboard,
                Key = e.Key,
                KeyStatus = KeyStatus.Down,
                Millis = stopwatch.ElapsedMilliseconds
            });
            stopwatch.Restart();
        }

        public MacroViewModel()
        {
            _hook = new UserActivityHook(true, true);
            Items = new ObservableCollection<Macro>();
            StartRecordingCommand = new DelegateCommand(StartHooking);
        }

        private bool stop = true;

        public void StartHooking()
        {
            if (stop)
            {
                macro = new Macro
                {
                    Name = "Temp1"
                };
                _hook.KeyDown += _hook_KeyDown;
                stopwatch.Start();
            }
            else
            {
                _hook.KeyDown -= _hook_KeyDown;
                stopwatch.Stop();
                macro.InputValues.Add(new InputValue
                {
                    InputType = InputType.Keyboard,
                    Key = Keys.None,
                    KeyStatus = KeyStatus.Down,
                    Millis = stopwatch.ElapsedMilliseconds
                });
                Items.Add((Macro)macro.Clone());
            }
            stop = !stop;
        }

        public void StartMacro(List<InputValue> inputValues)
        {
            if(inputValues != null && inputValues.Count != 0)
            {
                inputValues.ForEach(x => PressKey((int)x.Key));
            }
        }

        void PressKey(int keyCode)
        {
            const int KEYEVENTF_EXTENDEDKEY = 0x1;
            const int KEYEVENTF_KEYUP = 0x2;
            Console.WriteLine(byte.Parse(keyCode.ToString()));
            keybd_event(byte.Parse(keyCode.ToString()), 0x45, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(byte.Parse(keyCode.ToString()), 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
        }

        public bool LoadMacros()
        {
            return true;
        }
    }
}
