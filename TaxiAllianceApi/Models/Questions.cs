using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiAllianceApi
{
    public class Questions
    {
        [Key]
        public int ID_Question { get; set; }

        private int _question_number;

        public int Question_number
        {
            get => _question_number;
            set
            {
                if (value.ToString().Length == 3)
                    _question_number = value;
            }
        }

        private string _question;
        public string Question
        {
            get => _question;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 200))
                    _question = value;

            }
        }

        [ForeignKey("Test_ID")]
        public Test Test { get; set; } = new Test();
    }
}

