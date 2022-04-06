using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiAllianceApi
{
    public class Role
    {
        [Key]
        public int ID_Role { get; set; }

        private string _role;

        public string Name_Role
        {
            get => _role;
            set
            {
                if ((!string.IsNullOrWhiteSpace(value)) && (value.Length <= 100))
                    _role = value;

            }
        }
    }
}
