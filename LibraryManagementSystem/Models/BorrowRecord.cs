using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class BorrowRecord
    {
        public int ID {  get; set; }

        public Book Book { get; set; }
        public Member Member { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public BorrowRecord(int id , Book book , Member member) 
        {
            this.ID = id;
            this.Book = book;
            this.Member = member;
            this.BorrowDate = DateTime.Now;
            this.ReturnDate = null;
        }
        public bool IsLate()
        {
            return ReturnDate == null &&
                   DateTime.Now > BorrowDate.AddDays(Member.LoanDays);
        }

    }
}
