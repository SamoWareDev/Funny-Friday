using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FunnyFriday
{
    class AtlasParser
    {
        public AtlasParser(string _path)
        {
            path = _path;
        }

        private string path;

        public string GetFileName()
        {
            return File.ReadAllLines(path)[1].Split('\"')[1];
        }

        public int[] GetInfo(string name)
        {
            int[] temp = new int[4];

            string[] data = File.ReadAllLines(path);

            for (int i = 3; i < data.Length - 1; i++)
            {
                if (data[i].IndexOf(name) != -1)
                {
                    try
                    {
                        string[] a = data[i].Split("\"");
                        temp[0] = Convert.ToInt32(a[3]);
                        temp[1] = Convert.ToInt32(a[5]);
                        temp[2] = Convert.ToInt32(a[7]);
                        temp[3] = Convert.ToInt32(a[9]);
                    }
                    catch { }
                }
            }

            return temp;
        }

        public int[] GetInfo(int index)
        {
            int[] temp = new int[4];

            string[] data = File.ReadAllLines(path);

                try
                {
                    string[] a = data[index].Split("\"");
                    temp[0] = Convert.ToInt32(a[3]);
                    temp[1] = Convert.ToInt32(a[5]);
                    temp[2] = Convert.ToInt32(a[7]);
                    temp[3] = Convert.ToInt32(a[9]);
                }
                catch { }

            return temp;
        }
    }
}
