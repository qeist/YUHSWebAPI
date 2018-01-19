namespace YUHS.WebAPI.MCare.Patient.Models.CheckUp
{
    public class CheckupReservation
    {
        public string hospitalCd { get; set; } = "";
        public string checkupDt { get; set; } = "";
        public string checkupTm { get; set; } = "";
        public string visitNo { get; set; } = "";
        public string checkupKind { get; set; } = "";
        public string checkupPgmNm { get; set; } = "";
    }
}