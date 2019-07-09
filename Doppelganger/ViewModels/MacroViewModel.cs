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

        void _hook_KeyEvent(object sender, CustomKeyEventArgs e)
        {
            stopwatch.Stop();
            macro.InputValues.Add(new InputValue
            {
                InputType = InputType.Keyboard,
                Key = e.Key,
                KeyStatus = e.KeyStatus,
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
                _hook.KeyDown += _hook_KeyEvent;
                _hook.KeyUp += _hook_KeyEvent;
                stopwatch.Start();
            }
            else
            {
                _hook.KeyDown -= _hook_KeyEvent;
                _hook.KeyUp -= _hook_KeyEvent;
                stopwatch.Stop();
                macro.InputValues.Add(new InputValue
                {
                    InputType = InputType.Keyboard,
                    Key = Keys.None,
                    KeyStatus = KeyStatus.Down,
                    Millis = stopwatch.ElapsedMilliseconds
                });
                Items.Add((Macro)macro.Clone());
                macro = null;
            }
            stop = !stop;
        }

        public void ExcuteMacro(List<InputValue> inputValues)
        {
            if(inputValues != null && inputValues.Count != 0)
            {
                inputValues.ForEach(PressKey);
            }
        }

        void PressKey(InputValue input)
        {
            Thread.Sleep((int)input.Millis);
            Console.WriteLine(input.Key + " : " + input.KeyStatus.ToString());
            const uint KEYEVENTF_EXTENDEDKEY = 0x1;
            const uint KEYEVENTF_KEYUP = 0x2;
            uint dwFlags = input.KeyStatus == KeyStatus.Down ? KEYEVENTF_EXTENDEDKEY : KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP;
            keybd_event(byte.Parse(((int)input.Key).ToString()), 0x45, dwFlags, UIntPtr.Zero);
        }

        public bool LoadMacros()
        {
            return true;
        }
    }
}
