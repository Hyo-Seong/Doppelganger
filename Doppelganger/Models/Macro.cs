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

        private List<InputValue> _inputValues = new List<InputValue>();
        public List<InputValue> InputValues
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
                macro.InputValues.Add((InputValue)x.Clone());
            });

            return macro;
        }
    }
}
