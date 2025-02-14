using System.Data.SqlClient;
using ConsoleApp1.log;

namespace ConsoleApp1.Inlog
{
    public class HistoriaWypozyczenia
    {
        public int KlientId { get; set; }

        public string Tytul { get; set; }
        public DateTime TerminOddania { get; set; }

    }

    public static class His
    {
        public static List<HistoriaWypozyczenia> GetHistoria(int klientId)
        {
            var hiss = new List<HistoriaWypozyczenia>();
            //Console.WriteLine(klientId);
            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            {
                connection.Open();
                var query = "SELECT KlientID,Tytuł,Oddanie FROM Historia WHERE KlientID = @KlientID ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KlientID", klientId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var histor = new HistoriaWypozyczenia
                            {
                                KlientId = reader.GetInt32(reader.GetOrdinal("KlientID")),
                                Tytul = reader.GetString(reader.GetOrdinal("Tytuł")),

                                TerminOddania = reader.GetDateTime(reader.GetOrdinal("Oddanie"))
                            };
                            hiss.Add(histor);
                        }
                    }
                }
            }
            return hiss;
        }

    }

}