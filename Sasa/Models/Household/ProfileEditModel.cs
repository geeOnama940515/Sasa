using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.Models.Editable;

namespace Sasa.Models.Household
{
    class ProfileEditModel:EditModelBase<DataAccess.EF.Household>
    {
        public ProfileEditModel(DataAccess.EF.Household model) : base(model)
        {
        }
    }
}
