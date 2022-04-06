using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiAllianceApi
{
    public class Score
    {
        [Key]
        public int ID_Score { get; set; }

        private string _score_number;
        public string Score_number
        {
            get => _score_number;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _score_number = value;

            }
        }

        [ForeignKey("ID_Cabbie")]
        public Cabbie Cabbie { get; set; } = new();

        [ForeignKey("ID_Test")]
        public Test Test { get; set; } = new();
    }
}
