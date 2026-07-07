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

        private BorrowRecord[] _borrowRecords = new BorrowRecord[100];
        private ushort currentRecordIndex = 0;
        private ushort lastBorrowProcessId = 0;



        // Add a book feature
        private void ValidateBookInfo(BookInfoDto bookInfo)
        {

            if (string.IsNullOrWhiteSpace(bookInfo.Title))
                throw new ArgumentException("Book title cannot be empty.");

            if (string.IsNullOrWhiteSpace(bookInfo.Author))
                throw new ArgumentException("Book author cannot be empty.");

            if (string.IsNullOrWhiteSpace(bookInfo.Genre))
                throw new ArgumentException("Book genre cannot be empty.");

            if (bookInfo.Year <= 0 || bookInfo.Year > DateTime.Now.Year)
                throw new ArgumentException("Invalid book year.");

        }
        public void AddBook(BookInfoDto bookInfo) 
        {
            if (currentBookIndex >= _books.Length)
            {
                throw new InvalidOperationException("Library storage is full.");
            }

            ValidateBookInfo(bookInfo);
            int BookId = ++lastBookId;
            Book book = new Book(BookId,bookInfo.Title,bookInfo.Author,bookInfo.Year,bookInfo.Genre);
            _books[currentBookIndex++] = book;
        }

        // Add a member feature
        private void ValidateMemberInfo(MemberInfoDto MemberInfo)
        {

            if (string.IsNullOrWhiteSpace(MemberInfo.Name))
                throw new ArgumentException("Member Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(MemberInfo.Email))
                throw new ArgumentException("Member Email cannot be empty.");

        }
        private Member CreateMember(int id , MemberInfoDto MemberInfo)
        {
            ValidateMemberInfo(MemberInfo);
            if (MemberInfo.MemberType == enMemberType.PremiumMember)
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
        
        public void MemberRegistration(MemberInfoDto MemberInfo)
        {
            if (currentMemberIndex >= _members.Length)
            {
                throw new InvalidOperationException("Member List is full.");
            }
            
            _members[currentMemberIndex++]= CreateMember(lastMemberId++, MemberInfo);
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

        //
        private Member FindMemberById(int Id)
        {
            Member member = null;

            for (int i = 0; i < currentMemberIndex; i++) 
            {
                if (_members[i].Id == Id)
                {
                    member = _members[i];
                    break;
                }
            }

            return member;
        }
        private Book FindBookById(int Id)
        {
            Book book = null;
            for (int i = 0; i < currentBookIndex; i++)
            {
                if (_books[i].Id == Id)
                {
                    book = _books[i];
                    break;
                }
            }
            return book;
        }
        public void BorrowBook(int MemberId , int BookId)
        {
            if (currentRecordIndex >= _borrowRecords.Length)
            {
                throw new InvalidOperationException("Borrow records storage is full.");
            }

            Member member = FindMemberById(MemberId);
            if (member == null)
            {
                throw new ArgumentException("Member not found.");
            }

            Book book = FindBookById(BookId);
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

            BorrowRecord borrowRecord = new BorrowRecord(lastBorrowProcessId++, book,member);
            _borrowRecords[currentRecordIndex++] = borrowRecord;
            book.IsAvailable = false;
            
        }

        // Return a book
        public void ReturnBook(int BookId)
        {
            BorrowRecord borrowRecord = null;
            for (int i = 0; i < currentRecordIndex; i++)
            {
                if(_borrowRecords[i].Book.Id == BookId && _borrowRecords[i].ReturnDate == null)
                    borrowRecord = _borrowRecords[i];
            }
            if (borrowRecord == null)
                throw new ArgumentException("No active borrow record found for this book.");
                

            borrowRecord.ReturnDate = DateTime.Now;
            borrowRecord.Book.IsAvailable = true;
            borrowRecord.Member.RemoveBorrowedBook(BookId);
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
    }
}
