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

        public bool MatchesQuery(string query)
        {
            throw new NotImplementedException();
        }

    }
}
