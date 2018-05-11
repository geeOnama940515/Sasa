using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Models.Accessor;
using Sasa.Models.Editable;
using Sasa.Models.Person;

namespace Sasa.Models.Employee
{
    public class EmployeeModel : ModelBase<DataAccess.EF.Employee>, IEditableModel<DataAccess.EF.Employee>
    {
        public EmployeeModel(DataAccess.EF.Employee model) : base(model)
        {
        }

        public EmployeeModel(DataAccess.EF.Employee model, IRepository repository) : base(model, repository)
        {
        }

        private PersonModel _person;
        private AccessorModel _accessor;
        private bool _isAdmin;
        private bool _isClerk;
        private bool _isClearance;
        private bool _isAllowedToAdd;
        private bool _isAllowedtoDelete;
        private bool _isAllowedToEdit;
        private bool _isAllowedToAddCase;
        private bool _isAllowedToDeleteCase;
        private bool _isAllowedToEditCase;
        private bool _isAllowedToAddClearance;
        private bool _isAllowedToDeleteClearance;
        private bool _isAllowedToEditClearance;

        // Properties

        public bool IsClearance
        {
            get { return _isClearance; }
            set
            {
                _isClearance = value; 
                RaisePropertyChanged(nameof(IsClearance));
            }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                RaisePropertyChanged(nameof(IsAdmin));
            }
        }

        public bool IsClerk
        {
            get { return _isClerk; }
            set
            {
                _isClerk = value;
                RaisePropertyChanged(nameof(IsClerk));
            }
        }

        public bool IsAllowedToAdd
        {
            get { return _isAllowedToAdd; }
            set
            {
                _isAllowedToAdd = value; 
                RaisePropertyChanged(nameof(IsAllowedToAdd));
            }
        }

        public bool IsAllowedtoDelete
        {
            get { return _isAllowedtoDelete; }
            set
            {
                _isAllowedtoDelete = value; 
                RaisePropertyChanged(nameof(IsAllowedtoDelete));
            }
        }

        public bool IsAllowedToEdit
        {
            get { return _isAllowedToEdit; }
            set
            {
                _isAllowedToEdit = value; 
                RaisePropertyChanged(nameof(IsAllowedToEdit));
            }
        }

        public bool IsAllowedToAddCase
        {
            get { return _isAllowedToAddCase; }
            set
            {
                _isAllowedToAddCase = value; 
                RaisePropertyChanged(nameof(IsAllowedToAddCase));
            }
        }

        public bool IsAllowedToDeleteCase
        {
            get { return _isAllowedToDeleteCase; }
            set
            {
                _isAllowedToDeleteCase = value; 
                RaisePropertyChanged(nameof(IsAllowedToDeleteCase));
            }
        }

        public bool IsAllowedToEditCase
        {
            get { return _isAllowedToEditCase; }
            set
            {
                _isAllowedToEditCase = value; 
                RaisePropertyChanged(nameof(IsAllowedToEditCase));
            }
        }

        public bool IsAllowedToAddClearance
        {
            get { return _isAllowedToAddClearance; }
            set
            {
                _isAllowedToAddClearance = value; 
                RaisePropertyChanged(nameof(IsAllowedToAddClearance));
            }
        }

        public bool IsAllowedToDeleteClearance
        {
            get { return _isAllowedToDeleteClearance; }
            set
            {
                _isAllowedToDeleteClearance = value; 
                RaisePropertyChanged(nameof(IsAllowedToDeleteClearance));
            }
        }

        public bool IsAllowedToEditClearance
        {
            get { return _isAllowedToEditClearance; }
            set
            {
                _isAllowedToEditClearance = value; 
                RaisePropertyChanged(nameof(IsAllowedToEditClearance));
            }
        }

        public bool IsAllowedToAddEmployee
        {
            get { return _isAllowedToAddEmployee; }
            set
            {
                _isAllowedToAddEmployee = value; 
                RaisePropertyChanged(nameof(IsAllowedToAddEmployee));
            }
        }

        public bool IsAllowedToDeleteEmployee
        {
            get { return _isAllowedToDeleteEmployee; }
            set
            {
                _isAllowedToDeleteEmployee = value; 
                RaisePropertyChanged(nameof(IsAllowedToDeleteEmployee));
            }
        }

        public PersonModel Person
        {
            get { return _person; }
            set
            {
                _person = value;
                RaisePropertyChanged(nameof(Person));
            }
        }

        public AccessorModel Accessor
        {
            get { return _accessor; }
            set
            {
                _accessor = value;
                RaisePropertyChanged(nameof(Accessor));
            }
        }

        // Methods

        public void LoadRelatedInfo()
        {
            var person = _Repository.Person.Get(p => p.PersonId == Model.PersonId);
            var personmodel = new PersonModel(person, _Repository);
            Person = new PersonModel(personmodel.Model, _Repository);

            var accessor = _Repository.Accessor.Get(a => a.AccessorId == Model.AccessorId);
            var accessormodel = new AccessorModel(accessor, _Repository);
            Accessor = new AccessorModel(accessormodel.Model, _Repository);
        }

        private bool _isEditing;

        public bool isEditing
        {
            get { return _isEditing;}
            set
            {
                _isEditing = value;
                RaisePropertyChanged(nameof(isEditing));
            }
        }

        private EditModelBase<DataAccess.EF.Employee> _editModel;
        private bool _isAllowedToAddEmployee;
        private bool _isAllowedToDeleteEmployee;

        public EditModelBase<DataAccess.EF.Employee> EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value; 
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        // Commands

        public ICommand EditCommand => new RelayCommand(EditProc);
        public ICommand SaveEditCommand => new RelayCommand(SaveProc, SaveCondition);
        public ICommand CancelEditCommand => new RelayCommand(CancelProc);

        // Methods

        private void EditProc()
        {
            isEditing = true;
            EditModel?.Dispose();
            EditModel = new EmployeeEditModel(Model);
        }

        private bool SaveCondition()
        {
            return (EditModel != null) && EditModel.HasChanges;

        }

        private void SaveProc()
        {
            if (EditModel == null) return;
            if(!EditModel.HasChanges)return;

            try
            {
                isEditing = false;
                Model = EditModel.ModelCopy;
                _Repository.Employee.Update(EditModel.ModelCopy);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Edit Employee", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelProc()
        {
            EditModel?.Dispose();
            isEditing = false;
        }
    }
}
