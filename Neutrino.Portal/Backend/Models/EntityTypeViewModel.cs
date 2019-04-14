using Espresso.Portal;
using Espresso.Core;

namespace Neutrino.Portal.Models
{
    public class TypeEntityViewModel : ViewModelBase
    {
        #region [ Public Property(ies) ]
        public int eId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Code { get; set; }
        #endregion

        #region [ Constructor(s) ]
        public TypeEntityViewModel()
        {
            
        }
        #endregion


    }
}