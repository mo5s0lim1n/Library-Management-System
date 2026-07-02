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

        public Book book { get; set; }
        public Member member { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool IsLate()
        {
            return false;
        }

    }
}
