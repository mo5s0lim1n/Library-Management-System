# 📚 Library Management System

A robust, console-based **C# (.NET 8)** application designed to manage library operations efficiently. This project demonstrates core Object-Oriented Programming (OOP) principles, clean architecture, interface implementation, and structured data management.

---

## 🚀 Features

The application supports **8 core features** grouped into a user-friendly console menu loop with complete input validation and exception handling:

1. **Add a Book:** Prompt-driven book registration with auto-assigned IDs and strict validation.
2. **Register a Member:** Supports both **Regular** and **Premium** memberships, tracking registration dates automatically.
3. **Borrow a Book:** Checks availability, validates member borrowing limits, creates transaction records, and throws exceptions if a book is already loaned out.
4. **Return a Book:** Finds open borrow records, calculates dates, and updates availability status instantly.
5. **Search the Catalog:** A global search engine calling `MatchesQuery()` on both books and members for a case-insensitive match.
6. **View Available Books:** Dynamic filtering showing only currently available books utilizing overridden `GetInfo()` methods.
7. **Member Borrowing History:** Comprehensive logs showing all borrow records, book titles, and statuses for a specific member.
8. **Late Return Report:** Scans open records to identify overdue items using calculated loan limits, displaying days overdue.

---

## 🛠️ Architecture & OOP Design

This project is built from the ground up to showcase clean software engineering practices:

* **Abstraction:** Implemented via `LibraryItem` (abstract base class), ensuring shared properties like `Id`, `Title`, and `AddedDate` are inherited consistently.
* **Inheritance & Polymorphism:** * `Book` extends `LibraryItem` and overrides `GetInfo()`.
  * `PremiumMember` inherits from `Member` to extend limits (e.g., 10 books max, 30-day loan limit) and customize member information output.
* **Interfaces:** The `ISearchable` contract enforces search flexibility using the `MatchesQuery(string query)` method across multiple unrelated classes (`Book` and `Member`).
* **Service Layer Pattern:** The `Library` service encapsulates all business logic, shielding the user interface (`Program.cs`) from data manipulation details.
* **Robust Error Handling:** Wrapped in structured `try-catch` blocks to prevent console crashes on invalid inputs or business logic violations.

---

## 📂 Project Structure

```text
LibraryManagementSystem/
├── Models/
│   ├── LibraryItem.cs       ← Abstract base class
│   ├── Book.cs              ← Inherits LibraryItem, implements ISearchable
│   ├── Member.cs            ← Implements ISearchable
│   ├── PremiumMember.cs     ← Inherits Member (with Premium perks)
│   └── BorrowRecord.cs      ← Transaction & late-fee tracking
├── Interfaces/
│   └── ISearchable.cs       ← Search contract
├── Services/
│   └── Library.cs           ← Core business logic and state management
└── Program.cs               ← Menu loop, user I/O, and try-catch safety net
