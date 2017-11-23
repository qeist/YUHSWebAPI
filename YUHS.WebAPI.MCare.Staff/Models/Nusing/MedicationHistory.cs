using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Staff.Models.Nusing
{
    public class MedicationHistory
    {
        public string OrdYmd { get; set; }
        public string OrdNo { get; set; }
        public string DrgInjExecSeq { get; set; }
        public string OrdCd { get; set; }
        public string MedHmCd { get; set; }
        public string DsgCd { get; set; }
        public string Qty { get; set; }
        public string Frq { get; set; }
        public string OrdSeq { get; set; }
        public string OrdNm { get; set; }
        public string PRNOrdKindCd { get; set; }
        public string DrgInjExecStus { get; set; }
        public string DrgInjExecStrYmd { get; set; }
        public string DrgInjExecStrHms { get; set; }
        public string DrgInjExecEndYmd { get; set; }
        public string DrgInjExecEndHms { get; set; }
        public string ExecYn { get; set; }
        public string HoldResn { get; set; }
        public string DrgInjExecId { get; set; }
        public string InjExecStrId { get; set; }
        public string ClnDeptCd { get; set; }
        public string StusCd { get; set; }
        public string ExecStrHms { get; set; }
        public string ExecEndHms { get; set; }
        public string OrdKindCd { get; set; }
        public string Dcnt { get; set; }
        public string OrdTypCd { get; set; }
        public string OrdCd { get; set; }
        public string TQty { get; set; }
        public string CrNo { get; set; }
        public string PrescType { get; set; }
        public string SpcCd { get; set; }
        public string OrdGb { get; set; }
        public string KubGb { get; set; }
        public string OrdGrGb { get; set; }
        public string DrgYn { get; set; }
        public string Rmk { get; set; }
        public string ReqInfo { get; set; }
        public string HosiGb { get; set; }
        public string HosiResnCd { get; set; }
        public string CpYn { get; set; }
        public string OrdProgLoc { get; set; }
    }
}