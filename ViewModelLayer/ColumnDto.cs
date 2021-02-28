using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer
{
   public class ColumnDto
    {
        public int ColumnId { get; set; }
        public string ColumnText { get; set; }
        public string GridName { get; set; }
        public string ColumnType { get; set; }
        public string ColumnFilterId { get; set; }
        public string ColumnWidth { get; set; }
        public int? OrderBy { get; set; }
        public bool isMandatory { get; set; }
        public bool Isvisible { get; set; }


    }
}
