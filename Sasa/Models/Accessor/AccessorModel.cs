using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Sasa.DataAccess;
using Sasa.Models.Editable;

namespace Sasa.Models.Accessor
{
    public class AccessorModel : ModelBase<DataAccess.EF.Accessor>, IEditableModel<DataAccess.EF.Accessor>
    {
        public AccessorModel(DataAccess.EF.Accessor model) : base(model)
        {
        }

        public AccessorModel(DataAccess.EF.Accessor model, IRepository repository) : base(model, repository)
        {
        }

        public bool isEditing { get; }
        public EditModelBase<DataAccess.EF.Accessor> EditModel { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand CancelEditCommand { get; }
    }
}
