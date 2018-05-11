using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Models.Clearance;
using Sasa.Models.Person;
using Sasa.View.Clearance;

namespace Sasa.Modules
{
    public class ClearanceModule : ObservableObject
    {
        private IRepository _repository;
        private ClearanceModel _selectedClearance;
        private NewClearanceModel _newClearance;


        public ClearanceModule(IRepository repository)
        {
            _repository = repository;
        }

        public ClearanceModel SelectedClearance
        {
            get { return _selectedClearance; }
            set
            {
                _selectedClearance = value;
                if (value != null)
                {
                    LoadClearanceName();
                }

                RaisePropertyChanged(nameof(SelectedClearance));
            }
        }
        private DataAccess.EF.Person _loadedPerson;
        private void LoadClearanceName()
        {
            var person = _repository.Person.Get(c => c.PersonId == ViewModelLocatorStatic.Locator.ClearanceModule.SelectedClearance.Model.PersonId);
            LoadedPerson = person;

        }

        public DataAccess.EF.Person LoadedPerson
        {
            get { return _loadedPerson; }
            set
            {
                _loadedPerson = value;
                RaisePropertyChanged(nameof(LoadedPerson));
            }
        }

        public NewClearanceModel NewClearance
        {
            get { return _newClearance; }
            set
            {
                _newClearance = value;
                RaisePropertyChanged(nameof(NewClearance));
            }
        }

        public ICommand DeleteClearanceCommand => new RelayCommand(DeleteClearanceProc, DeleteClearanceCondition);

        private void DeleteClearanceProc()
        {
            var value = MessageBox.Show("Are you sure you want to delete?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (value == MessageBoxResult.Yes)
            {

                _repository.Clearance.Remove(SelectedClearance.Model);
                ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Clearance.Remove(SelectedClearance);

            }

            else
            {
                return;
            }
        }

        private bool DeleteClearanceCondition()
        {
            if (SelectedClearance == null) return false;
            return true;

        }

        public ICommand AddClearanceCommand => new RelayCommand(AddClearanceProc, AddClearanceCondition);

        private bool AddClearanceCondition()
        {
            if (ViewModelLocatorStatic.Locator.HouseholdProfileModule.SelectedPurok != null)
            {
                if (ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold != null)
                {
                    return true;
                }
            }
            return false;
        }

        public ICommand CancelClearanceCommand => new RelayCommand(CancelProc);
        public ICommand SaveClearanceCommand => new RelayCommand(SaveClearanceProc, SaveClearanceCondition);
        public AddClearanceWindow _addClearanceWindow;

        private bool SaveClearanceCondition()
        {
            return (NewClearance != null) && NewClearance.HasChanges;
        }

        private void SaveClearanceProc()
        {
            if (NewClearance == null) return;
            if (!NewClearance.HasChanges) return;

            try
            {
                NewClearance.ModelCopy.HouseNo =
                    ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Model.HouseNo;
                NewClearance.ModelCopy.IsDone = false;
                var clearancemodel = new ClearanceModel(NewClearance.ModelCopy, _repository);
                clearancemodel.LoadRelatedInfo();
                _repository.Clearance.Add(clearancemodel.Model);
                ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Clearance.Add(clearancemodel);

                _addClearanceWindow.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Case");
            }

            SelectedClearance = SelectedClearance;

        }

        private void CancelProc()
        {
            NewClearance?.Dispose();
            _addClearanceWindow.Close();
        }

        private void AddClearanceProc()
        {
            NewClearance = new NewClearanceModel();
            _addClearanceWindow = new AddClearanceWindow();
            _addClearanceWindow.Owner = Application.Current.MainWindow;
            _addClearanceWindow.ShowDialog();
        }
    }
}