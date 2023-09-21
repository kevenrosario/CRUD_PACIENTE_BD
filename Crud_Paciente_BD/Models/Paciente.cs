//using MySql.Data.MySqlClient;
using System.Data.OracleClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace Crud_Paciente_BD.Models
{
    internal class Paciente : Endereco
    {
        //atributos
        public int id_paciente;
        public string nome;
        public string dt_nasc;
        public string sexo;
        public string cpf;
        public string celular;
        public string email;
        public int id_endereco;

        public ConexaoOracle banco;
        Endereco endereco = new Endereco();

        public Paciente()
        {
            this.id_paciente = 0;
            this.nome = "";
            this.dt_nasc = "";
            this.sexo = "";
            this.cpf = "";
            this.celular = "";
            this.email = "";
            this.id_endereco = 0;       

            this.banco = new ConexaoOracle();
        }
        public void SetId_paciente(int novo) { this.id_paciente = novo; }
        public void SetNome(string novo) { this.nome = novo; }
        public void SetDt_nasc(string novo) { this.dt_nasc = novo; }
        public void SetSexo(string novo) { this.sexo = novo; }
        public void SetCpf(string novo) { this.cpf = novo; }
        public void SetCelular(string novo) { this.celular = novo; }
        public void SetEmail(string novo) { this.email = novo; }
        public void SetId_endereco(int novo) { this.id_endereco = novo; }

        public int GetId_paciente() { return this.id_paciente; }
        public string GetNome() { return this.nome; }
        public string GetDt_nasc() { return this.dt_nasc; }
        public string GetSexo() { return this.sexo; }
        public string GetCpf() { return this.cpf; }
        public string GetCelular() { return this.celular; }
        public string GetEmail() { return this.email; }
        public int GetId_endereco() { return this.id_endereco; }


        // CRIAR METODO PARA BUSCAR PACIENTES PARA O GRID
        public OracleDataReader ListarPaciente()
        {
            this.banco.conectar();
            return this.banco.Query("select p.id_paciente, p.Nome, p.dt_nasc,p.sexo,p.CPF, p.celular, p.email," +
                " e.id_endereco, e.logradouro, e.numero,e.complemento, e.bairro, e.municipio, e.uf, e.cep from paciente p " +
                "join endereco e on p.id_endereco = e.id_endereco");
        }
        // CRIAR METODO PARA BUSCAR PACIENTES PELO BOTÃO OK
        public OracleDataReader ListarPacientePorOk(string filtro)
        {
            this.banco.conectar();
            return this.banco.Query("select p.id_paciente, p.Nome,p.dt_nasc,p.sexo, p.CPF, p.celular, p.email, " +
                "e.id_endereco, e.logradouro, e.numero,e.complemento, e.bairro, e.municipio, e.uf, e.cep from paciente p " +
                "join endereco e on p.id_endereco = e.id_endereco where p.Nome like '%" + filtro + "%' ");
        }
        // ---INSERIR---
        public void CadastrarPaciente()
        {
            this.banco.conectar();
            this.banco.nonQuery("INSERT INTO paciente (Nome, dt_nasc,sexo," +
                "CPF, celular,email, id_endereco) VALUES ('" +
                this.GetNome() + "', '" +
                this.GetDt_nasc() + "', '" +
                this.GetSexo() + "', '" +
                this.GetCpf() + "', '" +
                this.GetCelular() + "', '" +
                this.GetEmail() + "', '" +
                this.GetId_endereco() + "')");
            this.banco.close();
        }
        // ---ALTERAR---
        public void AlterarPaciente(int id)
        {
            this.SetId_paciente(id);
            this.banco.conectar();
            this.banco.nonQuery("UPDATE paciente set nome='" + this.GetNome() +
                "', dt_nasc='" + this.GetDt_nasc() +
                "', sexo='" + this.GetSexo() +
                "', cpf='" + this.GetCpf() +
                "', celular='" + this.GetCelular() +
                "', email='" + this.GetEmail() +
                "' where id_paciente ='" + this.GetId_paciente() + "'");
            this.banco.close();
        }
        // ---EXCLUIR---
        public void ExcluirPaciente(int id)
        {
            this.SetId_paciente(id);
            this.banco.conectar();
            this.banco.nonQuery("Delete from paciente where id_paciente ='" + this.GetId_paciente() + "'");
            this.banco.close();
        }

        public int QuantidadePacientes()
        {
            this.banco.conectar();
            int contagem = 0;
            var temp = this.banco.Query("SELECT COUNT(*) FROM PACIENTE");
            while (temp.Read())
            {
                contagem = temp.GetInt32(0);
            }
            temp.Close();
            return contagem;
        }

        public List<Paciente> GetPacientes()
        {
            List<Paciente> lista = new List<Paciente>();           
            
            this.banco.conectar();
            var pacientes = ListarPaciente();

            try
            {
                while (pacientes.HasRows)
                {                    
                    while (pacientes.Read())
                    {
                        Paciente listaPaciente = new Paciente();
                        
                        listaPaciente.SetId_paciente(pacientes.GetInt32(0));
                        listaPaciente.SetNome(pacientes.GetString(1));
                        listaPaciente.SetDt_nasc(pacientes.GetString(2));
                        listaPaciente.SetSexo(pacientes.GetString(3));
                        listaPaciente.SetCpf(pacientes.GetString(4));
                        listaPaciente.SetCelular(pacientes.GetString(5));
                        listaPaciente.SetEmail(pacientes.GetString(6));
                        listaPaciente.SetId_endereco(pacientes.GetInt32(7));
                        listaPaciente.SetLogradouro(pacientes.GetString(8));
                        listaPaciente.SetNumero(pacientes.GetString(9));
                        listaPaciente.SetComplemento(pacientes.GetString(10));
                        listaPaciente.SetBairro(pacientes.GetString(11));
                        listaPaciente.SetMunicipio(pacientes.GetString(12));
                        listaPaciente.SetUf(pacientes.GetString(13));
                        listaPaciente.SetCep(pacientes.GetString(14));

                        lista.Add(listaPaciente);  
                    }
                    pacientes.NextResult();                    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return lista;
        }

        public int PegarIdEnderecoPaciente(int id)
        {
            this.banco.conectar();
            int idExcluir = 0;
            var temp = this.banco.Query("select p.id_endereco from paciente p where p.id_paciente = " + id +"");
            while (temp.Read())
            {
                idExcluir = temp.GetInt32(0);
            }
            return idExcluir;
        }

        public OracleDataReader ListarPacientePorId(int id)
        {
            this.banco.conectar();
            return this.banco.Query("select p.id_paciente, p.Nome, p.dt_nasc,p.sexo,p.CPF, p.celular, p.email," +
                " e.id_endereco, e.logradouro, e.numero,e.complemento, e.bairro, e.municipio, e.uf, e.cep from paciente p " +
                "join endereco e on p.id_endereco = e.id_endereco " +
                "where p.id_paciente = " + id + "");
        }

        public List<Paciente> GetPacientesPorId(int id)
        {
            List<Paciente> lista = new List<Paciente>();

            this.banco.conectar();
            var pacientes = ListarPacientePorId(id);

            try
            {
                while (pacientes.HasRows)
                {
                    while (pacientes.Read())
                    {
                        Paciente listaPaciente = new Paciente();

                        listaPaciente.SetId_paciente(pacientes.GetInt32(0));
                        listaPaciente.SetNome(pacientes.GetString(1));
                        listaPaciente.SetDt_nasc(pacientes.GetString(2));
                        listaPaciente.SetSexo(pacientes.GetString(3));
                        listaPaciente.SetCpf(pacientes.GetString(4));
                        listaPaciente.SetCelular(pacientes.GetString(5));
                        listaPaciente.SetEmail(pacientes.GetString(6));
                        listaPaciente.SetId_endereco(pacientes.GetInt32(7));
                        listaPaciente.SetLogradouro(pacientes.GetString(8));
                        listaPaciente.SetNumero(pacientes.GetString(9));
                        listaPaciente.SetComplemento(pacientes.GetString(10));
                        listaPaciente.SetBairro(pacientes.GetString(11));
                        listaPaciente.SetMunicipio(pacientes.GetString(12));
                        listaPaciente.SetUf(pacientes.GetString(13));
                        listaPaciente.SetCep(pacientes.GetString(14));

                        lista.Add(listaPaciente);
                    }
                    pacientes.NextResult();
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
            this.banco.nonQuery("update paciente set nome = 0 where nome is null");
            this.banco.nonQuery("update paciente set dt_nasc = 0 where dt_nasc is null");
            this.banco.nonQuery("update paciente set sexo = 0 where sexo is null");
            this.banco.nonQuery("update paciente set cpf = 0 where cpf is null");
            this.banco.nonQuery("update paciente set celular = 0 where celular is null");
            this.banco.nonQuery("update paciente set email = 0 where email is null");
            this.banco.nonQuery("Commit");
            this.banco.close();
        }
    }
}
