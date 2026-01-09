using Labb_två_Bokhandel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Labb_två_Bokhandel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new BokhandelContext();

            bool running = true;

            while (running)
            {
                Console.WriteLine(" === Bokhandel App === ");
                Console.WriteLine("1. Lista alla böcker");
                Console.WriteLine("2. Lägg till ny bok");
                Console.WriteLine("3. Ändra boktitel");
                Console.WriteLine("4. Ta bort bok");
                Console.WriteLine("5. Lista alla författare");
                Console.WriteLine("0. Avsluta");
                Console.Write("Välj ett alternativ: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ListBooks(db);
                        break;
                    case "2":
                        AddBook(db);
                        break;
                    case "3":
                        UpdateBook(db);
                        break;
                    case "4":
                        DeleteBook(db);
                        break;
                    case "5":
                        ListAuthors(db);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void ListBooks(BokhandelContext db)
        {
            var books = db.Books
                .OrderBy(b => b.Title)
                .ToList();

            if (!books.Any())
            {
                Console.WriteLine("Inga böcker finns.");
                return;
            }

            foreach (var book in books)
            {
                var authors = book.Authors.Any()
                    ? string.Join(", ", book.Authors.Select(a => a.FirstName + " " + a.LastName))
                    : "Ingen författare";
                Console.WriteLine($"Titel: {book.Title}, ISBN: {book.Isbn13}, Språk: {book.Language}, Pris: {book.Price:C}");
            }
        }

        static void AddBook(BokhandelContext db)
        {
            Console.WriteLine("=== Lägg till ny bok ===");

           
            Console.Write("Titel: ");
            var title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Titel får inte vara tom.");
                return;
            }

            string language;
            while (true)
            {
                Console.Write("Språk: ");
                language = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(language))
                {
                    language = "Okänt";
                    break;
                }
              
                if (Regex.IsMatch(language, @"^[A-Za-zåäöÅÄÖ\s-]+$"))
                    break;
                Console.WriteLine("Fel! Språk får bara innehålla bokstäver.");
            }

          
            decimal price;
            while (true)
            {
                Console.Write("Pris (t.ex. 199 eller 199.90): ");
                var input = Console.ReadLine()?.Replace(',', '.');

                if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out price) &&
                    price > 0 && price <= 9999.99m)
                    break;

                Console.WriteLine("Fel! Ange ett giltigt pris mellan 0 och 9999.99");
            }

         
            DateOnly publishDate;
            while (true)
            {
                Console.Write("Utgivningsdatum (yyyy-mm-dd): ");
                var input = Console.ReadLine();
                if (DateOnly.TryParse(input, out publishDate))
                    break;
                Console.WriteLine("Ange ett giltigt datum (yyyy-mm-dd).");
            }

            
            var authors = db.Authors.OrderBy(a => a.LastName).ToList();
            if (!authors.Any())
            {
                Console.WriteLine("Ingen författare finns i databasen. Lägg till författare.");
                return;
            }

            Console.WriteLine("Välj författare genom att skriva siffran:");
            for (int i = 0; i < authors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {authors[i].FirstName} {authors[i].LastName}");
            }

            int authorChoice;
            while (true)
            {
                Console.Write("Val: ");
                if (int.TryParse(Console.ReadLine(), out authorChoice) &&
                    authorChoice >= 1 && authorChoice <= authors.Count)
                    break;
                Console.WriteLine("Ange ett giltigt nummer från listan.");
            }

            var selectedAuthor = authors[authorChoice - 1];

            var newBook = new Book
            {
                Isbn13 = DateTime.Now.Ticks.ToString().Substring(0, 13),
                Title = title,
                Language = language,
                Price = price,
                PublishDate = publishDate
            };

            newBook.Authors.Add(selectedAuthor);
            db.Books.Add(newBook);
            db.SaveChanges();

            Console.WriteLine($"Boken '{title}' har lagts till med författaren {selectedAuthor.FirstName} {selectedAuthor.LastName}.");
            Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
            Console.ReadKey();
        }

        static void UpdateBook(BokhandelContext db)
        {
            var books = db.Books.OrderBy(b => b.Title).ToList();
            if (!books.Any())
            {
                Console.WriteLine("Ingen bok finns att uppdatera.");

                return;
            }

            Console.WriteLine("Välj bok att uppdatera:");
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {books[i].Title}");
            }

            int choice;
            while (true)
            {
                Console.Write("Val: ");
                if (int.TryParse(Console.ReadLine(), out choice) &&
                    choice >= 1 && choice <= books.Count)
                    break;
                Console.WriteLine("Ange ett giltigt nummer från listan.");
            }

            var book = books[choice - 1];

            Console.Write($"Nuvarande titel: {book.Title}. Skriv ny titel: ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                book.Title = newTitle;
                db.SaveChanges();
                Console.WriteLine("Bok uppdaterad!");
            }
            else
            {
                Console.WriteLine("Titeln uppdaterades inte.");
            }
        }

        static void DeleteBook(BokhandelContext db)
        {
            var books = db.Books
                .Include(b => b.Authors)
                .Include(b => b.Inventories)
                .Include(b => b.OrderDetails)
                .OrderBy(b => b.Title)
                .ToList();

            if (!books.Any())
            {
                Console.WriteLine("Ingen bok finns att ta bort.");
                return;
            }

            Console.WriteLine("Välj bok att ta bort:");
            for (int i = 0; i < books.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {books[i].Title}");
            }

            int choice;
            while (true)
            {
                Console.Write("Val: ");
                if (int.TryParse(Console.ReadLine(), out choice) &&
                    choice >= 1 && choice <= books.Count)
                    break;
                Console.WriteLine("Ange ett giltigt nummer.");
            }

            var book = books[choice - 1];

            if (book.Inventories.Any() || book.OrderDetails.Any())
            {
                Console.WriteLine("Boken kan inte tas bort eftersom den används i lager eller orderhistorik.");
                return;
            }

            book.Authors.Clear();
            db.Books.Remove(book);

            try
            {
                db.SaveChanges();
                Console.WriteLine($"Boken '{book.Title}' har tagits bort.");
            }
            catch
            {
                Console.WriteLine("Boken kunde inte tas bort p.g.a. relationer i databasen.");
            }
        }

        static void ListAuthors(BokhandelContext db)
        {
            var authors = db.Authors.OrderBy(a => a.LastName).ToList();
            if (!authors.Any())
            {
                Console.WriteLine("Inga författare finns.");
                return;
            }

            foreach (var author in authors)
            {
                Console.WriteLine($"ID: {author.AuthorId}, Namn: {author.FirstName} {author.LastName}");
            }
        }
    }
}
