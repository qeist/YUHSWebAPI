using System.Collections.Generic;

namespace YUHS.WebAPI.MCare.Staff.Models.TESExamRslt
{
    public class ExamRst
    {
        public string hospitalCd { get; set; }
        public string highClassCd { get; set; }
        public string highClassNm { get; set; }
        public string middleClassCd { get; set; }
        public string middleClassNm { get; set; }
        public string examinationCd { get; set; }
        public string examinationNm { get; set; }
        public string examinationDt { get; set; }
        public string examinationRst { get; set; }
        public string pacs { get; set; }
        public string ShtIdxNo { get; set; }
        public string AccNo { get; set; }
        public string RsltKey { get; set; }
        public string FrmClnKey { get; set; }
        public string OrdYmd { get; set; }
        public string OrdSlipCd { get; set; }

    }

    public class ExamRsts
    {
        public IEnumerable<IList<ExamRst>> examRsts { get; set; }
    }

}