﻿using System;
using System.Collections.Generic;

namespace PS.Database.Models.db_Item
{
    public partial class TblItemQuickdlvInvalid
    {
        public int Itemid { get; set; }
        public string Lastuserid { get; set; }
        public DateTime? Lastupdate { get; set; }
    }
}
