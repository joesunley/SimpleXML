namespace Sunley;

public class XMLNode
{
    private string _name;
    private string? _innerText;
    private XMLNodeCollection _children;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public XMLNodeCollection Children
    {
        get => _children;

        set
        {
            if (_innerText != null)
                throw new Exception("Cannot set Childeren when there is inner text");

            _children = value;
        }
    }
    public XMLAttributeCollection Attributes { get; set; }

    public string InnerText
    {
        get => _innerText ?? "";

        set
        {
            if (_children.Count > 0)
                throw new Exception("Cannot set InnerText when there are children");

            _innerText = value;
        }
    }

    public XMLNode()
    {
        _name = "";
        _children = new();
        Attributes = new();
    }

    public XMLNode(string name)
    {
        _name = name;
        _children = new();
        Attributes = new();
    }

    public void AddAttribute(string name, string value)
    {
        Attributes.Add(name, value);
    }
    public void AddChild(XMLNode child)
    {
        if (_innerText != null)
            throw new Exception("Cannot add child when there is inner text");
            
        Children.Add(child);
    }

#if DEBUG
    public override string ToString()
    {
        string s;
        if (!(_children.Count == 0))
            return $"{Name}, attr:{Attributes.Count} child:{Children.Count}";
        else
            return $"{Name}, attr:{Attributes.Count} child:{Children.Count} innerText:{_innerText}";
    }
#endif
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

#if DEBUG
    public override string ToString()
    {
        return Count.ToString();
    }
#endif
}