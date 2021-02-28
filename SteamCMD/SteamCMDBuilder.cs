using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SteamCMD
{
	public class SteamCMDBuilder
	{
		private string args = "";

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

		public SteamCMDBuilder(bool autoupdate = true)
		{
			if (autoupdate)
			{
				SteamCMDDownloader.Download();
			}
		}

		public void Run()
		{
			lock (args)
			{
				Process process = Process.Start(
					new ProcessStartInfo(
						RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
						? "steamcmd.exe"
						: "steamcmd.sh",
						args + " +quit"
					)
				);

				process.WaitForExit();
			}
		}

		public SteamCMDBuilder Login(string account = "anonymous", string password = null, string steamguard = null)
		{
			lock (args)
			{
				args += $" +login {account}";
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
		public SteamCMDBuilder WithAppUpdate(string appid, bool validate = true, string beta = null, string betapassword = null)
		{
			lock (args)
			{
				args += $" +app_update {appid}";

				if (beta is not null) args += $" -beta {beta}";
				if (betapassword is not null) args += $" -betapassword {betapassword}";

				if (validate) args += " validate";
			}
			return this;
		}
		public SteamCMDBuilder WithAppSetConfig(string appid, string name, string value)
		{
			lock (args)
			{
				args += $" +app_set_config {appid} {name} {value}";
			}
			return this;
		}
	}
}
