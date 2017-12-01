namespace YUHS.WebAPI.MCare.Staff.Models.Consult
{
    public class ConsultInfo
    {
        public string hospitalCd { get; set; }
        public string requestTxt { get; set; }
        public string responseTxt { get; set; }
        public string patientId { get; set; }
        public string patientNm { get; set; }
        public string inDt { get; set; }
        public string diagnosisCd { get; set; }
        public string diagnosisNm { get; set; }
        public string operatingDt { get; set; }
        public string operatingNm { get; set; }
        public string responseDtTm { get; set; }
        public string responseDrId { get; set; }
        public string requestDrTel { get; set; }
    }
}