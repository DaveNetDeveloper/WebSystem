namespace API
{
    public static class MessageProvider
    {
        private static readonly IConfiguration _config;

        static MessageProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Resources/messages.json", optional: false, reloadOnChange: true);

            _config = builder.Build();
        }

        public static string GetMessage(string section, string key)
        {
            return _config[$"v1:{section}:{key}"] ?? "Mensaje no definido.";
        }
    }
}
