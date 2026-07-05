using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class PremiumMember : Member
    {
        public PremiumMember(int Id, string Name, string Email) : base(Id, Name, Email)  {}
        public const int MaxBorrowLimit = 10;
        public const int LoanDays = 30;

        public override string GetInfo()
        {
            return $"{base.GetInfo()}, Max Borrow: {MaxBorrowLimit}, Loan Days: {LoanDays}";
        }
    }
}
