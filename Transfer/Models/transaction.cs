﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Transfer.Models
{
    public partial class transaction
    {
        [Key]
        public int? idtransactions { get; set; }
        public string borrower_name { get; set; }
        public string lender_name { get; set; }
        public int? amount { get; set; }
        public string transaction_type { get; set; }
        public DateTime? transaction_date { get; set; }
    }
}