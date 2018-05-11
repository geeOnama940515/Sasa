using GalaSoft.MvvmLight;

namespace Sasa.Reports.Purok
{
    public class AllPurokPersons : ObservableObject
    {
        private string _purokname;
        private string _name;
        private string _occupation;
        private string _gender;
        private string _hasCert;
        private string _isVoter;
        private int _age;
        private string _isDead;
        private string _tribe;
        private string _attainment;
        private string _religion;
        private string _dateOfBirth;
        private string _dateOfDeath;

        public string PurokName
        {
            get { return _purokname; }
            set
            {
                _purokname = value;
                RaisePropertyChanged(PurokName);
            }
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

        public string Occupation
        {
            get { return _occupation; }
            set
            {
                _occupation = value;
                RaisePropertyChanged(nameof(Occupation));
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }

        public string HasCert
        {
            get { return _hasCert; }
            set
            {
                _hasCert = value;
                RaisePropertyChanged(nameof(HasCert));
            }
        }

        public string IsVoter
        {
            get { return _isVoter; }
            set
            {
                _isVoter = value;
                RaisePropertyChanged(nameof(IsVoter));
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                RaisePropertyChanged(nameof(Age));
            }
        }

        public string Tribe
        {
            get { return _tribe; }
            set
            {
                _tribe = value;
                RaisePropertyChanged(nameof(Tribe));
            }
        }

        public string Attainment
        {
            get { return _attainment; }
            set
            {
                _attainment = value;
                RaisePropertyChanged(nameof(Attainment));
            }
        }

        public string Religion
        {
            get { return _religion; }
            set
            {
                _religion = value;
                RaisePropertyChanged(nameof(Religion));
            }
        }

        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                _dateOfBirth = value;
                RaisePropertyChanged(nameof(DateOfBirth));
            }
        }

        public string DateOfDeath
        {
            get { return _dateOfDeath; }
            set
            {

                _dateOfDeath = value;
                RaisePropertyChanged(nameof(DateOfDeath));
            }
        }

        public string IsDead
        {
            get { return _isDead; }
            set
            {
                _isDead = value;
                RaisePropertyChanged(nameof(IsDead));
            }
        }
    }
}
