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
        private Book[] BorrowedBooks { get; }

        public virtual int LoanDays => 14;
        public virtual int MaxBorrowLimit => 7;

        public Member(int Id,string Name,string Email) 
        {
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
            this.JoinDate = DateTime.Now;
            BorrowedBooks = new Book[this.MaxBorrowLimit];
        }
        public bool MatchesQuery(string query)
        {
            throw new NotImplementedException();
        }
        public virtual string GetInfo()
        {
            return $"Member Id {this.Id} Name {this.Name} Email {this.Email} join Date {this.JoinDate} ";
        }

        public int BorrowedBooksCount()
        {
            int count = 0;

            foreach (Book book in BorrowedBooks)
            {
                if (book != null)
                    count++;
            }

            return count;
        }
        public bool AddBorrowedBook(Book book)
        {
            for (int i = 0; i < BorrowedBooks.Length; i++)
            {
                if (BorrowedBooks[i] == null)
                {
                    BorrowedBooks[i] = book;
                    return true;
                }
            }

            return false; 
        }
        public bool RemoveBorrowedBook(int bookId)
        {
            for (int i = 0; i < BorrowedBooks.Length; i++)
            {
                if (BorrowedBooks[i] != null &&
                    BorrowedBooks[i].Id == bookId)
                {
                    BorrowedBooks[i] = null;
                    return true;
                }
            }

            return false;
        }
    }
}
