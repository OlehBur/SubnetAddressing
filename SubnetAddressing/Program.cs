int netsCnt = 0;
Dictionary<string, byte> netHosts;
string address;

//for (int i = 0; i < 20; i++) {
//    Console.WriteLine(RoundUpPowerOfTwo(i));
//}
Console.WriteLine("Net Address: ");
address = Console.ReadLine();
address = address.Substring(0, address.LastIndexOf('.') + 1);
Console.WriteLine("Nets cnt: ");
int.TryParse(Console.ReadLine(), out netsCnt);

if (netsCnt == 0)
    return;

netHosts = new Dictionary<string, byte>();
for (int i = 0; i < netsCnt; i++) {
    string key = $"Net{i + 1}";
    Console.WriteLine(key + ": ");
    if (byte.TryParse(Console.ReadLine(), out byte buff))
        netHosts[key] = buff;
}

netHosts = netHosts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
byte lastAddr = 0;
foreach (var net in netHosts) {
    Console.WriteLine("\t\t" + net.Key);

    byte round = RoundUpPowerOfTwo((byte)(net.Value + 2));
    byte step = (byte)Math.Log2(round);
    uint mask = 0xFFFFFFFF << step;

    Console.WriteLine($"\tHosts: {round}");
    Console.WriteLine($"\tPrefix: {32-step}");
    Console.WriteLine($"\tBin Mask: {LastBinByte(mask)}");
    Console.WriteLine($"\tMask: {QuadNum(mask)}");
    Console.WriteLine($"\tInverted Bin Mask: {LastBinByteInvert(mask)}");
    Console.WriteLine($"\tInverted Dec Mask: {QuadNumInvert(mask)}");
    Console.WriteLine($"\tAddress: {address + lastAddr.ToString()}");
    Console.WriteLine($"\tBin Address: {LastBinByte(lastAddr)}");
    Console.WriteLine($"\tFirst: {address + (1 + lastAddr).ToString()}");
    lastAddr += round;
    lastAddr -= 2;
    Console.WriteLine($"\tLast: {address + lastAddr.ToString()}");
    Console.WriteLine($"\tBroad: {address + (++lastAddr).ToString()}");
    lastAddr++;
    Console.WriteLine($"\tFree: {round - net.Value - 2}");
    Console.WriteLine();
}


static byte RoundUpPowerOfTwo(byte n) {
    if (n < 1) return 1; // мін значення

    n--;
    n |= (byte)(n >> 1);
    n |= (byte)(n >> 2);
    n |= (byte)(n >> 4);//byte
    //n |= n >> 8;
    //n |= n >> 16;//int 32b
    n++;
    return n;
}

static string QuadNum(uint number) {
    byte b1 = (byte)((number >> 24) & 0xFF); // Старший байт
    byte b2 = (byte)((number >> 16) & 0xFF);
    byte b3 = (byte)((number >> 8) & 0xFF);
    byte b4 = (byte)(number & 0xFF); // Молодший байт

    return $"{b1}.{b2}.{b3}.{b4}";
}

static string QuadNumInvert(uint number) {
    byte b = (byte)(number & 0xFF);

    return $"{255-b}";
}

static string LastBinByte(uint numb) {
    byte b = (byte)(numb & 0xFF);

    return Convert.ToString(b, 2).PadLeft(8, '0');
}

static string LastBinByteInvert(uint numb) {
    return new string(LastBinByte(numb).Select(x => x == '0' ? '1' : '0').ToArray());
}