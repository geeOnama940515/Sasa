using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.Models.Editable;
using Sasa.Models.Person;

namespace Sasa.Models.Case
{
    public class NewCaseModel : CaseEditModel
    {
        public NewCaseModel() : base(new DataAccess.EF.Case())
        {
            InitializeRequiredFields();
        }

        private void InitializeRequiredFields()
        {
            CaseId = string.Empty;
            CaseDescription = string.Empty;
            CaseName = string.Empty;
            Complainant = string.Empty;

        }
    }
        
    public class CaseEditModel : EditModelBase<DataAccess.EF.Case>
    {
        private PersonModel _selectedRespondent;
        private string _respondentOpt;

        public CaseEditModel(DataAccess.EF.Case model) : base(model)
        {
            ModelCopy = CreateCopy(model);
        }

        public static List<string> StatusList { get; } = new List<string> { "Settled", "Unsettled" };
        public static List<string> LevelList { get; } = new List<string> { "1 - Mediation", "2 - Consolidation" };

        private DataAccess.EF.Case CreateCopy(DataAccess.EF.Case model)
        {

            var copy = new DataAccess.EF.Case()
            {
                CaseId = model.CaseId,
                CaseHearingDate = model.CaseHearingDate,
                CaseLevel = model.CaseLevel,
                CaseName = model.CaseName,
                Complainant = model.Complainant,
                Respondent = model.Respondent,
                CaseStatus = model.CaseStatus,
                CaseDescription = model.CaseDescription,
                RepondentOpt = model.RepondentOpt
            };
            return copy;
        }

        public string CaseId
        {
            get { return _ModelCopy.CaseId; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(CaseId),
                    () => string.IsNullOrWhiteSpace(value), "Case ID should not be empty.");

                _ModelCopy.CaseId = newValue;

            }
        }

        public string CaseDescription
        {
            get { return _ModelCopy.CaseDescription; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(CaseDescription),
                    () => string.IsNullOrWhiteSpace(value), "Description should not be empty");

                _ModelCopy.CaseDescription = newValue;
            }
        }

        public PersonModel SelectedRespondent
        {
            get { return _selectedRespondent; }
            set
            {
                _selectedRespondent = value;
                if (value != null)
                {
                    Respondent = SelectedRespondent.Model.PersonId;
                }
                RaisePropertyChanged(nameof(SelectedRespondent));
            }
        }

        public string CaseName
        {
            get { return _ModelCopy.CaseName; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(CaseName),
                    () => string.IsNullOrWhiteSpace(value), "Case summary should not be empty.");

                _ModelCopy.CaseName = newValue;
            }
        }

        public string CaseLevel
        {
            get { return _ModelCopy.CaseLevel; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(CaseLevel),
                    () => String.IsNullOrWhiteSpace(value), "Case level should not be empty");

                _ModelCopy.CaseLevel = newValue;
            }
        }

        public string CaseStatus
        {
            get { return _ModelCopy.CaseStatus; }
            set
            {
                _ModelCopy.CaseStatus = value;
                RaisePropertyChanged(nameof(CaseStatus));
            }
        }

        public string Complainant
        {
            get { return _ModelCopy.Complainant; }
            set
            {
                string tmp = value;
                string newValue = ValidateInputAndAddErrors(ref tmp, value, nameof(Complainant),
                    () => String.IsNullOrWhiteSpace(value), "Complainant should not be empty");

                _ModelCopy.Complainant = newValue;
            }
        }

        public DateTime? CaseHearingDate
        {
            get { return _ModelCopy.CaseHearingDate; }
            set
            {
                _ModelCopy.CaseHearingDate = value;
                RaisePropertyChanged(nameof(CaseHearingDate));
            }
        }

        public int? Respondent
        {
            get { return _ModelCopy.Respondent; }
            set
            {
                _ModelCopy.Respondent = value;
                RaisePropertyChanged(nameof(Respondent));
            }
        }

        public string RespondentOpt
        {
            get { return _ModelCopy.RepondentOpt; }
            set
            {
                _ModelCopy.RepondentOpt = value; 
                RaisePropertyChanged(nameof(RespondentOpt));
            }
        }
    }
}
