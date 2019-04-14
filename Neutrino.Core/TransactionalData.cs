using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Espresso.Core;

namespace Neutrino.Core
{
    public class TransactionalData : ITransactionalData
    {
        public bool ReturnStatus { get; set; }
        public List<String> ReturnMessage { get; set; }
        public Hashtable ValidationErrors { get; set; }

        public TransactionalData()
        {
            RefreshData();
        }

        public ITransactionalData CreateInstance()
        {
            return new TransactionalData();
        }

        public void RefreshData()
        {
            ReturnMessage = new List<String>();
            ReturnStatus = true;
            ValidationErrors = new Hashtable();
        }
    }
}
