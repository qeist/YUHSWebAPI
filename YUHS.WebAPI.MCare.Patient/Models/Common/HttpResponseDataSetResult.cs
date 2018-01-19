using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace YUHS.WebAPI.MCare.Patient.Models.Common
{
    public class HttpResponseDataSetResult<T> where T: DataSet
    {
        public T result { get; set; }

        public ErrorInfo error { get; set; }
    }
}