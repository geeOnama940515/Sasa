using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Sasa.Reports.Case
{
    public class CaseDataSetModel:ObservableObject
    {
        public CaseDataSetModel()
        {
            
        }

        private string _caseid;
        public string CaseId
        {
            get { return _caseid; }
            set
            {
                _caseid = value;
                RaisePropertyChanged(nameof(CaseId));
            }
        }

        private string _casename;
        public string CaseName
        {
            get { return _casename; }
            set
            {
                _casename = value;
                RaisePropertyChanged(nameof(CaseName));
            }
        }
        private string _caselevel;
        public string CaseLevel
        {
            get { return _caselevel; }
            set
            {
                _caselevel = value;
                RaisePropertyChanged(nameof(CaseLevel));
            }
        }
        private string _caseheaeringdate;
        public string CaseHearingDate
        {
            get { return _caseheaeringdate; }
            set
            {
                _caseheaeringdate = value;
                RaisePropertyChanged(nameof(CaseHearingDate));
            }
        }
        private string _casestatus;
        public string CaseStatus
        {
            get { return _casestatus; }
            set
            {
                _casestatus = value;
                RaisePropertyChanged(nameof(CaseStatus));
            }
        }
        private string _complainant;
        public string Complainant
        {
            get { return _complainant; }
            set
            {
                _complainant = value;
                RaisePropertyChanged(nameof(Complainant));
            }
        }
        private string _repondent;
        public string Respondent
        {
            get { return _repondent; }
            set
            {
                _repondent = value;
                RaisePropertyChanged(nameof(Respondent));
            }
        }

    }
}
