using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Reporting.WinForms;
using Sasa.Helpers.ReportViewer;
using Sasa.Modules;

namespace Sasa.Reports.Purok
{
    /// <summary>
    /// Interaction logic for AllPurokPersonsReportWindow.xaml
    /// </summary>
    public partial class AllPurokPersonsReportWindow : Window
    {
        private ReportViewBuilder _reportView;
        private string _occupation;
        private int? _startAge = 0;
        private int? _endAge = 99;
        private string _selectedGender;
        private bool _isVoter;
        private bool _hasCert;
        ReportParameter[] parameters = new ReportParameter[10];
        private bool _selectedStatus;
        private bool _isGenderChecked = true;
        private bool _isAgeChecked = true;
        private bool _isAttainmentChecked;
        private bool _isOccupationChecked;
        private bool _isTribeChecked;
        private bool _isDateOfBirthChecked = true;
        private bool _isDateOfDeathChecked;
        private bool _isVoterChecked = true;
        private bool _isDeadChecked;
        private bool _isBirthCertChecked = true;


        public AllPurokPersonsReportWindow()
        {
            InitializeComponent();

            _reportView = new ReportViewBuilder("Sasa.Reports.Purok.AllPurokPersonsReport.rdlc", UpdateDatasetSource());
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
            //parameters[5] = new ReportParameter("ReligionVisibility", _isReligionChecked.ToString());
            parameters[5] = new ReportParameter("DateOfBirthVisibility", _isDateOfBirthChecked.ToString());
            parameters[6] = new ReportParameter("DateOfDeathVisibility", _isDateOfDeathChecked.ToString());
            parameters[7] = new ReportParameter("VoterVisibility", _isVoterChecked.ToString());
            parameters[8] = new ReportParameter("DeadVisibility", _isDeadChecked.ToString());
            parameters[9] = new ReportParameter("CertificationVisibility", _isBirthCertChecked.ToString());

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

            var _sources = new List<DataSetValuePair>();

            var persons = ViewModelLocatorStatic.Locator.PurokModule.EveryPerson;

            var _purokPersons = new ObservableCollection<AllPurokPersons>();


            foreach (var person in persons)
            {
                _purokPersons.Add(new AllPurokPersons
                {
                    
                    PurokName = person.Purok.Model.PurokName,
                    Name = person.Model.FirstName + " " + person.Model.LastName,
                    Age = Convert.ToInt32(person.Model.Age),
                    IsVoter = person.Model.IsVoter,
                    HasCert = person.Model.HasBirthCert,
                    Occupation = person.Model.Occupation,
                    Gender = person.Model.Gender,
                    Attainment = person.Model.Attainment,
                    DateOfBirth = person.Model.Birthdate?.ToString("MM/dd/yy"),
                    DateOfDeath = person.Model.DateOfDeath?.ToString("MM/dd/yy"),
                    IsDead = person.Model.IsDead,
                    Religion = person.Model.Religion,
                    Tribe = person.Model.Tribe
                });
            }

            if (!string.IsNullOrWhiteSpace(Occupation) || !string.IsNullOrEmpty(Occupation))
            {
                try
                {
                    var personCollection = new ObservableCollection<AllPurokPersons>();

                    foreach (var person in _purokPersons)
                    {
                        if (!string.IsNullOrWhiteSpace(person.Occupation) || !string.IsNullOrEmpty(person.Occupation))
                        {
                            if (person.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()))
                            {
                                personCollection.Add(person);
                            }
                        }
                    }

                    _purokPersons = new ObservableCollection<AllPurokPersons>(personCollection);
                }
                catch (Exception e) { }
            }

            if (StartAge != null && EndAge != null)
            {
                var filteredpersons = _purokPersons.Where(f => f.Age >= StartAge && f.Age <= EndAge);
                _purokPersons = new ObservableCollection<AllPurokPersons>(filteredpersons);
            }



            if (SelectedGender != null)
            {
                if (SelectedGender == "All")
                {
                    var filteredpersons = _purokPersons;
                    _purokPersons = new ObservableCollection<AllPurokPersons>(filteredpersons);
                }
                else
                {
                    var filteredpersons = _purokPersons.Where(f => f.Gender == SelectedGender);
                    _purokPersons = new ObservableCollection<AllPurokPersons>(filteredpersons);
                }
            }

            if (IsVoter == true)
            {
                var filteredpersons = _purokPersons.Where(f => f.IsVoter == "Yes");
                _purokPersons = new ObservableCollection<AllPurokPersons>(filteredpersons);
            }

            if (HasCert == true)
            {
                var filteredpersons = _purokPersons.Where(f => f.HasCert == "Yes");
                _purokPersons = new ObservableCollection<AllPurokPersons>(filteredpersons);
            }

            _sources.Add(new DataSetValuePair("AllPurokPersons", _purokPersons));

            return _sources;
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

        public bool IsDateOfBirthChecked
        {
            get { return _isDateOfBirthChecked; }
            set
            {
                _isDateOfBirthChecked = value;

                parameters[5] = new ReportParameter("DateOfBirthVisibility", _isDateOfBirthChecked.ToString());

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

                parameters[6] = new ReportParameter("DateOfDeathVisibility", _isDateOfDeathChecked.ToString());

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
                parameters[7] = new ReportParameter("VoterVisibility", _isVoterChecked.ToString());
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
                parameters[8] = new ReportParameter("DeadVisibility", _isDeadChecked.ToString());
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
                parameters[9] = new ReportParameter("CertificationVisibility", _isBirthCertChecked.ToString());
                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }
    }
}
