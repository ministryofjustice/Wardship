CREATE SCHEMA IF NOT EXISTS "dbo"
;

CREATE TABLE "dbo"."ADGroups"("ADGroupID" serial4 NOT NULL,"Name" varchar(80) NOT NULL,"RoleStrength" int4 NOT NULL,CONSTRAINT "PK_dbo.ADGroups" PRIMARY KEY ("ADGroupID"))
;

CREATE INDEX "ADGroups_IX_RoleStrength" ON "dbo"."ADGroups" ("RoleStrength")
;

CREATE TABLE "dbo"."Roles"("strength" int4 NOT NULL,"Detail" varchar(20) NOT NULL,CONSTRAINT "PK_dbo.Roles" PRIMARY KEY ("strength"))
;

CREATE TABLE "dbo"."Alerts"("AlertID" serial4 NOT NULL,"Live" boolean NOT NULL,"EventStart" timestamp NOT NULL,"RaisedHours" int4 NOT NULL,"WarnStart" timestamp NOT NULL,"Message" varchar(200) NOT NULL,CONSTRAINT "PK_dbo.Alerts" PRIMARY KEY ("AlertID"))
;

CREATE TABLE "dbo"."AuditEventDescriptions"("idAuditEventDescription" serial4 NOT NULL,"AuditDescription" varchar(40) NOT NULL,CONSTRAINT "PK_dbo.AuditEventDescriptions" PRIMARY KEY ("idAuditEventDescription"))
;

CREATE TABLE "dbo"."AuditEvents"("idAuditEvent" serial4 NOT NULL,"EventDate" timestamp NOT NULL,"UserID" varchar(40) NOT NULL,"idAuditEventDescription" text NOT NULL,"ChildSurname" varchar(100),"ChildForenames" varchar(100),"ChildDateofBirth" timestamp,"AuditEventDescription_idAuditEventDescription" int4,CONSTRAINT "PK_dbo.AuditEvents" PRIMARY KEY ("idAuditEvent"))
;

CREATE INDEX "AuditEvents_IX_AuditEventDescription_idAuditEventDescription" ON "dbo"."AuditEvents" ("AuditEventDescription_idAuditEventDescription")
;

CREATE TABLE "dbo"."AuditEventDataRows"("idAuditData" serial4 NOT NULL,"idAuditEvent" int4 NOT NULL,"ColumnName" varchar(200) NOT NULL,"Was" varchar(200) NOT NULL,"Now" varchar(200) NOT NULL,CONSTRAINT "PK_dbo.AuditEventDataRows" PRIMARY KEY ("idAuditData"))
;

CREATE INDEX "AuditEventDataRows_IX_idAuditEvent" ON "dbo"."AuditEventDataRows" ("idAuditEvent")
;

CREATE TABLE "dbo"."CAFCASSes"("CAFCASSID" serial4 NOT NULL,"Detail" varchar(10),"Description" text,CONSTRAINT "PK_dbo.CAFCASSes" PRIMARY KEY ("CAFCASSID"))
;

CREATE TABLE "dbo"."CaseTypes"("CaseTypeID" serial4 NOT NULL,"Detail" varchar(2),"Description" text,CONSTRAINT "PK_dbo.CaseTypes" PRIMARY KEY ("CaseTypeID"))
;

CREATE TABLE "dbo"."Courts"("CourtID" serial4 NOT NULL,"CourtName" varchar(100) NOT NULL,"AddressLine1" varchar(50),"AddressLine2" varchar(50),"AddressLine3" varchar(50),"AddressLine4" varchar(50),"Town" varchar(30),"County" varchar(30),"Country" varchar(20),"Postcode" varchar(8),"DX" varchar(60),"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.Courts" PRIMARY KEY ("CourtID"))
;

CREATE TABLE "dbo"."CWOes"("CWOID" serial4 NOT NULL,"Detail" varchar(15),CONSTRAINT "PK_dbo.CWOes" PRIMARY KEY ("CWOID"))
;

CREATE TABLE "dbo"."DataUploads"("DataUploadID" serial4 NOT NULL,"UploadStarted" timestamp NOT NULL,"UploadedBy" text,"FileName" text,"FullPathandName" text,"FileSize" int4 NOT NULL,"UploadCompleted" timestamp,"NumberofRows" int4 NOT NULL,"NumberOfErrs" int4 NOT NULL,CONSTRAINT "PK_dbo.DataUploads" PRIMARY KEY ("DataUploadID"))
;

CREATE TABLE "dbo"."DistrictJudges"("DistrictJudgeID" serial4 NOT NULL,"Name" varchar(100),CONSTRAINT "PK_dbo.DistrictJudges" PRIMARY KEY ("DistrictJudgeID"))
;

CREATE TABLE "dbo"."FAQs"("faqID" serial4 NOT NULL,"loggedInUser" boolean NOT NULL,"question" varchar(150) NOT NULL,"answer" varchar(4000) NOT NULL,CONSTRAINT "PK_dbo.FAQs" PRIMARY KEY ("faqID"))
;

CREATE TABLE "dbo"."Genders"("GenderID" serial4 NOT NULL,"Detail" varchar(1),CONSTRAINT "PK_dbo.Genders" PRIMARY KEY ("GenderID"))
;

CREATE TABLE "dbo"."Lapseds"("LapsedID" serial4 NOT NULL,"Detail" varchar(3),CONSTRAINT "PK_dbo.Lapseds" PRIMARY KEY ("LapsedID"))
;

CREATE TABLE "dbo"."Records"("RecordID" serial4 NOT NULL,"Detail" varchar(5),"Description" text,CONSTRAINT "PK_dbo.Records" PRIMARY KEY ("RecordID"))
;

CREATE TABLE "dbo"."Status"("StatusID" serial4 NOT NULL,"Detail" varchar(15),CONSTRAINT "PK_dbo.Status" PRIMARY KEY ("StatusID"))
;

CREATE TABLE "dbo"."Types"("TypeID" serial4 NOT NULL,"Detail" varchar(20),"Description" text,CONSTRAINT "PK_dbo.Types" PRIMARY KEY ("TypeID"))
;

CREATE TABLE "dbo"."Users"("UserID" serial4 NOT NULL,"Name" varchar(150) NOT NULL,"DisplayName" varchar(150),"LastActive" timestamp,"RoleStrength" int4 NOT NULL,CONSTRAINT "PK_dbo.Users" PRIMARY KEY ("UserID"))
;

CREATE INDEX "Users_IX_RoleStrength" ON "dbo"."Users" ("RoleStrength")
;

CREATE TABLE "dbo"."WardshipRecords"("WardshipCaseID" serial4 NOT NULL,"ChildSurname" varchar(100),"ChildForenames" varchar(100),"ChildDateofBirth" timestamp,"DateOfOS" timestamp,"FileNumber" varchar(15),"FileDuplicate" varchar(10),"Xreg" varchar(150),"TypeID" int4,"CourtID" int4,"StatusID" int4,"GenderID" int4,"RecordID" int4,"LapsedID" int4,"CWOID" int4,"DistrictJudgeID" int4,"CaseTypeID" int4,"CAFCASSID" int4,"LapseLetterSent" timestamp,"FirstAppointmentDate" timestamp,"HearingDate" timestamp,"Username" text,CONSTRAINT "PK_dbo.WardshipRecords" PRIMARY KEY ("WardshipCaseID"))
;

CREATE INDEX "WardshipRecords_IX_TypeID" ON "dbo"."WardshipRecords" ("TypeID")
;

CREATE INDEX "WardshipRecords_IX_CourtID" ON "dbo"."WardshipRecords" ("CourtID")
;

CREATE INDEX "WardshipRecords_IX_StatusID" ON "dbo"."WardshipRecords" ("StatusID")
;

CREATE INDEX "WardshipRecords_IX_GenderID" ON "dbo"."WardshipRecords" ("GenderID")
;

CREATE INDEX "WardshipRecords_IX_RecordID" ON "dbo"."WardshipRecords" ("RecordID")
;

CREATE INDEX "WardshipRecords_IX_LapsedID" ON "dbo"."WardshipRecords" ("LapsedID")
;

CREATE INDEX "WardshipRecords_IX_CWOID" ON "dbo"."WardshipRecords" ("CWOID")
;

CREATE INDEX "WardshipRecords_IX_DistrictJudgeID" ON "dbo"."WardshipRecords" ("DistrictJudgeID")
;

CREATE INDEX "WardshipRecords_IX_CaseTypeID" ON "dbo"."WardshipRecords" ("CaseTypeID")
;

CREATE INDEX "WardshipRecords_IX_CAFCASSID" ON "dbo"."WardshipRecords" ("CAFCASSID")
;

CREATE TABLE "dbo"."WordTemplates"("templateID" serial4 NOT NULL,"templateName" varchar(80) NOT NULL,"templateXML" text NOT NULL,"active" boolean NOT NULL,"deactivated" timestamp,"deactivatedBy" varchar(50),CONSTRAINT "PK_dbo.WordTemplates" PRIMARY KEY ("templateID"))
;

ALTER TABLE "dbo"."ADGroups" ADD CONSTRAINT "FK_dbo.ADGroups_dbo.Roles_RoleStrength" FOREIGN KEY ("RoleStrength") REFERENCES "dbo"."Roles" ("strength") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AuditEvents" ADD CONSTRAINT "FK_dbo.AuditEvents_dbo.AuditEventDescriptions_AuditEventDescription_idAuditEventDescription" FOREIGN KEY ("AuditEventDescription_idAuditEventDescription") REFERENCES "dbo"."AuditEventDescriptions" ("idAuditEventDescription")
;

ALTER TABLE "dbo"."AuditEventDataRows" ADD CONSTRAINT "FK_dbo.AuditEventDataRows_dbo.AuditEvents_idAuditEvent" FOREIGN KEY ("idAuditEvent") REFERENCES "dbo"."AuditEvents" ("idAuditEvent") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Users" ADD CONSTRAINT "FK_dbo.Users_dbo.Roles_RoleStrength" FOREIGN KEY ("RoleStrength") REFERENCES "dbo"."Roles" ("strength") ON DELETE CASCADE
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.CAFCASSes_CAFCASSID" FOREIGN KEY ("CAFCASSID") REFERENCES "dbo"."CAFCASSes" ("CAFCASSID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.CaseTypes_CaseTypeID" FOREIGN KEY ("CaseTypeID") REFERENCES "dbo"."CaseTypes" ("CaseTypeID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.Courts_CourtID" FOREIGN KEY ("CourtID") REFERENCES "dbo"."Courts" ("CourtID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.CWOes_CWOID" FOREIGN KEY ("CWOID") REFERENCES "dbo"."CWOes" ("CWOID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.DistrictJudges_DistrictJudgeID" FOREIGN KEY ("DistrictJudgeID") REFERENCES "dbo"."DistrictJudges" ("DistrictJudgeID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.Genders_GenderID" FOREIGN KEY ("GenderID") REFERENCES "dbo"."Genders" ("GenderID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.Lapseds_LapsedID" FOREIGN KEY ("LapsedID") REFERENCES "dbo"."Lapseds" ("LapsedID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.Records_RecordID" FOREIGN KEY ("RecordID") REFERENCES "dbo"."Records" ("RecordID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.Status_StatusID" FOREIGN KEY ("StatusID") REFERENCES "dbo"."Status" ("StatusID")
;

ALTER TABLE "dbo"."WardshipRecords" ADD CONSTRAINT "FK_dbo.WardshipRecords_dbo.Types_TypeID" FOREIGN KEY ("TypeID") REFERENCES "dbo"."Types" ("TypeID")
;

CREATE TABLE "dbo"."__MigrationHistory"("MigrationId" varchar(150) NOT NULL,"ContextKey" varchar(300) NOT NULL,"Model" bytea NOT NULL,"ProductVersion" varchar(32) NOT NULL,CONSTRAINT "PK_dbo.__MigrationHistory" PRIMARY KEY ("MigrationId","ContextKey"))
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201901291423549_InitialMigration',E'Wardship.Models.DataContext',decode('H4sIAAAAAAAEAO0923LcuLHvqTr/MDWPKUdjSd6txCUlJUt24hzbciw79puLnqFkVjjkLMnx2ieVL8vD+aTzCwcgCBKXBtAgeBk5rq1aawh0N9DduHY3+v/+/b9nf/q6TRdf4qJM8ux8eXz0cLmIs3W+SbK78+W+uv3d75d/+uN//ebs6Wb7dfF3Xu+U1iOQWXm+/FxVu8erVbn+HG+j8mibrIu8zG+ro3W+XUWbfHXy8OEfVsfHq5igWBJci8XZm31WJdu4/kF+XubZOt5V+yh9mW/itGy+k5KbGuviVbSNy120js+X76NiU35Odkes6nJxkSYRacZNnN4uF1GW5VVUkUY+flfGN1WRZ3c3O/IhSt9+28Wk3m2UlnHT+MdddWw/Hp7Qfqw6QI5qvS+rfOuJ8Pi0YcxKBe/F3mXLOMK6p4TF1Tfa65p958uLqz8X+X63XKi0Hl+mBa2nMfeoAXmw4AUPWh0gqkL/e7C43KfVvojPs3hfFVH6YPF6/ylN1v8df3ub/yPOzrN9mooNI00jZdIH8ul1ke/iovr2Jr6Vm/v8arlYyeArFb6F1kFZv55n1enJcvGKNCX6lMatFgg8uKnyIv5znMVFVMWb11FVxQUR4vNNXPNRa4RCkv6fUyNqR4bPcvEy+voizu6qz+fL35Px8iz5Gm/4h6YB77KEDDYCUxX7GGigneibPKU6XqN0dVXB9Cr6ktzVPQdwLhdv4rQupDJnY4vrwkdW4VmRb+lfLbPr7x9v8n2xpozIgcK3UXEXV3JTzladnlq1l5HFqy6tP4velq1AfNW2RIrSrbWv8sypPFdxFSWpRWdPBtBZtHgvUtI0r6mJAswzMVHKvaYlDjjVpPQi+dJOSk9yMiCizHuOefqF0LqpIiodhumKtOVtsu0xXUVJGW/+QqaI0nO2UhARoWfDNOllXJbRnW3mJsvrlMNgv0mqmuVXcbkukh3bC3gMCwjBLMMk2Rg64ztsjIimGkY1eYAyoC2Phlnnjatzx4kSXqQhTn2UoISV21VZX86dEEFrfIeon8rPrudhyj2dRjMREtDgCZScsopuSRtvRPjNCUA7yJ9jNOTyc5JubvZFZt//HyNXEQSxZ0TylFo5DTmqGfntk6To9qWdsoSucVEVvcl/7bm+MeA5xzxtQu8hz4CnGvE+84xDK/J0v80c591Bdk3Qxs+m9SMRfUU1dHSixiU/EuRmX/HZgPgoAoCLvVbPts7rlYOW+MuLZ5cXNzc+Q74BmWWgN7T7nPYE0KkGufNIfzzEsjDsettPi6Iypj+91KiBmUePGuK9FEmAPRhNOvlOFIlMfF6HjRpgHhWilHvpDwecSnlqgo4NAnaH6rlWX2w2RVyWL5IsPrZQ/2mIeVCgdTIhrdMJaT0am9bb/Ffb2D8d5BiT7zOqtBNQKWxkkPfpdjKv87KitW22piGm5g8WAj8P0ZFoXQ1wO72Jazx0mrKfTq2gT2xi66Hn+OXn/bXX4vP+ep6l5/11r4WHgR3MnuX4p/EkSc9J73ZpHm18BNpBzSLXjnwf8crQU0mZUaytP9CY972+rLE5ZoA+O1Cd1LMkjR2booEIERYQtn6Oss009EjHbpL/icOulZgkLvPtLo17zOav9ttPcZHfvsl/DTQuMkzXt08LPzMlfqpISiKKdfXX/ebO6wQrAc4zYYgt6DVnqAgOxD+nz4U5WuDPLv7mI2ZSfRbh3ka/9BFpAzaVINP87i7ePM+o4Sl09/jLPi4dtw/HyO2fJ+UoK3/t2g+azSb1OyDy2ND24LWUQcyiqIx0H13tIA9nUzrevPMi2pWx136UQcwiVEa6j1A7yIMR6ul4Qn0Tr/PCS6gMYhahMtJ9hNpBHoxQ/U+PB3nlTY5O1b70USAGMYsCMdJ9FKiDPBgFGvP+wdcaNpslrK8V7PAsYN+JLZVtpPGaQ+vPojnc1cxXc2QXtfmPmuOcKMjJepdG3wYi7vArj8rqQrq/x17TTBqxQgWvhau0HzWnl64kyMOFjwz/jZoMOcsY4ySpz0GfsabCT2Zy/uGGaZ4ZSL3r2+sbX7j6wrq+DB12TwNTutrviAILLsljuS99KOK70WdIcLeC8JrwhDFscu1AhksQx8QNn8dc6wR4MnfwAbAaOpc+y6Wyg5rJucoBZnDuQ/DjRUznthvBDxc/IAuy7u52eZJVW6MDvx3HX+KIqnwfULo+OubXngYl49LeeosCq7u8Wn5sq3ZLPVxDW/cN1aBNgLWtrU8iorFtXXNrmyrO5vJ63u1lrm+IxrKK5pbScmcz60rebaT+EYgW0mrm9r2/draOVPFtm2LGc7dSATC2V6rnarlc2bcP/Nrf3Xhe09hqVsHV3KaWbzv5Tba7nbymsZ2sgqudTS3fdvI9v7udvKaxnc0/jnY2//i2k98ButvJaxrbySq42tnU8m0ndj51zKWYedQ4h+IPfgTN23hLTuGV11WcCDfLoa9qiPc58ImwUx32OE3HXcc4b25w4h9evpg+2vA/0WnyoizzdVIrDndQFl8ikZv7NNssbM+SsPYKz5mQVpNRldCzJ6F9vvytxgEDSj5VCCjZVZOM73ipjr/r7CqmLk4LeoNG3wkiO7h1tNFZT/ixkb+QIRsXdIxE6SWZDskkQE4C+vhOsnWyi1JLqxUYcFYwvWNCW9bSUEuu4h1d4bPKIgUMcfmeUG9AS0dhmYtDZytBnRxa5gylN+oJPq5e0Ec4nlnWpodHR8c2BUWH54OEHWOhH+/0oERE6y0RiiDHeByxx1jGRDiiuHQAIxzRF8ygs71k4DfqEZIcokETzAKdDcGkSYBBoVMbZlzDq6Vugzj49UVr8mSLi8b5e7KyGG6uTDrhusbqFEQ1PeH1znEHJtBo79kQK1MvfbI3BSNiY9C2l3bZ2R7cjhn0jF86YpVAu4EcQ9PU60tR1dpb0ql0TWkMSsjGyO4gbVN4H96SGfStvjfG6oF8iTyGpkk30KKasfvtqXRMbAZKrHDcd5B2icwObMMMevX+Gq1V4tX/GDol2A1EjaL2iKn0qWsCSpJQKGeQLnUsDqI/vR7J9hikxA3GmRF0C7bsCJQUe9JE+gY2CyN5R+xYiA6CQhmoTdPrZWNqQ6qJancbQRMVo51AglsFJ9I9uSEYAZtChUK0TeZ4aCum16/GRIoUvmovHUG/FGOrQIJbcyfSL7khGMmaopZC9EvmeGgrptevRh+Qwlft3CPol2IkF2/UQNSj6ZfcENRtlSGAKkS/ZI6HtmJ6/WpcEpDCV/0TRtAvxblBIMG9JybSL7khGMma4qtC9EvmeGgrptcvnzuy0e/HTHdjk96L+d6JDX8f5nsXNuE9GPNaIDAVgYiLzuk+ot/ir9Dzgu/KuHEHKhtXCFUdKM6buJI9HshM0vlIqH4MmkbJKChbIXhmVHIA15krQOosbYcLXHlAH8SEShYAYuYv21tweiBiL8jY2setzQ6UraVEw9WWuDC0F+A6irbIhYNe3UEdaq49XeD0KkuHpV8dkN3zUBB18b0sFyLxkA7iki9CHOieXfwNQlI/0uIA5cddDZgXOOD5cUaD5wWuMVxPhuAobtYZBwK+H9HgeYEDvp6xAHCUJlJzLQTMbOQOYHVB1bCoFVz4JGdNHZtUrOASFgJtim7iDYUqUPosdVVyuK21jRdXAm1pcziqCUiatUD1B5D7hemzO9sIwAg/vyq5Y2jPKpFl+upj4x3ah0olwbs8JFuBJ92tHHV4W5l6ava3AjvJlkgcE83eVaPxT4j71ZllcOiRmq+79AhtbaYxS+91J54Rxp4p/EnvMcbdxGrkUhxOhM7YJ10MUoBDpt1ROI/aPZObSaCvhLVDqrfEMGxS3SNEPpm2gOGMYltDN5d0C7+1M5KNfxj+SEZ9kTnNtndw1tCdr5sxqona2gfBSD0MUwSrtMgSYNMezA9l7+3kjMXo6m92HYRbsJ1VQK2eP4ZmIT88OHkHGQY9TIODcEuxBQo4DWegYPbws5GTPZBdy8OyNQh7FFOWgNNwxAtmD2+jkz2QWcbDMDMIexRLjLgt4kfbofnDT7ZO/kBmBQ/DwiD8USwJAk7DAT2YPcj9kN9eaPh9kGkP1FxIoLnCg8za69q27GzFcs03H85WhqT0Zy+j3S7J7oQk9c2XxQ3LUH/5uxv/7O1bhmO1ljRQvVxuKVV5Ed3FSind9Wzi+pUIeuz6FNGIu8vNVqsmXk4brkk4JeX+WRcYvzjhAPRvWdOUZPLA/X0D+4z0ib5rUXcv1i89dEgCe7OO0qhQU5h0CeC7dHWWlPI2XCzKVETDvuAxyCEHIiZ7MALVQoU1mjlDE4AyElSBosTNzqghsoYO2AhBw2AmvpYgT82hJWZM/LE/EQ//djBSaYwwQUOwNtf0GIAwnHH48UTn0uCD06bb8LD85SIS9gWPQcxeLuIRv3sMZDGBuTSOxQI8vvddHnMRm/AZj6vNZC5iaj8ejhJrl7FB+gwaDXvoNw6PiffGbMSiLJD50G109ETk0gDTSg9L6sbb/T7yDhJyP8maxek9I7Hnt7QJ6QqwNtlw8ZdNRUTwK6nYPo6pvfIbjSJyucQTo/AQo4ZTKPPEKr23qOGVSg9wlNUGmoEm1sbbIWhSNeFwqCRLkQ2oIZR4G4FvwGEsZsGW1EP47rMRKNUtgJfK1imqpSOK7p/CFGsWxTSbnPAaacCB0EMjpFG2XdyuJFpzOK8Zl9/JwozFMC0f5nJvsZ55CNyABCNxI6hR5ELsrCRzS0ztD6HLQmcWtyCRQ/ZEjLxhOPPcvdePp8YQVwceaAloP3ts7aXExdK2XirphfHEiFF7shaJ8dSI8bQnxkdGjI98MLKEwiIm9sVLqnW6YEWkmf4unRNLAaEpvPB0+X5FRN1Xj+nkgzKLfPCB5i/PiRj4NzwW6eU5EZVU0Avfk29GjE9Ajs81TYKuAR5zpA6PmSEhKKPushhtSXPhsG0zjntxoyq6KIfIRHBm9heNDdjIXSn1rsRjS0pfG0Ylra50oyAX+eLUh6b4HY+ty58r4uq+emBSE+RKCNVCvxayRLhqC9lXX84JyXB19gmFHudEKUuudGCUSnwx8my5OkZecjhjXvbmCRr2Nl8mzMi3wxsHv/pogTT+XS8aWESpjQXTAJhJdHXkRojAaIyHv5hAKBMTm2y4IhfBvLo2HHKKWxGVXILH2CW6FbF1Xz32gU3iWmkf2Hw7GEUxOsrhVQVGgdAWE6CJod3bECJLzS9GmDHdiy2X0UkPLxoYBUI0JkATQ7tnFSQTtPGxhXsuGu6CFyIb2MkQIRsToImj3ZMEkh3e+FDBULL5Ti7rjM6OeFHDKBCiNgGaWNu9DiDy1fxmwFCinkk0zKUxRDI9b839bsyh2/IfN+UoCbMIshAJ1/s/fwnDYMZD6CDeBfhTheW802U2Vc46XYGHV5mQvVRe2Lvv37Fzp+KEHaKGdlQIhXQhMDGdw/E0o4r3nFT2w0HFY6i1qUKVO8Xmq+dtXZM8VLuva777YRMShKoIhSI8TpYHVETFvnjYmgZZAoeyRg63TRruSDrcLn24s9gg1o2RruCGdUMY0o9FS12qiUEs9BnZUE5TeYBDNfAUpIynImKpwM/pUl8huq+Hs9CLb5gELfMWRJhF3gpuYrOYWE5ktDlZHQabvomUS/wx1nngIIR1wQ8D+3hqLwf5mVxhjS/TeEYVyMB433Ia0wi58WNesdFZ2t+BGyaKduuuswVxukN00fic4KAOvNZmX+YZgaTvQD4vaQrFNkvhYHxTI1F1PdYCUtUq7ShqvrS/24DUJhhUilKt+UhjTmv+lU1gqhodyqosF4RBX5INjQx9tbsrf0m7Ly+jLLmNy4olGF0ePzz6ebm4SJOoZHHCTaDrY/WFQ1Tk6/EpjXyNN9uVCu4fP0uxlOVGyn+qp2gFA0lxSVKNsaPuHKkCKHtijOxkHjVziC0LqmdaT7aCMQpfomL9OSq0fKieKOUrDLHx2pOWz8np4Ov58p815OPF8w8fReAHi+uCqNPjxcPFvxytQOfbBbKyoSRpChB1C7IEWeHJU369aRbUiUtQaBYBcZtIbYeDNRG6zgHH1PQXQhLcT3meeiMQwz8Zmioh02QVbXf+Y0SM/QzQCyHqM7RJbdinTceGUzJ38CRO6dCLqksJjYjGVEo9/NLM/UcjMD+U42FsHpe3QnRk6NjgFowA4XgrHHsF2hOnfOdsbu6xPJL1xNoG1MLV8/DIpftni8gwCD134J67FC/spm0MPpu5bfaEoiS9xjEUFokexgx4zFFsni8QchKBPXaTgH4KEZoh6yO0glvHUh+UdRznJKs4nIUWpX3GC2W37gmgY2qee8997D3PIeb5gGnBkKwVJw+jtQAhEAF2XomcHJhAgGymOGnApjyEKDjgmHIQghXRuwDcdlgKWzTj/sl74MnhiyNhPh0N86NhMbPwRjPGU/8NXBPsODjOwor0xBtpFwVpufTynkU+2ND97N1IbsMJuK2QbDeBm2nFatNfE/ETp5auFzdtQjZ5xKTJwGbeTPw0EO9MsYE4FtpiAd2clKHHZKgSYBh8whfCC7HbAB1LF1gYgEONJQxrDosiDLhi1CIHAycTOWgwoGFyrCAaEX4cWVI944aS3acHMZpUBHNagpy3OmjGarFxOHaCIXFuJjZgY7JOjrALWLS74DrbOuG/r+bBdrb7y+FuBcBc1CgRm9wF3VLuIGdexQcaI2C+ZRQPTe6Nbh52kPPy8HQgHoI5hVE8NDmbunnYQc7LQ8dmcuqLEDD9LkoUJl9ktyg6yO9kY9/3aq/vtd6BXOkd2CWrHg+FkwIcBeWWgmx7nGnv12PPIQVaoVFj5CvGXAUeRg7aV8kW6ITTOXtYk1v3VPhRL5V/mK7bO5QmcioQkRg21X95gvEK0VND2sdYNNWgswW0iiEGNwPD2+6RphgE5QYuhDS89UHQ5oAhxOGjGII4BwwhDu+/MZN5AxhCHD5AIYhzwCB10y+xMcpGoULIWu+oEA1Q4IM4YDBDY9jQggY1AHZMwNDnkMHqJ0bUBa8fUBBdIFIpgC4QVxc/N/gu3xx/htt5mSPO3LsuEXbMHZccxWYxh/pv/qV4Nlg4uIvL/zBDqBD5w50AjCnghcR3LG81a06diY99eLlPq4Tu0wglejWpqu11dhVTo86CnqZo/A2ZBteR/kJwHRBlos3DY0Ty7Te5Bb/VEJMBExdUfaP0Ms/IShAlesjfazJhrJNdlIq9VSqBg9Cc1ets1SJVS67iHd2HZJXSPwxB++sdZ6sWt8JeFxukkDCHvjjjEI2CtLnGqjnjxRqykB8eHWlPjoNUDKhHUhps1qfeEQZ+SmXLSQRE1oTGYU6reMwn+WNkCjMI0IaxJjHNodqg97x0ZB31VsyBtNGcuMef/AQ6RzeAh7syiubgmjb78B2siYanxA5yQVTz3IMu653QeLkot/ab10qnJi4WEKpFo6iEOfmRhze+l1o4n/byJDyDdsAe9IJ68AqSfrQf75eCmJMl+cQHjK4iVsoz6Ajg1C8oSF0qaQf7cr9Uw5BUCR2pMLpSmMnOoBGat7KgD6RM0gb6+37pApg8Bul8PboemIhOrwU2f9FOmnItUZhKyb3SEVdWC2/n2LH1BkF+eg0CHSc7ITfFonD5p3ulLMY0CXiH0LHVw0Z3er0AnUE7eTbFohz5p3ulF8YcDXgn17H1wkZ3er0AHYOEuw9djrD8DlwvPORjfiV2XL2w0Z1eL0Bv206eTbEoR/7pXumFMZkE3ot4bL2w0Z1eL6w3G9qtxj280UDfZsxzkzHhLcZT+YHI9tSsPOaoaUPzOKlsfF4uOmu1auFlz0GeLzefciJvZu9uCrX36TX87DJeQ84+Q5hpiRste8EOaDT7DjaZFiEwwwY3nRJcD6QMVfVpiZW8i6ZXl7nxy9ZdXsfRVVbNTb29etdotiUQpaYQoSvd3a1OoS0CSTSlCBLsAlDHz76DyFledCdmepmk46VfQazvrxHNFeLHddxiIURCzI7rJCRfeOi05HKQnJyY00WRRp7qdOqvEPY6d6QLJz+Ca2h5AYSZlbmR83OchpwXQMhZGWL2bZZiff5tCsAZuEn45kLOd5Qacl4AIedlDtyG8Woeq7hxysyzGlr2GULLcjK50Ko7H42AWgEiJddBEBX9FwGSUjFIUKihk5NfqIZc1RZCFXnPABjs5WOrsClgaf7qD9qmCXBBE4Dab6rBXm46plvul92Bvvq5YcndsW0ylJfj7X4/sKMNiGhYRukeQHYeOTyGwrpiYq68nVEZy0uD2dI5qQA8MHiwBA4IcSbrkriFd8XgSwH0C+N1IdvJ5I1e3er2m6Wr8OwKJQobvvvt/tDdf4OpHfQsEDnQfjxQFrAtrLv/kD1Zt5qLPWdfDrTbdIft7rRuOFUNw2KH6e/D7K6yEXd23GoNNNtChR4pJYfJFr7Bd/LDmFpctewJjeefDrPr/Pjh7LoxdbdqvBIazz8dZtc5XmfXjbkyVfuMuLKDjT6UrvPjmbPrxkzRqglCaDx89juUriPXeccar63vs6ztWvqgtuxsxY6CzQfyU0sTdLZ6s89oABX7Rc4fyV2HgqZHyuK1dBvd1nme3eb8UlxpEa+ixFu9jKtoQzbiF0WV3EbrihSv47JMsrvl4u9RuidVnm4/0ae3rvfVbl+RLsfbT6mUWYxertvon620Np9ds0vYIbpAmplsaER/9mSfpJu23c+AhyEMKOitfRP2R2VZ0fC/u28tpld5hkTUsK81NrTn++vsJqJhdv5te0cjTu+i9bfXTfInMxK3IGS2n10l0V0RbcsGRwdPfhId3my//vH/AW0/Rh2OJAEA', 'base64'),E'6.2.0-61023')
