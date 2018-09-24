﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLinterface
{
    class XMLinterface
    {
        private string XmlName;
        public XMLinterface(string xmlname)
        {
            this.XmlName = xmlname;
        }
        public List<Event> ReadXML()
        {
            List<Event> events = new List<Event>();
            try
            {
                XmlReader reader = XmlReader.Create(XmlName);
                try
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "EVENT")
                        {
                            int id = int.Parse(reader.GetAttribute(0));
                            string name = reader.GetAttribute(1);
                            bool tree = false;
                            bool alwaysawake = false;
                            List<string> triggers = new List<string>();
                            List<string> responses = new List<string>();
                            while (reader.NodeType != XmlNodeType.EndElement)
                            {
                                reader.Read();
                                if (reader.Name == "tree")
                                {
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        if (reader.NodeType == XmlNodeType.Text)
                                        {
                                            tree = bool.Parse(reader.Value);
                                        }
                                    }
                                    reader.Read();
                                }//Read tree value
                                if (reader.Name == "alwaysawake")
                                {
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        if (reader.NodeType == XmlNodeType.Text)
                                        {
                                            alwaysawake = bool.Parse(reader.Value);
                                        }
                                    }
                                    reader.Read();
                                }//Read alwaysawake value

                                if (reader.Name == "triggerstrings")
                                {
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        if (reader.Name == "string")
                                        {
                                            while (reader.NodeType != XmlNodeType.EndElement)
                                            {
                                                reader.Read();
                                                if (reader.NodeType == XmlNodeType.Text)
                                                {
                                                    triggers.Add(reader.Value);
                                                }
                                            }
                                            reader.Read();
                                        } //end if
                                    }
                                }

                                if (reader.Name == "responsestrings")
                                {
                                    while (reader.NodeType != XmlNodeType.EndElement)
                                    {
                                        reader.Read();
                                        if (reader.Name == "string")
                                        {
                                            while (reader.NodeType != XmlNodeType.EndElement)
                                            {
                                                reader.Read();
                                                if (reader.NodeType == XmlNodeType.Text)
                                                {
                                                    triggers.Add(reader.Value);
                                                }
                                            }
                                            reader.Read();
                                        } //end if
                                    }
                                }
                            }
                            events.Add(new Event(name, id, tree, alwaysawake, triggers, responses));
                        }
                    }
                }
                catch
                {
                    Console.WriteLine(XmlName + " is invalid, Overwrite enabled");
                    Console.ReadLine();
                }
                reader.Close();
            }
            catch
            {
                Console.WriteLine(XmlName + " doesn't exsist, will make one when saving");
                Console.ReadLine();
            }
            return events;
        }
        public Event[] LoadEvents()
        {
            List<Event> TempEvents = ReadXML();
            Event[] Events = new Event[TempEvents.Count];
            foreach (Event TempEvent in TempEvents)
            {
                Events[TempEvent.id - 1] = TempEvent;
            }
            return Events;
        }
        public Event[] ConvertListToArray(List<Event> TempEvents)
        {
            Event[] Events = new Event[TempEvents.Count];
            foreach (Event TempEvent in TempEvents)
            {
                Events[TempEvent.id - 1] = TempEvent;
            }
            return Events;
        }
        public void SaveEvents(Event[] Events)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(XmlName, settings);
            writer.WriteStartDocument();
            writer.WriteComment("XML INTERFACE - List of events");
            foreach (Event TempEvent in Events)
            {
                writer.WriteStartElement("EVENT");
                writer.WriteAttributeString("ID", ""+TempEvent.id);
                writer.WriteAttributeString("Name", TempEvent.ename);
                writer.WriteElementString("tree", ""+TempEvent.tree);
                writer.WriteElementString("alwaysawake", ""+TempEvent.alwaysawake);
                writer.WriteStartElement("triggerstrings");
                foreach(string trigger in TempEvent.triggers)
                {
                    writer.WriteElementString("string", trigger);
                }
                writer.WriteEndElement();
                writer.WriteStartElement("responsestrings");
                foreach(string response in TempEvent.responses)
                {
                    writer.WriteElementString("string", response);
                }
                writer.WriteEndElement();
                    
                
            }
            //writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }
    }
}