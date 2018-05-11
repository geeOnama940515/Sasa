using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.DataAccess;
using Sasa.DataAccess.EF;
using Sasa.Models.Accessor;
using Sasa.Models.Editable;
using Sasa.Models.Household;
using Sasa.Models.Person;

namespace Sasa.Models.Employee
{
    public class NewEmployeeModel : EmployeeEditModel
    {
        public NewEmployeeModel() : base(new DataAccess.EF.Employee())
        {
        }
    }
    public class EmployeeEditModel:EditModelBase<DataAccess.EF.Employee>
    {
        private int? _personId;
        private int _employeeId;
        private int? _accessorId;
        private string _role;
        private PersonNewModel _person;
        private AccessorModel _accessor;
        private string _position;
        private HouseholdModel _household;
        private static readonly IRepository _Repository = new EfRepository();

        public EmployeeEditModel(DataAccess.EF.Employee model) : base(model)
        {   
            ModelCopy = CreateCopy(model);
            Person = new PersonNewModel();
            LoadRelatedInfo();
        }

        // properties

        public int? PersonId
        {
            get { return ModelCopy.PersonId; }
            set
            {
                ModelCopy.PersonId = value; 
                RaisePropertyChanged(nameof(PersonId));
            }
        }

        public int EmployeeId
        {
            get { return ModelCopy.EmployeeId; }
            set
            {
                ModelCopy.EmployeeId = value; 
                RaisePropertyChanged(nameof(EmployeeId));
            }
        }

        public int? AccessorId
        {
            get { return ModelCopy.AccessorId; }
            set
            {
                ModelCopy.AccessorId = value; 
                RaisePropertyChanged(nameof(AccessorId));
            }
        }

        public string Role
        {
            get { return ModelCopy.Role; }
            set
            {
                ModelCopy.Role = value; 
                RaisePropertyChanged(nameof(Role));
            }
        }

        public string Position
        {
            get { return ModelCopy.Position; }
            set
            {
                ModelCopy.Position = value; 
                RaisePropertyChanged(nameof(Position));
            }
        }

        // Other Properties
        public HouseholdModel Household
        {
            get { return _household; }
            set
            {
                _household = value;
                if (value != null)
                {
                    Person.ModelCopy.HouseNo = _household.Model.HouseNo;
                    Person.ModelCopy.PurokNo = _household.Model.PurokNo;
                }
                RaisePropertyChanged(nameof(Household));
            }
        }

        public PersonNewModel Person
        {
            get { return _person; }
            set
            {
                _person = value;
                RaisePropertyChanged(nameof(Person));
            }
        }

        public AccessorModel Accessor
        {
            get { return _accessor; }
            set
            {
                _accessor = value;
                if (value != null)
                {
                    AccessorId = _accessor.Model.AccessorId;
                }
                RaisePropertyChanged(nameof(Accessor));
            }
        }

        public ObservableCollection<string> GenderList { get; } = new ObservableCollection<string> {"Male", "Female"};
        public ObservableCollection<string> AttainmentList { get; } = new ObservableCollection<string> {"HighSchool Graduate", "College Graduate", "Elementary Graduate"}; 
        public ObservableCollection<string> StatusList { get; } = new ObservableCollection<string> {"Widow", "Separated", "Single", "Married"};
        public ObservableCollection<string> PositionList { get; } = new ObservableCollection<string> {"Kagawad", "Treasurer", "Captain", "Clearance Officer", "Cases Officer"};
        public ObservableCollection<HouseholdModel> Households { get; } = new ObservableCollection<HouseholdModel>(); 

        // Methods

        private void LoadRelatedInfo()
        {
            var households = _Repository.Household.GetRange();
            foreach (var household in households)
            {
                Households.Add(new HouseholdModel(household, _Repository));
            }
        }

        private DataAccess.EF.Employee CreateCopy(DataAccess.EF.Employee model)
        {
            var copy = new DataAccess.EF.Employee
            {
                Role = model.Role,
                PersonId = model.PersonId,
                AccessorId = model.AccessorId,
                EmployeeId = model.EmployeeId
            };

            return copy;
        }
    }
}
