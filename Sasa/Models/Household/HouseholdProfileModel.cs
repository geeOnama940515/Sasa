using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Sasa.DataAccess;
using Sasa.Models.Editable;

namespace Sasa.Models.Household
{
    public class HouseholdProfileModel: ModelBase<DataAccess.EF.Household>, IEditableModel<DataAccess.EF.Household>
    {
        public HouseholdProfileModel(DataAccess.EF.Household model) : base(model)
        {
        }

        public HouseholdProfileModel(DataAccess.EF.Household model, IRepository repository) : base(model, repository)
        {
        }

        public bool isEditing { get; }
        public EditModelBase<DataAccess.EF.Household> EditModel { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }
    }
}
