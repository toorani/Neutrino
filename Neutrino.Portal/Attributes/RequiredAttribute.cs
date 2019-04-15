using System.ComponentModel.DataAnnotations;

namespace Neutrino.Portal.Attributes
{
    public class NeuRequiredAttribute : RequiredAttribute
    {
        #region [ Varibale(s) ]
        private readonly string  defaultMsg = "{0} is required";
        private string errorMsg = "";
        #endregion

        #region [ Constructor(s) ]
        public NeuRequiredAttribute()
        {
            AllowEmptyStrings = false;
            this.ErrorMessageResourceType = typeof(App_LocalResources.NuResource);
            this.ErrorMessageResourceName = "RequireField";
        }
        #endregion

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        }
    }
}