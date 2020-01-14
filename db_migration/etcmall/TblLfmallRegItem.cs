﻿using System;
using System.Collections.Generic;

namespace db_migration.etcmall
{
    public partial class TblLfmallRegItem
    {
        public int Itemid { get; set; }
        public DateTime Regdate { get; set; }
        public string Reguserid { get; set; }
        public DateTime? LfmallRegdate { get; set; }
        public DateTime? LfmallLastUpdate { get; set; }
        public string LfmallGoodNo { get; set; }
        public decimal LfmallPrice { get; set; }
        public string LfmallSellYn { get; set; }
        public int LfmallStatCd { get; set; }
        public DateTime? LastConfirmdate { get; set; }
        public int? RctSellCnt { get; set; }
        public int? RegedOptCnt { get; set; }
        public int? AccFailCnt { get; set; }
        public string LastErrStr { get; set; }
        public DateTime? LastStatCheckDate { get; set; }
        public string Regitemname { get; set; }
        public int? OptAddPrcCnt { get; set; }
        public int? OptAddPrcRegType { get; set; }
        public string RegImageName { get; set; }
        public int? EditQuecnt { get; set; }
        public int? SetMargin { get; set; }
        public int? ProductGroupNoticeNo { get; set; }
        public DateTime? SellStartDate { get; set; }
        public string Catekey { get; set; }
    }
}
