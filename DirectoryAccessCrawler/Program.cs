using System.Security.AccessControl;
using System.IO;
using System.Security.Principal;
using System.Text;
using Mono.Options;
using System.Diagnostics;

#region Options
string targetDir = "";
string outFile = Environment.CurrentDirectory + "\\" + "output.csv";

bool shouldShowHelp = false;

var p = new OptionSet()
{
    {"t|target=", "Target Directory", (string h) => targetDir = h },

    {"o|outfile=", "the *.csv file the results should be saved to (default)", (string o) => outFile = o},
    {"?|help", "help message", s => shouldShowHelp = s != null }
};

void ShowHelp(OptionSet p)
{
    Console.WriteLine("Usage: DirectoryAccessCrawler [Target] + [OPTIONS]");
    Console.WriteLine("Iterate all subDirectories and save results to csv");
    Console.WriteLine("If no outFile is specified output.csv is used.");
    Console.WriteLine();
    Console.WriteLine("Options:");
    p.WriteOptionDescriptions(Console.Out);
}

List<string> extra;
try
{
    extra = p.Parse(args);
}
catch (OptionException e)
{
    Console.WriteLine(e.Message);
    Console.WriteLine(e.HelpLink);
    return;
}

// Check if Help should be displayed
if (shouldShowHelp)
{
    ShowHelp(p);
    return;
}
if (targetDir == "")
{
    targetDir = Environment.CurrentDirectory;
}


#endregion


List<DirectoryAccessCrawler.InfoContainer> infoContainers = new List<DirectoryAccessCrawler.InfoContainer>();

void CheckDirectory(DirectoryInfo dirInfo, DirectoryAccessCrawler.InfoContainer container)
{
    string output = "";
    try
    {
        DirectorySecurity directorySecurity = dirInfo.GetAccessControl();
        if (directorySecurity == null)
        {
            Console.WriteLine("Directory not found!");
            return;
        }

        Console.WriteLine(dirInfo.FullName);
        container.Directory = dirInfo.FullName;
        

        foreach (FileSystemAccessRule rule in directorySecurity.GetAccessRules(true, true, typeof(NTAccount)))
        {
            StringBuilder builder = new StringBuilder();
            if (rule.AccessControlType == AccessControlType.Deny)
            {
                builder.Append("[Deny] ");
            }
            if (rule.IsInherited)
            {
                builder.Append("[INHERITED] ");
            }
            builder.AppendFormat("{0}, ", rule.IdentityReference);
            builder.AppendFormat("{0} ", rule.FileSystemRights);


            Console.WriteLine(builder.ToString());
            output = output + builder.ToString();
        }
    }
    catch (System.UnauthorizedAccessException)
    {
        output = "Can't access file!";
        container.Directory = "Can't access file!";
    }
    Console.WriteLine();
    container.Rights = output;
    

}



void ProcessDirectory(string directory)
{
    try
    {
        DirectoryAccessCrawler.InfoContainer info = new DirectoryAccessCrawler.InfoContainer();

        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        CheckDirectory(directoryInfo, info);
        DirectoryInfo[] subdirs = directoryInfo.GetDirectories();

        //infoContainers.Add(info);

        foreach (DirectoryInfo subdir in subdirs)
        {
            DirectoryAccessCrawler.InfoContainer container = new DirectoryAccessCrawler.InfoContainer();
            CheckDirectory(subdir, container);
            ProcessDirectory(subdir.FullName);
            infoContainers.Add(container);
        }
    }
    catch (System.UnauthorizedAccessException)
    {

    }
}





var writer = new StreamWriter(outFile);
var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

Console.CancelKeyPress += delegate {
    writer.Close();
    Console.WriteLine("Terminated!");
    Environment.Exit(0);
};


Stopwatch sw = Stopwatch.StartNew();
DirectoryAccessCrawler.InfoContainer TMPcontainer = new DirectoryAccessCrawler.InfoContainer();
DirectoryInfo directoryInfo = new DirectoryInfo(targetDir);
CheckDirectory(directoryInfo, TMPcontainer);
infoContainers.Add(TMPcontainer);


ProcessDirectory(targetDir);
sw.Stop();
TimeSpan ts = sw.Elapsed;
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
Console.WriteLine("Crawling took {0}", elapsedTime);




csv.WriteHeader<DirectoryAccessCrawler.InfoContainer>();
csv.NextRecord();
foreach(DirectoryAccessCrawler.InfoContainer container in infoContainers)
{
    csv.WriteRecord(container);
    csv.NextRecord();
}

Console.WriteLine("Results Saved to: {0}", outFile);

writer.Close();







// Main

