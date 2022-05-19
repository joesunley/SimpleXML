namespace Sunley;

public partial class XMLDocument
{
    private XMLNode _root;

    public XMLNode Root
    {
        get { return _root; }
        set { _root = value; }
    }

    public XMLDocument()
    {
        _root = new();
    }
    public XMLDocument(XMLNode root)
    {
        _root = root;
    }
}