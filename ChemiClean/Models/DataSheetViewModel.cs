using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChemiClean.Models
{
    public class DataSheetViewModel
    {
        public int DataSheetId { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string DataSheetUrl { get; set; }
        public int? IsValid { get; set; }
        public string LocalUrl { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
