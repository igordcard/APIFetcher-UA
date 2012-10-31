using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIFetcher
{
    public class Ticket
    {
        private int id;
        private char letter;
        private String description;
        private int latestNumber;
        private TimeSpan balconyTime;
        private TimeSpan waitTime;
        private int waitQueue;
        private DateTime lastUpdate;
        private bool enabled;
        private String info;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public char Letter
        {
            get { return letter; }
            set { letter = value; }
        }

        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public int LatestNumber
        {
            get { return latestNumber; }
            set { latestNumber = value; }
        }

        public TimeSpan BalconyTime
        {
            get { return balconyTime; }
            set { balconyTime = value; }
        }

        public TimeSpan WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }

        public int WaitQueue
        {
            get { return waitQueue; }
            set { waitQueue = value; }
        }

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set { lastUpdate = value; }
        }

        public bool HasInfo()
        {
            if (info.Equals(""))
                return false;
            return true;
        }

        public String Info
        {
            get { return info; }
            set { info = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public void SetWaitTime(int waitSeconds)
        {
            waitTime = new TimeSpan(0, 0, waitSeconds);
        }

        public void SetBalconyTime(int balconySeconds)
        {
            balconyTime = new TimeSpan(0, 0, balconySeconds);
        }

        public Ticket()
        {
        }

        public Ticket(int id, char letter, String description, int latestNumber, int balconySeconds, int waitSeconds, int waitQueue, DateTime lastUpdate, bool enabled, String info)
        {
            this.id = id;
            this.letter = letter;
            this.description = description;
            this.latestNumber = latestNumber;
            //this.balconySeconds = balconySeconds;
            //this.waitSeconds = waitSeconds;
            this.waitQueue = waitQueue;
            this.lastUpdate = lastUpdate;
            this.enabled = enabled;
            this.info = info;
        }
    }
}
