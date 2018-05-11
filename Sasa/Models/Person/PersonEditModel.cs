using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sasa.Models.Editable;

namespace Sasa.Models.Person
{
    public class PersonNewModel : PersonEditModel
    {
        public PersonNewModel() : base(new DataAccess.EF.Person()) { }
    }
    public class PersonEditModel : EditModelBase<DataAccess.EF.Person>
    {
        private bool _selectedVoter;
        private bool _hasBirthCert;

        public PersonEditModel(DataAccess.EF.Person model) : base(model)
        {
            ModelCopy = CreateCopy(model);
        }

        public ObservableCollection<string> GenderList { get; } = new ObservableCollection<string> { "Male", "Female" };
        public ObservableCollection<string> StatusList { get; } = new ObservableCollection<string> { "Single", "Married", "Widowed", "Divorced" };
        public ObservableCollection<string> AttainmentList { get; } = new ObservableCollection<string> { "HighSchool Graduate", "College Graduate", "College Undergraduate" };

        private DataAccess.EF.Person CreateCopy(DataAccess.EF.Person model)
        {
            var copy = new DataAccess.EF.Person
            {

                Birthdate = model.Birthdate,
                Birthplace = model.Birthplace,
                CivilStatus = model.CivilStatus,
                ContactNo = model.ContactNo,
                Dialect = model.Dialect,
                HouseNo = model.HouseNo,
                Occupation = model.Occupation,
                Attainment = model.Attainment,
                FirstName = model.FirstName,
                Income = model.Income,
                IsHead = model.IsHead,
                IsPurokHead = model.IsPurokHead,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                PersonId = model.PersonId,
                Relationship = model.Relationship,
                Religion = model.Religion,
                Tribe = model.Tribe,
                Gender = model.Gender,
                Age = model.Age,
                PurokNo = model.PurokNo,
                Organization = model.Organization,
                DateOfDeath = model.DateOfDeath,
                IsDead = model.IsDead,
                IsVoter = model.IsVoter,
                HasBirthCert = model.HasBirthCert
            };

            return copy;
        }

        public int? PurokNo
        {
            get { return _ModelCopy.PurokNo; }
            set
            {
                _ModelCopy.PurokNo = value;
                RaisePropertyChanged(nameof(PurokNo));
            }
        }
        

        public string IsDead
        {
            get { return _ModelCopy.IsDead; }
            set
            {
                _ModelCopy.IsDead = value;
                RaisePropertyChanged(nameof(IsDead));
            }
        }

        public bool SelectedHasBirthCert
        {
            get { return _hasBirthCert; }
            set
            {
                _hasBirthCert = value;
                if (_hasBirthCert != null)
                {
                    if (_hasBirthCert)
                    {
                        HasBirthCert = "Yes";
                    }
                    else
                    {
                        HasBirthCert = "No";
                    }
                }
                
                RaisePropertyChanged(nameof(SelectedHasBirthCert));
                
            }
        }

        public string HasBirthCert
        {
            get { return _ModelCopy.HasBirthCert; }
            set
            {
                _ModelCopy.HasBirthCert = value;
                RaisePropertyChanged(nameof(HasBirthCert));
            }
        }

        public bool SelectedVoter
        {
            get { return _selectedVoter; }
            set
            {
                _selectedVoter = value;
                if (_selectedVoter != null)
                {
                    if (_selectedVoter)
                    {
                        IsVoter = "Yes";
                    }
                    else
                    {
                        IsVoter = "No";
                    }
                }
                RaisePropertyChanged(nameof(SelectedVoter));
            }
        }

        public string IsVoter
        {
            get { return _ModelCopy.IsVoter; }
            set
            {
                _ModelCopy.IsVoter = value;
                RaisePropertyChanged(nameof(IsVoter));
            }
        }

        public string Gender
        {
            get { return _ModelCopy.Gender; }
            set
            {
                _ModelCopy.Gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }

        public string Organization
        {
            get { return _ModelCopy.Organization; }
            set
            {
                _ModelCopy.Organization = value;
                RaisePropertyChanged(nameof(Organization));
            }
        }

        public string Age
        {
            get { return _ModelCopy.Age; }
            set
            {
                _ModelCopy.Age = value;
                RaisePropertyChanged(nameof(Age));
            }
        }


        public DateTime? Birthdate
        {
            get { return _ModelCopy.Birthdate; }
            set
            {
                _ModelCopy.Birthdate = value;
                RaisePropertyChanged(nameof(Birthdate));
            }
        }

        public DateTime? DateOfDeath
        {
            get { return _ModelCopy.DateOfDeath; }
            set
            {
                _ModelCopy.DateOfDeath = value;
                RaisePropertyChanged(nameof(DateOfDeath));
            }
        }

        public string Birthplace
        {
            get { return _ModelCopy.Birthplace; }
            set
            {
                _ModelCopy.Birthplace = value;
                RaisePropertyChanged(nameof(Birthplace));
            }
        }


        public string CivilStatus
        {
            get { return _ModelCopy.CivilStatus; }
            set
            {
                _ModelCopy.CivilStatus = value;
                RaisePropertyChanged(nameof(CivilStatus));
            }
        }

        public string ContactNo
        {
            get { return _ModelCopy.ContactNo; }
            set
            {
                _ModelCopy.ContactNo = value;
                RaisePropertyChanged(nameof(ContactNo));
            }
        }

        public string Dialect
        {
            get { return _ModelCopy.Dialect; }
            set
            {
                _ModelCopy.Dialect = value;
                RaisePropertyChanged(nameof(Dialect));
            }
        }

        public string HouseNo
        {
            get { return _ModelCopy.HouseNo; }
            set
            {
                _ModelCopy.HouseNo = value;
                RaisePropertyChanged(nameof(HouseNo));
            }
        }

        public string Occupation
        {
            get { return _ModelCopy.Occupation; }
            set
            {
                _ModelCopy.Occupation = value;
                RaisePropertyChanged(nameof(Occupation));
            }
        }

        public string Attainment
        {
            get { return _ModelCopy.Attainment; }
            set
            {
                _ModelCopy.Attainment = value;
                RaisePropertyChanged(nameof(Attainment));
            }
        }

        public string FirstName
        {
            get { return _ModelCopy.FirstName; }
            set
            {
                _ModelCopy.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public double? Income
        {
            get { return _ModelCopy.Income; }
            set
            {
                _ModelCopy.Income = value;
                RaisePropertyChanged(nameof(Income));
            }
        }

        public bool? IsHead
        {
            get { return _ModelCopy.IsHead; }
            set
            {
                _ModelCopy.IsHead = value;
                RaisePropertyChanged(nameof(IsHead));
            }
        }

        public bool? IsPurokHead
        {
            get { return _ModelCopy.IsPurokHead; }
            set
            {
                _ModelCopy.IsPurokHead = value;
                RaisePropertyChanged(nameof(IsPurokHead));
            }
        }

        public string LastName
        {
            get { return _ModelCopy.LastName; }
            set
            {
                _ModelCopy.LastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string MiddleName
        {
            get { return _ModelCopy.MiddleName; }
            set
            {
                _ModelCopy.MiddleName = value;
                RaisePropertyChanged(nameof(MiddleName));
            }
        }

        public int PersonId
        {
            get { return _ModelCopy.PersonId; }
            set
            {
                _ModelCopy.PersonId = value;
                RaisePropertyChanged(nameof(PersonId));
            }
        }

        public string Relationship
        {
            get { return _ModelCopy.Relationship; }
            set
            {
                _ModelCopy.Relationship = value;
                RaisePropertyChanged(nameof(Relationship));
            }
        }

        public string Religion
        {
            get { return _ModelCopy.Religion; }
            set
            {
                _ModelCopy.Religion = value;
                RaisePropertyChanged(nameof(Religion));
            }
        }

        public string Tribe
        {
            get { return _ModelCopy.Tribe; }
            set
            {
                _ModelCopy.Tribe = value;
                RaisePropertyChanged(nameof(Tribe));
            }
        }

    }
}
