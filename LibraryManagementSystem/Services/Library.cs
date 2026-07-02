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
        private Book[] BookList = new Book[100];
        private int currentIndex = 0;
        private int lastId = 0;

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
        private strBookInfo ReadBookInfo()
        {
            strBookInfo bookInfo;

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
            if (currentIndex >= BookList.Length)
            {
                Console.WriteLine("Library is full.");
                return;
            }
            strBookInfo bookInfo = ReadBookInfo();
            int BookId = ++lastId;
            Book book = new Book(BookId,bookInfo.Title,bookInfo.Author,bookInfo.Year,bookInfo.Genre);
            BookList[currentIndex++] = book;
        }
    }
}
