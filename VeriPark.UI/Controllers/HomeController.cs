using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VeriPark.UI.ViewModels;

namespace VeriPark.UI.Controllers
{
    public class HomeController : Controller
    {
        VeriParkEntities ctx = new VeriParkEntities();

        public ActionResult Index()
        {      
            var countries =   ctx.Country.ToList();
            ViewBag.ddlCountries = countries.Select(x => new SelectListItem { Text = x.CountryName, Value = x.Id.ToString() });
            return View();
        }


        [HttpPost]
        public ActionResult Hesapla(string CheckOutDate, string ReturnedDate, string Countries)
        {

            int countryId = Convert.ToInt32(Countries);
            var countryConfiguration = ctx.CountryConfiguration.FirstOrDefault(x => x.CountryId == countryId);

          
            HomeViewModel model = new HomeViewModel();

            // PublicHolidays
            List<DateTime> PublicHolidays = new List<DateTime>();
            string[]  publicholidays = countryConfiguration.PublicHoliday.Split(',');
            for (int i = 0; i < publicholidays.Length; i++)
            {
                PublicHolidays.Add(Convert.ToDateTime(publicholidays[i]));
            }
       
            int publicHolidayCount = 0;
            foreach (DateTime pHolidays in PublicHolidays)
            {
                string[] weekends = countryConfiguration.FreeDay.Split(',');
                foreach (var item in weekends)
                {
                    if (item != ((int)pHolidays.DayOfWeek).ToString() && (pHolidays >= Convert.ToDateTime(CheckOutDate)
                        && pHolidays <= Convert.ToDateTime(ReturnedDate)))
                    {
                        publicHolidayCount++;
                        break;
                    }

                }
            }
            int result = BusinessDayCalculate(Convert.ToDateTime(CheckOutDate), Convert.ToDateTime(ReturnedDate), countryConfiguration.FreeDay.Split(','));
            int tresult = publicHolidayCount;
            int businessDay = result - tresult;

            model.TotalBusinessDay = businessDay.ToString();

            if (businessDay <= 10)
                model.Penalty = Convert.ToDecimal(0);
            else
                model.Penalty = (countryConfiguration.Penalty ?? 0) * Convert.ToDecimal(businessDay - 10);

            return PartialView("_Hesapla", model);

        }


        public static int BusinessDayCalculate(DateTime starttDate, DateTime finishDate,string[] weekends)
        {
            DateTime temporalDate = starttDate;
            int dayCount = 0;
            string day = string.Empty;
            while (temporalDate <= finishDate)
            {

                if (!weekends.Contains(((int)temporalDate.DayOfWeek).ToString()))
                {
                    dayCount++;
                }         
                temporalDate = temporalDate.AddDays(1);
            }
            return dayCount;

        }
    }
}