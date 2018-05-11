using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.DataAccess.EF;
using Sasa.Models.Editable;

namespace Sasa.Models.Household
{
    class MembersEditModel:EditModelBase<Person>
    {
        public MembersEditModel(Person model) : base(model)
        {
        }
    }
}
