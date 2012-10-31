using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/* TODO: This is a work in progress */
namespace APIFetcher
{
    public class Meal
    {
        int canteenIndex;
        bool type; // Lunch: true, Dinner: false.
        String mealName;

        public int CanteenIndex
        {
            get { return canteenIndex; }
            set { canteenIndex = value; }
        }

        public bool Type
        {
            get { return type; }
            set { type = value; }
        }

        public String MealName
        {
            get { return mealName; }
            set { mealName = value; }
        }

        public Meal()
        {
        }

        public Meal(int canteenIndex, bool type, String mealName)
        {
            this.canteenIndex = canteenIndex;
            this.type = type;
            this.mealName = mealName;
        }
    }
}
