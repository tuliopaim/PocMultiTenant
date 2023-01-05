# PocMultiTenant

A Prove of Concept for a .NET 7 Web Api with a multi tenant cenario,
where each tenant has it's own DB.

- PocMultiTenant.Api
- PocMultiTenant.MigrationService

#### EF DbContexts:
- AdminDbContext
- PocDbContext

The **AdminDbContext** has the Tenants table, where 
the information regarding each tenant will be on.

The **PocDbContext** is the actual system context, where all
the tables will be on (could be separated in multiples DbContexts).

#### EF Migrations

Each DbContexts has it's own migrations, inside the
`Infrastructure/Migrations` folder

To generate new migrations:

`dotnet ef migrations add [MigrationName] --context [DbContext] -o Infrastructure/Migrations/[admin/poc]` 

#### MigrationService

A console application responsable for querying the Tenants
and applying the latests migrations for each DB.


