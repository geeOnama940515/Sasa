using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using GalaSoft.MvvmLight;

namespace Sasa.Reports.Household
{
    public class HouseholdDataSetModel : ObservableObject
    {
        public HouseholdDataSetModel()
        {
            
        }

        private string _houseNo;
        public string HouseNo
        {
            get { return _houseNo;}
            set
            {
                _houseNo = value;
                RaisePropertyChanged(nameof(HouseNo));
            }
        }

        private string _purokNo;
        public string PurokNo
        {
            get { return _purokNo; }
            set
            {
                _purokNo = value;
                RaisePropertyChanged(nameof(PurokNo));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged(nameof(Address));
            }
        }

        private string _head;
        public string Head
        {
            get { return _head; }
            set
            {
                _head = value;
                RaisePropertyChanged(nameof(Head));
            }
        }
    }
}
