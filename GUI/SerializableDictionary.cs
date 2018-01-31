using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
{
    #region IXmlSerializable Members
    public System.Xml.Schema.XmlSchema GetSchema() {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader) {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        bool wasEmpty = reader.IsEmptyElement;
        reader.Read();

        if (wasEmpty)
            return;

        while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
            reader.ReadStartElement("item");

            reader.ReadStartElement("key");
            TKey key = (TKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("value");
            TValue value = (TValue)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();

            this.Add(key, value);

            reader.ReadEndElement();
            reader.MoveToContent();
        }
        reader.ReadEndElement();
    }

    public void WriteXml(System.Xml.XmlWriter writer) {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (TKey key in this.Keys) {
            writer.WriteStartElement("item");

            writer.WriteStartElement("key");
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();

            writer.WriteStartElement("value");
            TValue value = this[key];
            valueSerializer.Serialize(writer, value);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
    #endregion
}
[XmlRoot("dictionary")]
public class SerializableDictionaryString
    : Dictionary<string, string>, IXmlSerializable
{
    // filters control characters but allows only properly-formed surrogate sequences
    private static Regex _invalidXMLChars = new Regex(
        @"(?<![\uD800-\uDBFF])[\uDC00-\uDFFF]|[\uD800-\uDBFF](?![\uDC00-\uDFFF])|[\x00-\x08\x0B\x0C\x0E-\x1F\x7F-\x9F\uFEFF\uFFFE\uFFFF]",
        RegexOptions.Compiled);

    /// <summary>
    /// removes any unusual unicode characters that can't be encoded into XML
    /// </summary>
    public static string RemoveInvalidXMLChars(string text) {
        if (string.IsNullOrEmpty(text)) return "";
        return _invalidXMLChars.Replace(text, "");
    }

    #region IXmlSerializable Members
    public System.Xml.Schema.XmlSchema GetSchema() {
        return null;
    }

    public void ReadXml(System.Xml.XmlReader reader) {
        XmlSerializer keySerializer = new XmlSerializer(typeof(string));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(string));

        bool wasEmpty = reader.IsEmptyElement;
        reader.Read();

        if (wasEmpty)
            return;

        while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {
            reader.ReadStartElement("item");

            reader.ReadStartElement("key");
            string key = (string)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("value");
            string value = (string)valueSerializer.Deserialize(reader);

            reader.ReadEndElement();

            this.Add(key, value);

            reader.ReadEndElement();
            reader.MoveToContent();
        }
        reader.ReadEndElement();
    }

    public void WriteXml(System.Xml.XmlWriter writer) {
        XmlSerializer keySerializer = new XmlSerializer(typeof(string));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(string));

        foreach (string key in this.Keys) {
            writer.WriteStartElement("item");

            writer.WriteStartElement("key");
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();

            writer.WriteStartElement("value");
            string value = RemoveInvalidXMLChars(this[key]);

            value = RemoveInvalidXMLChars(value.ToString());

            valueSerializer.Serialize(writer, value);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
    #endregion
}