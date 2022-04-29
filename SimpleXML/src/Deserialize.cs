using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleXML
{
    public static partial class SimplXML
    {
        public static XMLDocument Deserialize(string xml)
        {
            xml = RemoveLineBreaks(xml);

            List<string> split = xml.Split('>').ToList();

            if (split[0].Contains('?'))
                split.RemoveAt(0);


            var levels = GetLevels(split.ToArray());


            XMLNode root = GetNode(levels, 0, 0);

            return new(root);
        }

        private static string RemoveLineBreaks(string input)
        {
            bool contains = true;

            while (contains)
            {
                int index = input.IndexOf("\n");

                if (index != -1)
                {
                    input = input.Remove(index, 1);
                }
                else
                {
                    contains = false;
                }
            }

            return input;
        }

        public static XMLAttributeCollection GetAttributes(string input)
        {
            XMLAttributeCollection attributes = new();

            bool contains = true;

            while (contains)
            {
                if (input.Contains('='))
                {
                    string[] split = input.Split('=');

                    string key = split[0].Split(' ').Last();

                    int lastIndex = split[1].LastIndexOf('"');

                    string value = split[1].Substring(1, lastIndex - 1);

                    input = input.Substring(input.IndexOf('=') + 2);

                    attributes.Add(key, value);
                }
                else
                {
                    contains = false;
                }
            }

            return attributes;
        }

        private static Tuple<int, string>[] GetLevels(string[] input)
        {
            List<Tuple<int, string>> t = new();
            int level = 0;

            foreach (string s in input)
            {
                if (s == "")
                    continue;

                if (s[1] == '/')
                {
                    //t.Add(new(level - 1, s));
                    level--;
                }
                else if (s.Last() == '/')
                {
                    t.Add(new(level, s));
                }
                else
                {
                    t.Add(new(level, s));
                    level++;
                }
            }

            

            return t.ToArray();
        }

        static XMLNode GetNode(Tuple<int, string>[] levels, int startIndex, int thisLevel)
        {
            XMLNode node = GetNodeNameAttributes(levels[startIndex].Item2);


            List<int> nextLevelNodes = new();

            for (int i = startIndex + 1; i < levels.Length; i++)
            {
                var t = levels[i];

                if (t.Item1 == thisLevel + 1)
                {
                    nextLevelNodes.Add(i);
                }
                else if (t.Item1 <= thisLevel)
                    break;
            }

            foreach (int n in nextLevelNodes)
            {
                node.AddChild(GetNode(levels, n, thisLevel + 1));
            }

            return node;
        }

        private static XMLNode GetNodeNameAttributes(string line)
        {
            XMLNode node = new();

            
            int index = line.IndexOf(' ');

            string name;

            if (index == -1)
                name = line.Substring(1);
            else
                name = line.Substring(1, line.IndexOf(' ') - 1);

            node.Name = name;
            node.Attributes = GetAttributes(line);

            return node;
        }
    }
}
