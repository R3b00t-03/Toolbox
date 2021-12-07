using Mono.Options;


long size = 1024 * 1024 * 16;
string outFile = Environment.CurrentDirectory + "\\" + "output.dummy";
string unit = "B";
bool shouldShowHelp = false;

var p = new OptionSet()
{

    {"s|size=", "The size in Bytes", (long _size) => size = _size },
    {"u|unit=", "Unit (KB, MB, GB)", (string _unit) => unit = _unit },
    {"o|outfile=", "name of Dummy File", (string o) => outFile = o},
    {"?|help", "help message", s => shouldShowHelp = s != null }
};

void ShowHelp(OptionSet p)
{
    Console.WriteLine("Usage: DFG -o [outfile] -s [size] -u [unit]");
    Console.WriteLine("Unit (KB, MB, GB) default B");
    Console.WriteLine("Outfile default output.dummy");
    Console.WriteLine("Size default 16MB");
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

switch (unit.ToUpper())
{
    case "B":
        size = size;
        break;
    case "KB":
        size = size * 1024;
        break;
    case "MB":
        size = size * 1024 * 1024;
        break;
    case "GB":
        size = size * 1024 * 1024 * 1024;
        break;
    default:
        return;
        break;
}


FileStream fs = new FileStream(outFile, FileMode.CreateNew);
fs.Seek(size, SeekOrigin.Begin);
fs.WriteByte(0);
fs.Close();
Console.WriteLine("{0} {1} Bytes written to {2}", size, unit, outFile);


