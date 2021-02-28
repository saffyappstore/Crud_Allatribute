using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelLayer
{
    public  class ColumnGridDetailsDto
    {
        public List<ColumnDto> visibleColumns { get; set; }

        public List<ColumnDto> hiddenColumns { get; set; }

        public ColumnGridDetailsDto()
        {
            visibleColumns = new List<ColumnDto>();

            hiddenColumns = new List<ColumnDto>();
        }
    }
}
