using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiAllianceApi
{
    public class HR_Department
    {
        [Key]
        public int ID_HR_Department { get; set; }

        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _email = value;

            }
        }

        private string _password;

        public string Password
        {
            get => _password;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _password = value;

            }
        }

        [ForeignKey("Role_ID")]
        public Role Role { get; set; } = new Role();
    }
}
