namespace UklonBot.Models
{
    public enum DialogStates
    {
        Authorization,
        ProvidePhoneNumber,
        ProvideCity,
        ProvidePickupAddress,
        ProvideDestinationAddress,
        ReviewOrder,
        EditOrder,
        ConfirmOrder,
        WaitingForInfo,
        WaitingForCar
    }
}