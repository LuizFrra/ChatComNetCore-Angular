CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(95) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
);

CREATE TABLE `tbl_users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) NOT NULL,
    `PasswordHash` varchar(100) NOT NULL,
    `ImagePath` varchar(100) NOT NULL DEFAULT 'none',
    CONSTRAINT `PK_tbl_users` PRIMARY KEY (`Id`)
);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20191008151408_CreateUsersTable', '2.2.0-rtm-35687');

ALTER TABLE `tbl_users` CHANGE `PasswordHash` `Password` varchar(100) NOT NULL DEFAULT '';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20191023233254_MudancaNomePropriedadePasswordHashParaPassword', '2.2.0-rtm-35687');

