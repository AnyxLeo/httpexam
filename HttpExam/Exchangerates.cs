﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace httpexam
{
    public class Exchangerates
    {
        public Dictionary<string, decimal> rates { get; set; }
        public string  Base { get; set; }
        public DateTime date { get; set; }
    }
}
