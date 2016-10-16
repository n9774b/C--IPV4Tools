# ip.cs - C# Networking Tools
> Basic IPV4 Networking Methods packed into one source file.


### Methods

```
+-------------------------------------------+--------------+-------------------------------------------------------------------------------------+
|             Description                   | Return Type  |    Method To Call                                                                   |
+-------------------------------------------+--------------+------------------------------------------------------------------------------------ +
| Subnet Mask Calculation                    :   IPAddress |   ip.getSubnetmask(IPAddress <Local_IP_Address>)                                    |
| Wildcard mask calulation                   :   IPAddress |   ip.WildcardMask(IPAddress <Local_Subnet_Address>)                                 |
| Netmask Calculation                        :   IPAddress |   ip.getNetMask(IPAddress <Network_Broadcast_Address>)                              |
| Broadcast Address Calculation              :   IPAddress |   ip.getBroadcast(IPAddress <Local_IP_Address>, IPAddress <Wildcard_Address>)       |
| Network Address Calculation                :   IPAddress |   ip.getNetworkAddress(IPAddress <Local_IP_Address>, IPAddress <Netmask_Address>)   |
| Dump a List of hosts inside of a subnet    : IPAddress[] |   ip.collect_hosts(IPAddress <Broadcast_Address>, IPAddress <Network_Address>)      |
| Check if a host is alive                   :     bool    |   ip.isAlive(IPAddress <IP_To_Check>, int <Timeout_For_Ping_In_ms>)                 |
| First Useable Address                      :    string   |   ip.firstUseableAddress(string <Network_Address>)                                  |
| Last Useable Address                       :    string   |   ip.lastUseableAddress(string <Broadcast_Address>)                                 |
+--------------------------------------------+-------------+-------------------------------------------------------------------------------------+
```
### Installation

For VisualStudio
- Right click on your project name in the solution explorer
- Mouse over 'Add' then select 'New Item'
- Select Class and name it 'ip.cs'
- Click 'Ok'
- Done, You can now use this script.




### Example Usage
#### C# Console Application
```
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace iptest
{
    class Program
    {
        static void Main(string[] args)
        {
          IPAddress subnet = ip.getSubnetmask(localIP);
          IPAddress wildcard = ip.wildcardMask(subnet);
          int netmask = ip.getNetMask(subnet);
          IPAddress broadcastAddr = IPAddress.Parse(ip.getBroadcastAddress(localIP, wildcard));
          IPAddress networkAddr = IPAddress.Parse(ip.getNetworkAddress(localIP, subnet));
          string firstuse = firstUseableAddr(networkAddr.ToString());
          string lastuse = lastUseableAddr(broadcastAddr.ToString());

          Console.WriteLine("Local Ip Address:      {0}", localIP.ToString());
          Console.WriteLine("Subnet Address:        {0}", subnet.ToString());
          Console.WriteLine("Broadcast Address:     {0}", broadcastAddr.ToString());
          Console.WriteLine("Network Address:       {0}", networkAddr.ToString());
          Console.WriteLine("Wildcard Address:      {0}", wildcard.ToString());
          Console.WriteLine("Netmask:               {0}", netmask);
          Console.WriteLine("First Useable Address: {0}", firstuse);
          Console.WriteLine("Last Useable Address:  {0}", lastuse);
          
          Console.ReadKey();
         }
     }
}
```
