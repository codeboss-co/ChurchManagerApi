-- GROUP TYPE --
INSERT INTO "GroupType" ("Name", "Description", "GroupTerm", "GroupMemberTerm", "TakesAttendance")
VALUES ('Cell', 'Cell Group', 'Group', 'Member', true);

-- GROUP MEMBER ROLES --
INSERT INTO "GroupMemberRole" ("Name", "Description", "IsLeader") VALUES ('Member', 'Group Member', false);
INSERT INTO "GroupMemberRole" ("Name", "Description", "IsLeader") VALUES ('Cell Leader', 'Cell Leader', true);

-- GROUP --
INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "IsActive")
VALUES (NULL, 1, 1, 'First Cell Group','Amazing 1st Cell', true);

INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "IsActive")
VALUES (3, 1, 1, 'Split Cell Group','From 1st Cell', true);

INSERT INTO "Group" ("ParentGroupId", "GroupTypeId", "ChurchId", "Name", "Description", "IsActive")
VALUES (NULL, 1, 1, 'Second Cell Group','Amazing 2nd Cell', true);

-- GROUP MEMBERS --
INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "GroupMemberStatus", "DateTimeAdded", "IsArchived", "CommunicationPreference")
VALUES (3, 1, 1, 'Active', CURRENT_DATE, false, 'Email' );

INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "GroupMemberStatus", "DateTimeAdded", "IsArchived", "CommunicationPreference")
VALUES (3, 2, 1, 'Active', CURRENT_DATE, false, 'SMS' );

INSERT INTO "GroupMember" ("GroupId", "PersonId", "GroupMemberRoleId", "GroupMemberStatus", "DateTimeAdded", "IsArchived", "CommunicationPreference")
VALUES (3, 3, 1, 'Active', CURRENT_DATE, false, 'WhatsApp' );

-- GROUP ATTENDANCE --
INSERT INTO "GroupMemberAttendance" ("GroupMemberId", "GroupId", "AttendanceDate", "DidAttend", "IsFirstTime", "Note", "GroupAttendanceId")
VALUES (1, 3, CURRENT_DATE, true, true, 'Filled with the Holy Spirit')