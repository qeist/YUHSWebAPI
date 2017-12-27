namespace YUHS.WebAPI.MCare.Patient.Models.Order
{
    public class Prescription
    {
        public string hospitalCd { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string ioFlag { get; set; }
        public string insureCd { get; set; }
        public string orderDt { get; set; }
        public string orderKind { get; set; }
        public string orderGrpCd { get; set; }
        public string orderCd { get; set; }
        public string orderNm { get; set; }
        public string doseQtyPer1Tim { get; set; }
        public string doseUnit { get; set; }
        public string doseQtyPer1Day { get; set; }
        public string doseDay { get; set; }
        public string dosageCd { get; set; }
        public string dosageNm { get; set; }
        public string effectCd { get; set; }
        public string effectNm { get; set; }
        public string comments { get; set; }

    }
}