
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web;

using DtoLayer;

namespace BusinessLayer
{
    public class HomeServices
    {
        public List<CountriesDto> CountryStateList(string CountryOrState)
        {
            //string filePath = Server.MapPath("/Content/CountryStateList/countries.json");
            //string filePath = ExecutionDirectoryPathName();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Content\CountryStateList\countries.json";
            if (CountryOrState == "states")
            {
               
             filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Content\CountryStateList\states.json";
                //Server.MapPath("/Content/CountryStateList/states.json");
            }
            string jsonData = System.IO.File.ReadAllText(filePath);
            StreamReader file = System.IO.File.OpenText(filePath);
            var countrylist = JsonConvert.DeserializeObject<List<CountriesDto>>(jsonData);
            //return countrylist;
            var CountryStateData = countrylist.Select(x => new CountriesDto
            {
                name = x.name,
                id = x.id,
                country_id=x.country_id
            }).ToList();
            return CountryStateData;
        }


       
    }
}


