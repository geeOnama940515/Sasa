using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Sasa.DataAccess.EF;

namespace Sasa.Reports.Livestock
{
    public class PurokPets:ObservableObject
    {

        private string _male;
        public string Male
        {
            get { return _male; }
            set
            {
                _male = value;
                RaisePropertyChanged(nameof(Male));
            }
        }

        private string _female;
        public string Female
        {
            get { return _female; }
            set
            {
                _female = value; 
                RaisePropertyChanged(nameof(Female));
            }
        }

        private int? _vaccinated;
        public int? Vaccinated
        {
            get { return _vaccinated; }
            set
            {
                _vaccinated = value; 
                RaisePropertyChanged(nameof(Vaccinated));
            }
        }

        private int? _quantity;
        public int? Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        private string _animal;
        public string Animal
        {
            get { return _animal; }
            set
            {
                _animal = value; 
                RaisePropertyChanged(nameof(Animal));
            }
        }

        private string _houseNo;
        public string HouseNo
        {
            get { return _houseNo; }
            set
            {
                _houseNo = value; 
                RaisePropertyChanged(nameof(HouseNo));
            }
        }

        private string _householdhead;
        public string HouseholdHead
        {
            get { return _householdhead; }
            set
            {
                _householdhead = value; 
               RaisePropertyChanged(nameof(HouseholdHead));
            }
        }
    }
}
