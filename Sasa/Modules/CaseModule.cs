using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Case;
using Sasa.Models.Household;
using Sasa.Reports.Case;
using Sasa.View.Cases;

namespace Sasa.Modules
{
    public class CaseModule : ObservableObject
    {
        private IRepository _repository;

        public CaseModule(IRepository repository)
        {
            _repository = repository;
            LoadCases();
            InitializeView();
        }

        private void LoadCases()
        {
            var cases = _repository.Case.GetRange();
            foreach (var singleCase in cases)
            {
                var casemodel = new CaseModel(singleCase, _repository);
                casemodel.LoadRelatedInfo();
                Cases.Add(casemodel);
            }
        }

        public ObservableCollection<CaseModel> Cases { get; } = new ObservableCollection<CaseModel>();
        private CaseModel _selectedCase;
        private NewCaseModel _newCase;

        public CaseModel SelectedCase
        {
            get { return _selectedCase; }
            set
            {
                _selectedCase = value;
                if (_selectedCase != null)
                {
                    LoadRespondent();
                }

                RaisePropertyChanged(nameof(SelectedCase));
            }
        }

        private void LoadRespondent()
        {
            var respondent = _repository.Person.Get(c => c.PersonId == SelectedCase.Model.Respondent);
            getRespondent = respondent;
        }

        public Person getRespondent
        {
            get { return _getRespondent; }
            set
            {
                _getRespondent = value;
                RaisePropertyChanged(nameof(getRespondent));
            }
        }

        public NewCaseModel NewCase
        {
            get { return _newCase; }
            set
            {
                _newCase = value;
                RaisePropertyChanged(nameof(NewCase));
            }
        }


        public AddCaseWindow _AddCaseWindow;
        private Person _getRespondent;

        public ICommand AddCaseCommand => new RelayCommand(AddCaseProc);
        public ICommand CancelCaseCommand => new RelayCommand(CancelProc);
        public ICommand SaveCaseCommand => new RelayCommand(SaveCaseProc, SaveCaseCondition);
        public ICommand DeleteCaseCommand => new RelayCommand(DeleteCaseProc, DeleteCaseCondition);

        private void DeleteCaseProc()
        {
            var value = MessageBox.Show("Are you sure you want to delete?", "", MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (value == MessageBoxResult.Yes)
            {
                var getPerson = _repository.Person.Get(c => c.PersonId == SelectedCase.Model.Respondent);
                getPerson.HasCases = false;
                _repository.Person.Update(getPerson);
                _repository.Case.Remove(SelectedCase.Model);
                Cases.Remove(SelectedCase);
            }

            else
            {
                return;
            }
        }

        private bool DeleteCaseCondition()
        {
            if (SelectedCase == null) return false;
            return true;
        }

        private void SaveCaseProc()
        {
            if (NewCase == null) return;
            if (!NewCase.HasChanges) return;

            try
            {
                if (NewCase.ModelCopy.Respondent != null)
                {
                    var getPerson = _repository.Person.Get(c => c.PersonId == NewCase.ModelCopy.Respondent);
                    getPerson.HasCases = true;
                    _repository.Person.Update(getPerson);
                }

                NewCase.ModelCopy.CaseStatus = "Unsettled";
                _repository.Case.Add(NewCase.ModelCopy);
                Cases.Add(new CaseModel(NewCase.ModelCopy, _repository));


                _AddCaseWindow.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save!", "Case");
            }
        }

        private bool SaveCaseCondition()
        {
            return (NewCase != null) && NewCase.HasChanges && !NewCase.HasErrors;
        }

        private void CancelProc()
        {
            NewCase?.Dispose();
            _AddCaseWindow.Close();
        }

        private void AddCaseProc()
        {
            NewCase = new NewCaseModel();

            _AddCaseWindow = new AddCaseWindow();
            _AddCaseWindow.Owner = Application.Current.MainWindow;
            _AddCaseWindow.ShowDialog();
        }


        public ICommand PrintCaseCommand => new RelayCommand(PrintCaseProc);

        private SingleInstanceWindowViewer<CaseReportWindow> _printCaseWindow =
            new SingleInstanceWindowViewer<CaseReportWindow>();

        private void PrintCaseProc()
        {
            _printCaseWindow.Show();
        }

        //        private string _searchCase;
        //
        //        public string SearchCase
        //        {
        //            get { return _searchCase; }
        //            set
        //            {
        //                _searchCase = value;
        //                RaisePropertyChanged(nameof(SearchCase));
        //                var caseList = CollectionViewSource.GetDefaultView(ViewModelLocatorStatic.Locator.CaseModule.Cases);
        //
        //                if (string.IsNullOrWhiteSpace(SearchCase))
        //                {
        //                    caseList.Filter = null;
        //                }
        //                else
        //                {
        //                    caseList.Filter = obj =>
        //                    {
        //                        var cm = obj as CaseModel;
        //                        var sc = SearchCase.ToLowerInvariant();
        //                        if (cm == null) return false;
        //                        return cm.Model.CaseId.ToLowerInvariant().Contains(sc) ||
        //                               cm.Model.CaseName.ToLowerInvariant().Contains(sc) ||
        //                               cm.Respondent.Model.LastName.ToLowerInvariant().Contains(sc);
        //                    };
        //                }
        //            }

        private ListCollectionView _view;
        private string _searchcase;

        public string SearchCase
        {
            get { return _searchcase; }
            set
            {
                _searchcase = value;
                Filter();
                RaisePropertyChanged(nameof(SearchCase));
            }
        }

        private void Filter()
        {
            _view.Filter = Filter;
        }

        private bool Filter(object o)
        {
            var item = o as CaseModel;
            try
            {
                var sc = _searchcase.ToLowerInvariant();
                if (item.Model.CaseId.ToLowerInvariant().Contains(sc)
                    || item.Model.CaseName.ToLowerInvariant().Contains(sc)
                    || item.Model.CaseDescription.ToLowerInvariant().Contains(sc)
                    || item.Respondent.Model.FirstName.ToLowerInvariant().Contains(sc)
                    || item.Respondent.Model.LastName.ToLowerInvariant().Contains(sc)
                    )

                    return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }

        private void InitializeView()
        {
            _view = CollectionViewSource.GetDefaultView(Cases) as ListCollectionView;
            _view?.SortDescriptions.Clear();
        }
    }
}

