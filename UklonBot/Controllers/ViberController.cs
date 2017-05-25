using System.Net;
using System.Net.Http;
using System.Web.Http;
using UklonBot.Helpers;
using UklonBot.Models;
using UklonBot.Models.Recieve;

namespace UklonBot.Controllers
{
    public class ViberController : ApiController
    {
        private readonly ICommunicationHelper _communicationHelper;

        public ViberController(ICommunicationHelper communicationHelper)
        {
            _communicationHelper = communicationHelper;
        }

        public HttpResponseMessage Post([FromBody]ViberCallbackModel callback)
        {
            if (callback == null || callback.callback_event != "message")
                return Request.CreateResponse(HttpStatusCode.OK);

            Dialog currentDialog = _communicationHelper.GetCurrentDialog(callback.sender.id);

            switch (currentDialog.State)
            {
                case DialogStates.Authorization:
                    {
                        _communicationHelper.Authorize(callback, currentDialog);
                        break;
                    }

                case DialogStates.ProvidePhoneNumber:
                    {
                        _communicationHelper.SavePhoneNumber(callback);
                        _communicationHelper.PromptCity(callback, currentDialog);
                        break;
                    }

                case DialogStates.ProvideCity:
                    {
                        _communicationHelper.SaveCity(callback, currentDialog);
                        _communicationHelper.PromptPickupAdderss(callback, currentDialog);
                        break;
                    }

                case DialogStates.ProvidePickupAddress:
                    {
                        _communicationHelper.SavePickupAddress(callback, currentDialog);
                        _communicationHelper.PromptDestinationAddress(callback, currentDialog);
                        break;
                    }

                case DialogStates.ProvideDestinationAddress:
                {
                        _communicationHelper.SaveDestinationAddress(callback, currentDialog);
                        _communicationHelper.SendOrderToReview(callback, currentDialog);
                        break;
                    }

                case DialogStates.ReviewOrder:
                    {
                        _communicationHelper.PromptReviewResult(callback, currentDialog);
                        break;
                    }

                case DialogStates.EditOrder:
                    {
                        _communicationHelper.PromptEditMenu(callback, currentDialog);
                        break;
                    }
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }


    }
}
