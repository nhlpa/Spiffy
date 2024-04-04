DROP TABLE IF EXISTS test_values;

CREATE TABLE test_values (
  description TEXT NOT NULL  PRIMARY KEY);

INSERT INTO test_values (description)
VALUES ('spiffy');

DROP TABLE IF EXISTS file;

CREATE TABLE file (
  file_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
, data BLOB NOT NULL);