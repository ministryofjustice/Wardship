using System;
using System.Xml;
using System.Text;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Collections.Generic;

using System.Linq;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using Wardship.Models;


namespace Wardship
{
    internal static class generic
    {

        /// <summary>
        /// Returns a list of page numbers to display
        /// with the maximum page display set from config file
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="pageCurrent"></param>
        /// <returns></returns>
        internal static List<string> pagingDisplay(int pageCount, int pageCurrent)
        {
            string displayPagesString = System.Configuration.ConfigurationManager.AppSettings["displaypages"];
            int maxPages = 10; // Default value if the setting is missing or invalid

            if (!string.IsNullOrEmpty(displayPagesString))
            {
                if (!int.TryParse(displayPagesString, out maxPages))
                {
                    maxPages = 10; // Use default if parsing fails
                }
            }

            return pagingDisplay(pageCount, pageCurrent, maxPages);
        }
        /// <summary>
        /// Returns a list of page numbers to display
        /// with the maximum page display ste by the user
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="pageCurrent"></param>
        /// <param name="maxPages"></param>
        /// <returns></returns>
        internal static List<string> pagingDisplay(int pageCount, int pageCurrent, int maxPages)
        {
            List<string> pages = new List<string>();

            //show all
            if (pageCount <= maxPages)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(i.ToString());
                }
                return pages;
            }

            // set halfway break points
            int halfSplitFirstHalfMax = (maxPages - 1) / 2;
            int halfSplitSecondHalfMin = (pageCount - halfSplitFirstHalfMax) + 1;

            if ((pageCurrent <= halfSplitFirstHalfMax) || (pageCurrent >= halfSplitSecondHalfMin))
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    if ((i <= halfSplitFirstHalfMax) || (i >= halfSplitSecondHalfMin) || i == pageCurrent)
                    {
                        pages.Add(i.ToString());
                    }
                    else if (pages.Last() != "...")
                    {
                        pages.Add("...");
                    }
                }
            }
            else
            {
                //Current page is in the middle
                for (int i = 1; i <= pageCount; i++)
                {
                    if ((i <= 3) || (i >= pageCount - 2) || ((i >= (pageCurrent - 2)) && (i <= (pageCurrent + 2))))
                    {
                        pages.Add(i.ToString());
                    }
                    else if (pages.Last() != "...")
                    {
                        pages.Add("...");
                    }
                }
            }

            return pages;
        }


    }

    public static class genericFunctions
    {
        public static byte[] ConvertToBytes(XmlDocument doc)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] docAsBytes = encoding.GetBytes(doc.OuterXml);
            return docAsBytes;
        }
        public static string GetLowestError(Exception ex)
        {
            while (ex.Message.Contains("inner ex"))
            {
                return GetLowestError(ex.InnerException);
            };
            return ex.Message;
        }
        /// <summary>
        /// Send in the number of records and a pluralised string descriptor
        /// </summary>
        /// <param name="numberofRecords">Number of records (from database?)</param>
        /// <param name="toAssess">Field descriptor in plural form</param>
        /// <returns></returns>
        public static string DisplayFieldDescriptorWithRecordCount(int numberofRecords, string toAssess)
        {
            string returnVal = toAssess;
            if (numberofRecords == 1)
            {
                PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-GB"));
                returnVal = ps.Singularize(toAssess);
            }
            return string.Format("{0} {1}", numberofRecords, returnVal);
        }
        /// <summary>
        /// Send in the number of records and a series of pluralised words, each word will be singularised if
        /// the record count = 1
        /// </summary>
        /// <param name="numberofRecords">Number of records (from database?)</param>
        /// <param name="toAssess">A comma delimited series of strings that will be assessed and singularised</param>
        /// <returns></returns>
        public static string DisplayFieldDescriptorWithRecordCount(int numberofRecords, params string[] toAssess)
        {
            PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-GB"));
            List<string> output = new List<string>();
            output.Add(numberofRecords.ToString());
            foreach (var word in toAssess)
            {
                string returnVal = word;
                if (numberofRecords == 1)
                {
                    returnVal = ps.Singularize(returnVal);
                }
                output.Add(returnVal);
            }


            return string.Join(" ", output);
        }

    }
}