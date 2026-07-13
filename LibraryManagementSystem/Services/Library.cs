using LibraryManagementSystem.DTO;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.VisualBasic;
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

        private BorrowRecord[] _borrowRecords = new BorrowRecord[100];
        private ushort currentRecordIndex = 0;
        private ushort lastBorrowProcessId = 0;



        // Add a book feature
        private void ValidateBookInfo(string title, string author, int year, string genre)
        {

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Book title cannot be empty.");

            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Book author cannot be empty.");

            if (string.IsNullOrWhiteSpace(genre))
                throw new ArgumentException("Book genre cannot be empty.");

            if (year <= 0 || year > DateTime.Now.Year)
                throw new ArgumentException("Invalid book year.");

        }
        public void AddBook(string title, string author, int year, string genre) 
        {
            if (currentBookIndex >= _books.Length)
            {
                throw new InvalidOperationException("Library storage is full.");
            }

            ValidateBookInfo(title, author, year, genre);
            int BookId = ++lastBookId;
            Book book = new Book(BookId, title, author, year, genre);
            _books[currentBookIndex++] = book;
        }

        // Add a member feature
        private void ValidateMemberInfo(MemberInfoDto memberInfo)
        {

            if (string.IsNullOrWhiteSpace(memberInfo.Name))
                throw new ArgumentException("Member Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(memberInfo.Email))
                throw new ArgumentException("Member Email cannot be empty.");

        }
        private Member CreateMember(int id , MemberInfoDto memberInfo)
        {
            ValidateMemberInfo(memberInfo);
            if (memberInfo.MemberType == enMemberType.PremiumMember)
            {
                PremiumMember member = new PremiumMember(id, memberInfo.Name, memberInfo.Email);
                return member;
            }
            else
            {
                Member member = new Member(id, memberInfo.Name, memberInfo.Email);
                return member;
            }
        }
        
        public void MemberRegistration(MemberInfoDto memberInfo)
        {
            if (currentMemberIndex >= _members.Length)
            {
                throw new InvalidOperationException("Member List is full.");
            }
            
            _members[currentMemberIndex++]= CreateMember(++lastMemberId, memberInfo);
        }

        // Get Available Books
        public Book[] GetAvailableBooks()
        {
            int AvailableBooksCounter = 0;
            for (int i = 0; i < currentBookIndex; i++)
            {
                if (_books[i] != null && _books[i].IsAvailable)
                {
                    AvailableBooksCounter++;
                }
            }

            Book[] AvailableBooks = new Book[AvailableBooksCounter];
            int AvailableBooksindex = 0;
            for (int i =0;i<currentBookIndex;i++) {

                if (_books[i] != null && _books[i].IsAvailable)
                {
                    AvailableBooks[AvailableBooksindex++] = _books[i];
                }
            }
            return AvailableBooks;
        }

        // Borrow A Book
        private Member FindMemberById(int id)
        {
            Member member = null;

            for (int i = 0; i < currentMemberIndex; i++) 
            {
                if (_members[i].Id == id)
                {
                    member = _members[i];
                    break;
                }
            }

            return member;
        }
        private Book FindBookById(int id)
        {
            Book book = null;
            for (int i = 0; i < currentBookIndex; i++)
            {
                if (_books[i].Id == id)
                {
                    book = _books[i];
                    break;
                }
            }
            return book;
        }
        public void BorrowBook(int memberId , int bookId)
        {
            if (currentRecordIndex >= _borrowRecords.Length)
            {
                throw new InvalidOperationException("Borrow records storage is full.");
            }

            Member member = FindMemberById(memberId);
            if (member == null)
            {
                throw new ArgumentException("Member not found.");
            }

            Book book = FindBookById(bookId);
            if (book == null)
            {
                throw new ArgumentException("Book not found.");
            }

            if (!book.IsAvailable)
            {
                throw new ArgumentException("Book is not available.");
            }

            if (member.BorrowedBooksCount() >= member.MaxBorrowLimit)
            {
                throw new InvalidOperationException("Member cannot borrow more books.");
            }

            if (!member.AddBorrowedBook(book))
            {
                throw new InvalidOperationException("Failed to add the book to the member's borrowed books.");
            }

            BorrowRecord borrowRecord = new BorrowRecord(++lastBorrowProcessId, book,member);
            _borrowRecords[currentRecordIndex++] = borrowRecord;
            book.IsAvailable = false;
            
        }

        // Return a book
        public void ReturnBook(int bookId)
        {
            BorrowRecord borrowRecord = null;
            for (int i = 0; i < currentRecordIndex; i++)
            {
                if(_borrowRecords[i].Book.Id == bookId && _borrowRecords[i].ReturnDate == null)
                    borrowRecord = _borrowRecords[i];
            }
            if (borrowRecord == null)
                throw new ArgumentException("No active borrow record found for this book.");
                

            borrowRecord.ReturnDate = DateTime.Now;
            borrowRecord.Book.IsAvailable = true;
            borrowRecord.Member.RemoveBorrowedBook(bookId);
        }

        // Member Borrowing History
        public BorrowRecord[] GetMemberHistory(int memberId) 
        {
            if(FindMemberById(memberId) == null)
            {
                throw new ArgumentException("Member not found.");
            }

            int recordCount = 0;
            for (int i = 0; i < currentRecordIndex; i++)
            {
                if (_borrowRecords[i].Member.Id == memberId)
                    recordCount++;
            }

            BorrowRecord[] memberHistory = new BorrowRecord[recordCount];
            int memberHistoryIndex = 0;
            for (int i = 0; i < currentRecordIndex; i++) 
            {
                if (_borrowRecords[i].Member.Id == memberId)
                {
                    memberHistory[memberHistoryIndex++] = _borrowRecords[i];
                }
            }
            return memberHistory;
        }

        //Search the Catalog
        public ISearchable[] SearchCatalog(string query)
        {
            ISearchable[] tempResults = new ISearchable[currentMemberIndex + currentBookIndex];
            int resultCounter = 0;
            for (int i = 0; i < currentMemberIndex; i++) 
            {
                if (_members[i].MatchesQuery(query))
                    tempResults[resultCounter++] = _members[i];
            }
            for (int i = 0; i < currentBookIndex; i++)
            {
                if (_books[i].MatchesQuery(query))
                    tempResults[resultCounter++] = _books[i];
            }

            ISearchable[] searchResults = new ISearchable[resultCounter];
            for (int i = 0; i < resultCounter; i++)
            {
                searchResults[i] = tempResults[i];
            }

            return searchResults;
        }

        // Late Return Report
        private bool IsLate(BorrowRecord record)
        {
            //if (record == null)
            //    throw new ArgumentNullException(nameof(record));
            if (record.ReturnDate != null)
                return false;
            DateTime dueDate = record.BorrowDate.AddDays(record.Member.LoanDays);

            return DateTime.Now > dueDate;

        }
        public BorrowRecord[] GetOverdueBorrowRecords()
        {
            BorrowRecord[] TempOverdueRecords = new BorrowRecord[currentRecordIndex];
            int OverdueRecordsCounter = 0;
            for (int i = 0; i < currentRecordIndex; i++)
            {
                if (_borrowRecords[i] != null &&  _borrowRecords[i].ReturnDate == null)
                {
                    if (IsLate(_borrowRecords[i]))
                    {
                        TempOverdueRecords[OverdueRecordsCounter++] = _borrowRecords[i];
                    }
                        
                }
            }
            BorrowRecord[] OverdueRecords = new BorrowRecord[OverdueRecordsCounter];
            for(int i = 0; i < OverdueRecordsCounter; i++)
            {
                OverdueRecords[i] = TempOverdueRecords[i];
            }

            return OverdueRecords;
        }

        public int GetOverdueDays(BorrowRecord record)
        {
            if (!IsLate(record))
                return 0;

            DateTime dueDate = record.BorrowDate.AddDays(record.Member.LoanDays);

            return (DateTime.Now - dueDate).Days;
        }
    }
}
