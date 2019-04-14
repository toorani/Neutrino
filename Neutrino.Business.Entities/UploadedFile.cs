using System.ComponentModel.DataAnnotations;
using Espresso.Entites;

namespace Neutrino.Entities
{
    /// <summary>
    /// فایل های بارگذاری شده
    /// </summary>
    public class UploadedFile : EntityBase
    {
        public string OriginalFileName { get; set; }
        public string UploadedFileName { get; set; }
        public string HashValue { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}