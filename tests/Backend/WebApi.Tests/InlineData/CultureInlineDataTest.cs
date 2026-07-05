using System.Collections;

namespace WebApi.Tests.InlineData;

public class CultureInlineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return ["en"];
        yield return ["es"];
        yield return ["pt"];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}