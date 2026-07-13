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
            return $"\nBook Info:\n\tid = {base.Id}\n\tTitle : {base.Title}\n\tAuthor : {Author}\n\tYear :{Year}\n\tGenre : {Genre}";
        }

        public bool MatchesQuery(string query)
        {
            return this.Title.ToLower().Contains(query.ToLower());
        }
    }
}
