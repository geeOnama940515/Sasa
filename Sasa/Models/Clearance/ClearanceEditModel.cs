using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.Models.Editable;
using Sasa.Models.Person;

namespace Sasa.Models.Clearance
{
    public class NewClearanceModel : ClearanceEditModel
    {
        public NewClearanceModel() : base(new DataAccess.EF.Clearance())
        {
            InititalizeRequiredFields();
        }

        private void InititalizeRequiredFields()
        {
            

        }
    }

    public class ClearanceEditModel : EditModelBase<DataAccess.EF.Clearance>
    {
      
        private PersonModel _selectedPerson;
        


        public ClearanceEditModel(DataAccess.EF.Clearance model) : base(model)
        {
            ModelCopy = CreateCopy(model);
        }

        private DataAccess.EF.Clearance CreateCopy(DataAccess.EF.Clearance model)
        {
            var copy = new DataAccess.EF.Clearance()
            {
                DateRequested = model.DateRequested,
                PersonId = model.PersonId,
                Purpose = model.Purpose,
                IsDone = model.IsDone,
                ClearanceId = model.ClearanceId,
                HouseNo = model.HouseNo
                

            };
            return copy;
        }

        public int ClearanceId
        {
            get { return _ModelCopy.ClearanceId; }
            set
            {
                _ModelCopy.ClearanceId = value;
                RaisePropertyChanged(nameof(ClearanceId));
            }
        }

        public int PersonID
        {
            get { return _ModelCopy.PersonId; }
            set
            {
                _ModelCopy.PersonId = value;
                RaisePropertyChanged(nameof(PersonID));
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
                    PersonID = SelectedPerson.Model.PersonId;
                }
                
               
            }
        }

        public DateTime? DateRequested
        {
            get { return _ModelCopy.DateRequested; }
            set
            {
                _ModelCopy.DateRequested = value;
                RaisePropertyChanged(nameof(DateRequested));
            }
        }

        public string Purpose
        {
            get { return _ModelCopy.Purpose; }
            set
            {
                _ModelCopy.Purpose = value;
                RaisePropertyChanged(nameof(Purpose));
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

        public bool? IsDone
        {
            get { return _ModelCopy.IsDone; }
            set
            {
                _ModelCopy.IsDone = value;
                RaisePropertyChanged(nameof(IsDone));
            }
        }
    }
}
