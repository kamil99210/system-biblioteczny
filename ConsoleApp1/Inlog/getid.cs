using System.Data.SqlClient;
using ConsoleApp1.log;

namespace ConsoleApp1.Inlog
{

    public static class FindID
    {
        public static int GetKlientIdByLogin(string login)
        {
            int klientId = -1;
            using (SqlConnection connection = new SqlConnection(GlobalSettings.ConnectionString))
            {
                connection.Open();
                string query = "SELECT KlientID FROM konto WHERE Login = @Login";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Login", login);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    klientId = Convert.ToInt32(result);
                }
            }
            return klientId;
        }
    }
}
