CREATE DATABASE AspNet5Teste;

USE AspNet5Teste;

CREATE TABLE users (userID INT PRIMARY KEY,firstName VARCHAR(40), lastName VARCHAR(80), age SMALLINT, userType VARCHAR(20));

INSERT INTO users VALUES (123,'Joshep Edward', 'Simas Almeida', 61, 'Client'), (3421,'Claudete', 'Santos Simas Almeida', 48, 'Client');