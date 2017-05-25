namespace UklonBot.Models
{
    public class Dialog
    {
        public string ViberUserId { get; set; }

        public string PickupAddress { get; set; }

        public string DestinationAddress { get; set; }

        public double Ammount { get; set; }

        public DialogStates State { get; set; }
    }
}