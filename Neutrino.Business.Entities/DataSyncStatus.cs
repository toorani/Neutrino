using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    public class DataSyncStatus : EntityBase
    {
        [StringLength(50)]
        public string ServiceName { get; set; }
        public bool IsInsertedAllData { get { return ReadedCount == InsertedCount; } }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ReadedCount { get; set; }
        public int InsertedCount { get; set; }
        public int TryCount { get; set; }
        public DataSyncStatus()
        {
            TryCount = 0;
        }
    }
}