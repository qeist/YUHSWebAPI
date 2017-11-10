using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Staff.Models.Nusing
{
    public class MedicationHistory
    {
        public string OrdYmd { get; set; }
        public string OrdKindCd1 { get; set; }
        public string ClnDeptCd { get; set; }
        public string OrdNm { get; set; }
        public string OrdYmd1 { get; set; }
        public string OrdRetrRnk { get; set; }
        public string HosiGb { get; set; }
        public string ChosGb { get; set; }

    }
}