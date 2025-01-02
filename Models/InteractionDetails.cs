using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PRWebAPI.Models;

namespace PRWebAPI.Models
{
    public class InteractionDetails
    {
        public int ID { get; set; }
        public DateTime InteractionDate { get; set; }       
        public DateTime MeetingDate { get; set; }
        public string? Reason { get; set; }
        public string? Comment { get; set; }
        public int ContactDetailsID { get; set; }
        public ContactDetails? ContactDetails { get; set; }
    }

   
}