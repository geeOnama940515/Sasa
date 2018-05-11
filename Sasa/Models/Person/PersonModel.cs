using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Models.Case;
using Sasa.Models.Editable;
using Sasa.Models.Household;
using Sasa.Models.Purok;
using Sasa.Modules;

namespace Sasa.Models.Person
{
    public class PersonModel : ModelBase<DataAccess.EF.Person>, IEditableModel<DataAccess.EF.Person>
    {
        public PersonModel(DataAccess.EF.Person model) : base(model) { }

        public PersonModel(DataAccess.EF.Person model, IRepository repository) : base(model, repository)
        {
        }

        public ObservableCollection<CaseModel> Cases { get; } = new ObservableCollection<CaseModel>();

        public HouseholdModel Household
        {
            get { return _householdModel; }
            set
            {
                _householdModel = value;
                RaisePropertyChanged(nameof(Household));
            }
        }

        public PurokModel Purok
        {
            get { return _purok; }
            set
            {
                _purok = value; 
                RaisePropertyChanged(nameof(Purok));
            }
        }

        public void LoadRelatedInfo()
        {
            var cases = _Repository.Case.GetRange(c => c.Respondent == Model.PersonId);
            var household = _Repository.Household.Get(h => h.HouseNo == Model.HouseNo);
            Household = new HouseholdModel(household, _Repository);

            foreach (var item in cases)
            {
                var casemodel = new CaseModel(item, _Repository);
                Cases.Add(casemodel);
            }
        }

        private bool _isEditing;

        public bool isEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged(nameof(isEditing));
            }
        }

        private EditModelBase<DataAccess.EF.Person> _editModel;
        private HouseholdModel _householdModel;
        private PurokModel _purok;

        public EditModelBase<DataAccess.EF.Person> EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value;
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        public ICommand EditCommand => new RelayCommand(EditProc, EditCondition);

        private bool EditCondition()
        {
            //            var householdHead =
            //                _Repository.Person.Get(
            //                    c =>
            //                        c.HouseNo ==
            //                        ViewModelLocatorStatic.Locator.HouseholdProfileModule.SelectedHousehold.Model.HouseNo &&
            //                        c.IsHead == true);
            //            if (householdHead == null) return false;
            return true;
        }

        private void EditProc()
        {
            isEditing = true;
            EditModel?.Dispose();
            EditModel = new PersonEditModel(Model);

        }

        public ICommand SaveEditCommand => new RelayCommand(SaveProc, SaveCondition);

        private bool SaveCondition()
        {
            return (EditModel != null) && EditModel.HasChanges;
        }

        private void SaveProc()
        {
            if (EditModel == null) return;
            if (!EditModel.HasChanges) return;
            try
            {
                _Repository.Person.Update(EditModel.ModelCopy);
                Model = EditModel.ModelCopy;
                isEditing = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Update Person");
            }
        }

        public ICommand CancelEditCommand => new RelayCommand(CancelProc);

        private void CancelProc()
        {
            isEditing = false;
            EditModel?.Dispose();
        }
    }
}
