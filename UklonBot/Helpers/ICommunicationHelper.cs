using UklonBot.Models;
using UklonBot.Models.Recieve;
using UklonBot.Models.Send;

namespace UklonBot.Helpers
{
    public interface ICommunicationHelper
    {
        void PromptEditMenu(ViberCallbackModel callback, Dialog currentDialog);

        void PromptReviewResult(ViberCallbackModel callback, Dialog currentDialog);

        void SendOrderToReview(ViberCallbackModel callback, Dialog currentDialog);

        void PromptDestinationAddress(ViberCallbackModel callback, Dialog currentDialog);

        void PromptPickupAdderss(ViberCallbackModel callback, Dialog currentDialog);

        void PromptCity(ViberCallbackModel callback, Dialog currentDialog);

        Dialog GetCurrentDialog(string viberUserId);

        void Authorize(ViberCallbackModel callback, Dialog currentDialog);

        void SendResponse(string recieverViberId, string responseText, Keyboard keyboard);

        void SavePhoneNumber(ViberCallbackModel callback);

        void PromptPhoneNumber(ViberCallbackModel callback, Dialog currentDialog);

        void SaveCity(ViberCallbackModel callback, Dialog currentDialog);

        void SavePickupAddress(ViberCallbackModel callback, Dialog currentDialog);

        void SaveDestinationAddress(ViberCallbackModel callback, Dialog currentDialog);

        void HandleError(ViberCallbackModel callback, Dialog currentDialog);
    }
}
