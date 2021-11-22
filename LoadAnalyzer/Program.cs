using System.Diagnostics;
using Mono.Options;
using System.Management;
using Microsoft.Management.Infrastructure;

bool shouldShowHelp = false;
string host = "";

var p = new OptionSet()
{
    {"h|host=", "the host or address to ping", (string h) => host = h },
    {"?|help", "help message", s => shouldShowHelp = s != null }
};

void ShowHelp(OptionSet p)
{
    Console.WriteLine("Usage: Pinger [host] + [OPTIONS]");
    Console.WriteLine("Ping the defined host and write result to csv file");
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
Console.CancelKeyPress += delegate {
    Console.WriteLine("Terminated!");
    Environment.Exit(0);
};
string computer = @"\\" + host + @"\";
ManagementObjectSearcher searcher = new ManagementObjectSearcher(computer + @"root\CIMV2", "SELECT * FROM Win32_Processor");
ManagementObjectCollection colItems = searcher.Get();

foreach (ManagementObject queryObj in colItems)
{
    Console.WriteLine("Load: {0}", queryObj["LoadPercentage"]);
}