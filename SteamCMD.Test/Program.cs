using System;

namespace SteamCMD.Test
{
    class Program
    {
        static void Main(string[] args)
        {
			SteamCMDBuilder cmd = new SteamCMDBuilder();

			cmd.Run();
		}
    }
}
