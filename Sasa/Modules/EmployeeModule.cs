using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Models.Accessor;
using Sasa.Models.Employee;
using Sasa.Models.Person;
using Sasa.View.Employee;
using Sasa.View.Login;

namespace Sasa.Modules
{
    public class EmployeeModule : ObservableObject
    {
        private IRepository _repository;
        private string _username;
        private EmployeeModel _employeeLogged;
        private MainWindow _mainwindow;
        private LoginWindow _loginWindow;
        private ObservableCollection<string> _accessorUserNames = new ObservableCollection<string>();
        private ObservableCollection<AccessorModel> _accessor = new ObservableCollection<AccessorModel>();
        private EmployeeModel _selectedEmployee;

        public EmployeeModule(IRepository repository)
        {
            _repository = repository;
            LoadEmployees();
            _loginWindow = (LoginWindow)Application.Current.MainWindow;
        }

        // Properties

        public ObservableCollection<EmployeeModel> Employees { get; } = new ObservableCollection<EmployeeModel>();

        public EmployeeModel SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value; 
                RaisePropertyChanged(nameof(SelectedEmployee));
            }
        }

        public NewEmployeeModel NewEmployee
        {
            get { return _newEmployeeModel; }
            set
            {
                _newEmployeeModel = value; 
                RaisePropertyChanged(nameof(NewEmployee));
            }
        }

        public EmployeeModel EmployeeLogged
        {
            get { return _employeeLogged; }
            set
            {
                _employeeLogged = value;
                RaisePropertyChanged(nameof(EmployeeLogged));
            }
        }

        public string UserName
        {
            get { return _username; }
            set
            {
                _username = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get { return _loginWindow.UserPasswordBox.Password; }
            set
            {
                _loginWindow.UserPasswordBox.Password = value;

                RaisePropertyChanged(nameof(Password));
            }
        }

        // Commands

        public ICommand LoginCommand => new RelayCommand(LoginProc, LoginCondition);
        public ICommand LogoutCommand => new RelayCommand(LogoutProc);
        public ICommand AddEmployeeCommand => new RelayCommand(AddEmployeeProc);
        public ICommand DeleteEmployeeCommand => new RelayCommand(DeleteEmployeeProc, DeleteEmployeeCondition);
        public ICommand SaveEmployeeCommand => new RelayCommand(SaveEmployeeProc, SaveEmployeeCondition);

        // Methods

        private bool SaveEmployeeCondition()
        {
            return (NewEmployee != null) && NewEmployee.HasChanges;
        }

        private void SaveEmployeeProc()
        {
            if(NewEmployee == null) return;
            if(!NewEmployee.HasChanges) return;

            _repository.Person.Add(NewEmployee.Person.ModelCopy);
            
            // Locating the House of the Added Person
            ViewModelLocatorStatic.Locator.HouseholdProfileModule.SelectedHousehold =
                ViewModelLocatorStatic.Locator.HouseholdProfileModule.HouseholdList.FirstOrDefault(
                    h => h.Model.HouseNo == NewEmployee.Person.HouseNo);

            if (ViewModelLocatorStatic.Locator.HouseholdProfileModule.SelectedHousehold != null)
            {
                ViewModelLocatorStatic.Locator.HouseholdProfileModule.SelectedHousehold.Members.Add(new PersonModel(NewEmployee.Person.ModelCopy, _repository));
            }

            NewEmployee.ModelCopy.PersonId = NewEmployee.Person.ModelCopy.PersonId;
            NewEmployee.ModelCopy.AccessorId = 4;
            NewEmployee.ModelCopy.Role = "Visitor";
            var employeemodel = new EmployeeModel(NewEmployee.ModelCopy, _repository);
            employeemodel.LoadRelatedInfo();
            _repository.Employee.Add(NewEmployee.ModelCopy);
            Employees.Add(employeemodel);
            try
            {   

            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save", "Add Employee", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool DeleteEmployeeCondition()
        {
            return (SelectedEmployee != null);
        }

        private void DeleteEmployeeProc()
        {
            if (SelectedEmployee == null) return;

            try
            {
                _repository.Employee.Remove(SelectedEmployee.Model);
                Employees.Remove(SelectedEmployee);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Delete", "Delete Employee", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private AddingEmployeeWindow _addingEmployeeWindow;
        private NewEmployeeModel _newEmployeeModel;

        private void AddEmployeeProc()
        {   
            NewEmployee = new NewEmployeeModel();
            _addingEmployeeWindow = new AddingEmployeeWindow();
            _addingEmployeeWindow.Owner = Application.Current.MainWindow;
            _addingEmployeeWindow.ShowDialog();
        }

        private void LogoutProc()
        {
            var response = MessageBox.Show("Are you sure?", " Log out", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (response == MessageBoxResult.Yes)
            {
                _mainwindow.Close();
            }
        }

        private bool LoginCondition()
        {
            return (!string.IsNullOrWhiteSpace(UserName)) && (!string.IsNullOrWhiteSpace(Password));
        }

        private void LoginProc()
        {
            if ((string.IsNullOrWhiteSpace(UserName)) && (string.IsNullOrWhiteSpace(Password))) return;

            foreach (var accessor in Employees)
            {
                _accessorUserNames.Add(accessor.Accessor.Model.Username);
                _accessor.Add(accessor.Accessor);
            }

            if (_accessorUserNames.Contains(UserName))
            {
                var usernameindex = _accessorUserNames.IndexOf(UserName);
                if (_accessor[usernameindex].Model.Password == Password)
                {
                    EmployeeLogged = Employees[usernameindex];
                    if (EmployeeLogged.Model.Role.Contains("Admin"))
                    {
                        EmployeeLogged.IsAdmin = true;
                        EmployeeLogged.IsClerk = false;
                        EmployeeLogged.IsClearance = false;

                        EmployeeLogged.IsAllowedToAdd = true;
                        EmployeeLogged.IsAllowedtoDelete = true;
                        EmployeeLogged.IsAllowedToEdit = true;

                        EmployeeLogged.IsAllowedToAddClearance = true;
                        EmployeeLogged.IsAllowedToDeleteClearance = true;
                        EmployeeLogged.IsAllowedToEditClearance = true;

                        EmployeeLogged.IsAllowedToAddCase = true;
                        EmployeeLogged.IsAllowedToDeleteCase = true;
                        EmployeeLogged.IsAllowedToEditCase = true;

                        EmployeeLogged.IsAllowedToDeleteEmployee = true;
                        EmployeeLogged.IsAllowedToAddEmployee = true;
                    }

                    if (EmployeeLogged.Model.Role.Contains("Clerk"))
                    {
                        EmployeeLogged.IsAdmin = false;
                        EmployeeLogged.IsClerk = true;
                        EmployeeLogged.IsClearance = false;

                        EmployeeLogged.IsAllowedToAdd = true;
                        EmployeeLogged.IsAllowedtoDelete = false;
                        EmployeeLogged.IsAllowedToEdit = false;

                        EmployeeLogged.IsAllowedToAddClearance = true;
                        EmployeeLogged.IsAllowedToDeleteClearance = false;
                        EmployeeLogged.IsAllowedToEditClearance = false;

                        EmployeeLogged.IsAllowedToAddCase = true;
                        EmployeeLogged.IsAllowedToDeleteCase = true;
                        EmployeeLogged.IsAllowedToEditCase = true;

                        EmployeeLogged.IsAllowedToDeleteEmployee = false;
                        EmployeeLogged.IsAllowedToAddEmployee = false;
                    }

                    if (EmployeeLogged.Model.Role.Contains("Clearance"))
                    {
                        EmployeeLogged.IsAdmin = false;
                        EmployeeLogged.IsClerk = false;
                        EmployeeLogged.IsClearance = true;

                        EmployeeLogged.IsAllowedToAdd = true;
                        EmployeeLogged.IsAllowedtoDelete = false;
                        EmployeeLogged.IsAllowedToEdit = false;

                        EmployeeLogged.IsAllowedToAddClearance = true;
                        EmployeeLogged.IsAllowedToDeleteClearance = true;
                        EmployeeLogged.IsAllowedToEditClearance = true;

                        EmployeeLogged.IsAllowedToAddCase = false;
                        EmployeeLogged.IsAllowedToDeleteCase = false;
                        EmployeeLogged.IsAllowedToEditCase = false;

                        EmployeeLogged.IsAllowedToDeleteEmployee = false;
                        EmployeeLogged.IsAllowedToAddEmployee = false;
                    }

                    if (EmployeeLogged.Model.Role.Contains("Public"))
                    {
                        EmployeeLogged.IsAdmin = false;
                        EmployeeLogged.IsClerk = false;
                        EmployeeLogged.IsClearance = false;

                        EmployeeLogged.IsAllowedToAdd = false;
                        EmployeeLogged.IsAllowedtoDelete = false;
                        EmployeeLogged.IsAllowedToEdit = false;

                        EmployeeLogged.IsAllowedToAddClearance = false;
                        EmployeeLogged.IsAllowedToDeleteClearance = false;
                        EmployeeLogged.IsAllowedToEditClearance = false;

                        EmployeeLogged.IsAllowedToAddCase = false;
                        EmployeeLogged.IsAllowedToDeleteCase = false;
                        EmployeeLogged.IsAllowedToEditCase = false;

                        EmployeeLogged.IsAllowedToDeleteEmployee = false;
                        EmployeeLogged.IsAllowedToAddEmployee = false;
                    }

                    _mainwindow = new MainWindow();
                    _mainwindow.Owner = Application.Current.MainWindow;
                    _mainwindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("The password does not match!", "Login", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("The user name does not exist!", "Login", MessageBoxButton.OK,
                         MessageBoxImage.Exclamation);
            }

            UserName = "";
            _loginWindow.UserPasswordBox.Clear();
        }

        private void LoadEmployees()
        {
            var employees = _repository.Employee.GetRange();

            foreach (var employee in employees)
            {
                var employeemodel = new EmployeeModel(employee, _repository);
                employeemodel.LoadRelatedInfo();
                Employees.Add(employeemodel);
            }
        }
    }
}
