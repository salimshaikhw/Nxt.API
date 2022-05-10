
// DB Migration Commands

$env:ASPNETCORE_ENVIRONMENT='Development'

$env:ASPNETCORE_ENVIRONMENT='Production'

add-migration "Initial" -project Nxt.Repositories -startupproject Nxt.API 

update-database -project Nxt.Repositories -startupproject Nxt.API

remove-migration -project Nxt.Repositories -startupproject Nxt.API

