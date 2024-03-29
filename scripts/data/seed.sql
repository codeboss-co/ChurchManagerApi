-- CHURCHES --
INSERT INTO "ChurchGroup" ("Name", "Description", "RecordStatus")
VALUES ('Church Group', 'First Church Group', 'Active');
INSERT INTO "Church" ("ChurchGroupId", "Name", "Description", "ShortCode", "PhoneNumber", "Address", "RecordStatus")
VALUES (1, 'Church 1', 'First Church', 'CHURCH', '+2756565565', NULL, 'Active');


-- FAMILY --
INSERT INTO "Family" ("RecordStatus", "Name") VALUES ('Active', 'Cagnetta Family');
-- PERSON --
-- person 1
INSERT INTO "Person" ("RecordStatus", "ConnectionStatus", "AgeClassification", "Gender", "DeceasedStatus_IsDeceased", "FullName_Title", "FullName_FirstName", "FullName_NickName", "FullName_MiddleName", "FullName_LastName", "FullName_Suffix", "BirthDate_BirthDay", "BirthDate_BirthMonth", "BirthDate_BirthYear", "MaritalStatus", "AnniversaryDate", "Email_Address", "Email_IsActive", "CommunicationPreference", "PhotoUrl", "ChurchId", "FamilyId", "UserLoginId")
VALUES ('Active', 'Member', 'Adult', 'Male', false, 'Pastor', 'Dillan', NULL, 'Arthur', 'Cagnetta', NULL, 6, 11, 1981, 'Married', NULL, 'dillancagnetta@yahoo.com', true, 'EMail', 'https://secure.gravatar.com/avatar/6fdc48b6ec4d95f2fd682fc2982eb01b', 1, 1, '08925ade-9249-476b-8787-b3dd8f5dbc13');
-- person 2
INSERT INTO "Person" ("RecordStatus", "ConnectionStatus", "AgeClassification", "Gender", "DeceasedStatus_IsDeceased", "FullName_Title", "FullName_FirstName", "FullName_NickName", "FullName_MiddleName", "FullName_LastName", "FullName_Suffix", "BirthDate_BirthDay", "BirthDate_BirthMonth", "BirthDate_BirthYear", "MaritalStatus", "AnniversaryDate", "Email_Address", "Email_IsActive", "CommunicationPreference", "PhotoUrl", "ChurchId", "FamilyId", "UserLoginId")
VALUES ('Active', 'Member', 'Adult', 'Female', false, 'Mrs', 'Danielle', NULL, 'Philippa', 'Cagnetta', NULL, 6, 11, 1980, 'Married', NULL, 'danielle@yahoo.com', true, 'EMail', NULL, 1, 1, NULL);
-- person 3
INSERT INTO "Person" ("RecordStatus", "ConnectionStatus", "AgeClassification", "Gender", "DeceasedStatus_IsDeceased", "FullName_Title", "FullName_FirstName", "FullName_NickName", "FullName_MiddleName", "FullName_LastName", "FullName_Suffix", "BirthDate_BirthDay", "BirthDate_BirthMonth", "BirthDate_BirthYear", "MaritalStatus", "AnniversaryDate", "Email_Address", "Email_IsActive", "CommunicationPreference", "PhotoUrl", "ChurchId", "FamilyId", "UserLoginId")
VALUES ('Active', 'Member', 'Child', 'Male', false, 'Mr', 'Daniel', NULL, 'Athanasios', 'Cagnetta', NULL, 6, 11, 1980, 'Single', NULL, 'daniel@yahoo.com', true, 'EMail', NULL, 1, 1, NULL);

-------------------------------------------------------------------------------------------------
-- GROUP TYPE --
INSERT INTO "GroupType" ("Name", "Description", "GroupTerm", "GroupMemberTerm", "TakesAttendance")
VALUES ('Cell', 'Cell Group', 'Group', 'Member', true);

-- GROUP MEMBER ROLES --
INSERT INTO "GroupMemberRole" ("Name", "Description", "IsLeader") VALUES ('Cell Leader', 'Cell Leader', true);
INSERT INTO "GroupMemberRole" ("Name", "Description", "IsLeader") VALUES ('Member', 'Group Member', false);

-- GROUP -- Create 3 cells - 2 are 1 is parent of 1
INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "RecordStatus")
VALUES (NULL, 1, 1, 'First Cell Group','Amazing 1st Cell', 'Active');

INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "RecordStatus")
VALUES (1, 1, 1, 'Split Cell Group','From 1st Cell', 'Active');

INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "RecordStatus")
VALUES (NULL, 1, 1, 'Second Cell Group','Amazing 2nd Cell', 'Active');

-- GROUP MEMBERS -- 3 members of 1 cell (2 member, 1 leader)
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "RecordStatus", "CreatedDate","CommunicationPreference")
VALUES (1, 1, 1, 'Active', CURRENT_DATE, 'Email' ); -- LEADER
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "RecordStatus", "CreatedDate", "CommunicationPreference")
VALUES (1, 2, 2, 'Active', CURRENT_DATE, 'SMS' ); -- MEMBER 1
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "RecordStatus", "CreatedDate", "CommunicationPreference")
VALUES (1, 3, 2, 'Active', CURRENT_DATE, 'WhatsApp' ); -- MEMBER 2

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