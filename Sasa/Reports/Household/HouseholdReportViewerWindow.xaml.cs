using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Reporting.WinForms;
using Sasa.Helpers.ReportViewer;
using Sasa.Modules;
using Sasa.Reports.Population;

namespace Sasa.Reports.Household
{
    /// <summary>
    /// Interaction logic for HouseholdReportViewerWindow.xaml
    /// </summary>
    public partial class HouseholdReportViewerWindow : Window
    {
        private ReportViewBuilder _reportView;
        private bool _selectedStatus;
        private bool _isOccupationChecked;
        private bool _isCivilStatusChecked;
        ReportParameter[] parameters = new ReportParameter[11];
        private bool _isAgeChecked = true;
        private bool _isGenderChecked = true;
        private bool _isRelationshipChecked = true;
        private bool _isBirthdateChecked;
        private bool _isVoterChecked;
        private bool _isHasCertChecked;
        private bool _isAttainmentChecked;
        private bool _isDeadChecked;
        private bool _isDateOfDeathChecked;
        private string _occupation = string.Empty;
        private int? _startAge = 0;
        private int? _endAge = 99;
        private string _selectedGender = "All";
        public ObservableCollection<string> GenderList { get; } = new ObservableCollection<string>
        {
            "Male",
            "Female",
            "All"
        };

        public HouseholdReportViewerWindow()
        {
            InitializeComponent();
            _reportView = new ReportViewBuilder("Sasa.Reports.Household.HouseholdReportViewer.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;

            SetParameters();
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

        private void SetParameters()
        {
            parameters[0] = new ReportParameter("AgeVisibility", _isAgeChecked.ToString());
            parameters[1] = new ReportParameter("GenderVisibility", _isGenderChecked.ToString());
            parameters[2] = new ReportParameter("RelationshipVisibility", _isRelationshipChecked.ToString());
            parameters[3] = new ReportParameter("OccupationVisibility", _isOccupationChecked.ToString());
            parameters[4] = new ReportParameter("BirthdateVisibility", _isBirthdateChecked.ToString());
            parameters[5] = new ReportParameter("VoterVisibility", _isVoterChecked.ToString());
            parameters[6] = new ReportParameter("CertificationVisibility", _isHasCertChecked.ToString());
            parameters[7] = new ReportParameter("AttainmentVisibility", _isAttainmentChecked.ToString());
            parameters[8] = new ReportParameter("CivilStatusVisibility", _isCivilStatusChecked.ToString());
            parameters[9] = new ReportParameter("DeadVisibility", _isDeadChecked.ToString());
            parameters[10] = new ReportParameter("DateOfDeathVisibility", _isDateOfDeathChecked.ToString());

            _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
        }


        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var sources = new List<DataSetValuePair>();
            var household = ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold;
            var members = household.Members;

            var householddataset = new HouseholdDataSetModel
            {
                HouseNo = household.Model.HouseNo,
                PurokNo = household.Purok?.Model.PurokName,
                Address = household.Model?.Address,
                Head = household.HouseholdHead?.Model.FirstName + " "
                    + household.HouseholdHead?.Model.MiddleName + " "
                    + household.HouseholdHead?.Model.LastName
            };

            var membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>();

            foreach (var member in members)
            {
                membersdataset.Add(new HouseholdMembersDataSetModel
                {
                    Gender = member.Model?.Gender,
                    Member = member.Model?.FirstName + " " 
                            + member.Model?.MiddleName + " "
                            + member.Model?.LastName,
                    Age = member.Model.Age,
                    Occupation = member.Model?.Occupation,
                    Attainment = member.Model?.Attainment,
                    Birthdate = member.Model?.Birthdate?.ToString("MM/dd/yy"),
                    CivilStatus = member.Model?.CivilStatus,
                    Relationship = member.Model?.Relationship,
                    IsVoter = member.Model.IsVoter,
                    HasCert = member.Model.HasBirthCert,
                    IsDead = member.Model.IsDead,
                    DateOfDeath = member.Model.DateOfDeath?.ToString("MM/dd/yy"),
                });
            }

            if (!string.IsNullOrWhiteSpace(Occupation) || !string.IsNullOrEmpty(Occupation))
            {
                try
                {
                    var personCollection = new ObservableCollection<HouseholdMembersDataSetModel>();

                    foreach (var member in membersdataset)
                    {
                        if (!string.IsNullOrWhiteSpace(member.Occupation) || !string.IsNullOrEmpty(member.Occupation))
                        {
                            if (member.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()))
                            {
                                personCollection.Add(member);
                            }
                        }
                    }

                    membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>(personCollection);
                }
                catch (Exception e) { }
            }

            if (SelectedGender != null)
            {
                if (SelectedGender == "All")
                {
                    var filteredpersons = membersdataset;
                    membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>(filteredpersons);
                }
                else
                {
                    var filteredpersons = membersdataset.Where(f => f.Gender == SelectedGender);
                    membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>(filteredpersons);
                }
            }

            if (SelectedStatus)
            {
                var filteredpersons = membersdataset.Where(f => f.IsDead == "No");
                membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>(filteredpersons);
            }

            //            if (Occupation != null)
            //            {
            //                try
            //                {
            //                    //var filteredpersons = persons.Where(f => f.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()));
            //                    var filteredpersons = membersdataset.Where(f => f.Occupation.ToLowerInvariant().Contains(Occupation.ToLowerInvariant()));
            //                    membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>(filteredpersons);
            //                }
            //                catch (Exception e) { }
            //
            //            }

            if (StartAge != null && EndAge != null)
            {
                var filteredpersons = membersdataset.Where(f => Convert.ToInt32(f.Age) >= StartAge && Convert.ToInt32(f.Age) <= EndAge);
                membersdataset = new ObservableCollection<HouseholdMembersDataSetModel>(filteredpersons);
            }



            sources.Add(new DataSetValuePair("HouseholdDataSet", householddataset));
            sources.Add(new DataSetValuePair("HouseholdMemberDataSet", membersdataset));
            
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

        public bool IsRelationshipChecked
        {
            get { return _isRelationshipChecked; }
            set
            {
                _isRelationshipChecked = value;

                parameters[2] = new ReportParameter("RelationshipVisibility", _isRelationshipChecked.ToString());

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

        public bool IsBirthdateChecked
        {
            get { return _isBirthdateChecked; }
            set
            {
                _isBirthdateChecked = value;

                parameters[4] = new ReportParameter("BirthdateVisibility", _isBirthdateChecked.ToString());

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

                parameters[5] = new ReportParameter("VoterVisibility", _isVoterChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsHasCertChecked
        {
            get { return _isHasCertChecked; }
            set
            {
                _isHasCertChecked = value;

                parameters[6] = new ReportParameter("CertificationVisibility", _isHasCertChecked.ToString());

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

                parameters[7] = new ReportParameter("AttainmentVisibility", _isAttainmentChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }

        public bool IsCivilStatusChecked
        {
            get { return _isCivilStatusChecked; }
            set
            {
                _isCivilStatusChecked = value;

                parameters[8] = new ReportParameter("CivilStatusVisibility", _isCivilStatusChecked.ToString());

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

        public bool IsDateOfDeathChecked
        {
            get { return _isDateOfDeathChecked; }
            set
            {
                _isDateOfDeathChecked = value;

                parameters[10] = new ReportParameter("DateOfDeathVisibility", _isDateOfDeathChecked.ToString());

                _reportView.ReportContent.Viewer.LocalReport.SetParameters(parameters);
                _reportView.ReportContent.Viewer.RefreshReport();
            }
        }
    }
}
