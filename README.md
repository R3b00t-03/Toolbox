# UniversalToolkit
Some Random tools

## Tools i eventually start developing:
- [x] Pinger
- [ ] Pinger (GUI)
- [x] DirectoryAccessCrawler
- [ ] DirectoryAccessCrawler (GUI)
- [ ] Dummy File Generator
- [ ] Dummy File Generator (GUI)

## Pinger
This tool continuously sends Pings to a given host and writes them into a .CSV record.
### Usage
```Powershell
PingerLog -h [Hostname or IP] -o [output file] -i [intervall]
```
### Sample Output
```Console
Host: 8.8.8.8, RTT: 6, Pingable: True, Timestamp: 07.12.2021 09:56:56
Host: 8.8.8.8, RTT: 6, Pingable: True, Timestamp: 07.12.2021 09:57:11
Host: 8.8.8.8, RTT: 6, Pingable: True, Timestamp: 07.12.2021 09:57:26
```

## DirectoryAccessCrawler
This tool runs recursively through the file system and retrieves folder access rights into a .CSV record.

### Usage:
```Powershell
DirectoryAccessCrawler -t [root folder] -o [outputfile .csv]
```

## DummyFileGenerator
This tool generates files with random data in specified size.
### Usage:
```Powershell
DFG -o [outputFile] -s [size] -u [Unit]
```
Unit (KB, MB, GB) default B <br/>
Outfile default output.dummy<br/>
Size default 16MB<br/>

Options:<br/>
  -s, --size=VALUE           The size in Bytes<br/>
  -u, --unit=VALUE           Unit (KB, MB, GB)<br/>
  -o, --outfile=VALUE        name of Dummy File<br/>
  -?, --help                 help message