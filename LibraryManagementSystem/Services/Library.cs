using LibraryManagementSystem.DTO;
using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryManagementSystem.Services
{
    public class Library
    {
        private Member[] _members = new Member[100];
        private ushort currentMemberIndex = 0;
        private ushort lastMemberId = 0;

        private Book[] _books = new Book[100];
        private ushort currentBookIndex = 0;
        private ushort lastBookId = 0;

        private string ReadNotEmptyString(string fieldName)
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
        private int ReadValidBookYear()
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
        private BookInfoDto ReadBookInfo()
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
        public void AddBook() 
        {
            if (currentBookIndex >= _books.Length)
            {
                Console.WriteLine("Library is full.");
                return;
            }
            BookInfoDto bookInfo = ReadBookInfo();
            int BookId = ++lastBookId;
            Book book = new Book(BookId,bookInfo.Title,bookInfo.Author,bookInfo.Year,bookInfo.Genre);
            _books[currentBookIndex++] = book;
        }

        private Member CreateMember(int id , MemberInfoDto MemberInfo)
        {
            
            if(MemberInfo.MemberType == enMemberType.PremiumMember)
            {
                PremiumMember member = new PremiumMember(id, MemberInfo.Name, MemberInfo.Email);
                return member;
            }
            else
            {
                Member member = new Member(id, MemberInfo.Name, MemberInfo.Email);
                return member;
            }
        }
        private MemberInfoDto ReadMemberInfo()
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
            if(input == 1) 
                MemberInfo.MemberType = enMemberType.PremiumMember;
            else 
                MemberInfo.MemberType = enMemberType.RegularMember;

            Console.WriteLine("-----------------------------");
            return MemberInfo;

        }
        public void MemberRegistration()
        {
            if(currentMemberIndex >= _members.Length)
            {
                Console.WriteLine("Member List is full.");
                return;
            }
            _members[currentMemberIndex++]= CreateMember(lastMemberId++,ReadMemberInfo());
        }

        public void PrintAvailableBooks()
        {
            int availableCount = 0;

            foreach (var book in _books)
            {
                if (book.IsAvailable)
                {
                    Console.WriteLine(book.GetInfo());
                    availableCount++;
                }
            }

            if (availableCount == 0)
            {
                Console.WriteLine("No books available at the moment.");
            }
        }

    }
}
