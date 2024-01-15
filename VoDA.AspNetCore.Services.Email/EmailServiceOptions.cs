namespace VoDA.AspNetCore.Services.Email
{
    public class EmailServiceOptions
    {
        private int _port = 587;

        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Host { get; set; }
        public required int Port
        {
            get => _port;
            set
            {
                if (value < 0 || value > 65535)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Port must be between 0 and 65535");
                }
                _port = value;
            }
        }

        public bool UseSSL { get; set; } = true;
        public bool UseDefaultCredentials { get; set; } = false;

        public string EmailTemplatesFoulder { get; set; } = "EmailTemplates";
    }
}
