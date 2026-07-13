using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Services;
using System.Net;
using System.Reflection.PortableExecutable;

namespace LibraryManagementSystem
{
    internal class Program
    {
       
        public enum enUserChoice
        {
            AddBook = 1,
            RegisterMember,
            BorrowBook,
            ReturnBook,
            SearchCatalog,
            ViewAvailableBooks,
            MemberBorrowingHistory,
            LateReturnReport
        }

        #region Menu
        static public void PrintMainMenu()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Library Management System");
            Console.WriteLine("[1] Add a Book ");
            Console.WriteLine("[2] Register a Member");
            Console.WriteLine("[3] Borrow a Book");
            Console.WriteLine("[4] Return a Book");
            Console.WriteLine("[5] Search the Catalog");
            Console.WriteLine("[6] View Available Books");
            Console.WriteLine("[7] Member Borrowing History");
            Console.WriteLine("[8] Late Return Report");
            Console.WriteLine("------------------------------------");
        }
        static public enUserChoice ReadUserChoice()
        {
            int Choice ;
            while (true) 
            {
                Console.Write("Select the transaction number : ");
                if(int.TryParse(Console.ReadLine(), out Choice)  && (Choice >= 1 && Choice <= 8)) 
                {
                    break;
                }
                Console.WriteLine("Please enter a valid transaction number.");
            }

            return (enUserChoice) Choice;
            
        }
        static public void ExecuteUserChoice(Library library , enUserChoice userChoice)
        {
            switch (userChoice) 
            {
                case enUserChoice.AddBook:
                    AddABook(library);
                    break;
                case enUserChoice.RegisterMember:
                    RegisterMember(library);
                    break;
                case enUserChoice.BorrowBook:
                    BorrowABook(library);
                    break;
                case enUserChoice.ReturnBook:
                    ReturnABook(library);
                    break;
                case enUserChoice.SearchCatalog:
                    SearchTheCatalog(library);
                    break;
                case enUserChoice.ViewAvailableBooks:
                    ViewAvailableBooks(library);
                    break;
                case enUserChoice.MemberBorrowingHistory:
                    MemberBorrowingHistory(library);
                    break;
                case enUserChoice.LateReturnReport:
                    PrintLateReturnReport(library);
                    break;
            }
        }
        #endregion

        #region operation
        static public void AddABook(Library library)
        {
            PrintHeader("Add Book");
            string title;
            string author;
            string genre;
            int year;
            try
            {
                ReadBookInfo(out title, out author, out year, out genre);
                library.AddBook(title,  author, year, genre);
                PrintSuccess("Book Added Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static public void RegisterMember(Library library)
        {
            PrintHeader("Add Member");
            string name;
            string email;
            int memberType;
            try
            {
                ReadMemberInfo(out name , out email , out memberType);
                library.MemberRegistration(name,email,memberType);
                PrintSuccess("Member Added Successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static public void BorrowABook(Library library)
        {
            PrintHeader("Borrow A Book");
            int memberId = ReadPositiveNumber("Enter Member id", "Enter a valid id");
            int bookId = ReadPositiveNumber("Enter Book id", "Enter a valid id");

            try
            {
                library.BorrowBook(memberId, bookId);
                PrintSuccess("Member Borrow a Book");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Pause();
            }


        }
        static public void ReturnABook(Library library)
        {
            PrintHeader("Return Book");
            try
            {
                int bookId = ReadPositiveNumber("Enter Book id", "Enter a valid id");
                library.ReturnBook(bookId);
                PrintSuccess("Book Return Successul");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Pause();
            }

        }
        static public void SearchTheCatalog(Library library)
        {
            PrintHeader("Search The Catalog");
            Console.Write("Enter string query : ");
            string query = Console.ReadLine();

            ISearchable[] result;
            result = library.SearchCatalog(query);
            if(result.Length == 0)
            {
                Console.WriteLine("there is no Matches Query");
                Pause();
                return;
            }
            foreach (var item in result)
            {
                if (item is Book book)
                {
                    Console.WriteLine(book.GetInfo());
                }
                else if (item is Member member)
                {
                    Console.WriteLine(member.GetInfo());
                }
            }
            Pause();
        }
        static public void ViewAvailableBooks(Library library)
        {
            PrintHeader("Available Books");
            Book[] books;
            books = library.GetAvailableBooks();
            if(books.Length == 0)
            {
                Console.WriteLine("there is no Available Books .");
                Pause();
                return;
            }
            foreach (var book in books)
            {
                Console.WriteLine(book.GetInfo());
            }
            Pause();
        }
        static public void MemberBorrowingHistory(Library library) 
        {
            int memberId = ReadPositiveNumber("Enter Member id", "Enter a valid id");
            BorrowRecord[] records = library.GetMemberHistory(memberId);
            if (records.Length == 0)
            {
                Console.WriteLine("No borrowing history.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Member Borrowing History");
            foreach (var record in records)
            {
                Console.WriteLine("Book : ");
                Console.Write($"Title : {record.Book.Title}\tBorrowDate : {record.BorrowDate} ");
                if (record.ReturnDate == null)
                    Console.Write($"\tReturn status : not return");
                else
                    Console.Write($"\tReturn status : returned ");
            }
            Pause();
        }
        static public void PrintLateReturnReport(Library library) 
        {
            BorrowRecord[] report = library.GetOverdueBorrowRecords();
            if (report.Length == 0)
            {
                Console.WriteLine("there is no Late Return books");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Late Return Report");
            foreach (var record in report)
            {
                Console.WriteLine($"Member name : {record.Member.Name} Book title : {record.Book.Title} Borrow Date {record.BorrowDate} Days Overdue {library.GetOverdueDays(record)}");
            }
            Pause();
        }
        #endregion

        #region ConsoleHelper
        static public void PrintHeader(string header) 
        {
            Console.WriteLine("___________________________________________");
            Console.WriteLine($"\t\t{header.ToUpper()}\t\t");
            Console.WriteLine("___________________________________________");

        }
        static public void PrintSuccess(string message)
        {
            Console.WriteLine($"\t\t{message.ToUpper()}\t\t");
            Console.WriteLine("___________________________________________");
            Pause();
        }
        static public void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        #endregion

        static public int ReadPositiveNumber(string message , string errorMessage)
        {
            int number;
            while (true)
            {
                Console.Write(message + " ");
                if (int.TryParse(Console.ReadLine(), out number) && number > 0)
                {
                    break;
                }
                Console.Write(errorMessage + " .");
            }

            return number;

        } 
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
        static private string ReadNotEmptyString(string entity ,string fieldName)
        {
            string text;
            Console.Write($"{entity} {fieldName} : ");
            text = Console.ReadLine().Trim();
            while (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine($"{fieldName} cannot be empty.");
                Console.Write($"{entity} {fieldName} : ");
                text = Console.ReadLine().Trim();
            }
            return text;
        }
        static public void ReadBookInfo(out string title, out string author, out int year, out string genre)
        {
            Console.WriteLine("Book Info");
            title = ReadNotEmptyString("Book","Title");
            author = ReadNotEmptyString("Book","Author");

            year = ReadValidBookYear();
            genre = ReadNotEmptyString("Book","Genre");
            Console.WriteLine("___________________________________________");
        }
        static public void ReadMemberInfo(out string name , out string email ,out int membertype)
        {
           
            Console.WriteLine("Member info");
            name =ReadNotEmptyString("Member","Name");
            email = ReadNotEmptyString("Member", "Email");
            int input;
            while (true)
            {
                Console.Write("Member Type : [1]Regular   [2]Premium : ");
                if (int.TryParse(Console.ReadLine(), out input)
                && (input > 0 && input <= 2))
                    break;

                Console.WriteLine("Please enter a valid number .");
            }
            membertype = input;

            Console.WriteLine("___________________________________________");
        }
        

        static void Main(string[] args)
        {
            Library library = new Library();
            library.SeedTestData();
            while (true)
            {
                Console.Clear();
                PrintMainMenu();
                enUserChoice userChoice = ReadUserChoice();
                Console.Clear();
                ExecuteUserChoice(library, userChoice);

            }
        }
    }
}
