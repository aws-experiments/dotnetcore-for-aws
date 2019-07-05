namespace Dotnetcore.For.Aws.Domain.Config
{
    public class Swagger
    {
        public Doc Doc { get; set; }
    }
    public class Doc
    {
        public string Version { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}
