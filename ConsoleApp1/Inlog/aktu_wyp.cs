using System.Data.SqlClient;
using ConsoleApp1.log;
using ConsoleApp1.Inlog;
namespace ConsoleApp1.Inlog
{
    public class AktualneWypozyczenie
    {
        public int KsiazkaID { get; set; }
        public int KlientId { get; set; }

        public string Tytul { get; set; }
        public DateTime TerminOddania { get; set; }

    }
    
    public static class DatabaseOperations
    {
        public static List<AktualneWypozyczenie> GetAktualneWypozyczenie(int klientId)
        {
            var wypozyczenia = new List<AktualneWypozyczenie>();
            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            {
                connection.Open();
                var query = "SELECT KsiążkaID,KlientID,Tytuł,Oddanie FROM [Aktualne wypożyczenia] WHERE KlientID = @KlientID ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KlientID", klientId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var wypozyczenie = new AktualneWypozyczenie
                            {
                                KsiazkaID = reader.GetInt32(reader.GetOrdinal("KsiążkaID")),
                                KlientId = reader.GetInt32(reader.GetOrdinal("KlientID")),
                                Tytul = reader.GetString(reader.GetOrdinal("Tytuł")),
                                TerminOddania = reader.GetDateTime(reader.GetOrdinal("Oddanie"))
                            };
                            wypozyczenia.Add(wypozyczenie);
                        }
                    }
                }
            }

            return wypozyczenia;
        }
    }
}