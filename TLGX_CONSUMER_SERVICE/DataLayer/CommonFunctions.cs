
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class CommonFunctions
    {
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            char[] arr = str.Where(c => (char.IsLetterOrDigit(c))).ToArray();
            return new string(arr);
        }
        public static string RemoveSpecialCharactersAndAlphabates(string str)
        {
            StringBuilder sb = new StringBuilder();
            char[] arr = str.Where(c => (char.IsDigit(c))).ToArray();
            return new string(arr);
        }

        public static string GetDigits(string str, int lenghth)
        {
            str = RemoveSpecialCharactersAndAlphabates(str);
            int len = str.Length;
            if (len > lenghth)
                return str.Substring(len - lenghth);
            else
                return str;
        }

        public static string GetCharacter(string str,int lenghth)
        {
            str = RemoveSpecialCharacters(str);
            int len = str.Length;
            if (len > lenghth)
                return str.Substring(len - lenghth);
            else
                return str;
        }

        public static string LatLongTX(string param)
        {
            string ret = "";
            string[] brkparam = param.Split('.');
            if (brkparam.Length > 1)
            {
                if (!string.IsNullOrEmpty(brkparam[1]))
                {
                    ret = brkparam[0] + ".";
                    if (brkparam[1].Length > 4)
                        return ret + brkparam[1].Substring(0, 4);
                    else
                        return ret + brkparam[1];
                }
                else
                    return param;
            }
            else
                return param;

        }

        public static string RemoveSpecialChars(string str)
        {
            return str.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("'", "").Replace("!", "").Replace("#", "").Replace("\"", "");
        }
        public static string RemoveVowels(string str)
        {
            return str.Replace("A", "").Replace("E", "").Replace("I", "").Replace("O", "").Replace("U", "");
        }
        public static string RemoveNumbers(string str)
        {
            return str.Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "");
        }

        public static string HotelNameTX(string HotelName, string cityname, string countryname)
        {
            //replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(A.hotelname))), ' ',''), 'HOTEL','')
            //, 'APARTMENT',''), replace(ltrim(rtrim(upper(A.city))), '''','') ,''),  replace(ltrim(rtrim(upper(A.country))), '''',''),'')
            //, '&',''), 'AND',''), 'THE',''), '-',''), '_',''), '''','')
            if (!string.IsNullOrWhiteSpace(HotelName))
                return (HotelName ?? "").Trim().ToUpper().Replace(" ", "").Replace("HOTELS", "").Replace("APARTMENTS", "").Replace("MOTELS", "").Replace("APARTHOTELS", "").Replace("INNS", "").Replace("RESORTS", "")
                    .Replace("HOTEL", "").Replace("APARTMENT", "").Replace("MOTEL", "").Replace("APARTHOTEL", "").Replace("INN", "").Replace("RESORT", "")
                    .Replace((countryname ?? "~").Trim().ToUpper().Replace("'", ""), "").Replace((cityname ?? "~").Trim().ToUpper().Replace("'", ""), "")
                    .Replace("'", "").Replace("&", "").Replace("AND", "").Replace("THE", "").Replace("-", "").Replace("_", "")
                    .Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace("~", "").Replace(",", "").Replace(".", "").Replace("%", "").Replace("+", "")
                    .Replace("#", "").Replace("/", "").Replace("*", "");
            else
                return "";
        }

        public static string ReturnNumbersFromString(string str)
        {
            return Regex.Replace(str, @"[^\d]", "");
        }

        public static string NumberTo3CharString(int num)
        {
            int n = 0;
            string ret = "000";
            if (num < 0)
            {
                string negative = num.ToString().Replace("-", "");
                n = Convert.ToInt32(negative);
            }
            else
                n = num;
            
            if (n > 0)
            {
                if (n < 99)
                {
                    if (n < 9)
                    {
                        ret = "00" + n.ToString();
                    }
                    else
                        ret = "0" + n.ToString();
                }
                else
                    ret = n.ToString();
            }

            return ret;
        }

        public static string GenerateRoomId(Guid Accommodation_Id)
        {
            string code = "";
            string accocode = "";
            string num = "";
            DataContracts.DC_Accomodation acco = new DataContracts.DC_Accomodation();
            DL_Accomodation dla = new DataLayer.DL_Accomodation();

            if(Accommodation_Id != null && Accommodation_Id != Guid.Empty)
            {
                acco = dla.GetAccomodationShortInfo(Guid.Parse(Accommodation_Id.ToString()));
                if(acco != null)
                {
                    if(acco.CompanyHotelID != null)
                    {
                        accocode = acco.CompanyHotelID.ToString();
                        num = dla.GetNextRoomIdNumber(accocode);
                        code = accocode + "-" + num;
                    }
                }
            }

            return code;
        }
        public static string GenerateCityCode(DataContracts.Masters.DC_City param)
        {
            string code = "";
            string countrycode = "";
            string cityname = "";
            string num = "";
            List<DataContracts.Masters.DC_Country> country = new List<DataContracts.Masters.DC_Country>();
            DataContracts.Masters.DC_Country_Search_RQ RQ = new DataContracts.Masters.DC_Country_Search_RQ();
            DL_Masters dlm = new DL_Masters();
            if (param.Country_Id != null && param.Country_Id != Guid.Empty)
            {
                RQ.Country_Id = param.Country_Id;
                RQ.PageNo = 0;
                RQ.PageSize = int.MaxValue;
                country = dlm.GetCountryMaster(RQ);

                if (country != null && country.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(country[0].ISO3166_1_Alpha_3))
                        countrycode = country[0].ISO3166_1_Alpha_3.ToString();
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(country[0].ISO3166_1_Alpha_2))
                            countrycode = country[0].ISO3166_1_Alpha_2.ToString();
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(country[0].Code))
                                countrycode = country[0].Code.ToString();
                        }
                    }

                    cityname = RemoveSpecialChars(RemoveVowels(param.Name.ToUpper()));
                    int citynamelength = cityname.Length;
                    if (citynamelength > 4)
                        cityname = cityname.Substring(0, 4);
                    else
                        cityname = cityname.Substring(0, citynamelength);
                    num = dlm.GetNextCityCodeNumber(countrycode + "-" + cityname);
                    code = countrycode + "-" + cityname + num;
                }
            }

            return code;
        }
    }
}
