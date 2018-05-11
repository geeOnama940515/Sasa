using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Sasa.DataAccess;
using Sasa.Models.Clearance;
using Sasa.Models.Editable;
using Sasa.Models.Livestock;
using Sasa.Models.Person;
using Sasa.Models.Purok;

namespace Sasa.Models.Household
{
    public class HouseholdModel:ModelBase<DataAccess.EF.Household>, IEditableModel<DataAccess.EF.Household>
    {

        public HouseholdModel(DataAccess.EF.Household model) : base(model){}

        public HouseholdModel(DataAccess.EF.Household model, IRepository repository) : base(model, repository){}

        public ObservableCollection<LivestockModel> Livestocks { get; } = new ObservableCollection<LivestockModel>();
        
        public ObservableCollection<PersonModel> Members { get; } = new ObservableCollection<PersonModel>();

        private PurokModel _purok;

        public PurokModel Purok
        {
            get { return _purok; }
            set
            {
                _purok = value;
                RaisePropertyChanged(nameof(Purok));
            }
        }

        private PersonModel _householdHead;

        public PersonModel HouseholdHead
        {
            get { return _householdHead; }
            set
            {
                _householdHead = value;
                RaisePropertyChanged(nameof(HouseholdHead));
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


        public void LoadRelatedInfo()
        {
            LoadPurok();
            LoadPets();
            LoadPerson();
            LoadClearances();
        }
        public ObservableCollection<ClearanceModel> Clearance { get; } = new ObservableCollection<ClearanceModel>();

        private void LoadPurok()
        {
            var purok = _Repository.Purok.Get(p => p.PurokNo == Model.PurokNo);
            Purok = new PurokModel(purok, _Repository);
        }

        private void LoadClearances()
        {
            Clearance.Clear();
            var clearances = _Repository.Clearance.GetRange(c => c.HouseNo == Model.HouseNo);
            foreach (var clearance in clearances)
            {
                var person = _Repository.Person.Get(c => c.PersonId == clearance.PersonId);
                clearance.Person = person;
                var clearancemodel = new ClearanceModel(clearance, _Repository);
                clearancemodel.LoadRelatedInfo();
                Clearance.Add(clearancemodel);
            }
        }
   
        private void LoadPerson()
        {
            Members.Clear();
            var persons = _Repository.Person.GetRange(p => p.HouseNo == Model.HouseNo);
            foreach (var person in persons)
            {
                var personmodel = new PersonModel(person, _Repository);
                personmodel.LoadRelatedInfo();
                if (person.IsHead == true)
                {   
                    HouseholdHead = personmodel;
                }
              
                Members.Add(personmodel);
            }
        }

        private void LoadPets()
        {
            Livestocks.Clear();
            var livestocks = _Repository.Livestock.GetRange(c => c.HouseNo == Model.HouseNo);
            foreach (var livestock in livestocks)
            {
                Livestocks.Add(new LivestockModel(livestock, _Repository));
            }
        }

        private EditModelBase<DataAccess.EF.Household> _editModel;
        public EditModelBase<DataAccess.EF.Household> EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value;
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        public ICommand EditCommand => new RelayCommand(EditProc);
         
        private void EditProc()
        {
            isEditing = true;
            EditModel?.Dispose();
            EditModel = new HouseholdEditModel(Model);
        }

        public ICommand SaveEditCommand => new RelayCommand(SaveProc, SaveCondtion);

        private bool SaveCondtion()
        {
            return (EditModel != null) && EditModel.HasChanges;
        }

        private void SaveProc()
        {
            if (EditModel == null) return;
            if (!EditModel.HasChanges) return;

            try
            {
                _Repository.Household.Update(EditModel.ModelCopy);
                Model = EditModel.ModelCopy;
                isEditing = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!");
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
