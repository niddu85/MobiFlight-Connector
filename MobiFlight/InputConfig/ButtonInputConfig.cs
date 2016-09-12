﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MobiFlight.InputConfig
{
    public enum ButtonInputEventType
    {
        PRESS,
        RELEASE,
        REPEAT
    }

    public class ButtonInputConfig : IXmlSerializable, ICloneable
    {
        public InputAction onPress;
        public InputAction onRelease;


        public object Clone()
        {
            ButtonInputConfig clone = new ButtonInputConfig();
            if (onPress != null) clone.onPress = (InputAction) onPress.Clone();
            if (onRelease != null) clone.onRelease = (InputAction) onRelease.Clone();
            return clone;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {

            reader.Read(); // this should be the opening tag "onPress"
            if (reader.LocalName == "onPress")
            {
                switch (reader["type"])
                {
                    case "FsuipcOffsetInputAction":
                        onPress = new FsuipcOffsetInputAction();
                        onPress.ReadXml(reader);
                        reader.Read(); // this should be the closing tag "onPress"
                        break;

                    case "KeyInputAction":
                        onPress = new KeyInputAction();
                        onPress.ReadXml(reader);
                        break;

                    case "EventIdInputAction":
                        onPress = new EventIdInputAction();
                        onPress.ReadXml(reader);
                        break;

                    case "JeehellInputAction":
                        onPress = new JeehellInputAction();
                        onPress.ReadXml(reader);
                        break;
                }
            }

            reader.Read();
            if (reader.LocalName == "onRelease")
            {
                switch (reader["type"])
                {
                    case "FsuipcOffsetInputAction":
                        onRelease = new FsuipcOffsetInputAction();
                        onRelease.ReadXml(reader);
                        reader.ReadStartElement();
                        break;

                    case "KeyInputAction":
                        onRelease = new KeyInputAction();
                        onRelease.ReadXml(reader);
                        reader.ReadStartElement();
                        break;

                    case "EventIdInputAction":
                        onRelease = new EventIdInputAction();
                        onRelease.ReadXml(reader);
                        reader.ReadStartElement();
                        break;

                    case "JeehellInputAction":
                        onPress = new JeehellInputAction();
                        onPress.ReadXml(reader);
                        break;
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("onPress");
            if (onPress != null) onPress.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("onRelease");
            if (onRelease != null) onRelease.WriteXml(writer);
            writer.WriteEndElement();
        }

        internal void execute(FSUIPC.FSUIPCCacheInterface fsuipcCache, ButtonArgs e)
        {
            if (e.Value == 0 && onPress != null)
            {
                Log.Instance.log("Executing OnPress: " + e.ButtonId + "@" + e.Serial, LogSeverity.Debug);
                onPress.execute(fsuipcCache);
            }
            else if (e.Value == 1 && onPress != null)
            {
                Log.Instance.log("Executing OnRelease: " + e.ButtonId + "@" + e.Serial, LogSeverity.Debug);
                onRelease.execute(fsuipcCache);
            }

        }
    }

}
