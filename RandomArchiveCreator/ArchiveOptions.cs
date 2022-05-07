using System;
using System.Collections.Generic;
using System.Text;

namespace RandomArchiveCreator
{
    public class ArchiveOptions
    {
        public string ArchiveName { get; set; }
        public string Crn { get; set; }
        public string IsForReporter { get; set; }
        public IList<FileOptions> FileOptions { get; set; }
    }

    public class FileOptions
    {
        public string Prefix { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public string Unit { get; set; }
        public int NumberOfFiles { get; set; }
    }
}
