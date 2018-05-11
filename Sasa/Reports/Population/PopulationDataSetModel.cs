using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Sasa.Reports.Population
{
    public class PopulationDataSetModel:ObservableObject
    {
        public ObservableCollection<string> GenderList { get; } = new ObservableCollection<string> { "Male", "Female", "All" };
        private int? _age;
        private string _gender;
        private string _houseno;
        private string _isDead;
        private string _hasCert;
        private int _totalPopulation;
        private string _occupation;
        private string _attainment;
        private string _tribe;
        private string _religion;
        private string _organization;
        private bool _isHead;
        private string _name;
        private string _isVoter;
        private DateTime _dateOfBirth;
        private string _dateOfDeath;

        public PopulationDataSetModel()
        {
            
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
        
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
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

        public string IsVoter
        {
            get { return _isVoter; }
            set
            {
                _isVoter = value;
                RaisePropertyChanged(nameof(IsVoter));
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

        public int? Age
        {
            get { return _age; }
            set
            {
                _age = value;
                RaisePropertyChanged(nameof(Age));
            }
        }

        public string HouseNo
        {
            get { return _houseno; }
            set
            {
                _houseno = value;
                RaisePropertyChanged(HouseNo);
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

        public int TotalPopulation
        {
            get { return _totalPopulation; }
            set
            {
                _totalPopulation = value;
                RaisePropertyChanged(nameof(TotalPopulation));
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

        public string Organization
        {
            get { return _organization; }
            set
            {
                _organization = value;
                RaisePropertyChanged(nameof(Organization));
            }
        }

        public bool IsHead
        {
            get { return _isHead; }
            set
            {
                _isHead = value;
                RaisePropertyChanged(nameof(IsHead));
            }
        }

        public DateTime DateOfBirth
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
    }
}
