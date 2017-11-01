namespace YUHS.WebAPI.MCare.Patient.Models.Reservation
{
    public class ReserveDr
    {
        public string hospitalCd { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string majorPartNm { get; set; }
        public string specialYn { get; set; }
    }
}