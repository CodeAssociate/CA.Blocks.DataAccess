using NUlid;
using System.Text;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlidTests;

public class Tests
{
    [Fact]
    public void Test1()
    {
        var myulid = Ulid.NewUlid();

        Console.WriteLine(myulid);
        Console.WriteLine(myulid.ToGuid().ToString());
        Console.WriteLine(myulid.Time);

        var g = myulid.ToGuid();
        var myulidCopy = new Ulid(g.ToByteArray());

        Assert.Equal(myulid.ToString(), myulidCopy.ToString());
    }

    [Fact]
    public void Test2()
    {
        var Guid1 = new Guid("71f68801-fa12-51a4-ad85-71b38df2ab64");
        var Guid2 = new Guid("72f68801-b441-fe11-7c60-eedaaa881bce");

        Assert.True(Guid2 > Guid1);
    }

    [Fact]
    public void Test3()
    {
        var sb = new StringBuilder();
        DateTimeOffset dt = DateTimeOffset.UtcNow;
        Random rnd = new Random();
        for (int i = 0; i < 1000; i++)
        {
            dt = dt.AddHours(rnd.Next(1, 100));
            var myulid = Ulid.NewUlid(dt);

            sb.Append("(");
            sb.Append($"{i},");
            sb.Append($"'{dt.ToString("yyyy-MM-ddTHH:mm:ss")}'");
            sb.Append(",");
            sb.Append($"'{myulid.ToGuid()}'");
            sb.Append(",");
            sb.Append($"'{myulid.ToString()}'");
            sb.Append(",");
            var binaryString = "0x" + string.Join("", myulid.ToByteArray().Select(b => b.ToString("x2")));
            sb.Append(binaryString);
            sb.AppendLine("),");
        }

        Console.WriteLine(sb.ToString());
    }
}
