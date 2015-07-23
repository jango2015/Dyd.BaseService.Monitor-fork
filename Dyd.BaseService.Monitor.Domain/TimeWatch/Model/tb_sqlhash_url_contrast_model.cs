using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dyd.BaseService.Monitor.Domain.TimeWatch.Model
{
    public partial class tb_sqlhash_url_contrast_model
    {
        public decimal avgtime { get; set; }
        public decimal maxtime { get; set; }
        public decimal mintime { get; set; }
        public DateTime date { get; set; }
        public string sql { get; set; }
        public int count { get; set; }
    }
}
