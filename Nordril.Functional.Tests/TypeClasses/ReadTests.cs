using Nordril.Functional.Data;
using Nordril.Functional.TypeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Nordril.Functional.Tests.TypeClasses
{
    public class ReadTests
    {
        public static IEnumerable<object[]> TryParseData()
        {
            yield return new object[]
            {
                null,
                false,
                null
            };

            yield return new object[]
            {
                "",
                false,
                null
            };

            yield return new object[]
            {
                "{}",
                true,
                new HashSet<string>()
            };

            yield return new object[]
            {
                "{a}",
                true,
                new HashSet<string>{"a" }
            };

            yield return new object[]
            {
                "{abc}",
                true,
                new HashSet<string>{"abc"}
            };

            yield return new object[]
            {
                "{abc,def,ghi}",
                true,
                new HashSet<string>{"abc", "def", "ghi" }
            };

            yield return new object[]
            {
                "{abc\\,,def,ghi}",
                true,
                new HashSet<string>{"abc,", "def", "ghi" }
            };
        }

        [Theory]
        [MemberData(nameof(TryParseData))]
        public void TryParseTest(string input, bool isOk, HashSet<string> expected)
        {
            var actualInstance = new StringSet(new List<string>()).TryParse(input);
            var actualExtension = Read.TryParse(new StringSet(Array.Empty<string>()), input);
            var actualUnsafe = Read.TryParseUnsafe<StringSet>(input);

            if (!isOk)
            {
                Assert.False(actualInstance.HasValue);
                Assert.False(actualExtension.HasValue);
                Assert.False(actualUnsafe.HasValue);
            }
            else
            {
                var actualInstanceValue = actualInstance.Value();
                var actualExtensionValue = actualExtension.Value();
                var actualUnsafeValue = actualUnsafe.Value();

                Assert.Equal(expected, actualInstanceValue.Value);
                Assert.Equal(expected, actualExtensionValue.Value);
                Assert.Equal(expected, actualUnsafeValue.Value);
            }
        }

        private class StringSet : IRead<StringSet, Unit>
        {
            public HashSet<string> Value { get; } = new HashSet<string>();

            public StringSet(IEnumerable<string> values)
            {
                Value = new HashSet<string>(values ?? Array.Empty<string>());
            }

            public Maybe<StringSet> TryParse(IEnumerable<char> source, Unit options)
            {
                if (source == null)
                    return Maybe.Nothing<StringSet>();

                var xs = source.ToList();

                if (xs.Count < 2 || xs[0] != '{' || xs[^1] != '}')
                    return Maybe.Nothing<StringSet>();

                xs = xs.GetRange(1, xs.Count - 2);

                var values = new List<string>();

                var state = ParserState.Init;

                var begin = 0;

                foreach (var (x,i) in xs.ZipWithStream(0, i => i +1))
                {
                    if (state == ParserState.Init)
                    {
                        if (x == '\\')
                            state = ParserState.Escape;
                        else if (x == ',')
                        {
                            state = ParserState.Separator;
                            values.Add(new string(xs.GetRange(begin, i - begin).ToArray()));
                            begin = i + 1;
                        }
                    }
                    else if (state == ParserState.Escape)
                    {
                        state = ParserState.Init;
                    }
                    else if (state == ParserState.Separator)
                    {
                        if (x == '\\')
                            state = ParserState.Escape;
                        else
                            state = ParserState.Init;
                    }
                }

                if (state == ParserState.Init)
                {
                    if (xs.Count != 0)
                        values.Add(new string(xs.GetRange(begin, xs.Count - begin).ToArray()));

                    var unescapedValues = values.Select(v =>
                    {
                        return v
                            .Replace("\\,", ",")
                            .Replace("\\}", "}")
                            .Replace("\\{", "{")
                            .Replace("\\\\", "\\");

                    });

                    return Maybe.Just(new StringSet(unescapedValues));
                }
                else
                    return Maybe.Nothing<StringSet>();
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                var escapedValues = Value.Select(x =>
                {
                    return x
                        .Replace("\\", "\\\\")
                        .Replace("{", "\\{")
                        .Replace("}", "\\}")
                        .Replace(",", "\\,");
                });

                sb.Append("{");
                sb.AppendJoin(',', escapedValues);
                sb.Append("}");

                return sb.ToString();
            }

            private enum ParserState
            {
                Init,
                Escape,
                Separator
            }
        }
    }
}
