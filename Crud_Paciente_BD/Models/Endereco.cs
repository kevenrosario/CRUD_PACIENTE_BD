using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient; // conexao com o banco de dados
using System.Data.OracleClient; // conexao com o banco de dados

namespace Crud_Paciente_BD.Models
{
    internal class Endereco
    {
        //ATRIBUTOS
        public int id_endereco;
        public string logradouro;
        public string numero;
        public string complemento;
        public string bairro;
        public string municipio;
        public string uf;
        public string cep;
        private ConexaoOracle banco;

        //CONSTRUTOR
        public Endereco()
        {
            this.id_endereco = 0;
            this.logradouro = "";
            this.numero = "";
            this.complemento = "";
            this.bairro = "";
            this.municipio = "";
            this.uf = "";
            this.cep = "";
            this.banco = new ConexaoOracle();


        }
        public void SetId_endereco(int novo) { this.id_endereco = novo; }
        public void SetLogradouro(string novo) { this.logradouro = novo; }
        public void SetNumero(string novo) { this.numero = novo; }
        public void SetComplemento(string novo) { this.complemento = novo; }
        public void SetBairro(string novo) { this.bairro = novo; }
        public void SetMunicipio(string novo) { this.municipio = novo; }
        public void SetUf(string novo) { this.uf = novo; }
        public void SetCep(string novo) { this.cep = novo; }

        public int GetId_endereco() { return this.id_endereco; }
        public string GetLogradouro() { return this.logradouro; }
        public string GetNumero() { return this.numero; }
        public string GetComplemento() { return this.complemento; }
        public string GetBairro() { return this.bairro; }
        public string GetMunicipio() { return this.municipio; }
        public string GetUf() { return this.uf; }
        public string GetCep() { return this.cep; }

        //---INSERIR---
        public void cadastrarEndereco()
        {
            OracleDataReader reader;
            this.banco.conectar();
            this.banco.nonQuery("insert into endereco (logradouro,numero,complemento,bairro,municipio,uf,cep)  values " +
                "('" + this.GetLogradouro() + "', '" +
                this.GetNumero() + "','" +
                this.GetComplemento() + "','" +
                this.GetBairro() + "','" +
                this.GetMunicipio() + "','" +
                this.GetUf() + "','" +
                this.GetCep() + "')");
            reader = this.banco.Query("select id_endereco from endereco where" +
                " logradouro='" + this.GetLogradouro() +
                "' and numero='" + this.GetNumero() +
                "' and bairro='" + this.GetBairro() +
                "' and municipio='" + this.GetMunicipio() +
                "' and uf='" + this.GetUf() +
                "' and CEP= '" + this.GetCep()+"'");
            reader.Read();
            int endereco = reader.GetInt32(0);
            this.SetId_endereco(endereco);
            this.banco.close();
        }
        //---ALTERAR---
        public void AlterarEndereco(int id)
        {
            this.SetId_endereco(id);
            this.banco.conectar();
            this.banco.nonQuery("UPDATE endereco SET " +
                "Logradouro = '" + this.GetLogradouro() +
                "', Numero = '" + this.GetNumero() +
                "', complemento = '" + this.GetComplemento() +
                "', Bairro = '" + this.GetBairro() +
                "', municipio = '" + this.GetMunicipio() +
                "', UF = '" + this.GetUf() +
                "', CEP = '" + this.GetCep() +
                "' WHERE (id_endereco = '" + this.GetId_endereco() + "')");
            this.banco.close();
        }
        //---EXCLUIR---
        public void ExcluirEndereco(int id)
        {
            this.SetId_endereco(id);
            this.banco.conectar();
            this.banco.nonQuery("Delete from endereco where id_endereco = '" + this.GetId_endereco() + "'");
            this.banco.close();
        }
        //---LISTAR---
        public  OracleDataReader listarEnderecos()
        {
            this.banco.conectar();
            return this.banco.Query("select id_endereco, logradouro, numero,complemento,bairro, municipio, uf," +
                " CEP from endereco");
            
        }
        //---CONTAGEM--
        public int QuantidadeEnderecos()
        {
            this.banco.conectar();
            int contagem = 0;
            var temp = this.banco.Query("SELECT COUNT(*) FROM ENDERECO");
            while (temp.Read())
            {
                contagem = temp.GetInt32(0);
            }
            return contagem;            
        }

        public OracleDataReader ListarEnderecosPorIdPaciente(int id)
        {
            this.banco.conectar();
            return this.banco.Query("select e.id_endereco, e.logradouro, e.numero, e.complemento, e.bairro, e.municipio, e.uf, e.cep from endereco e " +
                "inner join paciente p on e.id_endereco = p.id_endereco " +
                "where p.id_paciente = " + id + "");
        }

        public List<Endereco> GetEnderecosPorIdPaciente(int id)
        {
            List<Endereco> lista = new List<Endereco>();

            this.banco.conectar();
            var enderecos = ListarEnderecosPorIdPaciente(id);

            try
            {
                while (enderecos.HasRows)
                {
                    while (enderecos.Read())
                    {
                        Endereco listaEndereco = new Endereco();

                        listaEndereco.SetId_endereco(enderecos.GetInt32(0));
                        listaEndereco.SetLogradouro(enderecos.GetString(1));
                        listaEndereco.SetNumero(enderecos.GetString(2));
                        listaEndereco.SetComplemento(enderecos.GetString(3));
                        listaEndereco.SetBairro(enderecos.GetString(4));
                        listaEndereco.SetMunicipio(enderecos.GetString(5));
                        listaEndereco.SetUf(enderecos.GetString(6));
                        listaEndereco.SetCep(enderecos.GetString(7));

                        lista.Add(listaEndereco);
                    }
                    enderecos.NextResult();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lista;
        }

        public OracleDataReader ListarEnderecosPorIdMedico(int id)
        {
            this.banco.conectar();
            return this.banco.Query("select e.id_endereco, e.logradouro, e.numero, e.complemento, e.bairro, e.municipio, e.uf, e.cep from endereco e " +
                "inner join medico m on e.id_endereco = p.id_endereco " +
                "where m.id_medico = " + id + "");
        }

        public List<Endereco> GetEnderecosPorIdMedico(int id)
        {
            List<Endereco> lista = new List<Endereco>();

            this.banco.conectar();
            var enderecos = ListarEnderecosPorIdPaciente(id);

            try
            {
                while (enderecos.HasRows)
                {
                    while (enderecos.Read())
                    {
                        Endereco listaEndereco = new Endereco();

                        listaEndereco.SetId_endereco(enderecos.GetInt32(0));
                        listaEndereco.SetLogradouro(enderecos.GetString(1));
                        listaEndereco.SetNumero(enderecos.GetString(2));
                        listaEndereco.SetComplemento(enderecos.GetString(3));
                        listaEndereco.SetBairro(enderecos.GetString(4));
                        listaEndereco.SetMunicipio(enderecos.GetString(5));
                        listaEndereco.SetUf(enderecos.GetString(6));
                        listaEndereco.SetCep(enderecos.GetString(7));

                        lista.Add(listaEndereco);
                    }
                    enderecos.NextResult();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lista;
        }

        public void CorrigeNulos()
        {
            this.banco.conectar();
            this.banco.nonQuery("update endereco set logradouro = 0 where logradouro is null");
            this.banco.nonQuery("update endereco set numero = 0 where numero is null");
            this.banco.nonQuery("update endereco set complemento = 0 where complemento is null");
            this.banco.nonQuery("update endereco set bairro = 0 where bairro is null");
            this.banco.nonQuery("update endereco set municipio = 0 where municipio is null");
            this.banco.nonQuery("update endereco set UF = 0 where uf is null");
            this.banco.nonQuery("update endereco set Cep = 0 where cep is null");
            this.banco.close();
        }
    }
}

