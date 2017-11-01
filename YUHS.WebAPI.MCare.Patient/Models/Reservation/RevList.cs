namespace YUHS.WebAPI.MCare.Patient.Models.Reservation
{
    public class RevList
    {
        public string hospitalCd { get; set; }
public string pId { get; set; }
        public string dataDate { get; set; }
        public string dataTime { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorNm { get; set; }
        public string receiptGubunKindNm { get; set; }
        public string reservationMethod { get; set; }
        public string ordExamKindNm { get; set; }
        public string doctorId { get; set; }
        public string receiptNo { get; set; }
    }
}