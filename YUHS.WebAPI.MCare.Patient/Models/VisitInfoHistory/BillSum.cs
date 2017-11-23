namespace YUHS.WebAPI.MCare.Patient.Models.VisitInfoHistory
{
    public class BillSum
    {
        public string hospitalCd { get; set; }
        public string organizationCd { get; set; }
        public string patientId { get; set; }
        public string ioFlag { get; set; }
        public string billNo { get; set; }
        public string receiptNo { get; set; }
        public string billReceiptNo { get; set; }
        public string billReceiptDt { get; set; }
        public string mainSubCd { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string totalAmt { get; set; }
        public string insureAmt { get; set; }
        public string patientAmt { get; set; }
        public string receiptAmt { get; set; }
        public string outstandingAmt { get; set; }
        public string treatDt { get; set; }
        public string admissionDt { get; set; }
        public string dischargeDt { get; set; }
    }
}