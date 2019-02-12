using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbCon = ConfigurationManager.AppSettings.Get("constr");

            try
            {
                using (SqlConnection con = new SqlConnection(dbCon))
                {
                    con.Open();
                    Console.WriteLine("OK!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
