using System.Data.SqlClient;
namespace ConsoleApp1.menu
{
    public class Ksiazka
    {
        public string Tytul { get; set; }
        public int KsiążkaID { get; set; }
        public string Gatunek { get; set; }
    }
    
    public static class Biblioteka
    {
        public static List<Ksiazka> PobierzWszystkieKsiazki()
        {
            var ksiazki = new List<Ksiazka>();

            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            {
                connection.Open();
                string sql = "SELECT [KsiążkaID],[Tytuł], [Gatunek] FROM [Książki]";
                SqlCommand command = new SqlCommand(sql, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ksiazka = new Ksiazka()
                        {
                            KsiążkaID = reader.GetInt32(reader.GetOrdinal("KsiążkaID")),
                            Tytul = reader.GetString(reader.GetOrdinal("Tytuł")),
                            Gatunek = reader.GetString(reader.GetOrdinal("Gatunek"))
                        };
                        ksiazki.Add(ksiazka);
                    }
                }
            }

            return ksiazki;
        }

        public static void WyswietlTytulyIGatunki()
        {
            List<Ksiazka> ksiazka = PobierzWszystkieKsiazki();

            // Najpierw znajdź najdłuższy tytuł
            int maxDlugoscTytulu = ksiazka.Max(k => k.Tytul.Length);
            //int maxDlugoscKsiazkaID = ksiazka.Max(z => z.KsiazkaID.ToString().Length) + 2;

            // Nagłówki kolumn
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"nr "}{"Tytul".PadRight(maxDlugoscTytulu)}| Gatunek");  
            Console.ResetColor();
            Console.WriteLine(new string('-', maxDlugoscTytulu + " | Gatunek".Length));
            // Wiersze danych
            foreach (var ks in ksiazka)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine($"{ks.KsiążkaID.PadRight(maxDlugoscNr)}{ks.Tytul.PadRight(maxDlugoscTytulu)} | {ks.Gatunek}");
                Console.WriteLine($"{ks.KsiążkaID} {ks.Tytul.PadRight(maxDlugoscTytulu)} | {ks.Gatunek}");

                Console.ResetColor();
            }
        }
    }


}
