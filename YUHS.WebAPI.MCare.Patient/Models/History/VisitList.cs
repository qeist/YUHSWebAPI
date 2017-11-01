namespace YUHS.WebAPI.MCare.Models.History
{
    public class VisitList
    {
        public string hospitalCd { get; set; }
        public string patientId { get; set; }
        public string ioeFlag { get; set; }
        public string visitDt { get; set; }
        public string visitTm { get; set; }
        public string dischargeDt { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string receiptNo { get; set; }
        public string amountAmt { get; set; }
        public string receiptKindNm { get; set; }
    }
}