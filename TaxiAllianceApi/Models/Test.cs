using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiAllianceApi
{
    public class Test
    {
        [Key]
        public int ID_Test { get; set; }

        private int _test_number;
        public int Test_number
        {
            get => _test_number;
            set
            {
                if (value.ToString().Length == 3)
                    _test_number = value;
            }
        }

        [ForeignKey("HR_Department_ID")]
        public HR_Department HR_Department { get; set; } = new HR_Department();
    }
}
