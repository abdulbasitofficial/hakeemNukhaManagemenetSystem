using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hakeem.Models
{
    public class Nuskha
    {
        public IEnumerable<Disease> diseaseslist { get; set; }
        public int ID { get; set; }
        public decimal Stars { get; set; }

        public float StarsNumber { get; set; }
        public int TotalUsers { get; set; }
        public int HakeemID { get; set; }
        public int AccountID { get; set; }
        public string HakeemName { get; set; }
        public string Symptoms { get; set; }
        public int Disease { get; set; }
        public string DiseaseName { get; set; }
        public string Comments { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public string Description { get; set; }
        public byte[] Imge { get; set; }
        public string Type { get; set; }
    }
}