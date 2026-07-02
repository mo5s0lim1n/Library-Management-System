using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public abstract class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set;}

        public LibraryItem(int Id,string Title) 
        {
            this.Id = Id;
            this.Title = Title;
        }

        public DateTime AddedDate { get; set; }

        public abstract string GetInfo();

    }
}
