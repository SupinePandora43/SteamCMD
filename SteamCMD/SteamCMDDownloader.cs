using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;

namespace SteamCMD
{
	internal static class SteamCMDDownloader
	{
		private const string WINDOWS = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";
		private const string LINUX = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd_linux.tar.gz";
		private const string MACOS = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd_osx.tar.gz";

		private static readonly bool zip = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

		internal static void Download()
		{
			string url = null;
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				url = WINDOWS;
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				url = LINUX;
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				url = MACOS;
			}
			else throw new PlatformNotSupportedException();

			// fetch
			WebRequest request = WebRequest.CreateHttp(url);
			WebResponse response = request.GetResponse();
			// extract
			Stream stream = response.GetResponseStream();
			if (zip)
			{
				ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);
				archive.ExtractToDirectory(".");
			}
			else
			{
				Tar.ExtractTarGz(stream, ".");
				chmod("steamcmd.sh", 0x40);
			}
		}

		[DllImport("libc", SetLastError = true)]
		private static extern int chmod(string pathname, int mode);
	}
}
