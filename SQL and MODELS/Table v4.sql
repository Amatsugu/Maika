CREATE TABLE "project"."user"
  (
     "user_id"  VARCHAR(50) NOT NULL,
     "username" VARCHAR(30) NOT NULL,
     "password" VARCHAR(30) NOT NULL,
     "email"    VARCHAR(30) NOT NULL,
     "created"  TIMESTAMP(0) NOT NULL,
     PRIMARY KEY ("user_id")
  ) without oids;
  
CREATE TABLE "project"."room"
  (
     "room_id"  VARCHAR(50) NOT NULL,
     "user_id"  VARCHAR(50) NOT NULL,
     "title"    VARCHAR(50) NOT NULL,
     "password" VARCHAR(30) NOT NULL,
     "type" bool DEFAULT false NOT NULL,
     PRIMARY KEY ("room_id")
  ) without oids;
  
CREATE TABLE "project"."room_member"
  (
     "room_id"        VARCHAR(50) NOT NULL,
     "user_id"        VARCHAR(50) NOT NULL
  ) without oids;
  
CREATE TABLE "project"."invite"
  (
     "invite_id"        VARCHAR(50) NOT NULL,
     "room_id"        VARCHAR(50) NOT NULL,
	 PRIMARY KEY ("invite_id")
  ) without oids;
  
ALTER TABLE "project"."room"
  ADD CONSTRAINT "user_id_FK_user" FOREIGN KEY ("user_id") REFERENCES
  "project"."user" ("user_id") ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE "project"."room_member"
  ADD CONSTRAINT "room_id_FK-room" FOREIGN KEY ("room_id") REFERENCES
  "project"."room" ("room_id") ON DELETE CASCADE ON UPDATE CASCADE;
  
ALTER TABLE "project"."room_member"
  ADD CONSTRAINT "user_id_FK-user" FOREIGN KEY ("user_id") REFERENCES
  "project"."user" ("user_id") ON DELETE CASCADE ON UPDATE CASCADE; 
  
ALTER TABLE "project"."invite"
  ADD CONSTRAINT "room_id_FK-room" FOREIGN KEY ("room_id") REFERENCES
  "project"."room" ("room_id") ON DELETE CASCADE ON UPDATE CASCADE; 
  
-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO "project"."user" VALUES ('c492f646-d54d-11e7-9296-cec278b6b50a', 'test', 'test', 'test@test.com', '2017-11-10 00:57:09');
INSERT INTO "project"."user" VALUES ('c492fa74-d54d-11e7-9296-cec278b6b50a', 'test123', 'test123', 'test@test.com', '2017-11-10 00:57:09');

-- ----------------------------
-- Records of room
-- ----------------------------
INSERT INTO "project"."room" VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492f646-d54d-11e7-9296-cec278b6b50a', 'title', '12345', 'false');

-- ----------------------------
-- Records of room_member
-- ----------------------------
INSERT INTO "project"."room_member" VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492f646-d54d-11e7-9296-cec278b6b50a');
INSERT INTO "project"."room_member" VALUES ('e7d74670-d54d-11e7-9296-cec278b6b50a', 'c492fa74-d54d-11e7-9296-cec278b6b50a');

-- ----------------------------
-- Records of invite
-- ----------------------------
INSERT INTO "project"."invite" VALUES ('2da7ebfa-d54e-11e7-9296-cec278b6b50a', 'e7d74670-d54d-11e7-9296-cec278b6b50a');
INSERT INTO "project"."invite" VALUES ('2da7ef42-d54e-11e7-9296-cec278b6b50a', 'e7d74670-d54d-11e7-9296-cec278b6b50a');