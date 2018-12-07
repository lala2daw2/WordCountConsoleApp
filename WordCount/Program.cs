using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WordCount
{
    class Program
    {

        static void Main(string[] args)
        {
            //string path = @"D:\Files\mobydick.txt";

            //if (!File.Exists(path))
            //{
            //    File.Create(path);
            //    //TextWriter tw = new StreamWriter(path);
            //    //tw.WriteLine("The very first line!");
            //    //tw.Close();
            //}
            //using (var client = new WebClient())
            //{
            //    client.DownloadFile("http://www.gutenberg.org/files/2701/2701-0.txt", @"D:\Files\mobydick.txt");
            //}

            string str;
            var fileStream = new FileStream("MobyDick.txt", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                str = streamReader.ReadToEnd();
            }
            str = str.Replace("&#xD;", " ").Replace("&#xA;", " ");
            str = str.Replace("\r", " ").Replace("\n", " ");

            var result = str.Split(new[] { ' ', '"','!','?','.',',','=',':',';','[',']','(',')' }, StringSplitOptions.RemoveEmptyEntries)
                            .GroupBy(r => r)
                            .Select(grp => new
                            {
                                Word = grp.Key,
                                Count = grp.Count()
                            });

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(dec);
            XmlElement root = xmlDoc.CreateElement("words");
            xmlDoc.AppendChild(root);
            foreach (var item in result)
            {
                XmlElement e = xmlDoc.CreateElement("word");
                root.AppendChild(e);
                XmlAttribute count = xmlDoc.CreateAttribute("count");
                count.Value = item.Count.ToString();
                e.Attributes.Append(count);
                XmlAttribute text = xmlDoc.CreateAttribute("text");
                text.Value = item.Word;
                e.Attributes.Append(text);

            }
            xmlDoc.Save("mobydickword.xml");

            Console.ReadKey();

        }
    }
}



