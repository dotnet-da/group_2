namespace backend
{
    public class ConnectionStringStorer
    {
        private static ConnectionStringStorer instance;
        public string ConnectionString { get; set; }

        private ConnectionStringStorer() { }

        public static ConnectionStringStorer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConnectionStringStorer();
                }
                return instance;
            }
        }
    }
}
