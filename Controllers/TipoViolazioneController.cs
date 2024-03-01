using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class TipoVIolazioneController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-7PVKHJM\\SQLEXPRESS01; Database=Polizia; Integrated Security=true;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult Index()
        {
            var tipoViolazioni = new List<TipoViolazione>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM dbo.[Tipo Violazione]", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tipoViolazione = new TipoViolazione()
                            {
                                IdViolazione = (int)reader["IdViolazione"],
                                Descrizione = reader["Descrizione"].ToString()
                            };
                            tipoViolazioni.Add(tipoViolazione);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }

            return View(tipoViolazioni);
        }
    }
}
