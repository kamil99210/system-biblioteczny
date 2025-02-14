using System.Data.SqlClient;
using ConsoleApp1.log;
using ConsoleApp1.menu;

namespace ConsoleApp1.Inlog
{

    public class Wyporzyczenie
    {
        public int KsiazkaID { get; set; }
        public string Tytul { get; set; }
        public DateTime TerminOddania { get; set; }
    }
    public static class Jaka
    {// znalezienie książki jaką chcemy wypożyczyć 
        public static Wyporzyczenie Getksiazka(int ksiazkaId)
        {
            using (SqlConnection connection = new SqlConnection(ConsoleApp1.log.GlobalSettings.ConnectionString))
            {   
                // otwarcie połączenie z bazą i wysłanie zapytania 
                connection.Open();
                var query = "SELECT KsiążkaID, Tytuł FROM Książki WHERE KsiążkaID = @KsiazkaID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KsiazkaID", ksiazkaId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var wyporzyczenie = new Wyporzyczenie
                            { 
                                KsiazkaID = reader.GetInt32(reader.GetOrdinal("KsiążkaID")),
                                Tytul = reader.GetString(reader.GetOrdinal("Tytuł")),
                            };
                            return wyporzyczenie; // Zwróć obiekt
                        }
                    }
                }
            }
            return null; 
        }
    }


    public static class dodajwybor
    {
        // dodanie wybranej książki do aktualnych wypożyczeń książki 
        public static void DodajWypozyczenie(int ksiazkaId, int klientId)
        {
            Wyporzyczenie wyporzyczenie = Jaka.Getksiazka(ksiazkaId);
            if (wyporzyczenie == null)
            {
                throw new Exception("Nie znaleziono książki o podanym ID.");
            }
            // przypisanie ID klienta 
            string login = LOG.PublicLogin;
            if (klientId == -1)
            {
                // Logowanie błędu lub inna forma powiadomienia, że klient nie został znaleziony.
                throw new Exception("Nie znaleziono klienta o podanym loginie.");
            }
            using (SqlConnection connection = new SqlConnection(ConsoleApp1.log.GlobalSettings.ConnectionString))
            { 
                // otwarcie połączenia żeby dodać książkę do wypożyczeń 
                connection.Open();
                string sql = "INSERT INTO [Aktualne wypożyczenia] (KlientID, Login, Tytuł, Oddanie, KsiążkaID) " +
                             "VALUES (@KlientID, @Login, @Tytul, @DataWypozyczenia, @KsiążkaID)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@KlientID", klientId);
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Tytul", wyporzyczenie.Tytul);
                    command.Parameters.AddWithValue("@DataWypozyczenia", DateTime.Now.AddDays(30));
                    command.Parameters.AddWithValue("@KsiążkaID", wyporzyczenie.KsiazkaID);
                    try
                    {
                        // sprawdzenie czy udało się wypożyć książkę 
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                        {
                            throw new Exception("Nie udało się dodać wypożyczenia.");
                        }
                        else
                        {
                            Console.WriteLine($"Udało się wypożyczyć książkę o nr : {wyporzyczenie.KsiazkaID}");
                        }
                    }
                    catch (SqlException e)
                    {
                        // wiadomośc jeśli się nie udz
                        throw new Exception("Wystąpił błąd podczas dodawania wypożyczenia: " + e.Message);
                    }
                }
            }
        }
    }
}





