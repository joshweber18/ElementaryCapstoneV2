﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V2Capstone.Models
{
    public class MessageModel
    {
        public int ID { get; set; }
        public string AddedBy { get; set; }
        public string message { get; set; }
        public int GroupId { get; set; }
    }
}