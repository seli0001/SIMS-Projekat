using SIMS.Model;
using SIMS.Model.AccommondationModel;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository
{
    internal class AccommondationRepository
    {
        private const string filePath = "../../../../SIMS/Resources/Data/Accommondations.csv";
        private readonly Serializer<Accommondation> _serializer;

        public AccommondationRepository()
        {
            _serializer = new Serializer<Accommondation>();
        }
    }
}
