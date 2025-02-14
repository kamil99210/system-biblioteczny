using System.Data.SqlClient;
using ConsoleApp1.menu;


namespace ConsoleApp1.Inlog
{

    public class DoUsuniecia
    {
        public int KlientId { get; set; }
        public  string Login { get; set; }
        public  string Tytul { get; set; }
        public int KsiazkaID { get; set; }
    }
    public static class zobacz
    {

        public static DoUsuniecia GetWypozyczenie(int klientId, int ksiazkaId)
        {
            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            { 
                connection.Open();
                var query = "SELECT KlientID, Login, Tytuł, KsiążkaID FROM [Aktualne wypożyczenia] WHERE KlientID = @KlientID AND KsiążkaID = @KsiazkaID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KlientID", klientId);
                    command.Parameters.AddWithValue("@KsiazkaID", ksiazkaId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new DoUsuniecia
                            {
                                KlientId = reader.GetInt32(reader.GetOrdinal("KlientID")),
                                Login = reader.GetString(reader.GetOrdinal("Login")), 
                                Tytul = reader.GetString(reader.GetOrdinal("Tytuł")),
                                KsiazkaID = reader.GetInt32(reader.GetOrdinal("KsiążkaID"))
                            };
                        }
                    }
                }
            }
            return null; // Jeśli nie znaleziono wypożyczenia, zwróć null
        }
    }
    public static class Oddajwybor
    {
        public static void UsunWypozyczenie(int klientId, int ksiazkaId)
        {
            //DoUsuniecia wypozyczenieDoUsuniecia = null;
            DoUsuniecia wypozyczenieDoUsuniecia = zobacz.GetWypozyczenie(klientId, ksiazkaId);
            if (wypozyczenieDoUsuniecia == null)
            {
                throw new Exception("Nie znaleziono wypożyczenia do usunięcia.");
            }
            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            {
                connection.Open();
                var query = "Delete From [Aktualne wypożyczenia] where KlientID=@KlientID and KsiążkaID = @KsiazkaID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KlientID", klientId);
                    command.Parameters.AddWithValue("@KsiazkaID", ksiazkaId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void ADDtoHist(DoUsuniecia wypozyczenieDoUsuniecia)
        {
           
            using (SqlConnection connection = new SqlConnection(ConsoleApp1.log.GlobalSettings.ConnectionString))
            {
                
                connection.Open();
                string sql = "INSERT INTO [Historia] (KlientID, Login, Tytuł, Oddanie) " +
                             "VALUES (@KlientID, @Login, @Tytul, @DataWypozyczenia)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@KlientID", wypozyczenieDoUsuniecia.KlientId);
                    command.Parameters.AddWithValue("@Login", wypozyczenieDoUsuniecia.Login);
                    command.Parameters.AddWithValue("@Tytul", wypozyczenieDoUsuniecia.Tytul);
                    command.Parameters.AddWithValue("@DataWypozyczenia", DateTime.Now);
                    try
                    {
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Console.WriteLine($"Udało się dodać wypożyczenie książki o nr: {wypozyczenieDoUsuniecia.KsiazkaID} do historii.");
                            Console.ReadKey();
                        }
                        else
                        {
                            throw new Exception("Nie udało się dodać wypożyczenia do historii.");
                        }
                    }
                    catch (SqlException e)
                    {
                        throw new Exception("Wystąpił błąd podczas dodawania wypożyczenia do historii: " + e.Message);
                    }
                }
            }
            List<HistoriaWypozyczenia> histor = His.GetHistoria(wypozyczenieDoUsuniecia.KlientId);


            foreach (var wypozyczenie in histor)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nTytuł: {wypozyczenie.Tytul},Termin Oddania: {wypozyczenie.TerminOddania}");
                Console.ResetColor();
            }


        }
    } 

}