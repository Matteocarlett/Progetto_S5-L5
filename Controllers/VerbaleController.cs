using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Polizia.Models;
using System;
using System.Collections.Generic;

namespace Polizia.Controllers
{
    public class VerbaleController : Controller
    {
        private readonly string _connectionString = "Server=DESKTOP-7PVKHJM\\SQLEXPRESS01; Database=Polizia; Integrated Security=true;TrustServerCertificate=True";

        [HttpGet]
        public IActionResult ListaVerbali()
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
                TempData["Errore"] = "Si è verificato un errore durante il recupero dei verbali: " + ex.Message;
                return RedirectToAction("ListaVerbali");
            }

            return View(verbali);
        }

        [HttpGet]
        public ActionResult AggiungiVerbale()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AggiungiVerbale(Verbale model)
        {
            if (ModelState.IsValid)
            {
                string query = "INSERT INTO dbo.Verbale (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti) " +
                               "VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti)";

                try
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        using (var cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@DataViolazione", model.DataViolazione);
                            cmd.Parameters.AddWithValue("@IndirizzoViolazione", model.IndirizzoViolazione);
                            cmd.Parameters.AddWithValue("@Nominativo_Agente", model.Nominativo_Agente);
                            cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", model.DataTrascrizioneVerbale);
                            cmd.Parameters.AddWithValue("@Importo", model.Importo);
                            cmd.Parameters.AddWithValue("@DecurtamentoPunti", model.DecurtamentoPunti);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["Messaggio"] = "Verbale aggiunto con successo!";
                    return RedirectToAction("ListaVerbali");
                }
                catch (Exception ex)
                {
                    TempData["Errore"] = "Si è verificato un errore durante l'aggiunta del verbale: " + ex.Message;
                }
            }
            else
            {
                TempData["Errore"] = "Il modello non è valido. Correggi gli errori e riprova.";
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ModificaVerbale(int IdVerbale)
        {
            Verbale verbale = null;

            try
            {
                using (var sqlConnection = new SqlConnection(_connectionString))
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM Verbale WHERE IdVerbale = @IdVerbale";

                    using (var cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@IdVerbale", IdVerbale);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                verbale = new Verbale()
                                {
                                    IdVerbale = (int)reader["IdVerbale"],
                                    DataViolazione = (DateTime)reader["DataViolazione"],
                                    IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(),
                                    Nominativo_Agente = reader["Nominativo_Agente"].ToString(),
                                    DataTrascrizioneVerbale = (DateTime)reader["DataTrascrizioneVerbale"],
                                    Importo = (decimal)reader["Importo"],
                                    DecurtamentoPunti = (int)reader["DecurtamentoPunti"]
                                };
                            }
                        }
                    }
                }

                if (verbale == null)
                {
                    TempData["Errore"] = "Verbale non trovato!";
                    return RedirectToAction("ListaVerbali");
                }

                return View(verbale);
            }
            catch (Exception ex)
            {
                TempData["Errore"] = "Si è verificato un errore durante il recupero del verbale: " + ex.Message;
                return RedirectToAction("ListaVerbali");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificaVerbale(Verbale verbaleModificato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var sqlConnection = new SqlConnection(_connectionString))
                    {
                        sqlConnection.Open();
                        string query =
                            "UPDATE Verbale SET " +
                            "DataViolazione = @DataViolazione, " +
                            "IndirizzoViolazione = @IndirizzoViolazione, " +
                            "Nominativo_Agente = @Nominativo_Agente, " +
                            "DataTrascrizioneVerbale = @DataTrascrizioneVerbale, " +
                            "Importo = @Importo, " +
                            "DecurtamentoPunti = @DecurtamentoPunti " +
                            "WHERE IdVerbale = @IdVerbale";

                        using (var cmd = new SqlCommand(query, sqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@IdVerbale", verbaleModificato.IdVerbale);
                            cmd.Parameters.AddWithValue("@DataViolazione", verbaleModificato.DataViolazione);
                            cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbaleModificato.IndirizzoViolazione);
                            cmd.Parameters.AddWithValue("@Nominativo_Agente", verbaleModificato.Nominativo_Agente);
                            cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbaleModificato.DataTrascrizioneVerbale);
                            cmd.Parameters.AddWithValue("@Importo", verbaleModificato.Importo);
                            cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbaleModificato.DecurtamentoPunti);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    TempData["Messaggio"] = "Verbale modificato con successo!";
                }
                catch (Exception ex)
                {
                    TempData["Errore"] = "Si è verificato un errore durante il salvataggio delle modifiche: " + ex.Message;
                }
            }
            return RedirectToAction("ListaVerbali");
        }
    }
}
