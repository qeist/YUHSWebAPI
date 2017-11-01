namespace YUHS.WebAPI.MCare.Patient.Models.Reservation
{
    public class ReserveDate
    {
        public string hospitalCd { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string availReserveDt { get; set; }
        public string weekCd { get; set; }
        public string weekNm { get; set; }
    }
}