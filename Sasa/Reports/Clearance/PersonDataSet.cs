using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Sasa.Models.Person;

namespace Sasa.Reports.Clearance
{
    public class PersonDataSet : ObservableObject
    {
        private string _name;
        private string _age;
        private string _residence;

        public PersonDataSet(PersonModel person)
        {
            Name = person.Model?.FirstName + " "
                   + person.Model?.MiddleName + " "
                   + person.Model?.LastName + " ";
            Age = person.Model?.Age;
            Residence = person.Household?.Model?.Address;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Age
        {
            get { return _age; }
            set
            {
                _age = value;
                RaisePropertyChanged(nameof(Age));
            }
        }

        public string Residence
        {
            get { return _residence; }
            set
            {
                _residence = value;
                RaisePropertyChanged(nameof(Residence));
            }
        }
    }
}
