DECLARE @DBName nvarchar(50), @SQLString nvarchar(200)
SELECT @DBName = db_name(), @SQLString = 'ALTER DATABASE [' + @DBName + '] SET ALLOW_SNAPSHOT_ISOLATION ON'
EXEC(@SQLString)
GO
DECLARE @DBName nvarchar(50), @SQLString nvarchar(200)
SELECT @DBName = db_name(), @SQLString = 'ALTER DATABASE [' + @DBName + '] SET READ_COMMITTED_SNAPSHOT ON'
EXEC(@SQLString)