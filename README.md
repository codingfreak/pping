# pping
A port ping console for Windows.

## Purpose

pping is designed to give you the easiest possible solution for discovering ports from a windows console. The design was heavily oriented towards the terminology and behavior of the classic ping tool under windows.

## Implementation

pping is developed using Microsoft .NET Framework version 4.6. It's delivered in the form of a copy-&-paste-deployment meaning that you just need 3 files in a single folder to be able to start it.

pping uses 2 libraries (dll) from the codingfreasks-project [cfUtils](https://github.com/codingfreak/cfUtils):
- cfUtils.cfBaseUtils.dll
- cfUtils.cfPortableUtils.dll
The 4rd file you need in the folder is pping.exe.

## Basic usage

The simplest possible call would be to check a single port on a DNS address or IP address. The following snippets checks, if port `80` is open on the address `google.com`:

    pping google.com 80

The result will be something like this:

    > pping google.com 80
    Starting pinging host google.com on TCP port(s) 80 4 times:
    #   1 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   2 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   3 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   4 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    Finished pinging host google.com (IP:-). 4 pings sent (4 OPEN, 0 CLOSED)

By default pping will try to reach a port 4 times.If you want pping to resolve the IP address behind a DNS entry just add the `-res` parameter:

    pping google.com 80 -res
    
    Starting pinging host google.com on TCP port(s) 80 4 times:
    #   1 -> Pinging host google.com (IP:2a00:1450:4005:808::200e) on TCP port 80 with timeout 1: OPEN
    #   2 -> Pinging host google.com (IP:2a00:1450:4005:808::200e) on TCP port 80 with timeout 1: OPEN
    #   3 -> Pinging host google.com (IP:2a00:1450:4005:808::200e) on TCP port 80 with timeout 1: OPEN
    #   4 -> Pinging host google.com (IP:2a00:1450:4005:808::200e) on TCP port 80 with timeout 1: OPEN
    Finished pinging host google.com (IP:2a00:1450:4005:808::200e). 4 pings sent (4 OPEN, 0 CLOSED)

You can perform constant pings too:

    pping google.com 80 -t

This will perform and "endless" ping to the address. 

## Special usage

### Waiting for a port to open

On of the common scenarios to use ping is to determine, if a certain service is already up. Consider a situation where you restart a server and want to know, when RDP connections are possible. A simple ping would'nt help you here because even if the machine is up, the RDP port (3389) might no be reachable yet. 

pping can help you in this situation:

    pping myserver.local 3389 -t -as

You perform a permanent pping (`-t`) but you tell pping to stop pinging when the port is reachable the first time (`-as`). "as" is the abbreviation for "autostop".

### Multiple ports

If you want to check lets say 2 ports (http and https) on a certain address. Just delimit the ports by `,`:

    pping google.com 80,443

    Starting pinging host google.com on TCP port(s) 80,443 4 times:
    #   1 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: CLOSED
    #   2 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: CLOSED
    #   3 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   4 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: OPEN
    #   5 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   6 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: OPEN
    #   7 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   8 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: OPEN
    Finished pinging host google.com (IP:-). 8 pings sent (6 OPEN, 2 CLOSED)

## Option table

The following table lists all available options:

| Abbreviation  | Full name                 | Sample            | Purpose
|:---           |:---                       |:---               |:---
| a             | address                   | -a=google.com     | The address to perform the pping to (can be omitted by specifying it after the command itself).
| p             | port                      | -p=80,443         | The comma-separated list of ports to pping to (can be omitted by specifying it after the address).
| t             | endless                   | -t                | Perform a constant pping.
| r             | repeats                   | -r=10             | Number of repeats in a non-endless pping (defaults to 4)
| tim           | timeout                   | -tim=2            | Timeout in seconds (defaults to 1)
| l             | logo                      | -logo             | If provided, pping will print detailed header informations.
| res           | resolve                   | -res              | If provided, pping will reolve the IP address for each pping.
| as            | autostop                  | -as               | If provided, pping will stop operation on the first opened port.
| elsc          | elsucesscount             | -elsc             | If provided, the process will retrieve the amount of opened ports as the process-result to DOS.
| elf           | elflag                    | -elf              | If provided, the process will return 0 if there was at least one open port, otherwise it returns 1.
| w             | waittime                  | -w=2000           | The amount of milliseconds to wait between 2 ppings.
| 
