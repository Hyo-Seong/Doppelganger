using Doppelganger.Models.Input;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppelganger.Models
{
    public class Macro : BindableBase, ICloneable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get => _createDate;
            set => SetProperty(ref _createDate, value);
        }

        private DateTime _runningTime;
        public DateTime RunningTime
        {
            get => _runningTime;
            set => SetProperty(ref _runningTime, value);
        }

        private List<KeyboardInput> _keyboardInputs = new List<KeyboardInput>();
        public List<KeyboardInput> KeyboardInputs
        {
            get => _keyboardInputs;
            set => SetProperty(ref _keyboardInputs, value);
        }

        private List<MouseInput> _mouseInputs = new List<MouseInput>();
        public List<MouseInput> MouseInputs
        {
            get => _mouseInputs;
            set => SetProperty(ref _mouseInputs, value);
        }

        public object Clone()
        {
            Macro macro =  new Macro
            {
                Name = this.Name,
                Description = this.Description,
                CreateDate = this.CreateDate,
                RunningTime = this.RunningTime,
            };

            KeyboardInputs.ForEach(x =>
            {
                macro.KeyboardInputs.Add((KeyboardInput)x.Clone());
            });

            MouseInputs.ForEach(x =>
            {
                macro.MouseInputs.Add((MouseInput)x.Clone());
            });

            return macro;
        }
    }
}
