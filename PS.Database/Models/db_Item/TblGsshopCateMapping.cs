﻿using System;
using System.Collections.Generic;

namespace PS.Database.Models.db_Item
{
    public partial class TblGsshopCateMapping
    {
        public string CateKey { get; set; }
        public string TenCateLarge { get; set; }
        public string TenCateMid { get; set; }
        public string TenCateSmall { get; set; }
        public DateTime Lastupdate { get; set; }
    }
}
