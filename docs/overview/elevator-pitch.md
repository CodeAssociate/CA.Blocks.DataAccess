# Take Control of Your Data Layer with CA.Blocks.DataAccess
Are you tired of "fighting" with heavy ORM frameworks or drowning in verbose ADO.NET boilerplate? Meet CA.Blocks.DataAccess, the high-performance micro-ORM designed by developers, for developers

Originally created by Kevin Bosch and used in production environments since 2003, CA.Blocks.DataAccess is an open-source (MIT license) library that bridges the gap between relational databases and .NET objects without the overhead of a full-blown ORM

## Why Choose "The Blocks"?
* 100% SQL Control: Stop worrying about "odd SQL generation strategies" from automated tools. With CA.Blocks, you are in complete control of the SQL generated, ensuring your queries are as efficient and predictable as possible
* Eliminate Boilerplate: Raw ADO.NET is powerful but verbose. The Blocks abstract away the low-level complexity of connections, commands, and readers, allowing you to reduce dozens of lines of standard ADO.NET code to a single execution call
* Built for Modern Architecture: While full ORMs excel at simple CRUD, CA.Blocks is specifically optimized for Onion, CQRS, and Repository-type architectures where performance and decoupling are critical
* Performance & Speed: Generally faster than full ORMs, this library focuses on the core "Object Mapping" (the O and M of ORM) to move data between C# types and SQL parameters with minimal friction
* Secure & Robust: Your data is protected by default against SQL injection attacks 
* The library  supports non-blocking, concurrent Async requests to keep your applications responsive

## Key Features at a Glance:
* Easy Setup: Get started quickly with providers for SQL Server, SQLite, and MySQL
* Flexible Translators: Use built-in RowSet and Column translators to map results, or extend them with custom converters for types like JSON or NUlid
* Managed Connections: The underlying database connection is fully managed, so you can focus on your business logic rather than connection state
* Predictability: Avoid the "tightly coupled" code and complex unit-of-work patterns that often plague larger frameworks

If you have a strong understanding of SQL and want a data access layer that is lightweight, predictable, and fast, it’s time to build with CA.Blocks.DataAccess
