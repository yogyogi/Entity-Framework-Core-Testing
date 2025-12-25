# Entity Framework Core Testing

<img src="https://www.yogihosting.com/wp-content/uploads/2025/12/EFCore-testing.png" alt="Entity Framework Core Testing"  title="Entity Framework Core Testing">

Learn all about how to perform testing in Entity Framework Core. Tutorial <a href="https://www.yogihosting.com/testing-entity-framework-core/" target="_blank">link</a>.

## Version
.NET 10.0

## Instruction
To create database change the connection string in the `appsettings.json` file and then perform migration - `PM> add-migration Migration1` `PM> Update-Database`

## What is covered
The following things are implemented in this repository:

- [x] Testing against the production database
- [x] Testing without the production database - here we use SQLite (in-memory mode) as a database fake, EF Core in-memory provider as a database fake, Mock DbSet and use repository layer to exclude EF Core entirely from testing and to fully mock the repositor
      
## Support

Your support of every $5 will be a great reward for me to carry on my work. Thank you !

<a href="https://www.buymeacoffee.com/YogYogi" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" width="200"  style="height: 60px !important;width: 200px !important;" ></a>
<a href="https://www.paypal.com/paypalme/yogihosting" target="_blank"><img src="https://raw.githubusercontent.com/yogyogi/yogyogi/main/paypal.png" alt="Paypal Me" width="300"></a>

