using System;
using System.Data.SQLite;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=kunden.db;Version=3;";
        
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS kunden (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    email TEXT,
                    telefon TEXT
                )";
            
            using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
            {
                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("SQLite-Datenbank & Tabelle erstellt!");
        }

        while (true)
        {
            Console.WriteLine("\nWähle eine Aktion:");
            Console.WriteLine("1 - Kunde hinzufügen");
            Console.WriteLine("2 - Kunden anzeigen");
            Console.WriteLine("3 - Kunde aktualisieren");
            Console.WriteLine("4 - Kunde löschen");
            Console.WriteLine("5 - Beenden");
            Console.Write("Auswahl: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Email: ");
                    string email = Console.ReadLine();
                    Console.Write("Telefon: ");
                    string telefon = Console.ReadLine();
                    AddCustomer(name, email, telefon);
                    break;

                case "2":
                    GetCustomers();
                    break;

                case "3":
                    Console.Write("ID des Kunden: ");
                    int idUpdate = int.Parse(Console.ReadLine());
                    Console.Write("Neuer Name: ");
                    string newName = Console.ReadLine();
                    Console.Write("Neue Email: ");
                    string newEmail = Console.ReadLine();
                    Console.Write("Neue Telefonnummer: ");
                    string newTelefon = Console.ReadLine();
                    UpdateCustomer(idUpdate, newName, newEmail, newTelefon);
                    break;

                case "4":
                    Console.Write("ID des Kunden zum Löschen: ");
                    int idDelete = int.Parse(Console.ReadLine());
                    DeleteCustomer(idDelete);
                    break;

                case "5":
                    Console.WriteLine("Programm beendet.");
                    return;

                default:
                    Console.WriteLine("Ungültige Eingabe!");
                    break;
            }
        }
    }

    static void AddCustomer(string name, string email, string telefon)
    {
        string connectionString = "Data Source=kunden.db;Version=3;";

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "INSERT INTO kunden (name, email, telefon) VALUES (@name, @email, @telefon)";
            
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.ExecuteNonQuery();
            }
        }
        Console.WriteLine("Kunde hinzugefügt!");
    }

    static void GetCustomers()
    {
        string connectionString = "Data Source=kunden.db;Version=3;";

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT * FROM kunden";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\nKundenliste:");
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}, Email: {reader["email"]}, Telefon: {reader["telefon"]}");
                }
            }
        }
    }

    static void UpdateCustomer(int id, string name, string email, string telefon)
    {
        string connectionString = "Data Source=kunden.db;Version=3;";

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "UPDATE kunden SET name=@name, email=@email, telefon=@telefon WHERE id=@id";
            
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@telefon", telefon);
                cmd.ExecuteNonQuery();
            }
        }
        Console.WriteLine("Kunde aktualisiert!");
    }

    static void DeleteCustomer(int id)
    {
        string connectionString = "Data Source=kunden.db;Version=3;";

        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();
            string query = "DELETE FROM kunden WHERE id=@id";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
        Console.WriteLine("Kunde gelöscht!");
    }
}
