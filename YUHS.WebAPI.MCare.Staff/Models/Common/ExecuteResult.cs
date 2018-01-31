using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Staff.Models.Common
{
    public class ExecuteResult
    {
        public string hospitalCd { get; set; }
        public string returnValue { get; set; }
        public string ReturnMsg { get; set; }
    }
}