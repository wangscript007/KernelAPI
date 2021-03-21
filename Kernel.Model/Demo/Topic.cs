using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Model.Demo
{
    public class Topic
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        public int Clicks { get; set; }
        public string Title { get; set; }
        [Column(DbType = "DATETIME")]
        public DateTime CreateTime { get; set; }
    }

}
