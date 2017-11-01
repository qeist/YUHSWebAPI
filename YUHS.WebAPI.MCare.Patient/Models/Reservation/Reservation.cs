namespace YUHS.WebAPI.MCare.Patient.Models.Reservation
{
    public class Reservation
    {
        public string hospitalCd { get; set; }
        public string patientId { get; set; }
        public string reservedDt { get; set; }
        public string reservedTm { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string receiptKindNm { get; set; }
        public string reservationPath { get; set; }
        public string orderExamKindNm { get; set; }
        public string receiptNo { get; set; }
    }
}