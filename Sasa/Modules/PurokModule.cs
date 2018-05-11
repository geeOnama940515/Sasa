using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Household;
using Sasa.Models.Person;
using Sasa.Models.Purok;
using Sasa.Reports.Population;
using Sasa.Reports.Purok;
using Sasa.View.Population.Purok;
using Sasa.View.Population.Purok.Print;

namespace Sasa.Modules
{
    public class PurokModule : ObservableObject
    {
        private IRepository _repository;

        public PurokModule(IRepository repository)
        {
            _repository = repository;
            LoadPurok();
            LoadEveryone();
        }

        public ObservableCollection<PurokModel> PurokList { get; } = new ObservableCollection<PurokModel>();
        public ObservableCollection<PurokModel> TempPurokList { get; } = new ObservableCollection<PurokModel>();
        public ObservableCollection<PersonModel> EveryPerson { get; } = new ObservableCollection<PersonModel>();

        //        public void LoadEveryone()
        //        {
        //            var puroks = _repository.Purok.GetRange();
        //            foreach (var purok in puroks)
        //            {
        //                TempPurokList.Add(new PurokModel(purok));
        //            }
        //
        //            foreach (var item in TempPurokList)
        //            {
        //                var households = _repository.Household.GetRange(c => c.PurokNo == item.Model.PurokNo);
        //                foreach (var household in households)
        //                {
        //                    var persons = _repository.Person.GetRange(c => c.HouseNo == household.HouseNo);
        //                    foreach (var person in persons)
        //                    {
        //                        var selHousehold = _repository.Household.Get(c => c.HouseNo == person.HouseNo);
        //                        person.Household = selHousehold;
        //                        var selPurok = _repository.Purok.Get(c => c.PurokNo == selHousehold.PurokNo);
        //                        person.Household.Purok = selPurok;
        //                        EveryPerson.Add(new PersonModel(person, _repository));
        //                    }
        //                }
        //            }
        //        }

        public void LoadEveryone()
        {
            var puroks = _repository.Purok.GetRange();
            foreach (var purok in puroks)
            {
                TempPurokList.Add(new PurokModel(purok));
            }


            foreach (var item in TempPurokList)
            {
                var households = _repository.Household.GetRange(c => c.PurokNo == item.Model.PurokNo);
                foreach (var household in households)
                {
                    var persons = _repository.Person.GetRange(c => c.HouseNo == household.HouseNo);
                    foreach (var person in persons)
                    {
                        var selPurok = _repository.Purok.Get(c => c.PurokNo == person.PurokNo);
                        var personmodel = new PersonModel(person, _repository);
                        personmodel.Purok = new PurokModel(selPurok, _repository);
                        EveryPerson.Add(personmodel);
                    }
                }
            }
        }
        private PurokModel _selectedPurok;

        public PurokModel SelectedPurok
        {
            get { return _selectedPurok; }
            set
            {
                _selectedPurok?.CancelEditCommand.Execute(null);
                _selectedPurok = value;
                if (value != null)
                {
                    _selectedPurok.LoadRelatedInfo();
                }
                RaisePropertyChanged(nameof(SelectedPurok));
            }
        }

        public ICommand PrintPurokSelection => new RelayCommand(PrintPurokSelectionProc);
        PurokPrintSelection _purokPrintSelection;
        public SingleInstanceWindowViewer<AllPurokPersonsReportWindow> _allPurokPersonsWindow = new SingleInstanceWindowViewer<AllPurokPersonsReportWindow>();
        private void PrintPurokSelectionProc()
        {
//            _purokPrintSelection = new PurokPrintSelection();
//            _purokPrintSelection.Show();

//            _allPurokPersonsWindow.Show();
            


        }

        public bool IsIndividualPurokSelected
        {
            get { return _isIndividualPurokSelected; }
            set
            {
                _isIndividualPurokSelected = value;
                RaisePropertyChanged(nameof(IsIndividualPurokSelected));
            }
        }

        public ICommand CancelPurokCommand => new RelayCommand(CancelPurokProc);

        private void CancelPurokProc()
        {
            NewPurokModel?.Dispose();
            _addPurokWindow.Close();
        }

        public ICommand AddPurokCommand => new RelayCommand(AddPurokProc);

        private AddPurokWindow _addPurokWindow;

        private void AddPurokProc()
        {
            NewPurokModel?.Dispose();
            NewPurokModel = new NewPurokModel();
            _addPurokWindow = new AddPurokWindow {Owner = Application.Current.MainWindow};
            _addPurokWindow.Show();
        }


        public ICommand SavePurokCommand => new RelayCommand(SavePurokProc, SavePurokCondition);

        private bool SavePurokCondition()
        {
            
            return (NewPurokModel != null) && NewPurokModel.HasChanges && !NewPurokModel.HasErrors;

        }

        private void SavePurokProc()
        {
            if (NewPurokModel == null) return;
            if (!NewPurokModel.HasChanges) return;

            try
            {
                _repository.Purok.Add(NewPurokModel.ModelCopy);
                var purokmodel = new PurokModel(NewPurokModel.ModelCopy, _repository);
                purokmodel.LoadRelatedInfo();
                PurokList.Add(purokmodel);
                _addPurokWindow.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Add Purok");
            }
        }

        public ICommand DeletePurokCommand => new RelayCommand(DeletePurokProc, DeletePurokCondition);

        private bool DeletePurokCondition()
        {
            if (SelectedPurok == null)
            {
                return false;
            }
            return true;
        }

        private void DeletePurokProc()
        {
            var value = MessageBox.Show("Are you sure you want to delete this purok?", "Purok", MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (value == MessageBoxResult.Yes)
            {
                var households = _repository.Household.GetRange(c => c.PurokNo == SelectedPurok.Model.PurokNo);
                if (households.Count > 0)
                {
                    MessageBox.Show("Purok with households cannot be deleted");
                }
                else
                {
                    _repository.Purok.Remove(SelectedPurok.Model);
                    PurokList.Remove(SelectedPurok);
                }
            }

            else
            {
                return;
            }
        }

        public ICommand PrintPurokCommand => new RelayCommand(PrintPurokProc);

        private SingleInstanceWindowViewer<PurokReportWindow> _purokPrintWindow =
            new SingleInstanceWindowViewer<PurokReportWindow>();

        private SingleInstanceWindowViewer<PopulationReportWindow> _printAssociatedPersons =
            new SingleInstanceWindowViewer<PopulationReportWindow>();

        private void PrintPurokProc()
        {
//            _purokPrintWindow.Show();

            _allPurokPersonsWindow.Show();
        }

        public ICommand PrintPurokPersonsCommand => new RelayCommand(PrintPurokPersonsProc);

        private void PrintPurokPersonsProc()
        {
            _printAssociatedPersons.Show();
        }

        private HouseholdNewModel _newHousehold;

        public HouseholdNewModel NewHousehold
        {
            get { return _newHousehold; }
            set
            {
                _newHousehold = value;
                RaisePropertyChanged(nameof(NewHousehold));
            }
        }

        private PersonNewModel _newPerson;

        public PersonNewModel NewPerson
        {
            get { return _newPerson; }
            set
            {
                _newPerson = value;
                RaisePropertyChanged(nameof(NewPerson));
            }
        }

        private NewPurokModel _newPurokModel;
        private string _searchPurok;
        private Person _purokHeadDisplay;
        private bool _isIndividualPurokSelected;


        public NewPurokModel NewPurokModel
        {
            get { return _newPurokModel; }
            set
            {
                _newPurokModel = value;
                RaisePropertyChanged(nameof(NewPurokModel));
            }
        }

        public Person PurokHeadDisplay
        {
            get { return _purokHeadDisplay; }
            set
            {
                _purokHeadDisplay = value;
                RaisePropertyChanged(nameof(PurokHeadDisplay));
            }
        }

        private void LoadPurok()
        {
            var puroks = _repository.Purok.GetRange();
            foreach (var purok in puroks)
            {
                var purokmodel = new PurokModel(purok, _repository);
                purokmodel.LoadRelatedInfo();
                PurokList.Add(purokmodel);
            }
        }

        public string SearchPurok
        {
            get { return _searchPurok; }
            set
            {
                _searchPurok = value;
                RaisePropertyChanged(nameof(SearchPurok));
                var viewPurokList = CollectionViewSource.GetDefaultView(PurokList);
                if (string.IsNullOrWhiteSpace(SearchPurok))
                {
                    viewPurokList.Filter = null;
                }
                else
                {
                    viewPurokList.Filter =
                        obj => ((PurokModel) obj).Model.PurokName.ToLower().Contains(SearchPurok.ToLower());
                }
            }
        }
    }
}