using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyd.BaseService.Monitor.Domain.CreateTable.Model
{
    public class CreateTableModel
    {
        public string SqlConn { get; set; }
        public string CreateSql { get; set; }
        public string TableName { get; set; }
    }
}
