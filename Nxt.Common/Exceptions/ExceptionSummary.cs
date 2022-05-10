﻿using System;
using System.Globalization;

namespace Nxt.Common.Exceptions
{
    public class ExceptionSummary
    {
        public string UserMessage { get; set; }
        public string ValidationMessage { get; set; }
        public string SupportMessages { get; set; }
        public string ReferenceId { get; set; }
        public string TimeStamp => DateTime.Now.ToString(CultureInfo.InvariantCulture);
    }
}
