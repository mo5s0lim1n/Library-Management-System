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

        public int Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; }
        private Book[] _BorrowedBooks ;

        public virtual int LoanDays => 14;
        public virtual int MaxBorrowLimit => 7;

        public Member(int id,string name,string email) 
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.JoinDate = DateTime.Now;
            _BorrowedBooks = new Book[this.MaxBorrowLimit];
        }
        public bool MatchesQuery(string query)
        {
            return (query.ToLower() == this.Name.ToLower());
        }
        public virtual string GetInfo()
        {
            return $"\n\tMember Info:\n\tMember Id {this.Id}\n\tName {this.Name}\n\tEmail {this.Email}\n\tjoin Date {this.JoinDate}";
        }

        public int BorrowedBooksCount()
        {
            int count = 0;

            foreach (Book book in _BorrowedBooks)
            {
                if (book != null)
                    count++;
            }

            return count;
        }
        public bool AddBorrowedBook(Book book)
        {
            // what if array full ? ---> handle in Library.cs
            for (int i = 0; i < _BorrowedBooks.Length; i++)
            {
                if (_BorrowedBooks[i] == null)
                {
                    _BorrowedBooks[i] = book;
                    return true;
                }
            }

            return false; 
        }
        public bool RemoveBorrowedBook(int bookId)
        {
            for (int i = 0; i < _BorrowedBooks.Length; i++)
            {
                if (_BorrowedBooks[i] != null &&
                    _BorrowedBooks[i].Id == bookId)
                {
                    _BorrowedBooks[i] = null;
                    return true;
                }
            }

            return false;
        }
    }
}
