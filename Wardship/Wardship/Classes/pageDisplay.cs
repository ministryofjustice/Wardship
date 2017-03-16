using System;
using System.Collections.Generic;
using System.Linq;
using Wardship.Models;

namespace Wardship
{
    internal static class pageDisplay
    {

        /// <summary>
        /// Returns a list of page numbers to display
        /// with the maximum page display set from config file
        /// NOTE:-Used by Ajax Paged list and helpers
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="pageCurrent"></param>
        /// <returns></returns>
        internal static List<string> pagingDisplay(int pageCount, int pageCurrent)
        {
            int maxPages = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["pageSize"]);
            return pagingDisplay(pageCount, pageCurrent, maxPages);
        }
        /// <summary>
        /// Returns a list of page numbers to display
        /// with the maximum page display ste by the user
        /// NOTE:-Used by Ajax Paged list and helpers
        /// </summary>
        /// <param name="pageCount"></param>
        /// <param name="pageCurrent"></param>
        /// <param name="maxPages"></param>
        /// <returns></returns>
        internal static List<string> pagingDisplay(int pageCount, int pageCurrent, int maxPages)
        {
            List<string> pages = new List<string>();

            //show all
            if (pageCount < maxPages)
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
                    if ((i <= halfSplitFirstHalfMax) || (i >= halfSplitSecondHalfMin))
                    {
                        pages.Add(i.ToString());
                    }
                    else
                    {
                        pages.Add("...");
                        i = halfSplitSecondHalfMin - 1;
                    }
                }
                return pages;
            }
            //Current page must be in the middle then!
            for (int i = 1; i <= pageCount; i++)
            {
                if ((i <= 3) || (i >= pageCount - 2) || ((i >= (pageCurrent - 2)) && (i <= (pageCurrent + 2))))
                {
                    pages.Add(i.ToString());
                }
                else if (pages.Last().ToString() != "...")
                {
                    pages.Add("...");
                }
            }
            return pages;
        }


    }
}