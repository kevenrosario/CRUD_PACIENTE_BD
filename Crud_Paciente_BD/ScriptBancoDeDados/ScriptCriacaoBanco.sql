
-- Formando estrutura do banco de dados para s
DROP DATABASE IF EXISTS s;
CREATE DATABASE IF NOT EXISTS s; 
USE s;

-- Formando estrutura para tabela s.endereco
DROP TABLE IF EXISTS endereco;
CREATE TABLE IF NOT EXISTS endereco (
  id_endereco int NOT NULL AUTO_INCREMENT,
  logradouro varchar(45) DEFAULT NULL,
  numero int DEFAULT NULL,
  complemento varchar(45) DEFAULT NULL,
  bairro varchar(45) DEFAULT NULL,
  municipio varchar(45) DEFAULT NULL,
  uf varchar(2) DEFAULT NULL,
  cep varchar(10) DEFAULT NULL,
  PRIMARY KEY (id_endereco)
);

-- Formando dados para a tabela s.endereco:
INSERT INTO endereco (id_endereco, logradouro, numero, complemento, bairro, municipio, uf, cep) VALUES
	(1, 'Brodway', 55, '', 'Campo Grande', 'Cariacica', 'ES', '29155-845'),
	(2, 'Avenida Obed', 189, '', 'Campo Verde', 'Cariacica', 'ES', '29155-845'),
	(3, 'Jose Sete', 300, '', 'Cariacica Sede', 'Cariacica', '', '29135-896'),
	(4, 'Uma rua', 23, 'Casa', 'Campo Verde', 'Cariacica', 'ES', '29111111'),
	(5, 'Uma rua', 23, 'Casa', 'Campo Verde', 'Cariacica', 'ES', '29111233'),
	(6, 'Rua dos medicos', 100, 'Predio', 'Centro', 'Vitoria', 'ES', '29546876'),
	(7, 'Rua dos medicos', 100, 'Predio', 'Centro', 'Vitoria', 'ES', '29100100'),
	(8, 'Rua dos medicos', 100, 'Predio', 'Centro', 'Vitoria', 'ES', '29132596'),
	(9, 'Avenida Brasil', 55, 'Triplex', 'Sitio', 'Atibaia', 'SP', '63569897'),
	(10, 'Avenida Brasil', 13, 'Triplex', 'Sitio', 'Atibaia', 'SP', '63963659');


-- Formando estrutura para tabela s.medico
DROP TABLE IF EXISTS medico;
CREATE TABLE IF NOT EXISTS medico (
  id_medico int NOT NULL AUTO_INCREMENT,
  nome varchar(45) NOT NULL,
  crm varchar(20) NOT NULL,
  celular varchar(17) NOT NULL,
  id_endereco int NOT NULL,
  PRIMARY KEY (id_medico),
  KEY id_endereco (id_endereco),
  CONSTRAINT medico_ibfk FOREIGN KEY (id_endereco) REFERENCES endereco (id_endereco) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Formando dados para a tabela s.medico
INSERT INTO medico (id_medico, nome, crm, celular, id_endereco) VALUES
	(1, 'Luiz Inacio Squid Da Silva', 'PT13', '(27) 96599-9636', 10);


-- Formando estrutura para tabela s.paciente
DROP TABLE IF EXISTS paciente;
CREATE TABLE IF NOT EXISTS paciente (
  id_paciente int NOT NULL AUTO_INCREMENT,
  nome varchar(45) NOT NULL,
  dt_nasc varchar(10) NOT NULL,
  sexo varchar(1) DEFAULT NULL,
  cpf varchar(17) NOT NULL,
  celular varchar(17) DEFAULT NULL,
  email varchar(45) DEFAULT NULL,
  id_endereco int NOT NULL,
  PRIMARY KEY (id_paciente),
  KEY id_endereco (id_endereco),
  CONSTRAINT paciente_ibfk FOREIGN KEY (id_endereco) REFERENCES endereco (id_endereco) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Formando dados para a tabela s.paciente:
INSERT INTO paciente (id_paciente, nome, dt_nasc, sexo, cpf, celular, email, id_endereco) VALUES
	(1, 'John Travolta', '01/01/1950', 'M', '123,456,789-10', '(27) 99988-7766', 'travolta@hollywood.com', 1),
	(2, 'Anjelina Jolie', '01/03/1970', 'F', '123,445,678-91', '(27) 99889-9889', 'jolie@hollywood.com', 2),
	(3, 'Vannila Ice', '20/09/1962', 'M', '123,654,987-55', '(27) 96335-6978', 'ice.vannila@music.com', 3),
	(5, 'Aristoteles Bidden', '01/01/1400', 'M', '123,456,987-65', '(27) 999999999', 'pensador@antigasso.com', 5);
    
-- Formando estrutura para tabela s.consulta
DROP TABLE IF EXISTS consulta;
CREATE TABLE IF NOT EXISTS consulta (
  id_consulta int NOT NULL AUTO_INCREMENT,
  id_medico int NOT NULL,
  id_paciente int NOT NULL,
  descricao_consulta varchar(200) NOT NULL,
  dt_consulta varchar(10) NOT NULL,
  PRIMARY KEY (id_consulta),
  KEY medico_fk (id_medico),
  KEY paciente_fk (id_paciente),
  CONSTRAINT medico_fk FOREIGN KEY (id_medico) REFERENCES medico (id_medico),
  CONSTRAINT paciente_fk FOREIGN KEY (id_paciente) REFERENCES paciente (id_paciente)
);

-- Formando dados para a tabela s.consulta:
INSERT INTO consulta (id_consulta, id_medico, id_paciente, descricao_consulta, dt_consulta) VALUES
	(1, 1, 2, 'Dores de cabeça constantes', '20/10/2022');
