
using System.Text.RegularExpressions;


namespace ConsoleApp1.menu
{

    public class Mail
    {
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
    
    public class Pes
    {
      
        public static bool IsValidPesel(string pesel, out DateTime birthDate)
        {

            birthDate = DateTime.MinValue;

         
            if (pesel.Length != 11)
            {
                return false; // PESEL musi mieć 11 cyfr
            }

           
            foreach (char c in pesel)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            // Wyciągnij rok, miesiąc i dzień z numeru PESEL
            int year = int.Parse(pesel.Substring(0, 2));
            int month = int.Parse(pesel.Substring(2, 2));
            int day = int.Parse(pesel.Substring(4, 2));

            // Ustal rok na podstawie pierwszych dwóch cyfr numeru PESEL
            if (month > 80 && month < 93)
            {
                year += 1800;
                month -= 80;
            }
            else if (month > 0 && month < 13)
            {
                year += 1900;
            }
            else if (month > 20 && month < 33)
            {
                year += 2000;
                month -= 20;
            }
            else
            {
                return false; // Niepoprawny miesiąc
            }

            // Utwórz obiekt DateTime z wyciągniętymi danymi
            try
            {
                birthDate = new DateTime(year, month, day);
                return true; // Zwróć true, jeśli numer PESEL jest poprawny
            }
            catch (ArgumentOutOfRangeException)
            {
                return false; // Niepoprawna data
            }
        }

    }

}











