using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.Models.Editable;
using Sasa.Models.Person;

namespace Sasa.Models.Purok
{
    public class NewPurokModel : PurokEditModel
    {
        public NewPurokModel() : base(new DataAccess.EF.Purok())
        {
            InitializeRequiredFields();
        }

        private void InitializeRequiredFields()
        {
            PurokName = string.Empty;
            Location = string.Empty;

        }
    }

    public class PurokEditModel:EditModelBase<DataAccess.EF.Purok>
    {
        private PersonModel _selectedPerson;


        protected PurokEditModel() : base(new DataAccess.EF.Purok())
        {

        }

        public PurokEditModel(DataAccess.EF.Purok model) : base(model)
        {
            ModelCopy = CreateCopy(model);
        }

        public int PurokNo
        {
            get { return _ModelCopy.PurokNo; }
            set { _ModelCopy.PurokNo = value; }
        }
        
        public string PurokName
        {
            get { return _ModelCopy.PurokName; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(PurokName),
                    () => string.IsNullOrWhiteSpace(value), "Purok name should not be empty.");

                _ModelCopy.PurokName = newValue;
            }
        }

        public string Location
        {
            get { return _ModelCopy.Location; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Location),
                    () => string.IsNullOrWhiteSpace(value), "Location should not be empty.");

                _ModelCopy.Location = newValue;

            }
        }

        public int? PurokHead
        {
            get { return _ModelCopy.PurokHead; }
            set
            {
                _ModelCopy.PurokHead = value;
                RaisePropertyChanged(nameof(PurokHead));
            }
        }

        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                if (value != null)
                {
                    PurokHead = SelectedPerson.Model.PersonId;
                }
                RaisePropertyChanged(nameof(SelectedPerson));
            }
        }


        private DataAccess.EF.Purok CreateCopy(DataAccess.EF.Purok model)
        {
            var copy = new DataAccess.EF.Purok()
            {
                PurokName = model.PurokName,
                Location = model.Location,
                PurokNo = model.PurokNo,
                PurokHead = model.PurokHead
            };
            return copy;

        }
    }
}
