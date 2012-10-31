using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace APIFetcher
{
    public class TicketService : IService
    {
        private const String serviceURL = "http://services.web.ua.pt/sac/senhas";

        //private const String serviceURL = "senhas.xml";

        private List<Ticket> tickets;

        private DateTime lastUpdate;

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
        }

        public TicketService()
        {
            tickets = new List<Ticket>();
        }

        public void InterpretXML()
        {
            tickets.Clear();

            XmlTextReader reader;

            try
            {
                reader = new XmlTextReader(serviceURL);

                while (reader.Read() && (reader.NodeType != XmlNodeType.Element || reader.Name != "item")) ;

                while (!reader.EOF)
                {
                    bool enabled = false;
                    String info = "";

                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name == "enabled" && reader.Value == "1")
                            enabled = true;
                        else if (reader.Name == "info")
                            info = reader.Value;
                    }

                    reader.MoveToElement();

                    Ticket newTicket = readChilds(reader);
                    newTicket.Enabled = enabled;
                    newTicket.Info = info;
                    tickets.Add(newTicket);

                    while (reader.Read() && (reader.NodeType != XmlNodeType.Element || reader.Name != "item")) ;
                }

                reader.Close();
                reader = null;

                lastUpdate = DateTime.Now;
            }
            catch (Exception)
            {
                tickets.Clear();
                return;
            }
        }

        private Ticket readChilds(XmlTextReader reader)
        {
            int parentDepth = reader.Depth;
            String parentName = reader.Name;

            Ticket newTicket = new Ticket();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Depth > parentDepth + 1)
                        continue;

                    switch (reader.Name)
                    {
                        case "id":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.Id = Int32.Parse(reader.Value);
                            break;
                        case "letter":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.Letter = Char.Parse(reader.Value);
                            break;
                        case "desc":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.Description = reader.Value;
                            break;
                        case "latest":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.LatestNumber = Int32.Parse(reader.Value);
                            break;
                        case "ast":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.SetBalconyTime(Int32.Parse(reader.Value));
                            break;
                        case "awt":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.SetWaitTime(Int32.Parse(reader.Value));
                            break;
                        case "wc":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.WaitQueue = Int32.Parse(reader.Value);
                            break;
                        case "date":
                            while (reader.Read() && reader.NodeType != XmlNodeType.Text) ;
                            newTicket.LastUpdate = DateTime.Parse(reader.Value);
                            break;
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                    if (reader.Name == parentName)
                        return newTicket;
            }
            return newTicket;
        }

        public List<Ticket> GetTicketList()
        {
            return tickets;
        }
    }
}
