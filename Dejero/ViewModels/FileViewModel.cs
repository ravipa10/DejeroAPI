using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dejero.Models
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Labels { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
