using System.Text;
using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlidTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var myulid = Ulid.NewUlid();

            
            TestContext.Out.WriteLine(myulid);
            TestContext.Out.WriteLine(myulid.ToGuid().ToString());
            TestContext.Out.WriteLine(myulid.Time);

            var g = myulid.ToGuid();
            var myulidCopy = new Ulid(g.ToByteArray());

            Assert.That(myulidCopy.ToString(), Is.EqualTo(myulid.ToString()));
            Assert.Pass();

            //var myulid = Ulid();
        }


        // 71f68801-fa12-51a4-ad85-71b38df2ab64  26/06/2023 6:43:05 AM +00:00 01H3V724QTMH8TV1BHPE6Z5AV4
        // 72f68801-b441-fe11-7c60-eedaaa881bce  26/06/2023 6:44:23 AM +00:00 01H3V74GDM27Z7RR7EVAN8G6YE
        // 7cf68801-4936-e662-80d8-5b9ac2278345  26/06/2023 6:55:15 AM +00:00 01H3V7RDJ9CBK81P2VKB12F0T5
        // 85f68801-dcbb-aaaa-c49e-cda8ff6b93b0  26/06/2023 7:05:39 AM +00:00 01H3V8BEYWNANC97PDN3ZPQ4XG

        [Test]
        public void Test2()
        {
            var Guid1 = new Guid("71f68801-fa12-51a4-ad85-71b38df2ab64");
            var Guid2 = new Guid("72f68801-b441-fe11-7c60-eedaaa881bce");

            //Assert.True(Guid2 > Guid1)
        }


        [Test]
        public void Test3()
        {
            var sb = new StringBuilder();
            DateTimeOffset dt = DateTimeOffset.UtcNow;
            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                dt = dt.AddHours(rnd.Next(1, 100));
                var myulid = Ulid.NewUlid(dt);

                sb.Append($"(");
                sb.Append($"{i},");
                sb.Append($"'{dt.ToString("yyyy-MM-ddTHH:mm:ss")}'");
                sb.Append($",");
                sb.Append($"'{myulid.ToGuid()}'");
                sb.Append($",");
                sb.Append($"'{myulid.ToString()}'");
                sb.Append($",");
                var binaryString = "0x" + string.Join("", myulid.ToByteArray().Select(b => b.ToString("x2")));
                sb.Append(binaryString);
                sb.AppendLine($"),");
            }

            TestContext.WriteLine(sb.ToString());

        }

    }
}