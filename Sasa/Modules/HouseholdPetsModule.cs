using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Case;
using Sasa.Models.Livestock;
using Sasa.Reports.Livestock;
using Sasa.View.Population.Household.Pets;

namespace Sasa.Modules
{
    public class HouseholdPetsModule : ObservableObject
    {
        private IRepository _repository;

        public HouseholdPetsModule(IRepository repository)
        {
            _repository = repository;
            LoadPet();
        }

        private void LoadPet()
        {
            var pets = _repository.Livestock.GetRange();
            foreach (var pet in pets)
            {
                LivestockList.Add(new LivestockModel(pet, _repository));
            }
        }

        public ObservableCollection<LivestockModel> LivestockList { get; } = new ObservableCollection<LivestockModel>();

        private LivestockNewModel _livestockNewModel;

        public LivestockNewModel LivestockNewModel
        {
            get { return _livestockNewModel; }
            set
            {
                _livestockNewModel = value;
                RaisePropertyChanged(nameof(LivestockNewModel));
            }
        }


        private LivestockModel _selectedLivestock;

        public LivestockModel SelectedLivestock
        {
            get { return _selectedLivestock; }
            set
            {
                _selectedLivestock?.CancelEditCommand.Execute(null);
                _selectedLivestock = value;
                RaisePropertyChanged(nameof(SelectedLivestock));
            }
        }

        public ICommand AddPetCommand => new RelayCommand(AddPetProc, AddPetCondition);

        private bool AddPetCondition()
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

        public ICommand CancelPetCommand => new RelayCommand(CancelPetProc);
        public ICommand SavePetCommand => new RelayCommand(SavePetProc, SavePetCondition);
        public ICommand DeletePetCommand => new RelayCommand(DeletPetProc, DeletePetCondition);

        private void DeletPetProc()
        {
            var value = MessageBox.Show("Are you sure you want to delete?", "", MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (value == MessageBoxResult.Yes)
            {
                _repository.Livestock.Remove(SelectedLivestock.Model);
                ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Livestocks.Remove(SelectedLivestock);
            }

            else
            {
                return;
            }
        }

        private bool DeletePetCondition()
        {
            if (SelectedLivestock == null) return false;
            return true;
        }

        private bool SavePetCondition()
        {
            return (LivestockNewModel != null) && LivestockNewModel.HasChanges;
        }

        private void SavePetProc()
        {
            if (LivestockNewModel == null) return;
            if (!LivestockNewModel.HasChanges) return;
            try
            {
                LivestockNewModel.ModelCopy.Quantity = GetLivestockQuantity();
                LivestockNewModel.ModelCopy.HouseNo =
                    ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Model.HouseNo;
                _repository.Livestock.Add(LivestockNewModel.ModelCopy);
                ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Livestocks.Add(
                    new LivestockModel(LivestockNewModel.ModelCopy, _repository));
                _addPetWindow.Close();
                //LivestockNewModel.Pets.Remove(LivestockNewModel.ModelCopy.Animal);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Add Household");
            }
        }

        private SingleInstanceWindowViewer<AllPurokLivestockWindow> _allLivestockWindow =
            new SingleInstanceWindowViewer<AllPurokLivestockWindow>();

        public ICommand PrintPurokAllPetCommand => new RelayCommand(PrintPurokAllPetProc);

        private void PrintPurokAllPetProc()
        {
            _allLivestockWindow.Show();
        }

        private int GetLivestockQuantity()
        {
            int male = Convert.ToInt32(LivestockNewModel.ModelCopy.Male);
            int female = Convert.ToInt32(LivestockNewModel.ModelCopy.Female);
           
            return male+female;
        }

        private void CancelPetProc()
        {
            LivestockNewModel?.Dispose();
            _addPetWindow.Close();
        }

        private AddPetWindow _addPetWindow;

        private void AddPetProc()
        {
            LivestockNewModel = new LivestockNewModel();
            _addPetWindow = new AddPetWindow();
            _addPetWindow.Owner = Application.Current.MainWindow;
            _addPetWindow.ShowDialog();
        }

        private string _searchPet;

        public string SearchPet
        {
            get { return _searchPet; }
            set
            {
                _searchPet = value;
                RaisePropertyChanged(nameof(SearchPet));
                var petList =
                    CollectionViewSource.GetDefaultView(
                        ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Livestocks);

                if (string.IsNullOrWhiteSpace(SearchPet))
                {
                    petList.Filter = null;
                }
                else
                {
                    petList.Filter = obj =>
                    {
                        var cm = obj as LivestockModel;
                        var sc = SearchPet.ToLowerInvariant();
                        if (cm == null) return false;
                        return cm.Model.Animal.ToLowerInvariant().Contains(sc)
                            ;

                    };
                }
            }


        }
    }
}
