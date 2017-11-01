using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Patient.Models.User
{
    public class ExtMember
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string pId_sc { get; set; }
        public string pId_gs { get; set; }
        public string pId_yi { get; set; }
        public string dormancy_yn { get; set; }
        public string fCnt { get; set; }
    }
}