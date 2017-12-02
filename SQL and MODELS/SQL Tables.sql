CREATE TABLE "user"
  (
     "user_id"  VARCHAR(50) NOT NULL,
     "username" VARCHAR(30) NOT NULL,
     "password" VARCHAR(30) NOT NULL,
     "email"    VARCHAR(30) NOT NULL,
     PRIMARY KEY ("user_id")
  ) without oids;
  
CREATE TABLE "room"
  (
     "room_id"  VARCHAR(50) NOT NULL,
     "owner_id"  VARCHAR(50) NOT NULL,
     "title"    VARCHAR(50) NOT NULL,
     "is_public" bool DEFAULT false NOT NULL,
     PRIMARY KEY ("room_id")
  ) without oids;
  
CREATE TABLE "room_member"
  (
     "room_id"        VARCHAR(50) NOT NULL,
     "user_id"        VARCHAR(50) NOT NULL
  ) without oids;
  
CREATE TABLE "invite"
  (
     "invite_id"        VARCHAR(50) NOT NULL,
     "room_id"        VARCHAR(50) NOT NULL,
	 PRIMARY KEY ("invite_id")
  ) without oids;
  
ALTER TABLE "room"
  ADD CONSTRAINT "user_id_FK_user" FOREIGN KEY ("owner_id") REFERENCES
  "user" ("user_id") ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE "room_member"
  ADD CONSTRAINT "room_id_FK-room" FOREIGN KEY ("room_id") REFERENCES
  "room" ("room_id") ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE "room_member"
  ADD CONSTRAINT "user_id_FK-user" FOREIGN KEY ("user_id") REFERENCES
  "user" ("user_id") ON DELETE CASCADE ON UPDATE CASCADE; 
  
ALTER TABLE "invite"
  ADD CONSTRAINT "room_id_FK-room" FOREIGN KEY ("room_id") REFERENCES
  "room" ("room_id") ON DELETE CASCADE ON UPDATE CASCADE; 
  
-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO "user" VALUES ('c492f646-d54d-11e7-9296-cec278b6b50a', 'test', 'test', 'test@test.com');
INSERT INTO "user" VALUES ('c492fa74-d54d-11e7-9296-cec278b6b50a', 'test123', 'test123', 'test@test.com');

-- ----------------------------
-- Records of room
-- ----------------------------
INSERT INTO "room" VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492f646-d54d-11e7-9296-cec278b6b50a', 'title', 'false');

-- ----------------------------
-- Records of room_member
-- ----------------------------
INSERT INTO "room_member" VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492f646-d54d-11e7-9296-cec278b6b50a');
INSERT INTO "room_member" VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492fa74-d54d-11e7-9296-cec278b6b50a');

-- ----------------------------
-- Records of invite
-- ----------------------------
INSERT INTO "invite" VALUES ('2da7ebfa-d54e-11e7-9296-cec278b6b50a', 'e7d74670-d54d-11e7-9296-cec278b6b50a');
INSERT INTO "invite" VALUES ('2da7ef42-d54e-11e7-9296-cec278b6b50a', 'e7d74670-d54d-11e7-9296-cec278b6b50a');