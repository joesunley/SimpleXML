namespace Sunley;

using NUnit.Framework;

public static partial class SimpleXML
{
    public static XMLDocument Deserialize(string xml)
    {
            
        xml = RemoveSpecialCharacters(xml);

        List<string> split = xml.Split('>', '<').ToList();

        split.RemoveAll(x => x == "");


        if (split[0].Contains('?'))
            split.RemoveAt(0);


        var levels = GetLevels(split.ToArray());


        for (int i = 0; i < levels.Length; i++)
        {
            try
            {
                TestContext.WriteLine($"{i}, {levels[i].Item1}, {new string('\t', levels[i].Item1)}{levels[i].Item2}");
            }
            catch
            {
                TestContext.WriteLine($"{i}, {levels[i].Item1}, {levels[i].Item2}");
            }
        }


        XMLNode root = GetNode(levels, 0, 0);

        return new(root);
    }

    private static string RemoveSpecialCharacters (string input)
    {
        input = input.Replace("\r", "");
        input = input.Replace("\n", "");
        input = input.Replace("\t", "");

        return input;
    }

    private static XMLAttributeCollection GetAttributes(string input)
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

        for (int i = 0; i < input.Length; i++)
        {
            string prev = "";
            string next = "";

            if (i != 0)
                prev = input[i - 1];
            string curr = input[i];

            if (i != input.Length - 1)
                next = input[i + 1];

            if (curr.First() == '/')
            {
                //t.Add(new(level, curr));
                if (next != null)
                {
                    try
                    {
                        if (next.First() == '/')
                        {
                            level--;
                        }
                    }
                    catch
                    {
                        TestContext.Write("");
                    }
                }
                    //if(next.First() == '/')
                    //level--;
                //else level --;
            }
            else if (curr.Last() == '/')
            {
                t.Add(new(level, curr));
                //level--;

                if (next != null && next.First() == '/')
                    level--;
            }
            else
            {
                string filtered = next.Replace("/", "");

                t.Add(new(level, curr));

                if (!prev.Contains(filtered))
                    //{
                    //    if (t[i - 1].Item1 == level + 1)
                    //        level++;
                    //    else
                    //        level--;
                    //}
                    level++;
                else
                    level--;
            }
        }

        return t.ToArray();
    }

    private static XMLNode GetNode(Tuple<int, string>[] levels, int startIndex, int thisLevel)
    {
        XMLNode node = GetNodeNameAttributes(levels[startIndex].Item2);

        if (node.Name == "depth")
            TestContext.Write("");
        

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

        if(nextLevelNodes.Count == 1)
        {
            if (node.Children[0].Children.Count == 0 && node.Children[0].Attributes.Count == 0 && node.Children[0].InnerText == "")
            {
                string name = node.Children[0].Name;
                node.Children.RemoveAt(0);
                node.InnerText = name;
            }
        }

        return node;
    }

    private static XMLNode GetNodeNameAttributes(string line)
    {
        XMLNode node = new();

            
        int index = line.IndexOf(' ');

        string name;

        if (index == -1)
            name = line;
        else
            name = line.Substring(0, line.IndexOf(' '));

        node.Name = name;
        node.Attributes = GetAttributes(line);

        return node;
    }

    private static XMLNode FixInnerText(XMLNode node)
    {
        if (node.Children.Count == 1)
        {
            if (node.Children[0].Attributes.Count == 0)
            {
                string innerText = node.Children[0].Name;
                node.Children.Clear();

                node.InnerText = innerText;
            }
        }
        else
        {
            for (int i = 0; i < node.Children.Count; i++)
            {
                node.Children[i] = FixInnerText(node.Children[i]);
            }
        }

        return node;
    }
}
