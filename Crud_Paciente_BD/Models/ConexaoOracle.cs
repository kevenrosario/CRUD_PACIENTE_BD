using System.Data.OracleClient;

namespace Crud_Paciente_BD.Models
{
    internal class ConexaoOracle
    {
        private string host = "localhost";
        private string database = "basedados_teste";
        private string user = "system";
        private string password = "jan.0495";
        private int port = 1521;
        private OracleConnection conn;
        private OracleCommand cmd;
        public ConexaoOracle()


        {
            conectar();
        }


        public void conectar()
        {
            string strCon = "Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe))); User ID = " + this.user + "; Password = " + this.password + "; ";
            this.conn = new OracleConnection(strCon);
            this.cmd = this.conn.CreateCommand();
            this.conn.Open();
        }
        public void close()
        {
            this.conn.Close();
        }
        public void nonQuery(string sql)
        {
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        public OracleDataReader Query(string sql)
        {
            this.cmd.CommandText = sql;
            return this.cmd.ExecuteReader();
        }
    }
}
