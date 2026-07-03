using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class PremiumMember : Member
    {
        public PremiumMember(int Id, string Name, string Email) : base(Id, Name, Email)  
        {

        }
        public int MaxBorrowLimit { get; } = 10;
        public int LoanDays { get; } = 30;

        //public override string GetInfo()
        //{
        //    return $"{base.GetInfo()}, Max Borrow: {MaxBorrowLimit}, Loan Days: {LoanDays}";
        //}
    }
}
