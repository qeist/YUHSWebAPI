namespace YUHS.WebAPI.MCare.Patient.Models.VisitInfoHistory
{
    public class BillDetail
    {
        public string hospitalCd { get; set; }
        public string patientId { get; set; }
        public string mainSubCd { get; set; }
        public string accumulationLclsCd { get; set; }
        public string accumulationLclsNm { get; set; }
        public string patientAmt { get; set; }
        public string insureAmt { get; set; }
        public string fullPatientAmt { get; set; }
        public string selectMedicalAmt { get; set; }
        public string exceptSelectMedicalAmt { get; set; }
        public string totalAmt { get; set; }
    }
}