create database hcasignaturasMDP;
use hcasignaturasMDP;

create table asignaturas (
	codigo varchar(20) primary key,
	nombre varchar(100),
	creditos int
);

create table decretos(
	numero int primary key,
	fecha date
);

create table decretos_asignaturas(
	id int primary key AUTO_INCREMENT,
	numero_decreto int,
	codigo_asignatura varchar(20),
	foreign key(numero_decreto) references decretos(numero) on delete cascade on update cascade,
	foreign key(codigo_asignatura) references asignaturas(codigo) ON DELETE CASCADE ON UPDATE CASCADE
);

create table programas(
	codigo varchar(20) primary key,
	nombre varchar(100),
	numero_decreto int,
	foreign key(numero_decreto) references decretos(numero)
	on delete cascade on update cascade
);

create table programaExterno(
	codigo varchar(20) primary key,
	nombre varchar(100),
	universidad varchar(50)
);

create table programaExterno_asignaturas(
	id int primary key AUTO_INCREMENT,
	codigo_programaExterno varchar(20),
	codigo_asignatura varchar(20),
	foreign key(codigo_programaExterno) references programaExterno(codigo) on delete cascade on update cascade,
	foreign key(codigo_asignatura) references asignaturas(codigo) ON DELETE CASCADE ON UPDATE CASCADE
);

create table equivalente(
	programaOrigen varchar(20),
	programaObjetivo varchar(20),
	codigoAsignaturaOrigen varchar(20),
	codigoAsignaturaObjetivo varchar(20),
	primary key(programaOrigen,programaObjetivo,codigoAsignaturaOrigen,codigoAsignaturaObjetivo),
	foreign key(programaOrigen) references programas(codigo),
	foreign key(programaObjetivo) references programas(codigo),
	foreign key(codigoAsignaturaOrigen) references asignaturas(codigo),
	foreign key(codigoAsignaturaObjetivo) references asignaturas(codigo)
);

create table convalidacion(
	run varchar(30),
	nombre varchar(20),
	apellido varchar(50),
	programaOrigen varchar(20),
	programaObjetivo varchar(20),
	emailSecretario varchar(50),
	foreign key(emailSecretario) references usuarios(email) on update cascade,
	foreign key(programaOrigen) references programas(codigo),
	foreign key(programaObjetivo) references programas(codigo),
	primary key(run, programaOrigen, programaObjetivo)
);

create table convalidacion_equivalente(
	run varchar(30),
	programaOrigen varchar(20),
	programaObjetivo varchar(20),
	foreign key(run, programaOrigen, programaObjetivo) references convalidacion(run, programaOrigen, programaObjetivo) on delete cascade,
	codigoAsignaturaOrigen varchar(20),
	codigoAsignaturaObjetivo varchar(20),
	nota float,
	foreign key(programaOrigen,programaObjetivo,codigoAsignaturaOrigen,codigoAsignaturaObjetivo) references equivalente(programaOrigen,programaObjetivo,codigoAsignaturaOrigen,codigoAsignaturaObjetivo)
);

create table homologacion(
	run varchar(30),
	nombre varchar(20),
	apellido varchar(50),
	programaOrigen varchar(20),
	programaObjetivo varchar(20),
	emailSecretario varchar(50),
    	foreign key(emailSecretario) references usuarios(email) on update cascade,
	foreign key(programaOrigen) references programaexterno(codigo),
	foreign key(programaObjetivo) references programas(codigo),
	primary key(run, programaOrigen, programaObjetivo)
);

create table equivalenteHomologacion(
	programaOrigen varchar(20),
	programaObjetivo varchar(20),
	codigoAsignaturaOrigen varchar(20),
	codigoAsignaturaObjetivo varchar(20),
	primary key(programaOrigen,programaObjetivo,codigoAsignaturaOrigen,codigoAsignaturaObjetivo),
	foreign key(programaOrigen) references programaexterno(codigo),
	foreign key(programaObjetivo) references programas(codigo),
	foreign key(codigoAsignaturaOrigen) references asignaturas(codigo),
	foreign key(codigoAsignaturaObjetivo) references asignaturas(codigo)
);

create table homologacion_equivalenteHomologacion(
	run varchar(30),
	programaOrigen varchar(20),
	programaObjetivo varchar(20),
	foreign key(run, programaOrigen, programaObjetivo) references homologacion(run, programaOrigen, programaObjetivo) on delete cascade,
	codigoAsignaturaOrigen varchar(20),
	codigoAsignaturaObjetivo varchar(20),
	foreign key(programaOrigen,programaObjetivo,codigoAsignaturaOrigen,codigoAsignaturaObjetivo) references equivalenteHomologacion(programaOrigen,programaObjetivo,codigoAsignaturaOrigen,codigoAsignaturaObjetivo),
	nota float
);

create table programasAcademicos(
	path varchar(150),
	codigo varchar(20) primary key,
	foreign key (codigo) references asignaturas(codigo) on update cascade on delete cascade
);

create table usuarios (
	nombre varchar(20),
	apellido varchar(50),
	run varchar(20),
	email varchar(50) primary key,
	area varchar(50),
	password varchar(100),
	tipoUsuario varchar(30)
);

create table usuariosTemp(
	token varchar(50),
	expira datetime,
	email varchar(50),
	primary key(email, token),
	foreign key (email) references usuarios(email) on delete cascade on update cascade
);

create table destinatarios(
	nombre varchar(20),
	apellido varchar(50),
	email varchar(50),
	tipoUsuario varchar(30),
	area varchar(50),
	primary key(email,nombre,apellido)
);

INSERT INTO usuarios(`nombre`, `apellido`,`run`,`email`,`area`,`password`,`tipoUsuario`) VALUES ( 'Admin', 'Default', '00.000.000-0','hcasignaturas.unab@gmail.com','','ed743e37d5c7be42fd6902e1664feefb','Administrador');