using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Models.Household;
using Sasa.Models.Livestock;
using Sasa.Models.Purok;
using Sasa.View.Population.Household;
using Sasa.View.Population.Household.Family;

namespace Sasa.Modules
{
    public class HouseholdProfileModule : ObservableObject
    {
        private IRepository _repository;
        
        public HouseholdProfileModule(IRepository repository)
        {
            _repository = repository;
         
        }

        public ObservableCollection<HouseholdModel> HouseholdList { get; } = new ObservableCollection<HouseholdModel>();

        private HouseholdModel _selectedHousehold;

        public HouseholdModel SelectedHousehold
        {
            get { return _selectedHousehold; }
            set
            {
                _selectedHousehold?.CancelEditCommand.Execute(null);
                _selectedHousehold = value;
                if (value != null)
                {
                    _selectedHousehold.LoadRelatedInfo();
                }
                RaisePropertyChanged(nameof(SelectedHousehold));
            }
        }

        public PurokModel SelectedPurok
        {
            get { return _selectedPurok; }
            set
            {
                _selectedPurok = value;
                if (value != null)
                {
                    //_selectedPurok.Households.Clear();
                    _selectedPurok.LoadRelatedInfo();
                    InitializeView();
                }

                RaisePropertyChanged(nameof(SelectedPurok));
                
            }
        }
      
        public ObservableCollection<HouseholdNewModel> NewHouseholds { get; } = new ObservableCollection<HouseholdNewModel>();
        //private List<Household> Households { get; }= new List<Household>();
       
        public ICommand CancelHouseholdCommand => new RelayCommand(CancelProc);

        private void CancelProc()
        {
            foreach (var household in NewHouseholds)
            {
                household?.Dispose();
            }
            _addHouseholdWindow.Close();
        }

        public ICommand AddMoreHouseholdCommand => new RelayCommand(AddMoreHouseholdProc);

        private void AddMoreHouseholdProc()
        {
            NewHouseholds.Add(new HouseholdNewModel
            {
                PurokNo = SelectedPurok.Model.PurokNo
            });
        }

        public ICommand AddHouseholdCommand => new RelayCommand(AddHouseholdProc, AddHouseholdCondition);
        public ICommand DeleteHouseholdCommand => new RelayCommand(DeleteHouseholdProc, DeleteHouseholdCondition);

        private void DeleteHouseholdProc()
        {
            var value = MessageBox.Show("Are you sure you want to delete?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (value == MessageBoxResult.Yes)
            {
                var persons = _repository.Person.GetRange(c => c.HouseNo == ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Model.HouseNo);
                if (persons.Count > 0)
                {
                    MessageBox.Show("Household with members cannot be deleted");
                }
                else
                {
                    _repository.Household.Remove(ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Model);
                    SelectedPurok.Households.Remove(ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold);
                }

            }

            else
            {
                return;
            }
        }

        private bool DeleteHouseholdCondition()
        {
            if (SelectedPurok != null)
            {
                if (ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold != null)
                {
                    return true;
                }
            }
            return false;
        }

        private bool AddHouseholdCondition()
        {
            return (SelectedPurok != null);
        }

        private AddHouseholdWindow _addHouseholdWindow;
        private PurokModel _selectedPurok;
        private LivestockModel _selectedLivestock;

        private void AddHouseholdProc()
        {
            NewHouseholds.Clear();
            foreach (var household in NewHouseholds)
            {
                household?.Dispose();
            }
            NewHouseholds.Add(new HouseholdNewModel
            {
                PurokNo = SelectedPurok.Model.PurokNo
            });

            _addHouseholdWindow = new AddHouseholdWindow {Owner = Application.Current.MainWindow};
            _addHouseholdWindow.Show();
        }

        public ICommand SaveHouseholdCommand => new RelayCommand(SaveHouseholdProc, SaveHouseholdCondition);

        private bool SaveHouseholdCondition()
        {
            return (NewHouseholds != null);
        }

        private void SaveHouseholdProc()
        {
            if (NewHouseholds == null) return;

            foreach (var household in NewHouseholds.Where(household => household != null))
            {            

                try
                {
                    _repository.Household.Add(household.ModelCopy);
                    HouseholdList.Add(new HouseholdModel(household.ModelCopy, _repository));
                    SelectedPurok.Households.Add(new HouseholdModel(household.ModelCopy, _repository));

                }
                catch (Exception e)
                {
                    MessageBox.Show("Unable to Save. Household number already exists", "Add Households");
                }

            }

            _addHouseholdWindow.Close();
        }

        //        private string _searchHousehold;
        //        public string SearchHousehold
        //        {
        //            get { return _searchHousehold; }
        //            set{
        //                _searchHousehold = value;
        //                RaisePropertyChanged(nameof(SearchHousehold));
        //                var householdList = CollectionViewSource.GetDefaultView(SelectedPurok.Households);
        //            
        //                if (string.IsNullOrWhiteSpace(SearchHousehold))
        //                {
        //                    householdList.Filter = null;
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        householdList.Filter = obj =>
        //                        {
        //                            var hm = obj as HouseholdModel;
        //                            var sh = SearchHousehold.ToLowerInvariant();
        //                            if (hm == null) return false;
        //                            return hm.Model.HouseNo.ToLowerInvariant().Contains(sh) || 
        //                                   hm.Model.Address.ToLowerInvariant().Contains(sh);
        //
        //                        };
        //
        //
        //                    }
        //                    catch (Exception)
        //                    {
        //                        
        //                      
        //                    }
        //                   
        //                        
        //                  
        //
        //                }
        //            }
        //        }

        private ListCollectionView _view;
        private string _searchhousehold;

        public string SearchHousehold
        {
            get { return _searchhousehold; }
            set
            {
                _searchhousehold = value;
                Filter();
                RaisePropertyChanged(nameof(SearchHousehold));
            }
        }

        private void Filter()
        {
            _view.Filter = Filter;
        }

        private bool Filter(object o)
        {
            var item = o as HouseholdModel;
            try
            {
                var sc = _searchhousehold.ToLowerInvariant();
                if (item.Model.HouseNo.ToLowerInvariant().Contains(sc)
                || item.Model.Address.ToLowerInvariant().Contains(sc)
                || item.Members.Any(c => c.Model.LastName.ToLowerInvariant().Contains(sc) ||
                                         c.Model.FirstName.ToLowerInvariant().Contains(sc))
                )              
             
                    return true;
            }
            catch (Exception e) { }

            return false;
        }

        private void InitializeView()
        {
            _view = CollectionViewSource.GetDefaultView(SelectedPurok?.Households) as ListCollectionView;
            _view?.SortDescriptions.Clear();
        }
    }
}
