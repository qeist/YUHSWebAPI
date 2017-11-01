namespace YUHS.WebAPI.MCare.Patient.Models.Reservation
{
    public class ReservationResult
    {

    }
    public class ReservationResult1
    {
        public string RcptGb { get; set; }
        public string ChosYmd { get; set; }
        public string LstClnDrId { get; set; }
    }

    public class ReservationResult2
    {
        public string WebRsvErrChk { get; set; }
        public string WebRsvErrMsg { get; set; }
        public string ChosNo { get; set; }
    }

    public class ReservationResult3
    {
        public string hospitalCd { get; set; }
        public string returnCd { get; set; }
        public string returnMsg { get; set; }

    }






}