﻿USE [master]
GO

IF EXISTS(select * from sys.databases where name = 'Covid19Dev')
BEGIN
    ALTER DATABASE Covid19Dev SET SINGLE_USER WITH ROLLBACK IMMEDIATE

    DROP DATABASE Covid19Dev
END

CREATE DATABASE Covid19Dev
GO

USE Covid19Dev
GO