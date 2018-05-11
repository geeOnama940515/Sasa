using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Editable;
using Sasa.Models.Household;
using Sasa.Models.Livestock;
using Sasa.Models.Person;
using Sasa.Modules;
using Sasa.Reports.Purok;

namespace Sasa.Models.Purok
{
    public class PurokModel: ModelBase<DataAccess.EF.Purok>, IEditableModel<DataAccess.EF.Purok>
    {
        private bool _isEditing;
        private IRepository _repository;
        private EditModelBase<DataAccess.EF.Purok> _editModel;

        public ObservableCollection<HouseholdModel> Households { get; } = new ObservableCollection<HouseholdModel>(); 
        public ObservableCollection<PersonModel> Constituents { get; } = new ObservableCollection<PersonModel>();


        private int _numberOfPersons;
        public int NumberOfPersons
        {
            get { return _numberOfPersons; }
            set
            {
                _numberOfPersons = value;
                RaisePropertyChanged(nameof(NumberOfPersons));
            }
        }

        private PersonModel _purokHead;
        public PersonModel PurokHead
        {
            get { return _purokHead; }
            set
            {
                _purokHead = value;
                RaisePropertyChanged(nameof(PurokHead));
            }
        }

        private int _femaleCount;
        public int FemaleCount
        {
            get { return _femaleCount;}
            set
            {
                _femaleCount = value;
                RaisePropertyChanged(nameof(FemaleCount));
            }
        }

        private int _maleCount;
        public int MaleCount
        {
            get { return _maleCount; }
            set
            {
                _maleCount = value;
                RaisePropertyChanged(nameof(MaleCount));
            }
        }

        private void LoadNumberOfPersons()
        {
            NumberOfPersons = 0;
            var households = _repository.Household.GetRange(c => c.PurokNo == Model.PurokNo);
            foreach (var household in households)
            {
                var persons = _repository.Person.GetRange(c => c.HouseNo == household.HouseNo);
                foreach (var person in persons)
                {
                    NumberOfPersons += 1;
                }
            }
        }
       
        //for editing Purok Leader
        private void LoadAllPersons()
        {
            Constituents.Clear();
            var households = _repository.Household.GetRange(c => c.PurokNo == Model.PurokNo);
            MaleCount = 0;
            FemaleCount = 0;
            foreach (var household in households)
            {
                var persons = _repository.Person.GetRange(c => c.HouseNo == household.HouseNo);
                
                foreach (var person in persons)
                {
                    //var singlePerson = _repository.Person.Get(c => c.PersonId == person.PersonId);
                    try
                    {
                        if (person.Gender.Equals("Male"))
                        {
                            MaleCount++;
                        }
                        else if (person.Gender.Equals("Female"))
                        {
                            FemaleCount++;
                        }
                    }
                    catch(Exception e) { }

                    Constituents.Add(new PersonModel(person, _repository));

                    if (person.IsPurokHead == true)
                    {
                        PurokHead = new PersonModel(person, _repository);
                    }
                }

            }
        }

        public PurokModel(DataAccess.EF.Purok model) : base(model){}

        public PurokModel(DataAccess.EF.Purok model, IRepository repository) : base(model, repository)
        {
            _repository = repository;
            InitializeView();
         
        }

        public void LoadRelatedInfo()
        {
            LoadHouseholds();
            LoadAllPersons();
            LoadNumberOfPersons();
            LoadPurokPets();
        }
        public ObservableCollection<LivestockModel> PurokLivestocks { get; } = new ObservableCollection<LivestockModel>();
        private void LoadPurokPets()
        {
            foreach (var household in Households)
            {
                var pets = _repository.Livestock.GetRange(c => c.HouseNo == household.Model.HouseNo);
                foreach (var pet in pets)
                {
                    var singlePet = _repository.Livestock.Get(c => c.LivestockId == pet.LivestockId);
                    PurokLivestocks.Add(new LivestockModel(singlePet));
                }
            }
        }

        private void LoadHouseholds()
        {
            Households.Clear();
            var households = _repository.Household.GetRange(c => c.PurokNo == Model.PurokNo);
            foreach (var household in households)
            {
                Households.Add(new HouseholdModel(household, _repository));
            }
        }

        public bool isEditing
        {
            get { return _isEditing; }
            private set
            {
                _isEditing = value;
                RaisePropertyChanged(nameof(isEditing));
            }
        }

        public EditModelBase<DataAccess.EF.Purok> EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value;
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        public ICommand EditCommand => new RelayCommand(EditProc);
        public ICommand SaveEditCommand => new RelayCommand(SaveEditProc, SaveEditCondition);
        public ICommand CancelEditCommand => new RelayCommand(CancelEditProc);
   
        private void EditProc()
        {
            isEditing = true;
            EditModel?.Dispose();
            EditModel = new PurokEditModel(Model);
        }
     
        private bool SaveEditCondition()
        {
            return (EditModel != null) && EditModel.HasChanges && !EditModel.HasErrors;
        }

        private void SaveEditProc()
        {
            if (EditModel == null) return;
            if (!EditModel.HasChanges) return;

            try
            {
                //updates old purok leader's status
                var oldPurokLeader = _repository.Person.Get(c => c.PurokNo == Model.PurokNo && c.IsPurokHead == true);

                //if no purok leader yet
                if (oldPurokLeader == null)
                {
                    _Repository.Purok.Update(EditModel.ModelCopy);

                    //updates new purok leader's status
                    var getPerson = _repository.Person.Get(c => c.PersonId == EditModel.ModelCopy.PurokHead);
                    getPerson.IsPurokHead = true;
                    _Repository.Person.Update(getPerson);

                    //replace the model with the edited copy
                    Model = EditModel.ModelCopy;

                    isEditing = false;
                }

                //if a purok leader already exists
                else
                {
                    oldPurokLeader.IsPurokHead = false;
                    _repository.Person.Update(oldPurokLeader);

                    _Repository.Purok.Update(EditModel.ModelCopy);

                    //updates new purol leader's status
                    var getPerson = _repository.Person.Get(c => c.PersonId == EditModel.ModelCopy.PurokHead);
                    getPerson.IsPurokHead = true;
                    _Repository.Person.Update(getPerson);

                    //replace the model with the edited copy
                    Model = EditModel.ModelCopy;

                    isEditing = false;
                }  
            }

            catch (Exception e)
            {

                MessageBox.Show("Unable to save. Reason" + e.Message);
            }

            var again = ViewModelLocatorStatic.Locator.PurokModule.SelectedPurok;
            ViewModelLocatorStatic.Locator.PurokModule.SelectedPurok = null;
            ViewModelLocatorStatic.Locator.PurokModule.SelectedPurok = again;
        }

        private ListCollectionView _view;
        private string _search;

        public string Search
        {
            get { return _search; }
            set
            {
                _search = value;
                Filter();
                RaisePropertyChanged(nameof(Search));
            }
        }


        private void Filter()
        {
            _view.Filter = Filter;
        }

        private bool Filter(object o)
        {
            var item = o as PersonModel;
            if (item.Model.FirstName.ToLowerInvariant().Contains(_search.ToLowerInvariant())
                || item.Model.MiddleName.ToLowerInvariant().Contains(_search.ToLowerInvariant())
                || item.Model.LastName.ToLowerInvariant().Contains(_search.ToLowerInvariant())
                || item.Model.HouseNo.ToLowerInvariant().Contains(_search.ToLowerInvariant()))
                return true;
            return false;
        }

        private void InitializeView()
        {
            _view = CollectionViewSource.GetDefaultView(Constituents) as ListCollectionView;
            _view?.SortDescriptions.Clear();

        }

        private void CancelEditProc()
        {
            isEditing = false;
            EditModel?.Dispose();
        }
    }
}
