using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class AnagraficaController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-7PVKHJM\\SQLEXPRESS01; Database=Polizia; Integrated Security=true;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult Index()
        {
            List<Anagrafica> anagrafiche = new List<Anagrafica>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM dbo.Anagrafica", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var anagrafica = new Anagrafica
                            {
                                IdAnagrafica = (int)reader["IdAnagrafica"],
                                IdVerbale = (int)reader["IdVerbale"],
                                Cognome = reader["Cognome"].ToString(),
                                Nome = reader["Nome"].ToString(),
                                Indirizzo = reader["Indirizzo"].ToString(),
                                Città = reader["Città"].ToString(),
                                CAP = reader["CAP"].ToString(),
                                Cod_Fisc = reader["Cod_Fisc"].ToString()
                            };
                            anagrafiche.Add(anagrafica);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante il recupero delle anagrafiche: " + ex.Message;
            }

            return View(anagrafiche);
        }

        [HttpGet]
        public ActionResult ListaTrasgressori()
        {
            List<Anagrafica> trasgressori = new List<Anagrafica>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Anagrafica";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var trasgressore = new Anagrafica
                                {
                                    IdAnagrafica = (int)reader["IdAnagrafica"],
                                    Cognome = reader["Cognome"].ToString(),
                                    Nome = reader["Nome"].ToString(),
                                    Indirizzo = reader["Indirizzo"].ToString(),
                                    Città = reader["Città"].ToString(),
                                    CAP = reader["CAP"].ToString(),
                                    Cod_Fisc = reader["Cod_Fisc"].ToString()
                                };

                                trasgressori.Add(trasgressore);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante il recupero dei trasgressori: " + ex.Message;
            }

            return View(trasgressori);
        }

        [HttpGet]
        public ActionResult AggiungiTrasgressore()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiTrasgressore(Anagrafica model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO dbo.Anagrafica (IdVerbale,Cognome, Nome, Indirizzo, Città, CAP, Cod_Fisc) VALUES (@IdVerbale, @Cognome, @Nome, @Indirizzo, @Città, @CAP, @Cod_Fisc)";

                try
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        using (var cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@IdVerbale", 1);
                            cmd.Parameters.AddWithValue("@Cognome", model.Cognome);
                            cmd.Parameters.AddWithValue("@Nome", model.Nome);
                            cmd.Parameters.AddWithValue("@Indirizzo", model.Indirizzo);
                            cmd.Parameters.AddWithValue("@Città", model.Città);
                            cmd.Parameters.AddWithValue("@CAP", model.CAP);
                            cmd.Parameters.AddWithValue("@Cod_Fisc", model.Cod_Fisc);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    TempData["Messaggio"] = "Trasgressore aggiunto con successo!";
                    return RedirectToAction("ListaTrasgressori");
                }
                catch (Exception ex)
                {
                    TempData["Errore"] = "Si è verificato un errore durante l'aggiunta del trasgressore: " + ex.Message;
                }
            }
            else
            {
                TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            }

            return View(model);
        }
    }
}
