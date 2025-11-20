namespace ObjectMapper.Test
{
    public class UnitTests
    {
        public class Source
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class Target
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        [Fact]
        public void Map_ShouldCopyMatchingProperties()
        {
            var mapping = new ConversionSet<Source, Target>();

            var source = new Source { Name = "Alice", Age = 30 };
            var target = mapping.Map(source);

            Assert.Equal(source.Name, target.Name);
            Assert.Equal(source.Age, target.Age);
        }

        [Fact]
        public void Map_ShouldApplyCustomConversion()
        {
            var mapping = new ConversionSet<Source, Target>();
            mapping.WithConversion(nameof(Source.Name), nameof(Target.Name), value => ((string)value).ToUpper());

            var source = new Source { Name = "Charlie", Age = 40 };
            var target = mapping.Map(source);

            Assert.Equal("CHARLIE", target.Name);
            Assert.Equal(source.Age, target.Age);
        }
    }
}
