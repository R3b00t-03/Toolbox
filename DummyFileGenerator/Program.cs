using Mono.Options;


long size = 1024 * 1024 * 16;
string outFile = Environment.CurrentDirectory + "\\" + "output.dummy";

bool shouldShowHelp = false;

var p = new OptionSet()
{

    {"s|size=", "The size in Bytes", (long _size) => size = _size },
    {"o|outfile=", "name of Dummy File", (string o) => outFile = o},
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

FileStream fs = new FileStream(outFile, FileMode.CreateNew);
fs.Seek(size, SeekOrigin.Begin);
fs.WriteByte(0);
fs.Close();
Console.WriteLine("{0} Bytes written to {1}", size, outFile);


