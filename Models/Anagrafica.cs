using System.ComponentModel.DataAnnotations;

namespace Polizia.Models
{
    public class Anagrafica
    {
        public int IdAnagrafica { get; set; }

        public int IdVerbale { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio")]
        public string? Cognome { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "L'indirizzo è obbligatorio")]
        public string? Indirizzo { get; set; }

        [Required(ErrorMessage = "La città è obbligatoria")]
        public string? Città { get; set; }

        [Required(ErrorMessage = "Il CAP è obbligatorio")]
        public string? CAP { get; set; }

        [Required(ErrorMessage = "Il codice fiscale è obbligatorio")]
        public string? Cod_Fisc { get; set; }
    }
}
