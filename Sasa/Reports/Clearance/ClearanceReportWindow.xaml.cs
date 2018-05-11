using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Person;
using Sasa.Modules;

namespace Sasa.Reports.Clearance
{
    /// <summary>
    /// Interaction logic for ClearanceReportWindow.xaml
    /// </summary>
    public partial class ClearanceReportWindow : Window
    {
        private ReportViewBuilder _reportView;
        private static readonly IRepository _Repository = new EfRepository();
        private readonly List<DataSetValuePair> _sources = new List<DataSetValuePair>();
        public ClearanceReportWindow()
        {
            InitializeComponent();
            _reportView = new ReportViewBuilder("Sasa.Reports.Clearance.ClearanceReport.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;
        }


        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var selectedClearance = ViewModelLocatorStatic.Locator.ClearanceModule.SelectedClearance;
            selectedClearance.LoadRelatedInfo();
            var person = selectedClearance.Person;
            var personmodel = new PersonModel(person.Model, _Repository);
            personmodel.LoadRelatedInfo();
            var residence = new PersonDataSet(personmodel);
            _sources.Add(new DataSetValuePair("ClearanceDataset", selectedClearance.Model));
            _sources.Add(new DataSetValuePair("PersonDataSet", residence));
            return _sources;
        }
    }
}
