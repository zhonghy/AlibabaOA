using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlibabaOA.DAL
{
    [Table("eCommerce", Schema = "Chapter2")]
    public class eCommerce : Business
    {
        public string URL { get; set; }
    }
}
