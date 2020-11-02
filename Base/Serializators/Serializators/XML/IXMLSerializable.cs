using System.Xml.Linq;

namespace Vkimow.Serializators.XML
{
    public interface IXMLSerializable
    {
        XElement ConvertToXML();
        XElement ConvertToXML(string name);
        void ReadXML(XElement xElement);
    }
}
