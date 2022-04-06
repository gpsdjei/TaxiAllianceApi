using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiAllianceApi
{
    public class Cabbie
    {
        [Key]
        public int ID_Cabbie { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _name = value;

            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _lastName = value;

            }
        }

        private string _midlleName;
        public string MidlleName
        {
            get => _midlleName;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _midlleName = value;
            }
        }

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

        private string _phone;
        public string Phone
        {
            get => _phone;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _phone = value;
            }
        }

        [ForeignKey("Role_ID")]
        public Role Role { get; set; } = new Role();
    }
}
