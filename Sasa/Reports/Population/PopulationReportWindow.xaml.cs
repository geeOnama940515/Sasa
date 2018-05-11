using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Reporting.WinForms;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Purok;
using Sasa.Modules;

namespace Sasa.Reports.Population
{
    /// <summary>
    ///     Interaction logic for PopulationReportWindow.xaml
    /// </summary>
    public partial class PopulationReportWindow : Window
    {

        private readonly ReportViewBuilder _reportView;
        private string _selectedAgeOption = "Lesser than or equal to";
        private string _selectedGender = "All";
        private PurokModel _selectedPurok;
        private readonly List<DataSetValuePair> _sources = new List<DataSetValuePair>();
        private string _titleFilter = string.Empty;
        private int? _startAge = 0;
        private int? _endAge = 99;
        private bool _selectedStatus;
        private bool _isAgeChecked = true ;
        private bool _isGenderChecked = true;
        private bool _isAttainmentChecked = true;
        private bool _isOccupationChecked = true;
        private bool _isReligionChecked = true;
        private bool _isTribeChecked = true;
        private bool _isVoter;
        private bool _hasCert;
        ReportParameter[] parameters = new ReportParameter[11];
        private string _occupation = string.Empty;
        private bool _isDateOfBirthChecked = false;
        private bool _isDateOfDeathChecked = false;
        private bool _isVoterChecked;
        private bool _isDeadChecked;
        private bool _isBirthCertChecked;

        public PopulationReportWindow()
        {
            InitializeComponent();
            
            _reportView = new ReportViewBuilder("Sasa.Reports.Population.PopulationReport.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;

            SetParameters();
        }

        private void SetParameters()
        {
            parameters[0] = new ReportParameter("AgeVisibility", _isAgeChecked.ToString());
            parameters[1] = new ReportParameter("GenderVisibility", _isGenderChecked.ToString());
            parameters[2] = new ReportParameter("AttainmentVisibility", _isAttainmentChecked.ToString());
            parameters[3] = new ReportParameter("OccupationVisibility", _isOccupationChecked.ToString());
            parameters[4] = new ReportParameter("TribeVisibility", _isTribeChecked.ToString());
            parameters[5] = new ReportParameter("ReligionVisibility", _isReligionChecked.ToString());
            parameters[6] = new ReportParameter("DateOfBirthVisibility", _isDateOfBirthChecked.ToString());
            parameters[7] = new ReportParameter("DateOfDeathVisibility", _isDateOfDeathChecked.ToString());
            parameters[8] = new ReportParameter("VoterVisibility", _isVoterChecked.ToString());
            parameters[9] = new ReportParameter("DeadVisibility", _isDeadChecked.ToString());
            parameters[10] = new ReportParameter("CertificationVisibility", _isBirthCertChecked.ToString());

            _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
        }

        public ObservableCollection<string> GenderList { get; } = new ObservableCollection<string>
        {
            "Male",
            "Female",
            "All"
        };

        public string Occupation
        {
            get { return _occupation; }
            set
            {
                _occupation = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public int? StartAge
        {
            get { return _startAge; }
            set
            {
                _startAge = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public int? EndAge
        {
            get { return _endAge; }
            set
            {
                _endAge = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public string SelectedGender
        {
            get { return _selectedGender; }
            set
            {
                _selectedGender = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public bool SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());

            }
        }

        public bool IsVoter
        {
            get { return _isVoter; }
            set
            {
                _isVoter = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        public bool HasCert
        {
            get { return _hasCert; }
            set
            {
                _hasCert = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var sources = new List<DataSetValuePair>();
            var population = ViewModelLocatorStatic.Locator.PurokModule.SelectedPurok;

            var selectedPurok = new PurokDataSetModel
            {
                PurokName = population.Model?.PurokName,
                Location = population.Model?.Location,
                Population = population.NumberOfPersons,
                PurokHead = population.PurokHead?.Model.FirstName + " "
                          + population.PurokHead?.Model.MiddleName + " "
                          + population.PurokHead?.Model.LastName + " " 
            };
            var persons = new ObservableCollection<PopulationDataSetModel>();
          
            foreach (var person in population.Constituents)
            {

                persons.Add(new PopulationDataSetModel
                {
                    Religion = person.Model?.Religion,
                    Organization = person.Model?.Organization,
                    IsHead = Convert.ToBoolean(person.Model?.IsHead), 
                    Occupation = person.Model?.Occupation,
                    DateOfBirth = Convert.ToDateTime(person.Model?.Birthdate), 
                    DateOfDeath = person.Model?.DateOfDeath?.ToString("MM/dd/yy"),
                    Attainment = person.Model?.Attainment,
                    Tribe = person.Model?.Tribe,
                    Gender = person.Model?.Gender,
                    IsDead = person.Model?.IsDead,
                    Age = Convert.ToInt32(person.Model?.Age),
                    Name = person.Model?.FirstName + " " 
                        + person.Model?.MiddleName + " "
                        + person.Model?.LastName + " ",
                    HouseNo = person.Model?.HouseNo,
                    IsVoter = person.Model?.IsVoter,
                    HasCert = person.Model?.HasBirthCert
                });
            }

            if (!string.IsNullOrWhiteSpace(Occupation) || !string.IsNullOrEmpty(Occupation))
            {
                try
                {
                    var personCollection = new ObservableCollection<PopulationDataSetModel>();

                    foreach (var person in persons)
                    {
                        if (!string.IsNullOrWhiteSpace(person.Occupation) || !string.IsNullOrEmpty(person.Occupation))
                        {
                            if (person.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()))
                            {
                                personCollection.Add(person);
                            }
                        }
                    }

                    persons = new ObservableCollection<PopulationDataSetModel>(personCollection);
                }
                catch (Exception e) { }
            }

//            if (Occupation != null)
//            {
//                try
//                {
//                    //var filteredpersons = persons.Where(f => f.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()));
//                    var filteredpersons = persons.Where(f => f.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()));
//                    persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
//                }
//                catch (Exception e) { }
//
//            }

            if (StartAge != null && EndAge != null)
            {
                var filteredpersons = persons.Where(f => f.Age >= StartAge && f.Age <= EndAge);
                persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
            }

            

            if (SelectedGender != null)
            {
                if (SelectedGender == "All")
                {
                    var filteredpersons = persons;
                    persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
                }
                else
                {
                    var filteredpersons = persons.Where(f => f.Gender == SelectedGender);
                    persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
                }
            }

            if (SelectedStatus)
            {
                var filteredpersons = persons.Where(f => f.IsDead == "No");
                persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
            }

            if (IsVoter == true)
            {
                var filteredpersons = persons.Where(f => f.IsVoter == "Yes");
                persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
            }

            if (HasCert == true )
            {
                var filteredpersons = persons.Where(f => f.HasCert == "Yes");
                persons = new ObservableCollection<PopulationDataSetModel>(filteredpersons);
            }

            sources.Add(new DataSetValuePair("PurokDataSet", selectedPurok));
            sources.Add(new DataSetValuePair("PopulationDataSet", persons));

            return sources;
        }

        public bool IsAgeChecked
        {
            get { return _isAgeChecked; }
            set
            {
                _isAgeChecked = value;

                parameters[0] = new ReportParameter("AgeVisibility", _isAgeChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsGenderChecked
        {
            get { return _isGenderChecked; }
            set
            {
                _isGenderChecked = value;

                parameters[1] = new ReportParameter("GenderVisibility", _isGenderChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsAttainmentChecked
        {
            get { return _isAttainmentChecked; }
            set
            {
                _isAttainmentChecked = value;

                parameters[2] = new ReportParameter("AttainmentVisibility", _isAttainmentChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsOccupationChecked
        {
            get { return _isOccupationChecked; }
            set
            {
                _isOccupationChecked = value;

                parameters[3] = new ReportParameter("OccupationVisibility", _isOccupationChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsTribeChecked
        {
            get { return _isTribeChecked; }
            set
            {
                _isTribeChecked = value;

                parameters[4] = new ReportParameter("TribeVisibility", _isTribeChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsReligionChecked
        {
            get { return _isReligionChecked; }
            set
            {
                _isReligionChecked = value;

                parameters[5] = new ReportParameter("ReligionVisibility", _isReligionChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsDateOfBirthChecked
        {
            get { return _isDateOfBirthChecked; }
            set
            {
                _isDateOfBirthChecked = value;

                parameters[6] = new ReportParameter("DateOfBirthVisibility", _isDateOfBirthChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsDateOfDeathChecked
        {
            get { return _isDateOfDeathChecked; }
            set
            {
                _isDateOfDeathChecked = value;

                parameters[7] = new ReportParameter("DateOfDeathVisibility", _isDateOfDeathChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsVoterChecked
        {
            get { return _isVoterChecked; }
            set
            {
                _isVoterChecked = value;
                parameters[8] = new ReportParameter("VoterVisibility", _isVoterChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsDeadChecked
        {
            get { return _isDeadChecked; }
            set
            {
                _isDeadChecked = value;
                parameters[9] = new ReportParameter("DeadVisibility", _isDeadChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();

            }
        }

        public bool IsBirthCertChecked
        {
            get { return _isBirthCertChecked; }
            set
            {
                _isBirthCertChecked = value;
                parameters[10] = new ReportParameter("CertificationVisibility", _isBirthCertChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }
    }
}