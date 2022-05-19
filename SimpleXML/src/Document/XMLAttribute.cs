namespace Sunley;

public class XMLAttribute
{
    private string _name;
    private string _value;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string Value
    {
        get => _value;
        set => _value = value;
    }

    public XMLAttribute()
    {
        _name = "";
        _value = "";
    }
    public XMLAttribute(string name, string value)
    {
        _name = name;
        _value = value;
    }
#if DEBUG
    public override string ToString() => $"{_name} : {_value}";
#endif
}

public class XMLAttributeCollection : List<XMLAttribute>
{
    public XMLAttribute this[string name]
    {
        get
        {
            foreach (XMLAttribute a in this)
            {
                if (a.Name == name)
                    return a;
            }

            throw new Exception();
        }
    }

    public void Add(string name, string value)
    {
        Add(new XMLAttribute(name, value));
    }

#if DEBUG
    public override string ToString() => Count.ToString();
#endif
}