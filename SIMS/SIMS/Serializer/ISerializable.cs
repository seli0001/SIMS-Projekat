using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Serializer
{
    internal interface ISerializable
    {
        string[] ToCSV();
        void FromCSV(string[] csv);

    }
}
