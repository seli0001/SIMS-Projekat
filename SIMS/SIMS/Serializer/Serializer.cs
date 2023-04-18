using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS.Serializer
{
    class Serializer<T> where T : ISerializable, new()
    {
        private const char Delimiter = '|';

        private static object _lockObject = new object();

        public void ToCSV(string fileName, List<T> objects)
        {
            lock (_lockObject)
            {
                StringBuilder csv = new StringBuilder();

                foreach (T obj in objects)
                {
                    string line = string.Join(Delimiter.ToString(), obj.ToCSV());
                    csv.AppendLine(line);
                }

                File.WriteAllText(fileName, csv.ToString());
            }

        }

        public List<T> FromCSV(string fileName)
        {
            lock (_lockObject)
            {
                List<T> objects = new List<T>();

                foreach (string line in File.ReadLines(fileName))
                {
                    string[] csvValues = line.Split(Delimiter);
                    T obj = new T();
                    obj.FromCSV(csvValues);
                    objects.Add(obj);
                }

                return objects;
            }
        }
    }
}
