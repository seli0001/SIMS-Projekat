using SIMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Domain.Model
{
    public class ComplexTour
    {
        public enum Status
        {
            Waiting,Rejected,Accepted
        }
        public string name { get; set; }

        public int ethapes { get; set; }

        public Status status {get;set;}


        public ComplexTour(string name,int ethapes,Status status)

        {
            this.name= name;
            this.ethapes = ethapes;
            this.status=   status;
        }







    }
}
