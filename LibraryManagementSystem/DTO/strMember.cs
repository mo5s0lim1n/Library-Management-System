using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.DTO
{
    public enum enMemberType{
        RegularMember,
        PremiumMember
    }
    public struct strMember
    {
        public string Name;
        public string Email;
        public enMemberType MemberType;

    }
}
