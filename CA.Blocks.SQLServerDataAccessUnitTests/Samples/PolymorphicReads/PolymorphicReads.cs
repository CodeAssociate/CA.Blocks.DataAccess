using CA.Blocks.DataAccess.DI;
using CA.Blocks.SQLServerDataAccess;
using System;
using System.Data;
using System.Threading.Tasks;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Extensions;
using NUnit.Framework;
using Newtonsoft.Json;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Samples.PolymorphicReads
{
    // Provide an example of using a Polymorphic json field in the db.  we going to store a shape the DB in the shape field in json format
    // in order to Deserialize the shape data we need to store the type of shape
    // in this example we do this by storing the type of as a class, this can be used to get the type
    // and Deserialize the object into a instance of that type. You can then cast that object as a that type or work with 
    // the non abstract instance
    public abstract class Shape
    {
        public abstract double Area();

        [JsonIgnore] public abstract string Describe { get; }
    }

    public class Square : Shape
    {
        public int Length { get; set; }

        public override double Area()
        {
            return Length * Length;
        }

        public override string Describe => $" = {Length} x {Length}";
    }

    public class Rectangle : Shape
    {
        public int Length { get; set; }

        public int Width { get; set; }

        public override double Area()
        {
            return Length * Width;
        }
        public override string Describe => $" = {Length} x {Width}";
    }

    public class Circle : Shape
    {
        public int Radius { get; set; }


        public override double Area()
        {
            return Math.PI * Radius * Radius;
        }
        public override string Describe => $" = {Math.PI} x {Radius}^2";
    }



    [TestFixture]
    public class PolymorphicReads : SqlServerDataAccess
    {
        public PolymorphicReads() : base(new SimpleConnectionStringDataAccessConfig("Server=(local);Database=tempdb;Integrated Security=SSPI;TrustServerCertificate=True"))

        {

        }

        private string DropTestTableIfExistsSQL()
        {
            return @"
If Exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'CABLOCKS_PolymorphicReads_Example')
BEGIN
	Drop Table CABLOCKS_PolymorphicReads_Example
END";
        }

        private string CreateTestTableSQL()
        {
            return @"
Create Table CABLOCKS_PolymorphicReads_Example (Id INT not null  identity(1,1) primary key,
												TypeOfShape varchar(256) NOT NULL,
												Shape varchar(max) NOT NULL
												)";
        }

        [OneTimeSetUp]
        public void Setup()
        {
            ExecuteNonQuery(CreateTextCommand(DropTestTableIfExistsSQL()));

            ExecuteNonQuery(CreateTextCommand(CreateTestTableSQL()));

        }

        [OneTimeTearDown]
        public void TearDown()
        {
           ExecuteNonQuery(CreateTextCommand(DropTestTableIfExistsSQL()));
        }

        private void InsertShape(Shape shape)
        {
            var sql =
                @"Insert into CABLOCKS_PolymorphicReads_Example (TypeOfShape, Shape) values (@typeOfShape, @shape)";
            var cmd = CreateTextCommand(sql);
            cmd.Parameters.Add(shape.GetType().FullName.ToSqlParameter("@typeOfShape", SpecificSQLStringType.VarChar));
            cmd.Parameters.Add(JsonConvert.SerializeObject(shape, Formatting.Indented).ToSqlParameter("@shape", SpecificSQLStringType.VarChar));
            ExecuteNonQuery(cmd);
        }

        [Test, Order(1)]
        public void InsertShapes()
        {
           var square = new Square { Length = 10 };
           var rectangle = new Rectangle() { Length = 10, Width = 15};
           var circle  = new Circle() { Radius = 10};

           InsertShape(square);
           InsertShape(rectangle);
           InsertShape(circle);
        }

        private Shape ReadPolymorphicData(IDataReader dr)
        {
            var typeOfShape = dr.AsString("TypeOfShape");
            var shape = dr.AsString("shape");
            var type = Type.GetType(typeOfShape);
            return (Shape)JsonConvert.DeserializeObject(shape, type);
        }

        [Test, Order(2)]
        public void ReadPolymorphicData()
        {
            var cmd = CreateTextCommand("Select * from CABLOCKS_PolymorphicReads_Example");
            var shapes = Execute(cmd).ToListOf<Shape>(ReadPolymorphicData);
            foreach (var shape in shapes)
            {
                TestContext.WriteLine(shape.Describe);
                TestContext.WriteLine(shape.Area());
            }
        }

        [Test, Order(3)]
        public async Task ReadPolymorphicDataAsync()
        {
            var cmd = CreateTextCommand("Select * from CABLOCKS_PolymorphicReads_Example");
            var shapes = await ExecuteAsync(cmd).ToListOf<Shape>(ReadPolymorphicData);
            foreach (var shape in shapes)
            {
                TestContext.WriteLine(shape.Describe);
                TestContext.WriteLine(shape.Area());
            }
        }

    }
}
