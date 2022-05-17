using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleXML 
{
    public static partial class SimplXML
    {
        public static void Serialize(XMLDocument doc, string fileName)
        {
            string s = Serialize(doc);
            File.WriteAllText(fileName, s);
        }
        public static string Serialize(XMLDocument doc)
        {
            string s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

            s += NodeSer(doc.Root);

            return s;
        }

        private static string NodeSer(XMLNode node)
        {
            string s = "";
            s += $"<{node.Name}";

            foreach (XMLAttribute attr in node.Attributes)
            {
                s += $" {attr.Name}=\"{attr.Value}\"";
            }

            if (node.Children.Count == 0 && node.InnerText == "")
            {
                s += " />";
            }
            else
            {
                if (node.Children.Count == 0)
                {
                    s += ">" + node.InnerText;
                }
                else
                {
                    s += ">";
                    foreach (XMLNode child in node.Children)
                    {
                        s += NodeSer(child);
                    }
                }
                s += $"</{node.Name}>";
            }

            return s;
        }
    }
}
