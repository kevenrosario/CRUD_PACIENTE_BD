using System.Data.OracleClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud_Paciente_BD.Models
{
    internal class Consulta
    {
        //atributos
        public int id_consulta;
        public string descricao_consulta;
        public string dt_consulta;
        public int id_medico;
        public int id_paciente;
        public string nome_paciente;
        public string nome_medico;
        public int totais;

        public Consulta()
        {
            this.id_consulta = 0;
            this.descricao_consulta = "";
            this.dt_consulta = "";
            this.id_medico = 0;
            this.id_paciente = 0;
            this.nome_paciente = "";
            this.nome_medico = "";
            this.totais = 0;

            this.banco = new ConexaoOracle();
        }

        public ConexaoOracle banco;

        //gets e sets
        public void SetID_consulta(int novo) { this.id_consulta = novo; }
        public void SetDescricao_consulta(string novoc) { this.descricao_consulta = novoc; }
        public void SetDt_Consulta(string novo) { this.dt_consulta = novo; }
        public void SetId_medico(int novo) { this.id_medico = novo; }
        public void SetId_paciente(int novo) { this.id_paciente = novo; }
        public void SetNome_medico(string novo) { this.nome_medico = novo; }
        public void SetNome_paciente(string novo) { this.nome_paciente = novo; }    
        public void SetTotais(int novo) { this.totais = novo; }

        public int GetID_consulta() { return this.id_consulta; }
        public string GetDescricao_consulta() { return this.descricao_consulta; }
        public string GetDt_Consulta() { return this.dt_consulta;}
        public int GetId_medico() { return this.id_medico; }
        public int GetId_paciente() { return this.id_paciente; }
        public string GetNome_paciente() { return this.nome_paciente;}
        public string GetNome_medico() { return this.nome_medico;}
        public int GetTotais() { return this.totais; }

        // CRIAR METODO PARA BUSCAR CONSULTAS
        public OracleDataReader ListarConsultas()
        {
            this.banco.conectar();
            return this.banco.Query("select c.id_consulta, c.descricao_consulta, c.dt_consulta, " +
                "m.id_medico, p.id_paciente,  m.nome, p.nome from consulta c " +                
                "join medico m on c.id_medico = m.id_medico " +
                "join paciente p on c.id_paciente = p.id_paciente");
        }

        // ---ALTERAR---
        public void AlterarConsulta(int id)
        {
            this.SetID_consulta(id);
            this.banco.conectar();
            this.banco.nonQuery("UPDATE consulta set descricao_consulta='" + this.GetDescricao_consulta() +
                "' where id_consulta ='" + this.GetID_consulta() + "'");
            this.banco.close();
        }

        // ---INSERIR---
        public void CadastrarConsulta()
        {
            this.banco.conectar();
            this.banco.nonQuery("INSERT INTO consulta (dt_consulta, id_medico, id_paciente, descricao_consulta) VALUES ('" +
                this.GetDt_Consulta() + "', '" +
                this.GetId_medico() + "', '" +
                this.GetId_paciente() + "', '" +
                this.GetDescricao_consulta() + "')");
            this.banco.close();
        }

        // ---EXCLUIR POR ID CONSULTA---
        public void ExcluirConsulta(int id)
        {
            this.SetID_consulta(id);
            this.banco.conectar();
            this.banco.nonQuery("Delete from consulta where id_consulta ='" + this.GetID_consulta() + "'");
            this.banco.close();
        }

        // ---EXCLUIR POR PACIENTE---
        public void ExcluirConsultaPorPaciente(int id)
        {
            this.SetId_paciente(id);
            this.banco.conectar();
            this.banco.nonQuery("Delete from consulta where id_paciente ='" + this.GetId_paciente() + "'");
            this.banco.close();
        }

        // ---EXCLUIR POR MEDICO---
        public void ExcluirConsultaPorMedico(int id)
        {
            this.SetId_medico(id);
            this.banco.conectar();
            this.banco.nonQuery("Delete from consulta where id_medico ='" + this.GetId_medico() + "'");
            this.banco.close();
        }

        //Contagem de consultas do banco
        public int QuantidadeConsulta()
        {
            this.banco.conectar();
            int contagem = 0;
            var temp = this.banco.Query("SELECT COUNT(*) FROM CONSULTA");
            while (temp.Read())
            {
                contagem = temp.GetInt32(0);
            }
            return contagem;
        }

        //LISTAR AS CONSULTAS
        public List<Consulta> GetConsultas()
        {
            List<Consulta> lista = new List<Consulta>();
            var consultas = ListarConsultas();

            try
            {
                while (consultas.HasRows)
                {
                    while (consultas.Read())
                    {
                        Consulta listaConsulta = new Consulta();
                        listaConsulta.SetID_consulta(consultas.GetInt32(0));
                        listaConsulta.SetDescricao_consulta(consultas.GetString(1));
                        listaConsulta.SetDt_Consulta(consultas.GetString(2));
                        listaConsulta.SetId_medico(consultas.GetInt32(3));
                        listaConsulta.SetId_paciente(consultas.GetInt32(4));
                        listaConsulta.SetNome_medico(consultas.GetString(5));
                        listaConsulta.SetNome_paciente(consultas.GetString(6));

                        lista.Add(listaConsulta);
                    }
                    consultas.NextResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lista;
        }

        public OracleDataReader ListarConsultasPorMedico()
        {
            this.banco.conectar();
            return this.banco.Query("select m.id_medico, m.nome, count(c.id_consulta) from medico m " +
                "left join consulta c on m.id_medico = c.id_medico " +
                "group by m.id_medico, m.nome");
        }

        public List<Consulta> ConsultasPorMedicos()
        {
            List<Consulta> lista = new List<Consulta>();
            var consultas = ListarConsultasPorMedico();

            try
            {
                while (consultas.HasRows)
                {
                    while (consultas.Read())
                    {
                        Consulta listaConsulta = new Consulta();
                        listaConsulta.SetId_medico(consultas.GetInt32(0));
                        listaConsulta.SetNome_medico(consultas.GetString(1));
                        listaConsulta.SetTotais(consultas.GetInt32(2));

                        lista.Add(listaConsulta);
                    }
                    consultas.NextResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lista;
        }

        public int PegarIdPacienteConsulta(int id)
        {
            this.banco.conectar();
            int idExcluir = 0;
            var temp = this.banco.Query("select c.id_paciente, count(c.id_paciente) from consulta c where c.id_paciente = " + id + "");
            while (temp.Read())
            {
                idExcluir = temp.GetInt32(1);
            }
            return idExcluir;
        }

        public int PegarIdMedicoConsulta(int id)
        {
            this.banco.conectar();
            int idExcluir = 0;
            var temp = this.banco.Query("select c.id_medico, count(c.id_medico) from consulta c where c.id_medico = " + id + "");
            while (temp.Read())
            {
                idExcluir = temp.GetInt32(1);
            }
            return idExcluir;
        }

        public OracleDataReader ListarConsultasPorId(int id)
        {
            this.banco.conectar();
            return this.banco.Query("select c.id_consulta, c.id_medico, c.id_paciente, c.descricao_consulta," +
                " c.dt_consulta from consulta c where c.id_consulta = " + id + "");
        }

        public List<Consulta> GetConsultasPorId(int id)
        {
            List<Consulta> lista = new List<Consulta>();
            var consultas = ListarConsultasPorId(id);

            try
            {
                while (consultas.HasRows)
                {
                    while (consultas.Read())
                    {
                        Consulta listaConsulta = new Consulta();

                        listaConsulta.SetID_consulta(consultas.GetInt32(0));
                        listaConsulta.SetId_medico(consultas.GetInt32(1));
                        listaConsulta.SetId_paciente(consultas.GetInt32(2));
                        listaConsulta.SetDescricao_consulta(consultas.GetString(3));

                        lista.Add(listaConsulta);
                    }
                    consultas.NextResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lista;
        }

        public void CorrigeNull()
        {
            this.banco.conectar();
            this.banco.nonQuery("update consulta set descricao_consulta = 0 where descricao_consulta is null");
            this.banco.nonQuery("update consulta set dt_consulta = 0 where dt_consulta is null");
        }
    }
}
