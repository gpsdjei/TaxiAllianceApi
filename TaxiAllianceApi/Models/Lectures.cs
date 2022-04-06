using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiAllianceApi
{
    public class Lectures
    {
        [Key]
        public int ID_Lecture { get; set; }

        private string _name_lecture;
        public string Name_Lecture
        {
            get => _name_lecture;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _name_lecture = value;

            }
        }

        private string _lecture;
        public string Lecture
        {
            get => _lecture;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 250))
                    _lecture = value;

            }
        }

        [ForeignKey("Test_ID")]
        public Test Test { get; set; } = new();
    }
}
