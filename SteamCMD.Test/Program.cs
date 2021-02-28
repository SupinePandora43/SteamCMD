using System.Runtime.InteropServices;

namespace SteamCMD.Test
{
    class Program
    {
        static void Main(string[] args)
        {
			SteamCMDBuilder cmd = new SteamCMDBuilder();

			cmd.Login();
			cmd.WithForcedPlatformType(OSPlatform.Linux);
			cmd.WithAppUpdate("4020");

			cmd.Run();
		}
    }
}
