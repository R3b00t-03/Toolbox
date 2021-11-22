using Mono.Options;


string host_or_address = "";
string outFile = Environment.CurrentDirectory + "\\" + "output.csv";
int interval = 10;
bool shouldShowHelp = false;

var p = new OptionSet()
{
    {"h|host=", "the host or address to ping", (string h) => host_or_address = h },
    {"i|interval=", "the time between pings (in seconds)", (int i) => interval = i },
    {"o|outfile=", "the *.csv file the results should be saved to (default)", (string o) => outFile = o},
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

// Check if Host is empty
if(host_or_address == "" || host_or_address == null)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Please define a Host!");
    Console.ResetColor();
    return;
}


var writer = new StreamWriter(outFile);
var csv = new CsvHelper.CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);

Console.CancelKeyPress += delegate{
    writer.Close();
    Console.WriteLine("Terminated!");
    Environment.Exit(0);
};

Pinger.Pinger pinger = new Pinger.Pinger(host_or_address);
csv.WriteHeader<Pinger.PingResultsItemTemplate>();
csv.NextRecord();


while (true)
{
    //Ping
    pinger.SendPing();
    #region Message
    //Console.WriteLine("Host: {0}, RTT: {1}, Pingable: {2}, Timestamp: {3}", pinger.Host, pinger.RoundTripTime, pinger.Pingable, pinger.Timestamp);
    Console.Write("Host: {0}, ", pinger.Host);
    Console.Write("RTT: ");
    if (pinger.Pingable)
    {
        if (pinger.RoundTripTime <= 20)
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        else if (pinger.RoundTripTime >= 21 && pinger.RoundTripTime <= 50)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        else if (pinger.RoundTripTime >= 51 && pinger.RoundTripTime <= 100)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        else if (pinger.RoundTripTime >= 101 && pinger.RoundTripTime <= 300)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else if (pinger.RoundTripTime > 300)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
    }
    
    Console.Write("{0}", pinger.RoundTripTime);
    Console.ResetColor();
    Console.Write(", ");

    Console.Write("Pingable: ");

    if (pinger.Pingable) { Console.ForegroundColor = ConsoleColor.Green; }
    else { Console.ForegroundColor = ConsoleColor.Red; }

    Console.Write("{0}", pinger.Pingable);

    Console.ResetColor();
    Console.Write(", ");

    Console.Write("Timestamp: {0}\n", pinger.Timestamp);
    #endregion



    //save Result to Object
    Pinger.PingResultsItemTemplate result = new Pinger.PingResultsItemTemplate();
    result.Host = pinger.Host;
    result.roundTripTime = pinger.RoundTripTime;
    result.Pingable = pinger.Pingable;
    result.TimeStamp = pinger.Timestamp;
    
    //Save to csv
    csv.WriteRecord(result);
    csv.NextRecord();

    //Sleep
    System.Threading.Thread.Sleep(interval * 1000);
}

