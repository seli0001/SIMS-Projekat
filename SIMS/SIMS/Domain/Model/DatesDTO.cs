using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class DatesDTO
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public DatesDTO(DateOnly startDate, DateOnly endDate)
        {
            StartDate = startDate;
            EndDate = endDate;  
        }
    }
}
