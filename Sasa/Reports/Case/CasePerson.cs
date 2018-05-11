using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.DataAccess.EF;

namespace Sasa.Reports.Case
{
    public class CasePerson
    {
        private readonly DataAccess.EF.Case _cases;
        private readonly Person _person;

        public CasePerson(DataAccess.EF.Case cases, Person person)
        {
            _cases = cases;
            _person = person;
        }

        public string CaseId
        {
            get { return _cases.CaseId; }
            set { _cases.CaseId = value; }
        }

        public string CaseName
        {
            get { return _cases.CaseName; }
            set { _cases.CaseName = value; }
        }

        public string CaseLevel
        {
            get { return _cases.CaseLevel; }
            set { _cases.CaseLevel = value; }
        }

        public DateTime? CaseHearingDate
        {
            get { return _cases.CaseHearingDate; }
            set { _cases.CaseHearingDate = value; }
        }

        public string CaseStatus
        {
            get { return _cases.CaseStatus; }
            set { _cases.CaseStatus = value; }
        }

        public string Complainant
        {
            get { return _cases.Complainant; }
            set { _cases.Complainant = value; }
        }

        public string RespondentFirstName
        {
            get { return _person.FirstName; }
            set { _person.FirstName = value; }
        }

        public string RespondentLastName
        {
            get { return _person.LastName; }
            set { _person.LastName = value; }
        }


    }
}
