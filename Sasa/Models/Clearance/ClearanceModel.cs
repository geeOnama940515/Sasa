using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Editable;
using Sasa.Models.Person;
using Sasa.Reports.Clearance;
using Sasa.Reports.Clearance.Residency;

namespace Sasa.Models.Clearance
{
    public class ClearanceModel : ModelBase<DataAccess.EF.Clearance>, IEditableModel<DataAccess.EF.Clearance>
    {
        private bool _isEditing;
        private EditModelBase<DataAccess.EF.Clearance> _editModel;
        private IRepository _repository;
        public ClearanceModel(DataAccess.EF.Clearance model) : base(model)
        {
        }

        public ClearanceModel(DataAccess.EF.Clearance model, IRepository repository) : base(model, repository)
        {
            _repository = repository;
        }

        public PersonModel Person
        {
            get { return _person; }
            set
            {
                _person = value;
                RaisePropertyChanged(nameof(Person));
            }
        }


        public bool isEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged(nameof(isEditing));
            }
        }

        public EditModelBase<DataAccess.EF.Clearance> EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value;
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        public ICommand PrintClearanceCommand => new RelayCommand(PrintClearanceProc);
        private SingleInstanceWindowViewer<ClearanceReportWindow> _printClearanceWindow = new SingleInstanceWindowViewer<ClearanceReportWindow>();
        private SingleInstanceWindowViewer<ResidencyReportWindow> _printResidencyWindow = new SingleInstanceWindowViewer<ResidencyReportWindow>();
        private PersonModel _person;


        private void PrintClearanceProc()
        {
            var person = _repository.Person.Get(c => c.PersonId == Model.PersonId);
            if (person.HasCases == true)
            {
                _printResidencyWindow.Show();
            }
            else
            {
                _printClearanceWindow.Show();
            }
            
        }

        public ICommand EditCommand => new RelayCommand(EditProc);

        private void EditProc()
        {
            isEditing = true;
            EditModel?.Dispose();
            EditModel = new ClearanceEditModel(Model);
        }

        public ICommand SaveEditCommand => new RelayCommand(SaveProc, SaveCondition);
        public ICommand CancelEditCommand => new RelayCommand(CancelProc);

        private void CancelProc()
        {
            isEditing = false;
            EditModel?.Dispose();
        }


        private void SaveProc()
        {
            if(EditModel == null) return;
            if(!EditModel.HasChanges) return;
            try
            {
                _Repository.Clearance.Update(EditModel.ModelCopy);
                Model = EditModel.ModelCopy;
                isEditing = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to save", "Clearance");
           
            }
        }

        private bool SaveCondition()
        {
            return (EditModel != null) && EditModel.HasChanges;
        }

        


        public void LoadRelatedInfo()
        {
            var person = _Repository.Person.Get(p => p.PersonId == Model.PersonId);
            Person = new PersonModel(person, _Repository);
        }
    }
}
