namespace YUHS.WebAPI.MCare.Patient.Models.Arrive
{
    public class ArriveConfirmTargetList
    {
        public string hospitalCd { get; set; } = "";
        public string reservedDt { get; set; } = "";
        public string reservedTm { get; set; } = "";
        public string departmentCd { get; set; } = "";
        public string departmentNm { get; set; } = "";
        public string doctorId { get; set; } = "";
        public string doctorNm { get; set; } = "";
        public string statusCd { get; set; } = "";
        public string statusNm { get; set; } = "";
        public string receiptNo { get; set; } = "";
    }
}