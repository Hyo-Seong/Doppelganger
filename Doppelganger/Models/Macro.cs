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

        private List<IInput> _inputValues = new List<IInput>();
        public List<IInput> InputValues
        {
            get => _inputValues;
            set => SetProperty(ref _inputValues, value);
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

            InputValues.ForEach(x =>
            {
                if(((InputValue)x).InputType == InputType.Keyboard)
                {
                    macro.InputValues.Add((KeyboardInput)((KeyboardInput)x).Clone());
                } else if(((InputValue)x).InputType == InputType.Mouse){

                }
            });

            return macro;
        }
    }
}
