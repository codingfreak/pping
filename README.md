# pping
A port ping console for Windows.

## Purpose

pping is designed to give you the easiest possible solution for discovering ports from a windows console. The design was heavily oriented towards the terminology and behavior of the classic ping tool under windows.

## Implementation

pping is developed using Microsoft .NET Core version 3.1. pping uses logic from our package [cfUtils](https://github.com/codingfreak/cfUtils).

## Installation

The easiest way to get pping running on your machine is to use chocolatey and install it

    choco install pping

Alternativeley you can simply download the 7z-package from [codingfreaks.de](https://codingfreaks.de/tools) and unzip the files in any directory. It will make sense to add this directory to your PATH variable.

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

    pping google.com 80,443 -d

    Starting pinging host google.com on TCP port(s) 80,443 4 times:
    #   1 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: CLOSED (TimedOut)
    #   2 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: CLOSED (TimedOut)
    #   3 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   4 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: OPEN
    #   5 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   6 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: OPEN
    #   7 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN
    #   8 -> Pinging host google.com (IP:-) on TCP port 443 with timeout 1: OPEN
    Finished pinging host google.com (IP:-). 8 pings sent (6 OPEN, 2 CLOSED)

It is also possible to define a range of ports by delimiting them with '-':

    pping google.com 80-82 -d                                                              
    Starting pinging host google.com on TCP port(s) 80-82 4 times:                           
    #   1 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN              
    #   2 -> Pinging host google.com (IP:-) on TCP port 81 with timeout 1: CLOSED (TimedOut) 
    #   3 -> Pinging host google.com (IP:-) on TCP port 82 with timeout 1: CLOSED (TimedOut) 
    #   4 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN              
    #   5 -> Pinging host google.com (IP:-) on TCP port 81 with timeout 1: CLOSED (TimedOut) 
    #   6 -> Pinging host google.com (IP:-) on TCP port 82 with timeout 1: CLOSED (TimedOut) 
    #   7 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN              
    #   8 -> Pinging host google.com (IP:-) on TCP port 81 with timeout 1: CLOSED (TimedOut) 
    #   9 -> Pinging host google.com (IP:-) on TCP port 82 with timeout 1: CLOSED (TimedOut) 
    #  10 -> Pinging host google.com (IP:-) on TCP port 80 with timeout 1: OPEN              
    #  11 -> Pinging host google.com (IP:-) on TCP port 81 with timeout 1: CLOSED (TimedOut) 
    #  12 -> Pinging host google.com (IP:-) on TCP port 82 with timeout 1: CLOSED (TimedOut) 
    Finished pinging host google.com (IP:-). 12 pings sent (4 OPEN, 8 CLOSED)                

## Option table

The following table lists all available options:

| Abbreviation  | Full name                 | Sample            | Purpose
|:---           |:---                       |:---               |:---
| t             | endless                   | -t                | If set, the app will run infinitely. (see -a option).
| r             | repeats                   | -r 10             | Number of repeats in a non-endless pping (defaults to 4)
| tim           | timeout                   | -tim 2            | Defines the timeout in seconds the app will wait for each requests to return.
| l             | logo                      | -logo             | If provided, pping will print detailed header informations.
| res           | resolve                   | -res              | If provided, pping will resolve the IP address for each pping.
| a             | autostop                  | -a                | If set, the app will stop working when it gets the first OPEN-result.
| els           | elsucc                    | -els              | If set, the app will return the amount of successful port requests as the result code.
| elf           | elfail                    | -elf              | If set, the app will return error level 0 on any open ping and error level 1 if all pings resulted in closed port.
| w             | waittime                  | -w 2000           | Defines a time in milliseconds to wait between calls. Defaults to 1000.
| d             | detailed                  | -d                | If provided, pping will try to write reason details at closed ports to te console.
| 4             | ipv4                      | -4                | If provided, pping will use IPv4 for resolutions.
| 6             | ipv6                      | -6                | If provided, pping will use IPv6 for resolutions.