using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Wardship.Models;

namespace Wardship.Models
{

    public class AdminListView
    {
        public bool onlyActive { get; set; }
        public int page { get; set; }
        public string detailContains { get; set; }
        public string sortOrder { get; set; }
        public int TotalRecordCount { get; set; }
        public int FilteredRecordCount { get; set; }

        public AdminListView()
        {
            onlyActive = true;
            page = 1;
            detailContains = "";
        }
    }
    public class SalutationListView : AdminListView
    {
        public IPagedList<Salutation> Salutations { get; set; }
    }
    public class CourtListView:AdminListView
    {
        public IPagedList<Court> Courts { get; set; }
    }
}
