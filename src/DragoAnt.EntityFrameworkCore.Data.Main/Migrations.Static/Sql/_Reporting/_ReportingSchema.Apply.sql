IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'ForReporting')
    EXEC [sys].[sp_executesql] @statement = N'CREATE SCHEMA [ForReporting]';