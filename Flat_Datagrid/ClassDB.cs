using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Flat_Datagrid
{
    class ClassDB
    {
        public string GetConnection()
        {
            string cn = "server = localhost; username = root; password = ; database = ui_inventory";
            return cn;
        }
    }
}
