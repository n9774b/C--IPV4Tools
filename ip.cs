using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public class ip
{
    public static int calcHosts(IPAddress broadcastAddr, IPAddress networkAddr)
    {
        return (ipToInt(broadcastAddr) - ipToInt(networkAddr) + 1);
    }

    public static IPAddress getSubnetmask(IPAddress ip)
    {
        foreach (NetworkInterface face in NetworkInterface.GetAllNetworkInterfaces())
        {
            foreach (UnicastIPAddressInformation unicastinfo in face.GetIPProperties().UnicastAddresses)
            {
                if (unicastinfo.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (ip.Equals(unicastinfo.Address))
                    {
                        return unicastinfo.IPv4Mask;
                    }
                }
            }
        }
        throw new ArgumentException("No Subnetmask Found!");
    }

    public static IPAddress getBroadcast(IPAddress ip, IPAddress wildcard)
    {
        try
        {
            byte[] subnetmaskBytes = wildcard.GetAddressBytes();
            byte[] subnetmaskmax = { 255, 255, 255, 255 };
            byte[] broadcast = { 0, 0, 0, 0 };
            if (subnetmaskBytes.Length == subnetmaskmax.Length)
            {
                for (int i = 0; i < 4; i++)
                {
                    byte res = Convert.ToByte(subnetmaskmax[i] - subnetmaskBytes[i]);
                    broadcast[i] = res;
                }
            }
            return IPAddress.Parse(String.Join(".", broadcast));
        }
        catch (Exception)
        { }
        throw new ArgumentException("Invalid IP Address or Subnetmask!");
    }

    public static string[] ipToBin(string ip)
    {
        return (ip.Split('.').Select(x => Convert.ToString(Int32.Parse(x), 2).PadLeft(8, '0'))).ToArray();
    }

    public static string binToIp(string binIp)
    {
        return String.Join(".", (binIp.Split('.').Select(x => Convert.ToInt32(x, 2).ToString())).ToArray());
    }

    public static string getBroadcastAddress(IPAddress ip, IPAddress netmask)
    {
        try
        {
            string[] ipBinArr = ipToBin(ip.ToString());
            string[] netBinArr = ipToBin(netmask.ToString());
            List<string> retme = new List<string>();
            for (int i = 0; i < ipBinArr.Length; i++)
            {
                char[] ipv = ipBinArr[i].ToCharArray();
                char[] netv = netBinArr[i].ToCharArray();
                List<char> r = new List<char>();
                for (int x = 0; x < ipv.Length; x++)
                {
                    if (ipv[x] == '1' || netv[x] == '1')
                    {
                        r.Add('1');
                    }
                    else
                    {
                        r.Add('0');
                    }
                }
                retme.Add(String.Join("", r));
            }
            return binToIp(String.Join(".", retme.ToArray()));
        }
        catch (Exception)
        { }
        throw new ArgumentException("Invalid IP Address or Subnetmask!");
    }

    public static string getNetworkAddress(IPAddress ip, IPAddress netmask)
    {
        try
        {
            string[] ipBinArr = ipToBin(ip.ToString());
            string[] netBinArr = ipToBin(netmask.ToString());
            List<string> retme = new List<string>();
            for (int i = 0; i < ipBinArr.Length; i++)
            {
                char[] ipv = ipBinArr[i].ToCharArray();
                char[] netv = netBinArr[i].ToCharArray();
                List<char> r = new List<char>();
                for (int x = 0; x < ipv.Length; x++)
                {
                    if (ipv[x] == '1' && netv[x] == '1')
                    {
                        r.Add('1');
                    }
                    else
                    {
                        r.Add('0');
                    }
                }
                retme.Add(String.Join("", r));
            }
            return binToIp(String.Join(".", retme.ToArray()));
        }
        catch (Exception)
        { }
        throw new ArgumentException("Invalid IP Address or Subnetmask!");
    }

    public static IPAddress wildcardMask(IPAddress subnet)
    {
        try
        {
            byte[] subnetmaskBytes = subnet.GetAddressBytes();
            byte[] subnetmaskmax = { 255, 255, 255, 255 };
            byte[] wildcard = { 0, 0, 0, 0 };
            if (subnetmaskBytes.Length == subnetmaskmax.Length)
            {
                for (int i = 0; i < 4; i++)
                {
                    byte res = Convert.ToByte(subnetmaskmax[i] - subnetmaskBytes[i]);
                    wildcard[i] = res;
                }
            }
            return IPAddress.Parse(String.Join(".", wildcard));
        }
        catch (Exception)
        { }
        throw new ArgumentException("Invalid IP Address or Subnetmask!");
    }

    public static int getNetMask(IPAddress broadcast)
    {
        try
        {
            int count = 0;
            foreach (string i in broadcast.ToString().Split('.'))
            {
                string bin = Convert.ToString(Convert.ToInt32(i), 2);
                foreach (char x in bin)
                    if (x == '1') count++;
            }
            return count;
        }
        catch (Exception)
        { }
        throw new ArgumentException("Invalid Broadcast Address");
    }

    public static int ipToInt(IPAddress ip)
    {
        return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ip.GetAddressBytes(), 0));
    }

    public static string firstUsableAddress(string networkAddr)
    {
        try
        {
            List<int> addr = new List<int>();
            foreach (string a in networkAddr.Split('.'))
            {
                addr.Add(int.Parse(a));
            }
            addr[3] = addr[3] + 1;
            return String.Join(".", addr.ToArray());
        }
        catch (Exception)
        {
            throw new ArgumentException("Invalid Network Address!");
        }
    }

    public static string lastUsableAddress(string broadcastAddr)
    {
        try
        {
            List<int> addr = new List<int>();
            foreach (string a in broadcastAddr.Split('.'))
            {
                addr.Add(int.Parse(a));
            }
            addr[3] = addr[3] - 1;
            return String.Join(".", addr.ToArray());
        }
        catch (Exception)
        {
            throw new ArgumentException("Invalid Broadcast Address!");
        }
    }

    public static bool isAlive(IPAddress host, int timeout = 250)
    {
        bool alive = false;
        try
        {
            PingReply pingr = new Ping().Send(host, timeout);
            if (pingr.Status == IPStatus.Success)
            {
                alive = true;
            }
            else
            {
                alive = false;
            }
        }
        catch (Exception)
        { }
        return alive;
    }

    public static IPAddress[] collect_hosts(object broadcastAddr, object networkAddr)
    {
        try
        {
            byte[] sbroadcast = IPAddress.Parse(broadcastAddr.ToString()).GetAddressBytes();
            byte[] snetwork = IPAddress.Parse(networkAddr.ToString()).GetAddressBytes();
            List<IPAddress> hosts = new List<IPAddress>();

            if (sbroadcast[3] != snetwork[3] && sbroadcast[2] == snetwork[2] && sbroadcast[1] == snetwork[1] && sbroadcast[0] == snetwork[0])
            {
                int[] host = { sbroadcast[0], sbroadcast[1], sbroadcast[2], 0 };
                for (int i = snetwork[3]; i <= sbroadcast[3]; i++)
                {
                    if (i == 255 || i == 0)
                        continue;
                    host[3] = i;
                    hosts.Add(IPAddress.Parse(String.Join(".", host)));
                }
            }
            if (sbroadcast[3] != snetwork[3] && sbroadcast[2] != snetwork[2] && sbroadcast[1] == snetwork[1] && sbroadcast[0] == snetwork[0])
            {
                int[] host = { sbroadcast[0], sbroadcast[1], 0, 0 };
                for (int i = snetwork[2]; i <= sbroadcast[2]; i++)
                {
                    if (i == 255)
                        continue;
                    host[2] = i;
                    for (int x = snetwork[3]; x <= sbroadcast[3]; x++)
                    {
                        if (x == 255 || x == 0)
                            continue;
                        host[3] = x;
                        hosts.Add(IPAddress.Parse(String.Join(".", host)));
                    }
                }
            }
            return hosts.ToArray();
        }
        catch (Exception)
        { }
        throw new ArgumentException("Invalid Broadcast or Network Address!");
    }
}
