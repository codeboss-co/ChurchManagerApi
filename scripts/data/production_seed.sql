-- CHURCHES --
INSERT INTO "Churches"."ChurchGroup" ("Name", "Description", "RecordStatus")
VALUES ('CT Group 1', 'Cape Town Group 1', 'Active');
INSERT INTO "Churches"."Church" ("ChurchGroupId", "Name", "Description", "ShortCode", "PhoneNumber", "Address", "RecordStatus")
VALUES (1, 'CE Cape Town', 'Christ Embassy Cape Town', 'CECT', '+2756565565', NULL, 'Active');