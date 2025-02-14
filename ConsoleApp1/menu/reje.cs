using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp1.menu
{
    public class Reje
    {
        public static void Rejestracja()
        {
            Console.WriteLine("Podaj nową nazwę użytkownika:");
            string username = Console.ReadLine();
            //-------------------------------------------------------------------
            Console.WriteLine("Podaj  hasło:");
            string password = Console.ReadLine();
            //-------------------------------------------------------------------
            Console.WriteLine("Podaj  E-mail:");
            string E = Console.ReadLine();
            while (!Mail.IsValidEmail(E))
            {
                Console.WriteLine(E + " Nie poprawny format email. Podaj jeszcze raz  E-mail:");
                E = Console.ReadLine(); ;
            }
            string Email = Console.ReadLine(); ;
            //-------------------------------------------------------------------
            Console.WriteLine("Podaj  Imie:");
            string Imie = Console.ReadLine();
            //-------------------------------------------------------------------
            Console.WriteLine("Podaj  Nazwisko:");
            string Nazwisko = Console.ReadLine();
            //-------------------------------------------------------------------
            Console.WriteLine("Podaj  Pesel  :");
            string p = Console.ReadLine();
            while (!Pes.IsValidPesel(p, out DateTime birthDate))
            {
                Console.WriteLine($"Numer PESEL jest poprawny. Podaj jeszcze raz ");
                p = Console.ReadLine();
            }
            
            string Pesel = Console.ReadLine(); ;
            //-------------------------------------------------------------------
            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO konto ([Login], [Haslo],[E-mail],[Imie],[Nazwisko],[Pesel]) " +
                    "VALUES (@username, @password,@Email,@Imie,@Nazwisko,@Pesel)";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@Imie", SqlDbType.NVarChar).Value = Imie;
                command.Parameters.Add("@Nazwisko", SqlDbType.NVarChar).Value = Nazwisko;
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
                command.Parameters.Add("@Pesel", SqlDbType.NVarChar).Value = Pesel;

                int result = command.ExecuteNonQuery();

                if (result == 1)
                {
                    Console.WriteLine("Rejestracja przebiegła pomyślnie.");
                }
                else
                {
                    Console.WriteLine("Rejestracja nie powiodła się.");
                }
            }
        }

    }
}
