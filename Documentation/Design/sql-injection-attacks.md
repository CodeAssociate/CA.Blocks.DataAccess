## SQL injection

Once of key responsibilities that the CA.Blocks.DataAccess passed on to you as the developer is that of protecting against sql injection attacks.  The Framework allows the full power of the underlying database and does not generated any SQL to be executed. Whilst all the core methods that allow processing of some sort SQL, the design is protected by default. This means you


 even at the provider level. Using the blocks there is no direct way to execute a SQL statement from the calling code. As the developer you may be tempted to expose this to avoid writing you own access methods by making the protected methods public. Working directly with the SQL means as a developer you are responsible for the SQL generated this means responsibility for injection attacks. The simplest way to avoid injection attacks is not executing any SQL that is not 100% controlled by the code and parameterized. The developer is responsible for generating the SQL to be executed and this will be controlled in the DataAccess Layer ie your class.


### Least privilege
run you connection of the database with the least privilege possible. 

### parameterise all variables 


### Whitelist
In the case you need to build dynamic tables and columns te best defense is a white list of options.   