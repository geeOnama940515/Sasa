using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Models.Editable;
using Sasa.Models.Person;

namespace Sasa.Models.Case
{
    public class CaseModel : ModelBase<DataAccess.EF.Case>, IEditableModel<DataAccess.EF.Case>
    {
        private bool _isEditing;
        private EditModelBase<DataAccess.EF.Case> _editModel;

        public CaseModel(DataAccess.EF.Case model) : base(model)
        {
        }

        public CaseModel(DataAccess.EF.Case model, IRepository repository) : base(model, repository)
        {
        }


        private PersonModel _respondent;
        public PersonModel Respondent
        {
            get { return _respondent; }
            set
            {
                _respondent = value;
                RaisePropertyChanged(nameof(Respondent));
            }
        }

        public void LoadRelatedInfo()
        {
            var person = _Repository.Person.Get(p => p.PersonId == Model.Respondent);
            Respondent = new PersonModel(person, _Repository);
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

        public EditModelBase<DataAccess.EF.Case> EditModel
        {
            get { return _editModel; }
            set
            {
                _editModel = value;
                RaisePropertyChanged(nameof(EditModel));
            }
        }

        public ICommand EditCommand => new RelayCommand(EditProc);

        private void EditProc()
        {
            isEditing = true;
            EditModel?.Dispose();
            EditModel = new CaseEditModel(Model);
        }

        public ICommand SaveEditCommand => new RelayCommand(SaveEditProc, SaveEditCondition);

        private void SaveEditProc()
        {
            if (EditModel == null) return;
            if (!EditModel.HasChanges) return;

            try
            {
                _Repository.Case.Update(EditModel.ModelCopy);

                //replace the model with the edited copy
                Model = EditModel.ModelCopy;

                isEditing = false;
            }
            catch (Exception e)
            {

                MessageBox.Show("Unable to save. Reason" + e.Message);
            }
        }

        private bool SaveEditCondition()
        {
            return (EditModel != null) && EditModel.HasChanges && !EditModel.HasErrors;
        }
        public ICommand CancelEditCommand => new RelayCommand(CancelEditProc);

        private void CancelEditProc()
        {
            isEditing = false;
            EditModel?.Dispose();
        }
    }
}
