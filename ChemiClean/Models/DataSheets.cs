using System;
using System.Collections.Generic;

namespace ChemiClean.Models
{
    public partial class DataSheets
    {
        public int DataSheetId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string DataSheetUrl { get; set; }
        public string? LocalURL { get; set; }
        public byte[]? HashValue { get; set; }
        public bool? IsValid { get; set; }
        public DateTime? UpdatedAt{ get; set; }
        public int? UserId { get; set; }
    }
}
