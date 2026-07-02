using LibraryManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class Book : LibraryItem, ISearchable
    {
        public bool IsAvailable { get; set;} 
        public int Year { get; set; }
        public string Author {  get; set; }
        public string Genre { get; set; }

        public Book(int id ,string title , string author , int year , string genre):base(id,title)
        {
            this.IsAvailable = true;
            this.Author = author;
            this.Year = year;
            this.Genre = genre;
        }
        public override string GetInfo()
        {
            return $"Book info : id {base.Id} Title {base.Title} Author {Author} Year {Year} Genre {Genre}";
        }

        public bool MatchesQuery(string query)
        {
            throw new NotImplementedException();
        }
    }
}
