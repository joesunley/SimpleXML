using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleXML
{
    public class XMLNode
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public XMLNodeCollection Children { get; set; }
        public XMLAttributeCollection Attributes { get; set; }

        public XMLNode()
        {
            _name = "";
            Children = new();
            Attributes = new();
        }

        public XMLNode(string name)
        {
            _name = name;
            Children = new();
            Attributes = new();
        }

        public void AddAttribute(string name, string value)
        {
            Attributes.Add(name, value);
        }
        public void AddChild(XMLNode child)
        {
            Children.Add(child);
        }
    }

    public class XMLNodeCollection : List<XMLNode>
    {
        public XMLNode this[string name]
        {
            get
            {
                foreach (XMLNode n in this)
                {
                    if (n.Name == name)
                        return n;
                }

                throw new Exception();
            }
        }

        public void Add(string name)
        {
            Add(new XMLNode(name));
        }
    }
}
