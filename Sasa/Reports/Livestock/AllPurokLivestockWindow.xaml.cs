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
using Sasa.Models.Purok;
using Sasa.Modules;

namespace Sasa.Reports.Livestock
{
    /// <summary>
    /// Interaction logic for AllPurokLivestockWindow.xaml
    /// </summary>
    public partial class AllPurokLivestockWindow : Window
    {
        private readonly ReportViewBuilder _reportView;
        private readonly List<DataSetValuePair> _sources = new List<DataSetValuePair>();
        private PurokModel _selectedPurok;
        public AllPurokLivestockWindow()
        {
            InitializeComponent();

            _reportView = new ReportViewBuilder("Sasa.Reports.Livestock.AllPurokLivestockReport.rdlc", UpdateDatasetSource());
            _reportView.RefreshDataSourceCallback = UpdateDatasetSource;
            ReportContainer.Content = _reportView.ReportContent;

            DataContext = this;
        }

        private IReadOnlyCollection<DataSetValuePair> UpdateDatasetSource()
        {
            _sources.Clear();

            _selectedPurok = ViewModelLocatorStatic.Locator.HouseholdProfileModule.SelectedPurok; //gets selected Purok.
            var purokpets = new ObservableCollection<PurokPets>();
           
            // load the purok pets DataSet
            foreach (var household in _selectedPurok.Households)
            {
                household.LoadRelatedInfo();
                var householdhead = household.HouseholdHead;
                foreach (var livestock in household.Livestocks)
                {
                    purokpets.Add(new PurokPets
                    {
                        HouseNo = household?.Model?.HouseNo,
                        Female = livestock?.Model?.Female,
                        Quantity = livestock?.Model?.Quantity,
                        Male = livestock?.Model?.Male,
                        HouseholdHead = householdhead?.Model?.FirstName + " "
                                        + householdhead?.Model?.MiddleName + " "
                                        + householdhead?.Model?.LastName,
                        Vaccinated = livestock?.Model?.IsVaccinated,
                        Animal = livestock?.Model?.Animal
                    });
                }
            }

            _sources.Add(new DataSetValuePair("PurokDataset", _selectedPurok.Model));
            _sources.Add(new DataSetValuePair("PetsDataSet", purokpets));  
            return _sources;

        }
    }
}
