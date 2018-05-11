using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Case;
using Sasa.Modules;

namespace Sasa.Reports.Case
{
    /// <summary>
    /// Interaction logic for CaseReportWindow.xaml
    /// </summary>
    public partial class CaseReportWindow : Window
    {
        private readonly ReportViewBuilder _reportView;
        private readonly List<DataSetValuePair> _sources = new List<DataSetValuePair>();
        private string _selectedMonth;
        private string _selectedYear;
        private string _selectedStatus = "All";
        private string _respondent = string.Empty;
        public List<int> YearOptions;
        private string _titleFilter = string.Empty;

        public CaseReportWindow()
        {
            InitializeComponent();
            LoadDays();
            _reportView = new ReportViewBuilder("Sasa.Reports.Case.CaseReport.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;
            
        }

        public ObservableCollection<string> MonthOptions { get; } = new ObservableCollection<string> {"All", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public ObservableCollection<string> DayOptions { get; } = new ObservableCollection<string>();

        private void LoadDays()
        {
            DayOptions.Add("All");
            int day = 1;

            while (day < 32)
            {
                DayOptions.Add(Convert.ToString(day));
                day++;
            }
        }

        private int? _day;
        public int? Day
        {
            get { return _day; }
            set
            {
                _day = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public string SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                _selectedYear = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public string TitleFilter
        {
            get { return _titleFilter; }
            set
            {
                _titleFilter = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public string Respondent
        {
            get { return _respondent; }
            set
            {
                _respondent = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        private ObservableCollection<CaseDataSetModel> _casesDataSet = new ObservableCollection<CaseDataSetModel>();
       
        private void LoadDataSet(ObservableCollection<CaseModel> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            _casesDataSet.Clear();
            foreach (var item in collection)
            {
                var casedatasetmodel = new CaseDataSetModel
                {
                    CaseHearingDate = item.Model.CaseHearingDate?.ToString("MMMM dd, yyy"),
                    Respondent = item.Respondent?.Model?.FirstName + " "
                            + item.Respondent?.Model?.MiddleName + " "
                            + item.Respondent?.Model?.LastName,
                    CaseId = item.Model.CaseId,
                    CaseLevel = item.Model?.CaseLevel,
                    CaseName = item.Model?.CaseName,
                    CaseStatus = item.Model?.CaseStatus,
                    Complainant = item.Model?.Complainant
                };

                if (item.Respondent == null)
                {
                    casedatasetmodel.Respondent = item.Model?.RepondentOpt;
                }

                _casesDataSet.Add(casedatasetmodel);

            }
        }

        public ObservableCollection<string> StatusList { get; } = new ObservableCollection<string>
        {
            "All",
            "Settled",
            "Unsettled"
        };

        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var cases = ViewModelLocatorStatic.Locator.CaseModule.Cases;
            LoadDataSet(cases);

            if (SelectedMonth != null)
            {
                if (SelectedMonth == "All")
                {
                    var filteredcases = cases;
                    cases = new ObservableCollection<CaseModel>(filteredcases);
                }

                else
                {
                    var monthSelected = MonthOptions.IndexOf(SelectedMonth);
                    var filteredcases = cases.Where(f => f.Model.CaseHearingDate?.Month == monthSelected);
                    cases = new ObservableCollection<CaseModel>(filteredcases); 
                }

                LoadDataSet(cases);

            }

            if (SelectedYear != null)
            {
                var year = 0;
                try
                {
                    year = Convert.ToInt32(SelectedYear);
                }
                catch (Exception e)
                {
                    year = DateTime.Now.Year;
                }

                var filteredcases = cases.Where(f => f.Model.CaseHearingDate?.Year == year);
                cases = new ObservableCollection<CaseModel>(filteredcases);
                LoadDataSet(cases);
            }

            if (Day != null)
            { 
                var filteredcases = cases.Where(f => f.Model.CaseHearingDate?.Day == Day);
                cases = new ObservableCollection<CaseModel>(filteredcases);
                LoadDataSet(cases);
            }

            if (SelectedStatus != null)
            {
                if (SelectedStatus == "All")
                {
                    var filteredcases = cases;
                    cases = new ObservableCollection<CaseModel>(filteredcases);
                    LoadDataSet(cases);
                }

                else
                {
                    var filteredpersons = cases.Where(f => f.Model.CaseStatus == SelectedStatus);
                    cases = new ObservableCollection<CaseModel>(filteredpersons);
                    LoadDataSet(cases);
                }
            }

            //            if (Respondent != null)
            //            {
            //                var r = Respondent.ToLowerInvariant();
            //                var filteredpersons =
            //                    cases.Where(f => f.Respondent.Model.FirstName.ToLowerInvariant().Contains(r) ||
            //                    f.Respondent.Model.LastName.ToLowerInvariant().Contains(r)
            //                    );
            //                cases = new ObservableCollection<CaseModel>(filteredpersons);
            //                LoadDataSet(cases);
            //            }

            if (!string.IsNullOrWhiteSpace(Respondent) || !string.IsNullOrEmpty(Respondent))
            {
                try
                {
                    var personCollection = new ObservableCollection<CaseModel>();

                    foreach (var person in cases)
                    {
                        if (!string.IsNullOrWhiteSpace(person.Respondent.ToString()) || !string.IsNullOrEmpty(person.Respondent.ToString()))
                        {
                            if (person.Respondent.Model.FirstName.ToLowerInvariant().Contains(Respondent.ToLowerInvariant()))
                            {
                                personCollection.Add(person);
                            }
                        }
                    }

                    cases = new ObservableCollection<CaseModel>(personCollection);
                    LoadDataSet(cases);
                }
                catch (Exception e) { }
            }

            _sources.Add(new DataSetValuePair("CaseDataSet", _casesDataSet.Where(c => TitleFilter != null && c.CaseId.ToLowerInvariant().Contains(TitleFilter.ToLowerInvariant()) || c.CaseName.ToLowerInvariant().Contains(TitleFilter.ToLowerInvariant()))));
            return _sources;
        }
    }
}
