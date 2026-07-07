using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public abstract class LibraryItem
    {
        public int Id { get; }
        public string Title { get; set;}
        public DateTime AddedDate { get; }

        public LibraryItem(int id,string title) 
        {
            this.Id = id;
            this.Title = title;
            this.AddedDate = DateTime.Now;
        }


        public abstract string GetInfo();

    }
}
