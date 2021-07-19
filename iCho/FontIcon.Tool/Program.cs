using System;
using System.IO;
using System.Text;

namespace FontIcon.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            //var stringPath = "/Users/sin/Code/FriendLoc/FriendLoc/iCho/FontIcon.Tool/icons.txt";

            //var icons = GetIcons("mdi-", stringPath, "MaterialDesignIcon");


            var stringPath = "/Users/sin/Code/FriendLoc/FriendLoc/iCho/FontIcon.Tool/icons_material.txt";

            var icons = GetIcons("md-", stringPath, "MaterialIcon");

        }

        static string GetIcons(string iconPrefix,string filePath,string iconModuleName)
        {
            var icons = File.ReadAllLines(filePath);
            var lines = new StringBuilder();
            foreach (var item in icons)
            {
                var val = item.Substring(item.IndexOf(iconPrefix));

                val = val.Remove(val.IndexOf("\","));

                var line = string.Format("<converter:{0} x:Key=\"{1}\" Icon=\"{2}\" />",iconModuleName,val,val);

                lines.AppendLine(line);
            }

            return lines.ToString();
        }
    }
}
