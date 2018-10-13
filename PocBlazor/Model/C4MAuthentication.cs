namespace PocBlazor.Model
{
    public class C4MAuthentication
    {
        public string ConsumerKey { get; set; }        
        public string Nonce { get; set; }
        public long Timestamp { get; set; }
        public string Version => "1.0";
        public string Signature { get; set; }

        public override string ToString()
        {
            return $"{{\"consumer_key\":\"{ConsumerKey}\", \"nonce\":\"{Nonce}\", \"Timestamp\":{Timestamp}, \"version\":\"{Version}\", \"signature\":\"{Signature}\"}}";
        }
    }
}
