CREATE DATABASE studyconnect;
USE studyconnect;

CREATE TABLE UserRole (
    URole_ID UNIQUEIDENTIFIER PRIMARY KEY,
    Name VARCHAR(255) NOT NULL UNIQUE,
    Description VARCHAR(255) NOT NULL
);

INSERT INTO UserRole (URole_ID, Name, Description)
SELECT NEWID(), Name, Description
FROM (
    SELECT 'Admin' AS Name, 'Administrator' AS Description
    UNION ALL
    SELECT 'Student', 'Student'
    UNION ALL
    SELECT 'Instructor', 'Instructor'
) AS Roles
WHERE NOT EXISTS (
    SELECT 1 FROM UserRole ur WHERE ur.Name = Roles.Name
);

CREATE TABLE User (
    User_GUID UNIQUEIDENTIFIER PRIMARY KEY,
    URole_ID UNIQUEIDENTIFIER NOT NULL,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    FOREIGN KEY (URole_ID) REFERENCES UserRole(URole_ID),
    ON DELETE CASCADE
);

CREATE TABLE Group (
    Group_ID UNIQUEIDENTIFIER PRIMARY KEY,
    User_GUID UNIQUEIDENTIFIER NOT NULL,
    Name VARCHAR(255) NOT NULL,
    Description VARCHAR(255) NOT NULL,
    Visibility BOOLEAN NOT NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (User_GUID) REFERENCES User(User_GUID),
    ON DELETE CASCADE
);

CREATE TABLE GroupRole (
    GRole_ID UNIQUEIDENTIFIER PRIMARY KEY,
    Name VARCHAR(255) NOT NULL UNIQUE,
    Description VARCHAR(255) NOT NULL
);

INSERT INTO GroupRole (GRole_ID, Name, Description)
SELECT NEWID(), Name, Description
FROM (
    SELECT 'Admin' AS Name, 'Administrator' AS Description
    UNION ALL
    SELECT 'Member', 'Member'
) AS Roles
WHERE NOT EXISTS (
    SELECT 1 FROM GroupRole gr WHERE gr.Name = Roles.Name
);

CREATE TABLE GroupMember(
    User_GUID UNIQUEIDENTIFIER NOT NULL,
    Group_ID UNIQUEIDENTIFIER NOT NULL,
    GRole_ID UNIQUEIDENTIFIER NOT NULL,
    JoinDate DATETIME NOT NULL,
    Status VARCHAR(50) NOT NULL,
    FOREIGN KEY (User_GUID) REFERENCES User(User_GUID),
    FOREIGN KEY (Group_ID) REFERENCES Group(Group_ID),
    FOREIGN KEY (GRole_ID) REFERENCES GroupRole(GRole_ID)
    ON DELETE CASCADE
);

-- Adding a composite primary key to GroupMember
ALTER TABLE GroupMember
    ADD CONSTRAINT pk_GMConstraint PRIMARY KEY (User_GUID, Group_ID, GRole_ID);
GO

CREATE TABLE ForumCategory(
    FCategory_ID UNIQUEIDENTIFIER PRIMARY KEY,
    Name VARCHAR(255) NOT NULL UNIQUE,
    Description VARCHAR(255) NOT NULL
);

INSERT INTO ForumCategory (FCategory_ID, Name, Description)
SELECT NEWID(), Name, Description
FROM (
    SELECT 'SWEN 1' AS Name, 'Software Engineering 2' AS Description
    UNION ALL
    SELECT 'SWEN 2', 'Software Engineering 2'
    UNION ALL
    SELECT 'CT1', 'Computer Technik 1'
    UNION ALL
    SELECT 'CT2', 'Computer Technik 2'
    UNION ALL
    SELECT 'BSY', 'Betriebssysteme'
) AS Categories
WHERE NOT EXISTS (
    SELECT 1 FROM ForumCategory fc WHERE fc.Name = Categories.Name
);

CREATE TABLE ForumPost(
    FPost_ID UNIQUEIDENTIFIER PRIMARY KEY,
    FCategory_ID UNIQUEIDENTIFIER NOT NULL,
    User_GUID UNIQUEIDENTIFIER NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Content TEXT NOT NULL,
    CreatedDate DATETIME NOT NULL,
    UpdatedDate DATETIME NOT NULL,
    Status VARCHAR(50) NOT NULL,
    FOREIGN KEY (FCategory_ID) REFERENCES ForumCategory(FCategory_ID),
    FOREIGN KEY (User_GUID) REFERENCES User(User_GUID)
);

CREATE TABLE ForumComment(
    FComment_ID UNIQUEIDENTIFIER PRIMARY KEY,
    FPost_ID UNIQUEIDENTIFIER NOT NULL,
    User_GUID UNIQUEIDENTIFIER NOT NULL,
    Content TEXT NOT NULL,
    CreatedDate DATETIME NOT NULL,
    UpdatedDate DATETIME NOT NULL,
    Status VARCHAR(50) NOT NULL,
    FOREIGN KEY (FPost_ID) REFERENCES ForumPost(FPost_ID),
    FOREIGN KEY (User_GUID) REFERENCES User(User_GUID)
);