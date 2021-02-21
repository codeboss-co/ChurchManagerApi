# Church Manager

## Running Locally

### Database

*Migrations*

1. In `Package manager console` change to `DbMigrations` project

2.
	- `Add-Migration InitialGroupsDbMigration -Context GroupsDbContext -o Migrations/Groups`
    - `Add-Migration InitialPeopleDbMigration -Context PeopleDbContext -o Migrations/People`
    - `Add-Migration InitialChurchesDbMigration -Context ChurchesDbContext -o Migrations/Churches`

3. 
	- `Update-Database -Context GroupsDbContext`
	- `Update-Database -Context PeopleDbContext`
	- `Update-Database -Context ChurchesDbContext`


