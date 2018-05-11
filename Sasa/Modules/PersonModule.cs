using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Models.Editable;
using Sasa.Models.Household;
using Sasa.Models.Person;
using Sasa.Models.Purok;
using Sasa.Reports.Household;
using Sasa.View.Population.Household.Family;

namespace Sasa.Modules
{
    public class PersonModule : ObservableObject
    {
        private AddFamilyHeadWindow _addFamilyHeadWindow;

        private AddFamilyMemberWindow _addFamilyMemberWindow;

        private bool _isEditingMembers;

        private bool _isEditingHouseholdHead;

        private PersonNewModel _personNewModel;
        private readonly IRepository _repository;

        private HouseholdModel _selectedHouseHold;

        private PersonModel _selectedPerson;

        private PersonModel _selectedHead;


        public PersonModule(IRepository repository)
        {
            _repository = repository;
            LoadAllPersons();
        }

        private void LoadAllPersons()
        {
            var puroks = _repository.Purok.GetRange();
            foreach (var purok in puroks)
            {
                var households = _repository.Household.GetRange(c => c.PurokNo == purok.PurokNo);
                foreach (var household in households)
                {
                    var persons = _repository.Person.GetRange(c => c.HouseNo == household.HouseNo);
                    foreach (var person in persons)
                    {
                        var singlePerson = _repository.Person.Get(c => c.PersonId == person.PersonId);
                        
                        Persons.Add(new PersonModel(singlePerson));
                    }
                }

            }
        }

        public ObservableCollection<PersonModel> Persons { get; } = new ObservableCollection<PersonModel>();
        public ObservableCollection<PersonModel> PersonList { get; } = new ObservableCollection<PersonModel>();
        public ObservableCollection<PersonNewModel> NewPersonList { get; } = new ObservableCollection<PersonNewModel>();

        
        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                RaisePropertyChanged(nameof(SelectedPerson));
            }
        }

        public PersonModel SelectedHead
        {
            get { return _selectedHead; }
            set
            {
                _selectedHead = value;
                RaisePropertyChanged(nameof(SelectedHead));
            }
        }

        public HouseholdModel SelectedHousehold
        {
            get { return _selectedHouseHold; }
            set
            {
                _selectedHouseHold?.CancelEditCommand.Execute(null);
                _selectedHouseHold = value;
                if (value != null)
                {
                    //_selectedHouseHold.Members.Clear();
                    _selectedHouseHold.LoadRelatedInfo();
                }
                RaisePropertyChanged(nameof(SelectedHousehold));
            }
        }

        private PersonEditModel _householdEditModel;

        public PersonEditModel HouseholdEditModel
        {
            get { return _householdEditModel;}
            set
            {
                _householdEditModel = value;
                RaisePropertyChanged(nameof(HouseholdEditModel));
            }
        } 

        public PersonNewModel PersonNewModel
        {
            get { return _personNewModel; }
            set
            {
                _personNewModel = value;
                RaisePropertyChanged(nameof(PersonNewModel));
            }
        }

        public bool IsEditingMembers
        {
            get { return _isEditingMembers; }
            set
            {
                _isEditingMembers = value;
                RaisePropertyChanged(nameof(IsEditingMembers));
            }
        }

        public bool IsEditingHouseholdHead
        {
            get { return _isEditingHouseholdHead; }
            set
            {
                _isEditingHouseholdHead = value;
                RaisePropertyChanged(nameof(IsEditingHouseholdHead));
            }
        }

        

        public ObservableCollection<EditModelBase<Person>> EditModels { get; } =
            new ObservableCollection<EditModelBase<Person>>();

        public ICommand PrintHouseholdCommand => new RelayCommand(PrintHouseholdProc);

        public ICommand EditHouseholdHeadCommand => new RelayCommand(EditHouseholdHeadProc);

        public ICommand DemoteHouseholdHeadCommand => new RelayCommand(DemoteHouseholdHeadProc, DemoteHouseholdHeadCondition);

        public ICommand SaveHouseholdEditCommand => new RelayCommand(SaveHouseholdHeadProc, SaveHouseholdHeadCondition);

        public ICommand MemberEditCommand => new RelayCommand(MemberEditProc);

        public ICommand CancelMemberEditCommand => new RelayCommand(CancelMemberEditProc);

        public ICommand SaveMemberEditCommand => new RelayCommand(SaveMemberEditProc);

        public ICommand CancelPersonCommand => new RelayCommand(CancelPersonProc);

        public ICommand CancelFamilyMemberCommand => new RelayCommand(CancelFamilyMemberProc);

        public ICommand DeleteSelectedPersonCommand
            => new RelayCommand(DeleteSelectedPersonProc, DeleteSelectedPersonCondition);

        public ICommand AddFamilyMemberCommand => new RelayCommand(AddFamilyMemberProc);

        public ICommand AddMoreFamilyMemberCommand => new RelayCommand(AddMoreFamilyMemberProc);

        public ICommand SaveHouseHeadCommand => new RelayCommand(SaveHeadProc, SaveHeadCondition);

        public ICommand SaveFamilyMembersCommand => new RelayCommand(SaveFamilyMembersProc, SaveFamilyMembersCondition);

        private List<Person> Members { get; } = new List<Person>();

        public ICommand SavePersonCommand => new RelayCommand(SavePersonProc, SavePersonCondition);

        public ICommand AddHeadCommand => new RelayCommand(AddHouseholdHeadProc, AddPersonCondition);

        private HouseholdReportViewerWindow _householdReportViewer;
        private void PrintHouseholdProc()
        {
            _householdReportViewer = new HouseholdReportViewerWindow();
            _householdReportViewer.Owner = Application.Current.MainWindow;
            _householdReportViewer.Show();
        }

        private void EditHouseholdHeadProc()
        {
            IsEditingHouseholdHead = true;
            HouseholdEditModel = new PersonEditModel(SelectedHousehold.HouseholdHead.Model);
            
        }

        private bool SaveHouseholdHeadCondition()
        {
            return (HouseholdEditModel != null) && HouseholdEditModel.HasChanges;
        }

        private void SaveHouseholdHeadProc()
        {
            if (HouseholdEditModel == null) return;
            if (!HouseholdEditModel.HasChanges) return;

            try
            {
                IsEditingHouseholdHead = false;
                SelectedHousehold.HouseholdHead.Model = HouseholdEditModel.ModelCopy;
                _repository.Person.Update(HouseholdEditModel.ModelCopy);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Edit", "Edit Household Head", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool DemoteHouseholdHeadCondition()
        {
            return (SelectedHousehold?.HouseholdHead != null);
        }

        private void DemoteHouseholdHeadProc()
        {
            if (SelectedHousehold?.HouseholdHead == null) return;
            try
            {
                SelectedHousehold.HouseholdHead.Model.IsHead = false;
                _repository.Person.Update(SelectedHousehold.HouseholdHead.Model);
                SelectedHousehold.HouseholdHead = null;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Edit!", "Edit Household Head", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MemberEditProc()
        {
            IsEditingMembers = true;
            EditModels.Clear();
            foreach (var member in SelectedHousehold.Members)
            {
                EditModels.Add(new PersonEditModel(member.Model));
            }
        }

        private void CancelMemberEditProc()
        {
            IsEditingMembers = false;
            foreach (var editModel in EditModels)
            {
                editModel?.Dispose();
            }
        }

        private void SaveMemberEditProc()
        {
            IsEditingMembers = false;
            SelectedHousehold.Members.Clear();
            foreach (var member in EditModels)
            {
                if (member.ModelCopy.Birthdate != null)
                {
                    if (member.ModelCopy.DateOfDeath != null)
                    {
                        member.ModelCopy.IsDead = "Yes";
                        member.ModelCopy.Age = GetDeadAge(member.ModelCopy.Birthdate, member.ModelCopy.DateOfDeath);

                    }
                    else
                    {
                        member.ModelCopy.Age = GetAge(member.ModelCopy.Birthdate);
                    }
                }
                _repository.Person.Update(member.ModelCopy);
                var personmodel = new PersonModel(member.ModelCopy);
                personmodel.Model = member.ModelCopy;
                SelectedHousehold.Members.Add(personmodel);
            }
        }

        private void CancelPersonProc()
        {
            PersonNewModel?.Dispose();
            _addFamilyHeadWindow.Close();
        }

        private void CancelFamilyMemberProc()
        {
            foreach (var family in NewPersonList)
            {
                family?.Dispose();
            }
            _addFamilyMemberWindow.Close();
        }

        private bool DeleteSelectedPersonCondition()
        {
            return SelectedPerson != null;
        }

        private void DeleteSelectedPersonProc()
        {
            try
            {
                _repository.Person.Remove(SelectedPerson.Model);
                SelectedHousehold.Members.Remove(SelectedPerson);
                PersonList.Remove(SelectedPerson);

            }
            catch (Exception e)
            {
                if (!(SelectedPerson.Model.IsHead == true && SelectedPerson.Model.IsPurokHead == true))
                {
                    if (SelectedPerson.Cases.Count > 0)
                    {
                        MessageBox.Show("There is an existing case involving this person! Unable to Delete!", "Delete Person",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Unable to Delete! Is Purok Head!", "Delete Member", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }

        }

        private void AddFamilyMemberProc()
        {
            NewPersonList.Clear();
            var selectedPurok = _repository.Purok.Get(c => c.PurokNo == SelectedHousehold.Model.PurokNo);
            NewPersonList.Add(new PersonNewModel
            {
                PurokNo = selectedPurok.PurokNo,
                HouseNo = SelectedHousehold.Model.HouseNo
            });

            _addFamilyMemberWindow = new AddFamilyMemberWindow {Owner = Application.Current.MainWindow};
            _addFamilyMemberWindow.ShowDialog();
        }

        private void AddMoreFamilyMemberProc()
        {
            var selectedPurok = _repository.Purok.Get(c => c.PurokNo == SelectedHousehold.Model.PurokNo);
            NewPersonList.Add(new PersonNewModel
            {
                PurokNo = selectedPurok.PurokNo,
                HouseNo = SelectedHousehold.Model.HouseNo
            });
        }

        private bool SaveHeadCondition()
        {
            return (SelectedHead != null);
        }

        private string GetAge(DateTime? birthdate)
        {
            TimeSpan? difference = DateTime.Now - birthdate;
            return Convert.ToString(difference.Value.Days / 365) ;
        }

        private void SaveHeadProc()
        {
            if (PersonNewModel == null) return;

            try
            {
                SelectedHead.Model.IsHead = true;
                _repository.Person.Update(SelectedHead.Model);
                SelectedHousehold.HouseholdHead = SelectedHead;
                _addFamilyHeadWindow.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Add Person");
            }
        }

        private bool SaveFamilyMembersCondition()
        {
            return NewPersonList != null;
        }

        private void SaveFamilyMembersProc()
        {
            if (NewPersonList == null) return;

            foreach (var member in NewPersonList.Where(member => member != null))
            {
             

                if (member.ModelCopy.Birthdate != null)
                {
                    if (member.ModelCopy.DateOfDeath != null)
                    {
                        member.ModelCopy.IsDead = "Yes";
                        member.ModelCopy.Age = GetDeadAge(member.ModelCopy.Birthdate, member.ModelCopy.DateOfDeath);
                    }
                    else
                    {
                        member.ModelCopy.Age = GetAge(member.ModelCopy.Birthdate);
                    }
                    
                }

                PersonList.Add(new PersonModel(member.ModelCopy, _repository));
                SelectedHousehold.Members.Add(new PersonModel(member.ModelCopy, _repository));

                try
                {
                    _repository.Person.Add(member.ModelCopy);
                }

                catch (Exception e)
                {
                    MessageBox.Show("Unable to Save!", "Save Family Member");
                }


                
                _addFamilyMemberWindow.Close();
            }

            
        }

        private string GetDeadAge(DateTime? birthdate, DateTime? dateOfDeath)
        {
            TimeSpan? difference = dateOfDeath - birthdate;
            return Convert.ToString(difference.Value.Days / 365);

        }

        private bool SavePersonCondition()
        {
            return (PersonNewModel != null) && PersonNewModel.HasChanges;
        }

        private void SavePersonProc()
        {
            if (PersonNewModel == null) return;
            if (!PersonNewModel.HasChanges) return;

            try
            {
                _repository.Person.Add(PersonNewModel.ModelCopy);
                PersonList.Add(new PersonModel(PersonNewModel.ModelCopy, _repository));
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Add Person");
            }
        }

        private bool AddPersonCondition()
        {
            if (SelectedHousehold != null)
            {
                if (SelectedHousehold.HouseholdHead != null)
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        private void AddHouseholdHeadProc()
        {
            PersonNewModel?.Dispose();
            var selectedPurok = _repository.Purok.Get(c => c.PurokNo == SelectedHousehold.Model.PurokNo);
            PersonNewModel = new PersonNewModel
            {
                PurokNo = selectedPurok.PurokNo,
                IsHead = true,
                HouseNo = SelectedHousehold.Model.HouseNo
            };
            _addFamilyHeadWindow = new AddFamilyHeadWindow {Owner = Application.Current.MainWindow};
            _addFamilyHeadWindow.Show();
        }
    }
}