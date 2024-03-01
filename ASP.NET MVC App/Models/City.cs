using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_MVC_App.Models
{
    public class City
    {
        [Key]
        public int cityid { get; set; }
        public string cityname { get; set; }
        public int stateid { get; set; }
    }
}