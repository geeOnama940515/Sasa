using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.Models.Editable;

namespace Sasa.Models.Livestock
{
    public class LivestockNewModel : LivestockEditModel
    {
        public LivestockNewModel() : base(new DataAccess.EF.Livestock())
        {
            InitializeRequiredFields();
        }

        private void InitializeRequiredFields()
        {
            Animal = "Kabaw";
        }
    }
    public class LivestockEditModel : EditModelBase<DataAccess.EF.Livestock>
    {
        public LivestockEditModel(DataAccess.EF.Livestock model) : base(model)
        {
            ModelCopy = CreateCopy(model);
        }

        public List<string> Pets { get; } = new List<string> { "Kabaw", "Baka", "Kabayo", "Unggoy", "Baboy", "Kanding", "Iro", "Iring", "Pabo", "Pato", "Dove", "Turtle", "Fish" };


        private DataAccess.EF.Livestock CreateCopy(DataAccess.EF.Livestock model)
        {
            var copy = new DataAccess.EF.Livestock
            {
                IsVaccinated = model.IsVaccinated,
                Animal = model.Animal,
                Female = model.Female,
                HouseNo = model.HouseNo,
                LivestockId = model.LivestockId,
                Male = model.Male,
                Quantity = model.Quantity
            };

            return copy;
        }


        public string Animal
        {
            get { return _ModelCopy.Animal; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Animal),
                    () => string.IsNullOrWhiteSpace(value), "Field should not be empty");

                _ModelCopy.Animal = newValue;
            }
        }

        public int? Quantity
        {
            get { return _ModelCopy.Quantity; }
            set
            {
                _ModelCopy.Quantity = value;
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        public int LivestockId
        {
            get { return _ModelCopy.LivestockId; }
            set
            {
                _ModelCopy.LivestockId = value;
                RaisePropertyChanged(nameof(LivestockId));
            }
        }

        public string Male
        {
            get { return _ModelCopy.Male; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Male),
                    () =>
                    {
                        long x;
                        var result = long.TryParse(value, out x);
                        return !result;
                    }, "Field should not contain letters");

                _ModelCopy.Male = newValue;
            }
        }

        public string Female
        {
            get { return _ModelCopy.Female; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Female),
                    () =>
                    {
                        long x;
                        var result = long.TryParse(value, out x);
                        return !result;
                    }, "Field should not contain letters");

                _ModelCopy.Female = newValue;
            }
        }

        public string HouseNo
        {
            get { return _ModelCopy.HouseNo; }
            set
            {
                _ModelCopy.HouseNo = value;
                RaisePropertyChanged(nameof(HouseNo));
            }
        }

        public int? IsVaccinated
        {
            get { return _ModelCopy.IsVaccinated; }
            set
            {
                _ModelCopy.IsVaccinated = value;
                RaisePropertyChanged(nameof(IsVaccinated));
            }
        }

    }
}
