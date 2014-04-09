using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace envset.Models
{
    public class ConfigFile
    {
        public static Dictionary<string, List<EnvItem>> LoadDic(string fileanme)
        {
            if (!File.Exists(fileanme)) return new Dictionary<string, List<EnvItem>>();

            using (var reader = new StreamReader(fileanme))
            {
                var result = new Dictionary<string, List<EnvItem>>();

                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine().Trim();
                    var item = line.Split(',');
                    if (item.Length < 2) break;
                    result.Add(item[0], Load(item[1]));
                }

                return result;
            }
        }

        public static List<EnvItem> Load(string filename)
        {
            if (!File.Exists(filename)) return new List<EnvItem>();

            using (var reader = new StreamReader(filename))
            {
                var result = new List<EnvItem>();

                while (reader.Peek() > 0)
                {
                    var line = reader.ReadLine().Trim();
                    var item = line.Split(',');
                    if (item.Length < 2) break;
                    result.Add(new EnvItem()
                    {
                        Key = item[0],
                        Value = item[1],
                        IsUpdated = false,
                        IsUserEnv = true,
                    });
                }

                return result;
            }
        }

        public static void Save(string filename, IList<EnvItem> env)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var item in env)
                {
                    writer.WriteLine(string.Format("{0},{1}", item.Key, item.Value));
                }
            }
        }
    }
}
