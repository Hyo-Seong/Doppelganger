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

        RamGecTools.MouseHook mouseHook = new RamGecTools.MouseHook();

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

        #region Command
        public DelegateCommand StartKeyboardRecordingCommand { get; private set; }
        public DelegateCommand StartMouseRecordingCommand { get; private set; }


        #endregion

        public MacroViewModel()
        {
            _hook = new UserActivityHook(true, true);
            Items = new ObservableCollection<Macro>();
            StartKeyboardRecordingCommand = new DelegateCommand(StartKeyboardHooking);
            StartMouseRecordingCommand = new DelegateCommand(StartMouseHooking);
        }

        private bool flag2 = true;

        private void StartMouseHooking()
        {
            if (flag2)
            {
                mouseHook.MouseHookReceived += new RamGecTools.MouseHook.MouseHookCallback(mouseHook_MouseWheel);

                mouseHook.Install();
            }
            else
            {
                mouseHook.Uninstall();
            }
            flag2 = !flag2;
        }

        void mouseHook_MouseWheel(RamGecTools.MouseHook.MSLLHOOKSTRUCT mouseStruct, MouseMessages mouseMessages)
        {
            Console.WriteLine("dwExtraInfo : {0}\nflags : {1}\nmouseData : {2}\nx : {3}\ny : {4}\ntime : {5}\nmouseMessages : {6}", mouseStruct.dwExtraInfo, mouseStruct.flags, mouseStruct.mouseData, mouseStruct.pt.x, mouseStruct.pt.y, mouseStruct.time, mouseMessages.ToString());
        }

        private void _hook_KeyEvent(object sender, KeyboardInput e)
        {
            macro.InputValues.Add(e);
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
                _hook.StartStopwatch();
                _hook.KeyDown += _hook_KeyEvent;
                _hook.KeyUp += _hook_KeyEvent;
            }
            else
            {
                _hook.StopStopwatch();
                _hook.KeyDown -= _hook_KeyEvent;
                _hook.KeyUp -= _hook_KeyEvent;
                macro.InputValues.Add(new KeyboardInput(Keys.None, KeyStatus.Down, stopwatch.ElapsedMilliseconds));
                Items.Add((Macro)macro.Clone());
                macro = null;
            }
            stop = !stop;
        }

        public void ExcuteMacro(List<KeyboardInput> inputValues)
        {
            if(inputValues != null && inputValues.Count != 0)
            {
                inputValues.ForEach(PressKey);
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
