using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SteamCMD
{
	public class SteamCMDBuilder
	{
		private string args = null;

		public OSPlatform sSteamCmdForcePlatformType
		{
			set
			{
				lock (args)
					if (value == OSPlatform.Windows)
					{
						args += " +@sSteamCmdForcePlatformType windows";
					}
					else if (value == OSPlatform.Linux)
					{
						args += " +@sSteamCmdForcePlatformType linux";
					}
					else if (value == OSPlatform.OSX)
					{
						args += " +@sSteamCmdForcePlatformType macos";
					}
			}
		}

		public string force_install_dir
		{
			set
			{
				lock (args)
					args += $" force_install_dir {value}";
			}
		}

		public void Run()
		{
			Process process = Process.Start(
				new ProcessStartInfo(
					"steamcmd",
					args + " +quit"
				)
			);

			process.WaitForExit();
		}

		public SteamCMDBuilder Login(string account = "anonymous", string password = null, string steamguard = null)
		{
			lock (args)
			{
				args += $"+login {account}";
				if (password is not null)
				{
					args += " " + password;
					if (steamguard is not null)
					{
						args += " " + steamguard;
					}
				}
			}
			return this;
		}
		public SteamCMDBuilder WithForcedPlatformType(OSPlatform platform)
		{
			sSteamCmdForcePlatformType = platform;
			return this;
		}

	}
}
