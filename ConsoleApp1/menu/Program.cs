using ConsoleApp1.Inlog;
namespace ConsoleApp1.menu
{

    public static class GlobalSettings
    {
        public static string ConnectionString { get; set; } = "Server=LAPTOP-CLEQ397M\\SQLEXPRESS;Database=baza_w67119;Integrated Security=SSPI;";
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool programIsRunning = true;

            while (programIsRunning)
            {
                Console.WriteLine("\nWitaj w E-bibliotece!");
                Console.WriteLine("1. Zaloguj się");
                Console.WriteLine("2. Zarejestruj się");
                Console.WriteLine("3. Dostępne książki");
                Console.WriteLine("0. Wyjście");

                Console.Write("Wybierz opcję: ");
                string wybor = Console.ReadLine();

                switch (wybor)
                {
                    case "1":
                        /*
                        LOG.ZalogujSie();
                        AktualneWypozyczenie wypozyczenie = new AktualneWypozyczenie(LOG.PublicLogin);
                        */


                        if (LOG.ZalogujSie()) // Zakładając, że ta funkcja ustawia PublicLogin na zalogowany login
                        {
                            int klientId = FindID.GetKlientIdByLogin(LOG.PublicLogin);
                            List<AktualneWypozyczenie> wypozyczenia = DatabaseOperations.GetAktualneWypozyczenie(klientId);

                            // Tutaj możesz zrobić coś z listą wypozyczenia, na przykład wyświetlić je
                            foreach (var wypozyczenie in wypozyczenia)
                            {
                                Console.WriteLine($"Tytuł: {wypozyczenie.Tytul}, Termin Oddania: {wypozyczenie.TerminOddania}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nie udało się zalogować.");
                        }
                        break;
                    case "2":
                        Reje.Rejestracja();
                        break;
                    case "3":
                        Biblioteka.WyswietlTytulyIGatunki();
                        break;
                    case "0":
                        programIsRunning = false;
                        Console.WriteLine("\nZamykanie programu...");
                        break;
                    default:
                        Console.WriteLine("\nNiepoprawna opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }
    }
}
