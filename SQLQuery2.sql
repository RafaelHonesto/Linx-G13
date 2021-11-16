CREATE DATABASE GESTAOFINANCAS;

USE GESTAOFINANCAS;

CREATE TABLE TipoUsuario (
	IdTipoUsuario			INT PRIMARY KEY IDENTITY,
	Titulo					VARCHAR(20)
);

CREATE TABLE Usuario (
	IdUsuario		INT PRIMARY KEY IDENTITY,
	Acesso			VARCHAR(20),
	SenhaDeAcesso	VARCHAR(50)
);

ALTER TABLE Usuario
ADD IdTipoUsuario INT FOREIGN KEY REFERENCES TipoUsuario (IdTipoUsuario)

CREATE TABLE Setor (
	IdSetor		INT PRIMARY KEY IDENTITY,
	Nome		VARCHAR (50),
);

CREATE TABLE Funcionario (
	IdFuncionario		INT PRIMARY KEY IDENTITY,
	IdSetor				INT FOREIGN KEY REFERENCES Setor (IdSetor),
	IdUsuario			INT UNIQUE FOREIGN KEY REFERENCES Usuario (IdUsuario),
	Nome				VARCHAR (100),
	CPF					VARCHAR(11),
	Foto				VARCHAR(150) DEFAULT ('user.png'),
	Funcao				VARCHAR (50)
);

CREATE TABLE TipoDespesa (
	IdTipoDespesa		INT PRIMARY KEY IDENTITY,
	IdSetor				INT FOREIGN KEY REFERENCES Setor (IdSetor),
	Titulo				VARCHAR (20)
);


CREATE TABLE Despesa (
	IdTipoDespesa		INT FOREIGN KEY REFERENCES TipoDespesa (IdTipoDespesa),
	IdDespesa			INT PRIMARY KEY IDENTITY,
	IdSetor				INT FOREIGN KEY REFERENCES Setor (IdSetor),
	DataDespesa			DATE,
	Nome				VARCHAR (80),
	Descricao			VARCHAR (200)
);

ALTER TABLE Despesa
ADD Valor VARCHAR (20)

CREATE TABLE Empresa (
	IdEmpresa		INT PRIMARY KEY IDENTITY,
	IdSetor			INT FOREIGN KEY REFERENCES Setor (IdSetor),
	CNPJ			VARCHAR(16),
	NomeEmpresa		VARCHAR(30)
);
CREATE TABLE Valores (
	TipoEntrada			BIT,
	IdValor				INT PRIMARY KEY IDENTITY,
	IdSetor				INT FOREIGN KEY REFERENCES Setor (IdSetor),
	Titulo				VARCHAR(20),
	Valor				VARCHAR (20),
	DataValor			DATE,
	Foto				VARCHAR(150) DEFAULT ('default.png'),
	IdEmpresa			INT FOREIGN KEY	REFERENCES Empresa (IdEmpresa),
	Descricao			VARCHAR (200)
)


CREATE TABLE Perdas (
	IdValor			INT FOREIGN KEY REFERENCES Valores (IdValor)
)

INSERT INTO TipoUsuario (Titulo) 
VALUES  ('ADM'),
		('GESTOR'),
		('FUNCIONARIO')

INSERT INTO Usuario (Acesso, SenhaDeAcesso, IdTipoUsuario)
VALUES		('adm@123', 'adm123', 1)

DELETE FROM Usuario WHERE IdUsuario=1

INSERT INTO Empresa (IdSetor, CNPJ, NomeEmpresa)
VALUES		(1, '55345123000157', 'InfoTec')