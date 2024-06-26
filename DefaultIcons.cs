﻿using System.Drawing;
using System.Runtime.InteropServices;

namespace file_explorer
{
    // promptly stolen from https://stackoverflow.com/a/59129804, thanks!
    // icon ids: https://superuser.com/questions/1406594/is-there-a-reference-for-the-full-list-of-windows-10-shell-icon-numbers
    public static class DefaultIcons
    {
        private static Icon folderIcon;
        private static Icon fixedDriveIcon;
        private static Icon networkDriveIcon;

        public static Icon FolderLarge => folderIcon ?? (folderIcon = GetStockIcon(3, SHGSI_LARGEICON));
        public static Icon FixedDriveLarge => fixedDriveIcon ?? (fixedDriveIcon = GetStockIcon(8, SHGSI_LARGEICON));
        public static Icon NetworkDriveLarge => networkDriveIcon ?? (networkDriveIcon = GetStockIcon(51, SHGSI_LARGEICON));

        private static Icon GetStockIcon(uint type, uint size)
        {
            var info = new SHSTOCKICONINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            SHGetStockIconInfo(type, SHGSI_ICON | size, ref info);

            var icon = (Icon)Icon.FromHandle(info.hIcon).Clone(); // Get a copy that doesn't use the original handle
            DestroyIcon(info.hIcon); // Clean up native icon to prevent resource leak

            return icon;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHSTOCKICONINFO
        {
            public uint cbSize;
            public IntPtr hIcon;
            public int iSysIconIndex;
            public int iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szPath;
        }

        [DllImport("shell32.dll")]
        public static extern int SHGetStockIconInfo(uint siid, uint uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);

        private const uint SHGSI_ICON = 0x100;
        private const uint SHGSI_LARGEICON = 0x0;
        private const uint SHGSI_SMALLICON = 0x1;
    }
}
