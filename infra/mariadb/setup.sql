CREATE DATABASE AspNet5Teste;

USE AspNet5Teste;

CREATE TABLE Users (userID INT PRIMARY KEY,firstName VARCHAR(40), lastName VARCHAR(80), age SMALLINT, userType VARCHAR(20));

INSERT INTO Users VALUES 
    (1,'Charlene', 'Bass', 32, 'Employee'),
    (2,'Clovis', 'Mineirin', 18, 'Client'),
    (3,'Colleen', 'Watts', 22, 'Employee'),
    (4,'Phillip', 'Elliott', 38, 'Visitor'),
    (5,'Terry', 'Jennings', 34, 'Client'),
    (6,'Aubrey', 'Gibbs', 78, 'Client'),
    (7,'Kirk', 'Ross', 26, 'Visitor'),
    (8,'Jerald', 'Phelps', 31, 'Visitor'),
    (9,'Felipe', 'Swanson', 20, 'Partner'),
    (10,'Melody', 'Allen', 22, 'Visitor'),
    (11,'Sylvester', 'Warner', 43, 'Employee'),
    (12,'Pamela', 'Jenkins', 24, 'Visitor'),
    (13,'Leonard', 'Rose', 36, 'Visitor'),
    (14,'Otis', 'Francis', 28, 'Visitor'),
    (15,'Constance', 'Nash', 29, 'Client'),
    (16,'Deanna', 'Stokes', 53, 'Visitor'),
    (17,'Yvonne', 'Marsh', 51, 'Visitor'),
    (18,'Bernadette', 'Quinn', 40, 'Client'),
    (19,'Muriel', 'Wheeler', 39, 'Client'),
    (20,'Shaun', 'Bryan', 19, 'Client'),
    (21,'Christopher', 'Steele', 11, 'Client'),
    (22,'Darnell', 'Garner', 44, 'Employee'),
    (23,'Rufus', 'Strickland', 33, 'Visitor'),
    (24,'Kari', 'Perkins', 42, 'Employee'),
    (25,'Gwendolyn', 'Morrison', 56, 'Visitor'),
    (26,'Carole', 'Terry', 59, 'Visitor');