using LibraryManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LibraryManagementSystem.Models
{
    public class Member : ISearchable
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public Book[] BorrowedBooks { get; set; }

        public Member(int Id,string Name,string Email) 
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.JoinDate = DateTime.Now;
        }
        public bool MatchesQuery(string query)
        {
            throw new NotImplementedException();
        }
        public virtual string GetInfo()
        {
            return $"Member Id {this.Id} Name {this.Name} Email {this.Email} join Date {this.JoinDate} ";
        }
    }
}
