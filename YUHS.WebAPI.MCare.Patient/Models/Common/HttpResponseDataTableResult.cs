using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Patient.Models.Common
{
    public class HttpResponseDataTableResult<T> where T : DataTable
    {
        public T result { get; set; }

        public ErrorInfo error { get; set; }
    }
}