using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VeriPark.UI.ViewModels
{
    public class HomeViewModel
    {

       
        public string CheckOutDate { get; set; }
        
        public string ReturnedDate { get; set; }

        public string TotalBusinessDay { get; set; }

        public decimal Penalty { get; set; }

        public string Countries { get; set; }


    }
}