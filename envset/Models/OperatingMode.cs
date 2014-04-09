using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace envset.Models
{
    public class OperatingMode
    {
        public const string OverWrite = "上書き";
        public const string Diff = "差分";

        private static string[] _modeList = new string[]
        {
            OverWrite,
            Diff,
        };
        public static string[] ModeList { get { return _modeList; } }
    }
}
