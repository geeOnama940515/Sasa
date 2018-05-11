using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Household;
using Sasa.Models.Person;
using Sasa.Models.Purok;
using Sasa.Modules;
using Sasa.Reports.Household;

namespace Sasa.Reports.Livestock
{
    /// <summary>
    /// Interaction logic for HouseholdLivestockWindow.xaml
    /// </summary>
    public partial class HouseholdLivestockWindow : Window
    {
        private readonly ReportViewBuilder _reportView;
        private readonly List<DataSetValuePair> _sources = new List<DataSetValuePair>();
        private int _total;

        public HouseholdLivestockWindow()
        {
            InitializeComponent();

            _reportView = new ReportViewBuilder("Sasa.Reports.Livestock.HouseholdLivestockReport.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;
            DataContext = this;
        }

        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            var sources = new List<DataSetValuePair>();
            var household = ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold;

            var householddataset = new HouseholdDataSetModel
            {
                HouseNo = household.Model.HouseNo,
                PurokNo = household.Purok?.Model.PurokName,
                Address = household.Model.Address,
                Head = household.HouseholdHead?.Model.FirstName + " "
                    + household.HouseholdHead?.Model.MiddleName + " "
                    + household.HouseholdHead?.Model.LastName
            };

            var livestockdataset = new ObservableCollection<PurokPets>();

            foreach (var livestock in household.Livestocks)
            {
                livestockdataset.Add(new PurokPets
                {
                    Animal = livestock.Model?.Animal,
                    Female = livestock.Model?.Female,
                    Male = livestock.Model?.Male,
                    Quantity = livestock.Model?.Quantity,
                    Vaccinated = livestock.Model?.IsVaccinated
                });
            }

            sources.Add(new DataSetValuePair("PetsDataSet", livestockdataset));
            sources.Add(new DataSetValuePair("HouseholdDataSet", householddataset));

            return sources;
        }

       
    }
}
