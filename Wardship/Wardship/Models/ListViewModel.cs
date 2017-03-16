using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public abstract class ListViewModel
    {
        /// <summary>
        /// Defines a list view model for paging a sorted and filtered list of objects
        /// Define a specific model with a paged list of data that inherits a ListView model e.g. 
        ///     public class DataTypeListViewModel : ListViewModel
        ///     {
        ///         public IPagedList<DataType> Warrants { get; set; }
        ///     }
        /// </summary>

        public int? page { get; set; }
        public string sortOrder { get; set; }
        [Display(Name = "Show resolved cases?")]
        public bool includeCheck { get; set; }

        public ListViewModel()
        {
            includeCheck = true;
        }
    }
}