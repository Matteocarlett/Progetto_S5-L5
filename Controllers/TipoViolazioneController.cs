using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;

namespace Polizia.Controllers
{
    public class TipoViolazioneController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-7PVKHJM\\SQLEXPRESS01; Database=Polizia; Integrated Security=true;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult ListaViolazioni()
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

        [HttpGet]
        public ActionResult AggiungiViolazione()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiViolazione(TipoViolazione model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO dbo.[Tipo Violazione] (Descrizione) " +
                               "VALUES (@Descrizione)";

                try
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        using (var cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@Descrizione", model.Descrizione);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["Messaggio"] = "Violazione aggiunta con successo!";
                    return RedirectToAction("ListaViolazioni");
                }
                catch (Exception ex)
                {
                    TempData["Errore"] = "Si è verificato un errore durante l'aggiunta della violazione: " + ex.Message;
                }
            }
            else
            {
                TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminaViolazione(int IdViolazione)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    string query = "DELETE FROM [Tipo Violazione] WHERE IdViolazione = @IdViolazione";

                    using (var cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdViolazione", IdViolazione);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Messaggio"] = "Violazione eliminata con successo!";
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante l'eliminazione della violazione: " + ex.Message;
            }
            return RedirectToAction("ListaViolazioni");
        }

        [HttpGet]
        public ActionResult ModificaViolazione(int IdViolazione)
        {
            TipoViolazione violazione = null;

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM [Tipo Violazione] WHERE IdViolazione = @IdViolazione";

                    using (var cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdViolazione", IdViolazione);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                violazione = new TipoViolazione()
                                {
                                    IdViolazione = (int)reader["IdViolazione"],
                                    Descrizione = reader["Descrizione"].ToString()
                                };
                            }
                        }
                    }
                }

                if (violazione == null)
                {
                    TempData["Errore"] = "Violazione non trovata!";
                    return RedirectToAction("ListaViolazioni");
                }

                return View(violazione);
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante il recupero della violazione: " + ex.Message;
                return RedirectToAction("ListaViolazioni");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvaModifiche(TipoViolazione violazioneModificata)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        string query =
                            "UPDATE [Tipo Violazione] SET " +
                            "Descrizione = @Descrizione " +
                            "WHERE IdViolazione = @IdViolazione";

                        using (var cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@IdViolazione", violazioneModificata.IdViolazione);
                            cmd.Parameters.AddWithValue("@Descrizione", violazioneModificata.Descrizione);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["Messaggio"] = "Violazione modificata con successo!";
                }
                catch (Exception ex)
                {
                    TempData["Errore"] = "Si è verificato un errore durante il salvataggio delle modifiche: " + ex.Message;
                }
            }
            return RedirectToAction("ListaViolazioni");
        }
    }
}
