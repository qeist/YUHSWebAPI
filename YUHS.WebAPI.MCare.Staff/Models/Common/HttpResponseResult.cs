﻿using System.Collections.Generic;

namespace YUHS.WebAPI.MCare.Patient.Models.Common
{
    public class HttpResponseResult<T>
    {
        public IEnumerable<T> result { get; set; }
        public ErrorInfo error { get; set; }
    }

    public class ErrorInfo
    {
        public bool flag { get; set; }
        public string message { get; set; }
    }
}