using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class VerbaleController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-7PVKHJM\\SQLEXPRESS01; Database=Polizia; Integrated Security=true;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult Index()
        {
            var verbali = new List<Verbale>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM dbo.Verbale", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var verbale = new Verbale()
                            {
                                IdVerbale = (int)reader["IdVerbale"],
                                DataViolazione = (DateTime)reader["DataViolazione"],
                                IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                                DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                Importo = (decimal)reader["Importo"],
                                DecurtamentoPunti = (int)reader["DecurtamentoPunti"]
                            };
                            verbali.Add(verbale);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }

            return View(verbali);
        }
    }
}
