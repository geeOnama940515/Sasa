using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.Models.Editable;

namespace Sasa.Models.Household
{
    public class HouseholdNewModel : HouseholdEditModel
    {
        public HouseholdNewModel() : base(new DataAccess.EF.Household())
        {
        }
    }
    public class HouseholdEditModel:EditModelBase<DataAccess.EF.Household>
    {
        public HouseholdEditModel(DataAccess.EF.Household model) : base(model)
        {
            ModelCopy = CreateCopy(model);
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

        public string Address
        {
            get { return _ModelCopy.Address; }
            set
            {
                _ModelCopy.Address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }

        public int? PurokNo
        {
            get { return _ModelCopy.PurokNo; }
            set
            {
                _ModelCopy.PurokNo = value;
                RaisePropertyChanged(nameof(PurokNo));
            }
        }

        private DataAccess.EF.Household CreateCopy(DataAccess.EF.Household model)
        {
            var household = new DataAccess.EF.Household
            {
                HouseNo = model.HouseNo,
                PurokNo = model.PurokNo,
                Address = model.Address
            };

            return household;
        }
    }
}
