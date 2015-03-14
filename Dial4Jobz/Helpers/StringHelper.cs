using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Dial4Jobz.Helpers
{
    public class StringHelper
    {
        public static bool IsReservedWord(string word)
        {
            return (word == "home" ||
                    word == "candidate" ||
                    word == "organization" ||
                    word == "employer" ||
                    word == "candidates" ||
                    word == "organizations" ||
                    word == "employers" ||
                    word == "account" ||
                    word == "job" ||
                    word == "jobs" ||
                    word == "functions" ||
                    word == "industries" ||
                    word == "languages" ||
                    word == "location" ||
                    word == "skills" ||
                    word == "user" ||
                    word == "users" ||                    
                    word == "error" ||
                    word == "feedback" ||
                    word == "bugs" ||
                    word == "about" ||
                    word == "index" ||
                    word == "search" ||
                    word == "terms" ||
                    word == "privacy" ||
                    word == "poll" ||
                    word == "updates" ||                   
                    word == "delete" ||
                    word == "like" ||
                    word == "unlike" ||
                    word == "sharebyemail" ||
                    word == "share" ||
                    word == "email" ||                    
                    word == "help" ||                    
                    word == "admin" ||
                    word == "invite" ||
                    word == "contact" ||
                    word == "contacts");
        }

        //public static string ToSeoUrl(this string url)
        //{
        //    // make the url lowercase
        //    string encodedUrl = (url ?? "").ToLower();

        //    // replace & with and
        //    encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

        //    // remove characters
        //    encodedUrl = encodedUrl.Replace("'", "");

        //    // remove invalid characters
        //    encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

        //    // remove duplicates
        //    encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

        //    // trim leading & trailing characters
        //    encodedUrl = encodedUrl.Trim('-');

        //    return encodedUrl;
        //}

        public static string NumbersToWords(int inputNumber)
        {
            int inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }

        public static string AssembleQueryStrings(Dictionary<string, string> filters)
        {
            return AssembleQueryStrings(filters, false);
        }
        public static string AssembleQueryStrings(Dictionary<string, string> filters, bool appended)
        {
            if (filters == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder(appended ? "&" : "?");

            if (filters.ContainsKey("what"))
                sb.Append("what=" + filters["what"] + "&");

            if (filters.ContainsKey("where"))
                sb.Append("where=" + filters["where"] + "&");

            if (filters.ContainsKey("loc"))
                sb.Append("loc=" + filters["loc"] + "&");

            if (filters.ContainsKey("skill"))
                sb.Append("skill=" + filters["skill"] + "&");

            if (filters.ContainsKey("position"))
                sb.Append("posi=" + filters["position"] + "&");

            if (filters.ContainsKey("organization"))
                sb.Append("org=" + filters["organization"] + "&");

            //if (filters.ContainsKey("preferredType"))
            //    sb.Append("preType=" + filters["preferredType"] + "&");

            if (filters.ContainsKey("minexperience"))
                sb.Append("minexperience=" + filters["minexperience"] + "&");

            if (filters.ContainsKey("maxexperience"))
                sb.Append("maxexperience=" + filters["maxexperience"] + "&");

            if (filters.ContainsKey("minsalaray"))
                sb.Append("minsalaray=" + filters["minsalaray"] + "&");

            if (filters.ContainsKey("maxsalary"))
                sb.Append("maxsalary=" + filters["maxsalary"] + "&");

            if (filters.ContainsKey("function"))
                sb.Append("function=" + filters["function"] + "&");

            if (filters.ContainsKey("gender"))
                sb.Append("gen=" + filters["gender"] + "&");
     
            return sb.ToString();           
        }
    }
}