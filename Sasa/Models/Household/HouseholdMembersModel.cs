using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Models.Editable;

namespace Sasa.Models.Household
{
    public class HouseholdMembersModel: ModelBase<Person>, IEditableModel<Person>
    {
        public HouseholdMembersModel(Person model) : base(model)
        {
        }

        public HouseholdMembersModel(Person model, IRepository repository) : base(model, repository)
        {
        }

        public bool isEditing { get; }
        public EditModelBase<Person> EditModel { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }
    }
}
