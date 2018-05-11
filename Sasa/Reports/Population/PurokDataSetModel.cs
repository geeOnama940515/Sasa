using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Sasa.Reports.Population
{
    public class PurokDataSetModel:ObservableObject
    {

        public PurokDataSetModel()
        {
            
        }

        private string _purokName;

        public string PurokName
        {
            get { return _purokName; }
            set
            {
                _purokName = value;
                RaisePropertyChanged(nameof(PurokName));
            }
        }
        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                RaisePropertyChanged(nameof(Location));
            }
        }
        private string _purokhead;
        private int _population;

        public string PurokHead
        {
            get { return _purokhead; }
            set
            {
                _purokhead = value;
                RaisePropertyChanged(nameof(PurokHead));
            }
        }

        public int Population
        {
            get { return _population; }
            set
            {
                _population = value;
                RaisePropertyChanged(nameof(Population));
            }
        }

    }
}
