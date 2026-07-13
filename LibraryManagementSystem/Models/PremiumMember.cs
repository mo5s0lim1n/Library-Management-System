using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Models
{
    public class PremiumMember : Member
    {
        public PremiumMember(int id, string name, string email) : base(id, name, email)  {}

        public override int LoanDays => 30;
        public override int MaxBorrowLimit => 15;
        public override string GetInfo()
        {
            return $"{base.GetInfo()}\n\tMax Borrow: {MaxBorrowLimit}\n\tLoan Days: {LoanDays}";
        }
    }
}
