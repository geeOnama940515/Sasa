using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Sasa.DataAccess;
using Sasa.Helpers.ReportViewer;
using Sasa.Models.Editable;
using Sasa.Modules;
using Sasa.Reports.Livestock;

namespace Sasa.Models.Livestock
{
    public class LivestockModel:ModelBase<DataAccess.EF.Livestock>, IEditableModel<DataAccess.EF.Livestock>
    {
        public LivestockModel(DataAccess.EF.Livestock model) : base(model)
        {
        }

        public LivestockModel(DataAccess.EF.Livestock model, IRepository repository) : base(model, repository)
        {
        }

        private SingleInstanceWindowViewer<HouseholdLivestockWindow> _selectedHouseholdPurokPetWindow = new SingleInstanceWindowViewer<HouseholdLivestockWindow>();
        public ICommand PrintSelectedHouseholdPetCommand => new RelayCommand(PrintSelectedHouseholdPetProc);

        private void PrintSelectedHouseholdPetProc()
        {
            _selectedHouseholdPurokPetWindow.Show();
        }

        private bool _isEditing;
        public bool isEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged(nameof(isEditing));
            }
        }

        public int NumberOfPets
        {
            get { return _numberOfPets; }
            set
            {
                _numberOfPets = value;
                RaisePropertyChanged(nameof(NumberOfPets));
            }
        }

        private void LoadNumberOfPets()
        {
            var pets = _Repository.Livestock.GetRange(c => c.HouseNo == Model.HouseNo);
            var NumberOfPets = pets.Count;
        }

        private EditModelBase<DataAccess.EF.Livestock> _editModel;
        private int _numberOfPets;

        public EditModelBase<DataAccess.EF.Livestock> EditModel
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
            EditModel = new LivestockEditModel(Model);
        }

        public ICommand SaveEditCommand => new RelayCommand(SaveProc, SaveCondition);

        private bool SaveCondition()
        {
            return (EditModel != null) && EditModel.HasChanges;
        }

        private void SaveProc()
        {
            if (EditModel == null) return;
            if (!EditModel.HasChanges) return;
            try
            {
                EditModel.ModelCopy.Quantity = GetNewLivestockQuantity();
                EditModel.ModelCopy.HouseNo =
                    ViewModelLocatorStatic.Locator.PersonModule.SelectedHousehold.Model.HouseNo;
                _Repository.Livestock.Update(EditModel.ModelCopy);
                Model = EditModel.ModelCopy;
                isEditing = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to Save", "Update Livestock");
            }
        }

        private int GetNewLivestockQuantity()
        {
            int male = Convert.ToInt32(EditModel.ModelCopy.Male);
            int female = Convert.ToInt32(EditModel.ModelCopy.Female);
            return male + female;
        }

        public ICommand CancelEditCommand => new RelayCommand(CancelProc);

        private void CancelProc()
        {
            isEditing = false;
            EditModel?.Dispose();
        }
    }
}
