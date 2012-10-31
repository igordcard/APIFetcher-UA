using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
/* TODO: This is a work in progress */
namespace APIFetcher
{
    public class MealService : IService
    {
        private const String serviceURL = "http://services.web.ua.pt/sas/ementas";

        private List<String> canteens;
        private List<Meal> meals;

        public MealService()
        {
            canteens = new List<String>();
            meals = new List<Meal>();
        }

        public void InterpretXML()
        {
            XmlTextReader reader = new XmlTextReader(serviceURL);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        canteens.Add("Depth: " + reader.Depth + ", " + reader.Name);
                        while (reader.MoveToNextAttribute())
                            canteens.Add(reader.Name + "=" + reader.Value);
                        break;
                }
            }
        }

        public List<String> GetCanteens()
        {
            return canteens;
        }
    }
}
