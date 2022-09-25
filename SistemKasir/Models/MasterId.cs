using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public partial class MasterId
    {
        [Key]
        public int IdMaster { get; set; }

        public string? PrefixId { get; set; }

        public int? Counter { get; set; }
    }
}
