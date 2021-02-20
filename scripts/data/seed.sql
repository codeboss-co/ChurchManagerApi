-- FAMILY --
INSERT INTO "Family" ("RecordStatus", "Name") VALUES ('Active', 'Cagnetta Family');
-- PERSON --
INSERT INTO "Person" ("RecordStatus", "ConnectionStatus", "AgeClassification", "Gender", "IsDeceased", "Title", "FirstName", "NickName", "MiddleName", "LastName", "Suffix", "BirthDay", "BirthMonth", "BirthYear", "MaritalStatus", "AnniversaryDate", "Email", "IsEmailActive", "CommunicationPreference", "PhotoUrl", "ChurchId", "FamilyId")
VALUES ('Active', 'Member', 'Adult', 'Male', false, 'Pastor', 'Dillan', NULL, 'Arthur', 'Cagnetta', NULL, 6, 11, 1981, 'Married', NULL, 'dillancagnetta@yahoo.com', true, 'EMail', NULL, 1, 1);

-------------------------------------------------------------------------------------------------
-- GROUP TYPE --
INSERT INTO "GroupType" ("Name", "Description", "GroupTerm", "GroupMemberTerm", "TakesAttendance")
VALUES ('Cell', 'Cell Group', 'Group', 'Member', true);

-- GROUP MEMBER ROLES --
INSERT INTO "GroupMemberRole" ("Name", "Description", "IsLeader") VALUES ('Cell Leader', 'Cell Leader', true);
INSERT INTO "GroupMemberRole" ("Name", "Description", "IsLeader") VALUES ('Member', 'Group Member', false);

-- GROUP -- Create 3 cells - 2 are 1 is parent of 1
INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "IsActive")
VALUES (NULL, 1, 1, 'First Cell Group','Amazing 1st Cell', true);

INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "IsActive")
VALUES (1, 1, 1, 'Split Cell Group','From 1st Cell', true);

INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "IsActive")
VALUES (NULL, 1, 1, 'Second Cell Group','Amazing 2nd Cell', true);

-- GROUP MEMBERS -- 3 members of 1 cell (2 member, 1 leader)
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "GroupMemberStatus", "DateTimeAdded", "IsArchived", "CommunicationPreference")
VALUES (1, 1, 1, 'Active', CURRENT_DATE, false, 'Email' ); -- LEADER
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "GroupMemberStatus", "DateTimeAdded", "IsArchived", "CommunicationPreference")
VALUES (1, 2, 2, 'Active', CURRENT_DATE, false, 'SMS' ); -- MEMBER 1
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "GroupMemberStatus", "DateTimeAdded", "IsArchived", "CommunicationPreference")
VALUES (1, 3, 2, 'Active', CURRENT_DATE, false, 'WhatsApp' ); -- MEMBER 2

-- GROUP ATTENDANCE --
INSERT INTO "GroupAttendance" ("GroupId", "AttendanceDate", "DidNotOccur", "AttendanceCount", "FirstTimerCount", "NewConvertCount", "Notes")
VALUES (1, CURRENT_DATE, NULL, 3, 1, 1, 'Great first cell meeting' );

-- GROUP MEMBER ATTENDANCE --
INSERT INTO "GroupMemberAttendance" ("GroupMemberId", "GroupId", "AttendanceDate", "DidAttend", "IsFirstTime", "IsNewConvert","Note", "GroupAttendanceId")
VALUES (1, 1, CURRENT_DATE, true, false, false, NULL, 1); -- LEADER
INSERT INTO "GroupMemberAttendance" ("GroupMemberId", "GroupId", "AttendanceDate", "DidAttend", "IsFirstTime", "IsNewConvert","Note", "GroupAttendanceId")
VALUES (2, 1, CURRENT_DATE, true, true, true, 'First timer gave his life to Christ', 1); -- MEMBER 1
INSERT INTO "GroupMemberAttendance" ("GroupMemberId", "GroupId", "AttendanceDate", "DidAttend", "IsFirstTime", "IsNewConvert","Note", "GroupAttendanceId")
VALUES (3, 1, CURRENT_DATE, true, false, false, 'Great member', 1); -- MEMBER 2