using LibraryManagementSystem.DTO;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem
{
    internal class Program
    {
        static private int ReadValidBookYear()
        {
            int year;
            while (true)
            {
                Console.Write("Book year : ");
                if (int.TryParse(Console.ReadLine(), out year)
                && (year > 0 && year <= DateTime.Now.Year))
                    break;

                Console.WriteLine("Please enter a valid year.");
            }
            return year;
        }
        static private string ReadNotEmptyString(string fieldName)
        {
            string text;
            Console.Write($"Book {fieldName} : ");
            text = Console.ReadLine().Trim();
            while (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine($"{fieldName} cannot be empty.");
                Console.Write($"Book {fieldName} : ");
                text = Console.ReadLine().Trim();
            }
            return text;
        }
        static public BookInfoDto ReadBookInfo()
        {
            BookInfoDto bookInfo;

            Console.WriteLine("-----------------------------");
            Console.WriteLine("Book info : ");
            bookInfo.Title = ReadNotEmptyString("Title");
            bookInfo.Author = ReadNotEmptyString("Author");

            bookInfo.Year = ReadValidBookYear();
            bookInfo.Genre = ReadNotEmptyString("Genre");
            Console.WriteLine("-----------------------------");

            return bookInfo;
        }
        static private MemberInfoDto ReadMemberInfo()
        {
            MemberInfoDto MemberInfo;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Member info : ");
            MemberInfo.Name = ReadNotEmptyString("Name");
            MemberInfo.Email = ReadNotEmptyString("Email");
            int input;
            while (true)
            {
                Console.Write("Member Type : ");
                if (int.TryParse(Console.ReadLine(), out input)
                && (input > 0 && input <= 2))
                    break;

                Console.WriteLine("Please enter a valid number .");
            }
            if (input == 1)
                MemberInfo.MemberType = enMemberType.PremiumMember;
            else
                MemberInfo.MemberType = enMemberType.RegularMember;

            Console.WriteLine("-----------------------------");
            return MemberInfo;

        }
        static void Main(string[] args)
        {


        }
    }
}
