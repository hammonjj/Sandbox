using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace RandomArchiveCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                //Show help
            }

            var csvOutput = "CustomerReferenceNumber,IsForReporter,AttachmentCreated,FileName,RelativeFolderPath\n";
            var optionsString = File.ReadAllText(args[0]);

            var archiveOptions = JsonConvert.DeserializeObject<ArchiveOptions>(optionsString);
            Console.WriteLine("Successfully Read Options File");

            var path = Directory.GetCurrentDirectory();
            var directoryInfo = Directory.CreateDirectory($"{archiveOptions.ArchiveName}");

            var processList = new List<System.Diagnostics.Process>();
            foreach(var fileOption in archiveOptions.FileOptions)
            {
                for(int i = 0; i < fileOption.NumberOfFiles; i++)
                {
                    //rdfc.exe <file> <size> [<unit> [<overwrite>]]
                    var combinedPath = Path.Combine(directoryInfo.FullName, $"{fileOption.Prefix}{i}.{fileOption.Extension}");

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        FileName = $"{path}\\rdfc.exe",
                        Arguments = $"{combinedPath} {fileOption.Size} {fileOption.Unit}"
                    };

                    process.StartInfo = startInfo;
                    process.Start();
                    processList.Add(process);

                    csvOutput += $"{archiveOptions.Crn},{archiveOptions.IsForReporter},5/20/2020,{fileOption.Prefix}{i}.{fileOption.Extension},\\\n";
                }
            }

            //Write CSV File
            File.WriteAllText(Path.Combine(path, $"{archiveOptions.ArchiveName}.csv"), csvOutput);

            //Check if processes are running
            bool isComplete;
            do
            {
                isComplete = true;
                foreach(var process in processList)
                {
                    process.Refresh();

                    if(!process.HasExited)
                    {
                        isComplete = false;
                    }
                }
            } while(!isComplete);

            Console.WriteLine("Creating Zip Archive");

            ZipFile.CreateFromDirectory($".\\{archiveOptions.ArchiveName}", $".\\{archiveOptions.ArchiveName}.zip");

            Console.WriteLine("Finished Creating Zip Archive");
        }
    }
}
