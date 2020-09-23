namespace TheXDS.Proteus.Plugins
{
    public class PaymentInfo
    {
        public decimal Amount { get; }

        public bool IsInvalid { get; private set; }

        public PaymentInfo(decimal amount, string? tag = null)
        {
            Amount = amount;
            Tag = tag;
        }

        public string? Tag { get; set; }

        public static PaymentInfo Invalid => new PaymentInfo(0m, null) { IsInvalid = true };
        public static PaymentInfo Manual(string? tag) => new PaymentInfo(0m, tag);
    }
}
