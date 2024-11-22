DROP TABLE zoo_animal;
DROP TABLE animal;
DROP TABLE zoo;


CREATE TABLE zoo
(id            INTEGER,
 name      VARCHAR(15),
 PRIMARY KEY (id)
);


CREATE TABLE animal
(id            INTEGER,
 name          VARCHAR(15),
 PRIMARY KEY (id)
);


CREATE TABLE zoo_animal
(id             INTEGER,
 zoo_id 		INTEGER,
 animal_id      INTEGER NOT NULL,
 PRIMARY KEY(id)
 FOREIGN KEY (zoo_id) REFERENCES zoo(id) ON DELETE CASCADE
 FOREIGN KEY (animal_id) REFERENCES animal(id) ON DELETE CASCADE
);


INSERT INTO zoo VALUES ( 1, 'New York' );
INSERT INTO zoo VALUES ( 2, 'Tokyo' );
INSERT INTO zoo VALUES ( 3, 'Berlin' );
INSERT INTO zoo VALUES ( 4, 'Kairo' );
INSERT INTO zoo VALUES ( 5, 'Milan' );


INSERT INTO animal VALUES ( 1, 'Shark' );
INSERT INTO animal VALUES ( 2, 'Clownfish' );
INSERT INTO animal VALUES ( 3, 'Monkey' );
INSERT INTO animal VALUES ( 4, 'Wolf' );
INSERT INTO animal VALUES ( 5, 'Gecko' );
INSERT INTO animal VALUES ( 6, 'Crocodile' );
INSERT INTO animal VALUES ( 7, 'Owl' );
INSERT INTO animal VALUES ( 8, 'Parrot' );


INSERT INTO zoo_animal VALUES ( 1, 1, 1 );
INSERT INTO zoo_animal VALUES ( 2, 1, 2 );
INSERT INTO zoo_animal VALUES ( 3, 2, 3 );
INSERT INTO zoo_animal VALUES ( 4, 2, 4 );
INSERT INTO zoo_animal VALUES ( 5, 3, 5 );
INSERT INTO zoo_animal VALUES ( 6, 3, 6 );
INSERT INTO zoo_animal VALUES ( 7, 4, 7 );
INSERT INTO zoo_animal VALUES ( 8, 4, 8 );
INSERT INTO zoo_animal VALUES ( 9, 5, 1 );
INSERT INTO zoo_animal VALUES ( 10, 5, 2 );
INSERT INTO zoo_animal VALUES ( 11, 5, 3 );
INSERT INTO zoo_animal VALUES ( 12, 5, 4 );
INSERT INTO zoo_animal VALUES ( 13, 5, 5 );
INSERT INTO zoo_animal VALUES ( 14, 5, 6 );
INSERT INTO zoo_animal VALUES ( 15, 5, 7 );
INSERT INTO zoo_animal VALUES ( 16, 5, 8 );
