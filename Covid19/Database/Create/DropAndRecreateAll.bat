sqlcmd -E -S localhost -i ..\..\Database\Create\DropAndCreate.sql
sqlcmd -E -S localhost -d Covid19Dev -i ..\..\Database\Create\Tables\Country.sql
sqlcmd -E -S localhost -d Covid19Dev -i ..\..\Database\Create\Tables\Record.sql

cd /d "..\..\DataAccess"
dotnet-ef dbcontext scaffold "Server=localhost;Database=Covid19Dev;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models --context-dir Models -c ModelDataContext --force

pause