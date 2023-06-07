using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
   public  class Ethape
    {
        public enum Status
        {
            Waiting, Rejected, Accepted
        }

        public string TourName { get; set; }
        public string EthapeName { get; set; }

        public Status status { get; set; }

        public Ethape(string name, string EthapeName, Status status)

        {
            this.TourName = name;
            this.EthapeName = EthapeName;
            this.status = status;
        }
    }
}
