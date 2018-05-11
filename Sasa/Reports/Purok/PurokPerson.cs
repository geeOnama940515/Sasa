using GalaSoft.MvvmLight;

namespace Sasa.Reports.Purok
{
    public class PurokPerson:ObservableObject
    {

        public PurokPerson()
        {

        }

        //purok

        private int _purokNo;

        public int PurokNo
        {
            get { return _purokNo; }
            set
            {
                _purokNo = value;
                RaisePropertyChanged(nameof(PurokNo));
            }
        }

        private string _purokname;
        public string PurokName
        {
            get { return _purokname; }
            set
            {
                _purokname = value; 
                RaisePropertyChanged(PurokName);
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

        //purok head
        private string _purokheadname;
        public string PurokHeadName
        {
            get { return _purokheadname; }
            set
            {
                _purokheadname = value; 
                RaisePropertyChanged(nameof(PurokHeadName));
            }
        }


        //population

        private int _population;
        public int Population
        {
            get { return _population; }
            set
            {
                _population = value; 
                RaisePropertyChanged(nameof(Population));
            }
        }

        private int _female;
        public int Female
        {
            get { return _female; }
            set
            {
                _female = value;
                RaisePropertyChanged(nameof(Female));
            }
        }

        private int _male;
        private int _totalPop;

        public int Male
        {
            get { return _male;}
            set
            {
                _male = value;
                RaisePropertyChanged(nameof(Male));
            }
        }

        
    }
}
