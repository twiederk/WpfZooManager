IF OBJECT_ID('dbo.zoo_animal', 'U') IS NOT NULL 
    DROP TABLE dbo.zoo_animal;

IF OBJECT_ID('dbo.zoo', 'U') IS NOT NULL 
    DROP TABLE dbo.zoo;

IF OBJECT_ID('dbo.animal', 'U') IS NOT NULL 
    DROP TABLE dbo.animal;

-- Create zoo table
CREATE TABLE dbo.zoo (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(100) NOT NULL
);

-- Create animal table
CREATE TABLE dbo.animal (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(100) NOT NULL
);

-- Create zoo_animal table with foreign keys and ON DELETE CASCADE
CREATE TABLE dbo.zoo_animal (
    zoo_id INT,
    animal_id INT,
    PRIMARY KEY (zoo_id, animal_id),
    FOREIGN KEY (zoo_id) REFERENCES dbo.zoo(id) ON DELETE CASCADE,
    FOREIGN KEY (animal_id) REFERENCES dbo.animal(id) ON DELETE CASCADE
);

-- Insert data into zoo table
INSERT INTO dbo.zoo (name) VALUES ('New York');
INSERT INTO dbo.zoo (name) VALUES ('Tokyo');
INSERT INTO dbo.zoo (name) VALUES ('Berlin');
INSERT INTO dbo.zoo (name) VALUES ('Kairo');
INSERT INTO dbo.zoo (name) VALUES ('Milan');
-- SELECT * FROM dbo.zoo;

-- Insert data into animal table
INSERT INTO dbo.animal (name) VALUES ('Shark');
INSERT INTO dbo.animal (name) VALUES ('Clownfish');
INSERT INTO dbo.animal (name) VALUES ('Monkey');
INSERT INTO dbo.animal (name) VALUES ('Wolf');
INSERT INTO dbo.animal (name) VALUES ('Gecko');
INSERT INTO dbo.animal (name) VALUES ('Crocodile');
INSERT INTO dbo.animal (name) VALUES ('Owl');
INSERT INTO dbo.animal (name) VALUES ('Parrot');
-- SELECT * FROM dbo.animal;

-- Insert data into zoo_animal table
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 1, 1 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 1, 2 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 2, 3 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 2, 4 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 3, 5 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 3, 6 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 4, 7 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 4, 8 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 1 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 2 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 3 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 4 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 5 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 6 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 7 );
INSERT INTO dbo.zoo_animal (zoo_id, animal_id) VALUES ( 5, 8 );
-- SELECT * FROM dbo.zoo_animal;

