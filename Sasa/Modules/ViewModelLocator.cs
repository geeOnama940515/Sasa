using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Sasa.DataAccess.EF;

namespace Sasa.Modules
{
    public class ViewModelLocator
    {
        private static readonly EfRepository _repository = new EfRepository();

        public ViewModelLocator()
        {
            PurokModule = new PurokModule(_repository);
            HouseholdProfileModule = new HouseholdProfileModule(_repository);
            HouseholdPetsModule = new HouseholdPetsModule(_repository);
            PersonModule = new PersonModule(_repository);
            CaseModule = new CaseModule(_repository);
            ClearanceModule = new ClearanceModule(_repository);
            EmployeeModule = new EmployeeModule(_repository);
        }

        public ClearanceModule ClearanceModule { get; set; }

        public CaseModule CaseModule { get; set; }

        public PersonModule PersonModule { get; set; }

        public HouseholdPetsModule HouseholdPetsModule { get; set; }

        public HouseholdProfileModule HouseholdProfileModule { get; set; }

        public PurokModule PurokModule { get; set; }

        public EmployeeModule EmployeeModule { get; set; }
    }

    public static class ViewModelLocatorStatic
    {
        public static ViewModelLocator Locator = Application.Current.Resources["Locator"] as ViewModelLocator;
    }
}
