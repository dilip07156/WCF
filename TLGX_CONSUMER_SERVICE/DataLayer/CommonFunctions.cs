
using DataContracts.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class CommonFunctions
    {
        public static string[] unitNumerMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN" };

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

        public static string GetCharacter(string str, int lenghth)
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

        public static string HotelNameTX(string HotelName, string cityname, string countryname, ref List<DataContracts.Masters.DC_Keyword> Keywords)
        {

            string returnString = string.Empty;
            List<DC_SupplierRoomName_AttributeList>  AttributeList = new List<DC_SupplierRoomName_AttributeList>();
            string TX_Value = string.Empty;
            string SX_Value = string.Empty;

            if (!string.IsNullOrWhiteSpace(HotelName))
            {
                returnString = CommonFunctions.TTFU(ref Keywords, ref AttributeList, ref TX_Value, ref SX_Value, returnString, new string[] { cityname, countryname });
                return returnString;
            }
            else
                return "";

            //replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(replace(ltrim(rtrim(upper(A.hotelname))), ' ',''), 'HOTEL','')
            //, 'APARTMENT',''), replace(ltrim(rtrim(upper(A.city))), '''','') ,''),  replace(ltrim(rtrim(upper(A.country))), '''',''),'')
            //, '&',''), 'AND',''), 'THE',''), '-',''), '_',''), '''','')
            //if (!string.IsNullOrWhiteSpace(HotelName))
            //    return (HotelName ?? "").Trim().ToUpper().Replace(" ", "").Replace("HOTELS", "").Replace("APARTMENTS", "").Replace("MOTELS", "").Replace("APARTHOTELS", "").Replace("INNS", "").Replace("RESORTS", "")
            //        .Replace("HOTEL", "").Replace("APARTMENT", "").Replace("MOTEL", "").Replace("APARTHOTEL", "").Replace("INN", "").Replace("RESORT", "")
            //        .Replace((countryname ?? "~").Trim().ToUpper().Replace(" ", "").Replace("'", ""), "").Replace((cityname ?? "~").Trim().ToUpper().Replace(" ", "").Replace("'", ""), "")
            //        .Replace("'", "").Replace("&", "").Replace("AND", "").Replace("THE", "").Replace("-", "").Replace("_", "")
            //        .Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace("~", "").Replace(",", "").Replace(".", "").Replace("%", "").Replace("+", "")
            //        .Replace("#", "").Replace("/", "").Replace("*", "");
            //else
            //    return "";
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

            if (Accommodation_Id != null && Accommodation_Id != Guid.Empty)
            {
                acco = dla.GetAccomodationShortInfo(Guid.Parse(Accommodation_Id.ToString()));
                if (acco != null)
                {
                    if (acco.CompanyHotelID != null)
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

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string TTFU(ref List<DataContracts.Masters.DC_Keyword> Keywords,
                                ref List<DataContracts.Mapping.DC_SupplierRoomName_AttributeList> AttributeList,
                                ref string TX_text,
                                ref string SX_text,
                                string OriginalValue, string[] HardRemove)
        {
            string text = OriginalValue;

            #region PRE TTFU

            #region HTML Decode
            text = System.Web.HttpUtility.HtmlDecode(text);
            #endregion

            #region To Upper
            text = text.ToUpper().Trim();
            #endregion

            #region ExtraToBeRemoved
            foreach (var extra in HardRemove)
            {
                if(extra.Trim().Length > 0)
                {
                    text = (" " + text + " ").Replace(" " + extra.ToUpper() + " ", " ");
                }
            }
            #endregion

            #region Remove DiaCritics
            text = CommonFunctions.RemoveDiacritics(text);
            #endregion

            #region Replace the braces
            text = text.Replace("{", "( ");
            text = text.Replace("}", " )");
            text = text.Replace("[", "( ");
            text = text.Replace("]", " )");

            text = text.Replace("(", "( ");
            text = text.Replace(")", " )");


            text = text.Replace(",", " ");
            #endregion

            #region Necessary Replace (Commented)
            //Necessary Replace
            //BaseRoomName = BaseRoomName.Replace("/", " OR ");
            #endregion

            #region Replace Multiple whitespaces into One Whitespace
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s{2,}", " ");
            #endregion

            #region trim both end
            text = text.Trim();
            #endregion

            #endregion

            #region Perform Wildcard Special Operations
            List<DataContracts.Masters.DC_Keyword> SpecialKeywords = Keywords.Where(w => w.Keyword.StartsWith("##") && w.Attribute == false).ToList();
            bool bGeneralisedNumbers = false;

            foreach (var keyword in SpecialKeywords)
            {
                if (keyword.Keyword.ToUpper() == "##_REMOVE_CONTENTS_IN_BRACKETS")
                {
                    try
                    {
                        text = text.Replace("((", "(");
                        text = text.Replace("))", ")");

                        var specialAlias = keyword.Alias.FirstOrDefault();
                        if (specialAlias != null)
                        {
                            if (specialAlias.Value.Trim().ToUpper() == "YES")
                            {
                                int fromIndex = text.IndexOf("(");
                                int toIndex = text.LastIndexOf(")");

                                if (fromIndex != -1 && toIndex != -1 && toIndex > fromIndex)
                                {
                                    if (text.Remove(fromIndex, toIndex - fromIndex + 1).Trim().Length != 0)
                                    {
                                        text = text.Remove(fromIndex, toIndex - fromIndex + 1).Trim();
                                    }
                                }

                                specialAlias.NewHits += 1;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (keyword.Keyword.ToUpper() == "##_REMOVE_WORD_FROM_START")
                {
                    try
                    {
                        var specialAliases = keyword.Alias.OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                        foreach (var alias in keyword.Alias)
                        {
                            if (text.IndexOf(alias.Value.ToUpper().Trim() + " ") != -1)
                            {
                                text = text.Remove(text.IndexOf(alias.Value.ToUpper().Trim() + " "), alias.Value.ToUpper().Trim().Length + 1).Trim();
                                alias.NewHits += 1;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (keyword.Keyword.ToUpper() == "##_REMOVE_WORD_FROM_END")
                {
                    try
                    {
                        var specialAliases = keyword.Alias.OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                        foreach (var alias in keyword.Alias)
                        {
                            if (text.LastIndexOf(" " + alias.Value.ToUpper().Trim()) != -1)
                            {
                                text = text.Remove(text.LastIndexOf(" " + alias.Value.ToUpper().Trim()), alias.Value.ToUpper().Trim().Length + 1).Trim();
                                alias.NewHits += 1;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (keyword.Keyword.ToUpper() == "##_REMOVE_WORD_FROM_STRING")
                {
                    try
                    {
                        var specialAliases = keyword.Alias.OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                        foreach (var alias in keyword.Alias)
                        {
                            text = (" " + text + " ").Replace(" " + alias.Value.ToUpper().Trim() + " ", " ").Trim();
                            alias.NewHits += 1;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (keyword.Keyword.ToUpper() == "##_REMOVE_ANYWHERE_IN_STRING")
                {
                    try
                    {
                        foreach (var alias in keyword.Alias)
                        {
                            text = text.Replace(alias.Value.ToUpper().Trim(), string.Empty).Trim();
                            alias.NewHits += 1;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (keyword.Keyword.ToUpper() == "##_REPLACE_NUMBERS_WITH_WORDS")
                {
                    try
                    {
                        var specialAlias = keyword.Alias.FirstOrDefault();
                        if (specialAlias != null)
                        {
                            if (specialAlias.Value.Trim().ToUpper() == "YES")
                            {
                                bGeneralisedNumbers = true;
                                specialAlias.NewHits += 1;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            #endregion

            #region Single Char replace
            List<DataContracts.Masters.DC_Keyword> SingleCharKeywords = Keywords.Where(w => w.Keyword.Length == 1 && w.Attribute == false && w.Alias.Any(a => a.Value.Length == 1)).ToList();
            foreach (DataContracts.Masters.DC_Keyword singlechar in SingleCharKeywords.OrderBy(o => o.Sequence))
            {
                var singleCharAliases = singlechar.Alias.Where(w => w.Value.Length == 1).OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                foreach (var alias in singleCharAliases)
                {
                    if (text.Contains(alias.Value))
                    {
                        text = text.Replace(alias.Value, singlechar.Keyword);
                        alias.NewHits += 1;
                    }
                }
            }
            #endregion

            #region Take only valid characters (This is commented to take all the chars)
            //string ValidChars = string.Empty;
            //foreach (char c in text)
            //{
            //    if ((Convert.ToInt16(c) >= 32 && Convert.ToInt16(c) <= 196))// || (Convert.ToInt16(c) >= 97 && Convert.ToInt16(c) <= 122) || Convert.ToInt16(c) == 32)
            //    {
            //        ValidChars = ValidChars + c;
            //    }
            //}

            //text = ValidChars;
            #endregion

            #region Check for Spaced Keyword and Replace
            List<DataContracts.Masters.DC_Keyword> SpacedKeywords = Keywords.Where(w => w.Keyword.Length > 1 && !w.Keyword.StartsWith("##") && w.Attribute == false && w.Alias.Any(a => a.Value.Contains(' '))).ToList();
            foreach (DataContracts.Masters.DC_Keyword spacedkey in SpacedKeywords.OrderBy(o => o.Sequence))
            {
                var spacedAliases = spacedkey.Alias.Where(w => w.Value.Contains(' ')).OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                foreach (var alias in spacedAliases)
                {
                    if (text.Contains(alias.Value.ToUpper()))
                    {
                        text = text.Replace(alias.Value.ToUpper(), spacedkey.Keyword);
                        text = text.Replace("()", string.Empty);
                        text = text.Trim();

                        alias.NewHits += 1;
                    }
                }
            }
            #endregion

            #region Keyword Replacement (Split words and replace keywords)

            string[] roomWords = text.Split(' ');

            text = " " + text + " ";

            foreach (string word in roomWords)
            {
                DataContracts.Masters.DC_Keyword keywordSearch = Keywords.Where(k => k.Alias.Any(a => a.Value.ToUpper() == word.ToUpper()) && k.Attribute == false && !k.Keyword.StartsWith("##")).OrderBy(o => o.Sequence).FirstOrDefault();

                if (keywordSearch != null)
                {
                    text = text.Replace(" " + word + " ", " " + keywordSearch.Keyword + " ");
                    var foundAlias = keywordSearch.Alias.Where(w => w.Value.ToUpper() == word.ToUpper()).FirstOrDefault();
                    foundAlias.NoOfHits += 1;
                }

                keywordSearch = null;
            }

            text = text.Trim();

            #endregion

            TX_text = text;

            #region Attribute Extraction
            bool isRoomHaveAttribute = false;
            string sAttributeAlias = string.Empty;
            foreach (var Attribute in Keywords.Where(w => w.Attribute == true && !w.Keyword.StartsWith("##")).OrderBy(o => o.Sequence))
            {
                isRoomHaveAttribute = false;

                var aliases = Attribute.Alias.OrderBy(o => o.Sequence).ThenByDescending(o => (o.NoOfHits + o.NewHits)).ToList();
                foreach (var alias in aliases)
                {
                    isRoomHaveAttribute = false;
                    sAttributeAlias = alias.Value.Replace(",", " ").Trim().ToUpper();
                    sAttributeAlias = sAttributeAlias.Replace("(", "( ");
                    sAttributeAlias = sAttributeAlias.Replace(")", " )");
                    sAttributeAlias = System.Text.RegularExpressions.Regex.Replace(sAttributeAlias, @"\s{2,}", " ");

                    if (sAttributeAlias.StartsWith("(") || sAttributeAlias.EndsWith(")"))
                    {
                        if (text.Trim().Contains(sAttributeAlias))
                        {
                            isRoomHaveAttribute = true;
                        }
                        else
                        {
                            isRoomHaveAttribute = false;
                        }
                    }
                    else
                    {
                        if ((" " + text.Trim() + " ").Contains(" " + sAttributeAlias + " "))
                        {
                            isRoomHaveAttribute = true;
                        }
                        else
                        {
                            isRoomHaveAttribute = false;
                        }
                    }

                    if (isRoomHaveAttribute)
                    {
                        AttributeList.Add(new DataContracts.Mapping.DC_SupplierRoomName_AttributeList
                        {
                            SystemAttributeKeywordID = Attribute.Keyword_Id,
                            SupplierRoomTypeAttribute = alias.Value,
                            SystemAttributeKeyword = Attribute.Keyword
                        });

                        if ((Attribute.AttributeType ?? string.Empty).ToUpper().Contains("STRIP"))
                        {
                            text = text.Replace(sAttributeAlias, string.Empty);
                        }
                        else if ((Attribute.AttributeType ?? string.Empty).ToUpper().Contains("REPLACE"))
                        {
                            text = text.Replace(sAttributeAlias, Attribute.Keyword);
                        }

                        text = System.Text.RegularExpressions.Regex.Replace(text, @"\s{2,}", " ");
                        text = text.Replace("( )", string.Empty);
                        text = text.Replace("()", string.Empty);

                        text = text.Trim();

                        alias.NewHits += 1;

                        isRoomHaveAttribute = false;

                        break;

                    }

                }
            }
            #endregion

            #region Replace 1 to 10 with words
            if(bGeneralisedNumbers)
            {
                roomWords = text.Split(' ');
                foreach (string word in roomWords)
                {
                    int numCheck;
                    if (int.TryParse(word, out numCheck))
                    {
                        if (numCheck >= 0 && numCheck <= 10)
                        {
                            text = text.Replace(word, unitNumerMap[numCheck]);
                        }
                    }
                }
            }
            #endregion

            #region POST TTFU CLEANUP

            #region HardRemoveAgain
            foreach (var extra in HardRemove)
            {
                if (extra.Trim().Length > 0)
                {
                    text = text.Replace(extra.ToUpper(), " ");
                }
            }
            #endregion

            #region Replace UnNecessary chars
            text = text.Replace('<', ' ');
            text = text.Replace('>', ' ');
            text = text.Replace('?', ' ');
            text = text.Replace('#', ' ');
            text = text.Replace('!', ' ');
            text = text.Replace('@', ' ');
            text = text.Replace("&", " AND ");
            //BaseRoomName = BaseRoomName.Replace("+", " INCLUDING ");
            text = text.Replace('(', ' ');
            text = text.Replace(')', ' ');
            text = text.Replace('-', ' ');
            text = text.Replace(',', ' ');
            text = text.Replace('.', ' ');
            text = text.Replace('"', ' ');
            #endregion

            #region Replace Multiple whitespaces into One Whitespace
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\s{2,}", " ");
            #endregion

            #region trim whitespace both end
            text = text.Trim();
            #endregion

            #region Remove logical words from end
            int lastIndex = text.Trim().LastIndexOf(" ");
            if (text.EndsWith(" AND") || text.EndsWith(" OR"))
            {
                if (lastIndex != -1)
                {
                    text = text.Trim().Substring(0, lastIndex).Trim();
                }
            }
            #endregion

            #endregion

            SX_text = text;

            //Retain original value if the post CleanUp data is blank / insufficient.
            if(text.Length <= 1)
            {
                text = OriginalValue.ToUpper();
            }

            return text;
        }
    }
}
