using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalKendaraan_072.Models.Upload
{
    public class FileDetails
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
    public class FilesViewModel
    {
        public List<FileDetails> Files { get; set; } = new List<FileDetails>();
    }
}
