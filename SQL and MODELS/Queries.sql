-- Retrieve all, if username and password match from user table --
SELECT * FROM project.user WHERE username = 'test' AND password = 'test';

-- Retrieve fname, mname, lname, email, age if username and password match from user table --
SELECT (email) FROM project.user WHERE username = 'test' AND password = 'test';

-- Retrieve user.user_id, room.room_id, room.title, room.password, room.type --
SELECT u.user_id, r.room_id, r.title, r.password, r.type
FROM project.user u
LEFT JOIN project.room r ON u.user_id = r.user_id

-- Retrieve all room and which user is in what room --
SELECT r.room_id, u.user_id
FROM project.user u
LEFT JOIN project.room r ON u.user_id = r.user_id





-- Update Room title --
UPDATE project.room SET title = 'test title' WHERE room_id = 'e7d74670-d54d-11e7-9296-cec278b6b50a';

-- Update Room password --
UPDATE project.room SET password = '1234567' WHERE room_id = 'e7d74670-d54d-11e7-9296-cec278b6b50a';

-- Update Room type --
UPDATE project.room SET type = 'false' WHERE room_id = 'e7d74670-d54d-11e7-9296-cec278b6b50a';

-- Update Room number, password, and type --
UPDATE project.room SET password = '12345', type = 'false' WHERE room_id = 'e7d74670-d54d-11e7-9296-cec278b6b50a';





-- Insert User --
INSERT INTO project.user (username, password, email, created) VALUES ('test1', 'test1', 'test@test.com', current_timestamp);

-- Insert Room --
INSERT INTO project.room (user_id, title, password, type) VALUES ('c492f646-d54d-11e7-9296-cec278b6b50a', 'test title', '123', 'false');

-- Insert Room_Member --
INSERT INTO project.room_member (room_id, user_id) VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492f646-d54d-11e7-9296-cec278b6b50a');




-----
--- Update room owner --
UPDATE project.room SET user_id = '1';

--- Number of members of a room --
SELECT COUNT(room_id) AS Num_of_Members FROM project.room_member r WHERE room_id = 'e7d74670-d54d-11e7-9296-cec278b6b50a';

--- List of users in what room and output their username and email --
SELECT DISTINCT u.username, u.email
FROM project.room_member r
LEFT JOIN project.user u ON u.user_id = r.user_id
WHERE r.room_id = '1';

--- Display username by email --
SELECT username FROM project.user WHERE email = 'test@test1.com';

--- Update username based on email --
UPDATE project.user SET username = 'test12345' WHERE email = 'test@test1.com';

--- Display password by email --
SELECT password FROM project.user WHERE email = 'test@test1.com';

--- Display password by username --
SELECT password FROM project.user WHERE username = 'test12345';

--- Check if user exist via email --
SELECT username FROM project.user WHERE email = 'test@test1.com';

--- Check if username is taken --
SELECT username FROM project.user WHERE username = 'test12345';

--- Check if account exist with email --
SELECT * FROM project.user WHERE username = 'test12345' AND email = 'test@test1.com';

--- Create invite link (invite_id, room_id) --
INSERT INTO project.invite VALUES ('2da7ebfa-d54e-11e7-9296-cec278b6b50a', 'e7d74670-d54d-11e7-9296-cec278b6b50a');

--- Delete invite link --
DELETE FROM project.invite WHERE invite_id = '2da7ebfa-d54e-11e7-9296-cec278b6b50a';

--- Getting room info from invite link --
SELECT DISTINCT r.title, r.password, r.type
FROM project.room r
LEFT JOIN project.invite i ON r.room_id = i.room_id;