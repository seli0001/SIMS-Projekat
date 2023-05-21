using SIMS.Domain.Model;
using SIMS.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Repository.GuideRepository
{
    class CheckpointRepository
    {
        private const string _filePath = "../../../../SIMS/Resources/Data/CheckPoints.csv";
        private readonly Serializer<Checkpoint> _serializer;

        public CheckpointRepository()
        {
            _serializer = new Serializer<Checkpoint>();
        }
        
        public int GetNextId(List<Checkpoint> checkpoints)
        {
            if(checkpoints.Count < 1)
            {
                return 0;
            }
            return checkpoints.Max(checkPoint => checkPoint.Id) + 1;
        }
        
        public List<Checkpoint> GetAll()
        {
            return _serializer.FromCSV(_filePath);
        }
        
        public List<Checkpoint> GetAllByTourId(int tourId)
        {
            List<Checkpoint> checkpoints = GetAll();
            return checkpoints.Where(checkPoint => checkPoint.Tour.Id == tourId).ToList();
        }
        
        public void Save(Checkpoint checkpoint)
        {
            List<Checkpoint> checkpoints = GetAll();
            checkpoint.Id = GetNextId(checkpoints);

            checkpoints.Add(checkpoint);
            _serializer.ToCSV(_filePath, checkpoints);
        }
        
        public void SaveAll(List<Checkpoint> checkpoints)
        {
            List<Checkpoint> allCheckpoints = GetAll();
            int id = 0;
            if (allCheckpoints != null)
            {
                id = allCheckpoints.Max(checkPoint => checkPoint.Id) + 1;
            }
            else
            {
                allCheckpoints = new List<Checkpoint>();
            }
            
            foreach (Checkpoint checkpoint in checkpoints)
            {
                checkpoint.Id = id;
                allCheckpoints.Add(checkpoint);
                id++;
            }
            
            _serializer.ToCSV(_filePath, allCheckpoints);
        }
        
        public void Update(Checkpoint checkpoint)
        {
            List<Checkpoint> checkpoints = GetAll();
            checkpoints.RemoveAll(c => c.Id == checkpoint.Id);
            checkpoints.Add(checkpoint);
            _serializer.ToCSV(_filePath, checkpoints);
        }
    }
}
