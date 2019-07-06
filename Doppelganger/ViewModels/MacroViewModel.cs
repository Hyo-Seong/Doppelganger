using Doppelganger.Models;
using Doppelganger.Util;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.ViewModels
{
    public class MacroViewModel : BindableBase
    {
        #region 
        private UserActivityHook _hook;

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
            macro.InputValues.Add(new KeyboardInput
            {
                InputType = InputType.Keyboard,
                Key = e.Key,
                KeyStatus = KeyStatus.Down,
                Millis = DateTime.Now.Millisecond
            });
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
            }
            else
            {
                _hook.KeyDown -= _hook_KeyDown;
                Items.Add((Macro)macro.Clone());
            }
            stop = !stop;
        }

        public bool LoadMacros()
        {
            return true;
        }
    }
}
