using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using GalaSoft.MvvmLight;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Person;
using Sasa.Models.Purok;
using Sasa.Modules;

namespace Sasa.Reports.Purok
{
    /// <summary>
    /// Interaction logic for PurokReportWindow.xaml
    /// </summary>
    public partial class PurokReportWindow : Window
    {
        private ReportViewBuilder _reportView;
        private string _titleFilter = string.Empty;

        public PurokReportWindow()
        {
            InitializeComponent();
            _reportView = new ReportViewBuilder("Sasa.Reports.Purok.PurokReport.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;
        }

        
        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var sources = new List<DataSetValuePair>();
            var puroks = ViewModelLocatorStatic.Locator.PurokModule.PurokList;

            var _purokPersons = new ObservableCollection<PurokPerson>();
           

            foreach (var purok in puroks)
            {
                _purokPersons.Add(new PurokPerson
                {
                    Male = purok.MaleCount,
                    Population = purok.NumberOfPersons,
                    Female = purok.FemaleCount,
                    Location = purok.Model?.Location,
                    PurokHeadName = purok.PurokHead?.Model?.FirstName + " "
                                + purok.PurokHead?.Model?.MiddleName + " "
                                + purok.PurokHead?.Model?.LastName + " ",
                    PurokName = purok.Model?.PurokName,
                    PurokNo = purok.Model.PurokNo
                });
            }

            if (StartPopulation != null && EndPopulation > 0)
            {
                var purokpersons = _purokPersons.Where(p => p.Population <= EndPopulation && p.Population >= StartPopulation);
                _purokPersons = new ObservableCollection<PurokPerson>(purokpersons);
            }

            sources.Add(new DataSetValuePair("AllPurokDataSet", _purokPersons.Where(c => TitleFilter != null && c.PurokName.ToLowerInvariant().Contains(TitleFilter.ToLowerInvariant()))));
            return sources;
        }

        private int? _startPopulation = 0;

        public int? StartPopulation
        {
            get { return _startPopulation; }
            set
            {
                _startPopulation = value;
                _reportView.ReportContent.UpdateDataSource(UpdateDatasetSource());
            }
        }

        private int _endPopulation = 9999;

        public int EndPopulation
        {
            get { return _endPopulation; }
            set
            {
                _endPopulation = value;
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
    }
}
