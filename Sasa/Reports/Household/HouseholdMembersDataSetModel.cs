using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;

namespace Sasa.Reports.Household
{
    public class HouseholdMembersDataSetModel:ObservableObject
    {
        public HouseholdMembersDataSetModel()
        {
            
        }

        private string _member;
        public string Member
        {
            get { return _member; }
            set
            {
                _member = value;
                RaisePropertyChanged(nameof(Member));
            }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
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

        private string _civilstatus;
        public string CivilStatus
        {
            get { return _civilstatus; }
            set
            {
                _civilstatus = value;
                RaisePropertyChanged(nameof(CivilStatus));
            }
        }

        private string _birthdate;
        public string Birthdate
        {
            get { return _birthdate; }
            set
            {
                _birthdate = value;
                RaisePropertyChanged(nameof(Birthdate));
            }
        }

        private string _relationship;
        public string Relationship
        {
            get { return _relationship; }
            set
            {
                _relationship = value;
                RaisePropertyChanged(nameof(Relationship));
            }
        }

        private string _attainment;
        public string Attainment
        {
            get { return _attainment; }
            set
            {
                _attainment = value;
                RaisePropertyChanged(nameof(Attainment));
            }
        }

        private string _occupation;
        private string _age;
        private string _isDead;
        private string _isVoter;
        private string _hasCert;
        private string _dateOfBirth;
        private string _dateOfDeath;

        public string Occupation
        {
            get { return _occupation; }
            set
            {
                _occupation = value;
                RaisePropertyChanged(nameof(Occupation));
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
    }
}
