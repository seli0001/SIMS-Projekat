using SIMS.Domain.Model;
using SIMS.Repository.GuideRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Service.Services
{
    internal class CheckpointService
    {
        private readonly CheckpointRepository _checkpointRepository;

        public CheckpointService()
        {
            _checkpointRepository = new CheckpointRepository();
        }




        public List<Checkpoint> GetAllByTourId(int tourId)
        {
            return _checkpointRepository.GetAllByTourId(tourId).ToList();
        }
    }
}
