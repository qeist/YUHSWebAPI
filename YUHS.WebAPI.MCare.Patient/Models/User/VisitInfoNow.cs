namespace YUHS.WebAPI.MCare.Patient.Models.User
{
    public class VisitInfoNow
    {
        public string hospitalCd { get; set; }
        public string departmentCd { get; set; }
        public string departmentNm { get; set; }
        public string doctorId { get; set; }
        public string doctorNm { get; set; }
        public string visitDt { get; set; }
        public string visitTm { get; set; }
        public string visitKind { get; set; }
        public string visitMemo { get; set; }
        public string payYn { get; set; }
        public string statusCd { get; set; }
        public string statusNm { get; set; }
        public string nextLoc { get; set; }
        public string poiNm { get; set; }
    }
}