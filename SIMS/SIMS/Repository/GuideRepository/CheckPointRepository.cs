using SIMS.Model.Guide;
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
        private const string filePath = "../../../../SIMS/Resources/Data/CheckPoints.csv";
        private readonly Serializer<Checkpoint> serializer;

        public CheckpointRepository()
        {
            serializer = new Serializer<Checkpoint>();
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
            return serializer.FromCSV(filePath);
        }
        
        public List<Checkpoint> GetByTourId(int tourId)
        {
            List<Checkpoint> checkpoints = GetAll();
            return checkpoints.Where(checkPoint => checkPoint.Tour.Id == tourId).ToList();
        }
        
        public void Save(Checkpoint checkpoint)
        {
            List<Checkpoint> checkpoints = GetAll();
            checkpoint.Id = GetNextId(checkpoints);

            checkpoints.Add(checkpoint);
            serializer.ToCSV(filePath, checkpoints);
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
            
            serializer.ToCSV(filePath, allCheckpoints);
        }
        
    }
}
