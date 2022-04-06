using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiAllianceApi
{
    public class Answers
    {
        [Key]
        public int ID_Answer { get; set; }

        private string _answer;
        public string Answer
        {
            get => _answer;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 250))
                    _answer = value;

            }
        }

        public bool Right_answer { get; set; }

        [ForeignKey("Question_ID")]
        public Questions Questions { get; set; } = new();
    }
}
