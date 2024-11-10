using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PRWebAPI.Models
{
    public class InteractionDetails
    {
        public int ID { get; set; }
        public DateTime InteractionDate { get; set; }
        public string PatronName { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }            
    }

   
}