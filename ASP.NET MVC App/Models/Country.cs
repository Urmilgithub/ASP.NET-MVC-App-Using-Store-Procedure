using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_App.Models
{
    public class Country
    {
        [Key]
        public int countryid { get; set; }
        public string countryname { get; set; }
    }
}