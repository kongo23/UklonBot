using System;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using Newtonsoft.Json;
using UklonBot.Models;
using UklonBot.Models.Recieve;
using UklonBot.Models.Repositories.Abstract;
using UklonBot.Models.Send;

namespace UklonBot.Helpers
{
    public class CommunicationHelper : ICommunicationHelper
    {
        private readonly IUnitOfWork _uow;

        public CommunicationHelper(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void PromptEditMenu(ViberCallbackModel callback, Dialog currentDialog)
        {
            if (callback.message.text == "add")
            {
                currentDialog.Ammount += 5;
                DialogsStorage.Update(currentDialog);

                currentDialog.State = DialogStates.ReviewOrder;
                DialogsStorage.Update(currentDialog);
                SendResponse(callback.sender.id, $"Please, review your order\nPickup address: {currentDialog.PickupAddress}\n Destination address: {currentDialog.DestinationAddress}\n Ammount: {currentDialog.Ammount}", KeyboardManager.ReviewOrderKeyboard);
                return;
            }

            if (callback.message.text == "change")
            {
                currentDialog.State = DialogStates.ProvideDestinationAddress;
                DialogsStorage.Update(currentDialog);

                SendResponse(callback.sender.id, "Please, provide your destination address", null);
                return;
            }

            if (callback.message.text == "cancel")
            {
                currentDialog.State = DialogStates.Authorization;
                currentDialog.PickupAddress = callback.message.text;

                DialogsStorage.EndDialog(currentDialog);
                SendResponse(callback.sender.id, "Your order was canceled.\nFeel free to write me any message to start ordering a car.", null);
            }

            HandleError(callback, currentDialog);
        }

        public void PromptReviewResult(ViberCallbackModel callback, Dialog currentDialog)
        {
            if (callback.message.text == "confirm")
            {
                currentDialog.State = DialogStates.WaitingForInfo;
                DialogsStorage.Update(currentDialog);
                //// todo confirm order with Uklon api.
                SendResponse(callback.sender.id, "Your order was confirmed.\nWe are looking for your car.\nWe will notify you, when we find a car for you.", null);
            }

            if (callback.message.text == "edit")
            {
                currentDialog.State = DialogStates.EditOrder;
                DialogsStorage.Update(currentDialog);
                SendResponse(callback.sender.id, "Please, select, what do you want to edit.", KeyboardManager.EditOrderKeyboard);
            }

            HandleError(callback, currentDialog);
        }

        public void SendOrderToReview(ViberCallbackModel callback, Dialog currentDialog)
        {
            //// todo calculate ammount with uklon api
            currentDialog.State = DialogStates.ReviewOrder;
            DialogsStorage.Update(currentDialog);

            SendResponse(callback.sender.id, $"Please, review your order\nPickup address: {currentDialog.PickupAddress}\n Destination address: {currentDialog.DestinationAddress}\n Ammount: {currentDialog.Ammount}", KeyboardManager.ReviewOrderKeyboard);
        }

        public void SaveDestinationAddress(ViberCallbackModel callback, Dialog currentDialog)
        {
            currentDialog.DestinationAddress = callback.message.text;
            DialogsStorage.Update(currentDialog);
        }

        public void PromptDestinationAddress(ViberCallbackModel callback, Dialog currentDialog)
        {
            currentDialog.State = DialogStates.ProvideDestinationAddress;
            DialogsStorage.Update(currentDialog);

            SendResponse(callback.sender.id, "Please, provide your destination address", null);
        }

        public void SavePickupAddress(ViberCallbackModel callback, Dialog currentDialog)
        {
            currentDialog.PickupAddress = callback.message.text;
            DialogsStorage.Update(currentDialog);
        }

        public void PromptPickupAdderss(ViberCallbackModel callback, Dialog currentDialog)
        {
            currentDialog.State = DialogStates.ProvidePickupAddress;
            DialogsStorage.Update(currentDialog);

            SendResponse(callback.sender.id, "Please, provide your pickup address", null);
        }

        public void SaveCity(ViberCallbackModel callback, Dialog currentDialog)
        {
            //// todo think of it more
            var currentUser = _uow.Users.FirstOrDefault(u => u.ViberId == callback.sender.id);
            Cities usersCity;
            bool canParse = Enum.TryParse(callback.message.text, out usersCity);
            if (!canParse)
            {
                HandleError(callback, currentDialog);
            }
            currentUser.City = usersCity;
            _uow.Users.Update(currentUser);
            _uow.Save();
        }

        public void SavePhoneNumber(ViberCallbackModel callback)
        {
            var currentUser = _uow.Users.FirstOrDefault(u => u.ViberId == callback.sender.id);
            //// todo add validation
            currentUser.PhoneNumber = callback.message.text;
            _uow.Users.Update(currentUser);
            _uow.Save();
        }

        public void PromptCity(ViberCallbackModel callback, Dialog currentDialog)
        {
            currentDialog.State = DialogStates.ProvideCity;
            DialogsStorage.Update(currentDialog);

            SendResponse(callback.sender.id, "Which city are you located", KeyboardManager.CitiesKeyboard);
        }

        public Dialog GetCurrentDialog(string viberUserId)
        {
            //// check the state of the dialog
            Dialog currentDialog =
                DialogsStorage.FirstOrDefault(d => d.ViberUserId == viberUserId);
            if (currentDialog == null)
            {
                currentDialog = new Dialog
                {
                    ViberUserId = viberUserId,
                    State = DialogStates.Authorization
                };
                DialogsStorage.Add(currentDialog);
            }

            return currentDialog;
        }

        public void Authorize(ViberCallbackModel callback, Dialog currentDialog)
        {
            var currentUser = _uow.Users.FirstOrDefault(u => u.ViberId == callback.sender.id);

            if (currentUser == null)
            {
                currentUser = new Models.User
                {
                    ViberId = callback.sender.id
                };

                _uow.Users.Create(currentUser);
                _uow.Save();

                PromptPhoneNumber(callback, currentDialog);
                return;
            }

            if (currentUser.City == Cities.Unknown)
            {
                PromptCity(callback, currentDialog);
                return;
            }

            if (currentUser.PhoneNumber == null)
            {
                PromptPhoneNumber(callback, currentDialog);
                return;
            }

            currentDialog.State = DialogStates.ProvidePickupAddress;
            DialogsStorage.Update(currentDialog);
            SendResponse(callback.sender.id, "Please, provide your pickup address", null);
        }

        public void SendResponse(string recieverViberId, string responseText, Keyboard keyboard)
        {
            try
            {
                ViberMessageModel sendResponse = new ViberMessageModel
                {
                    receiver = recieverViberId,
                    text = responseText,
                    type = "text",
                };
                if (keyboard != null)
                    sendResponse.keyboard = keyboard;

                string jsonBody = JsonConvert.SerializeObject(sendResponse);

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri("https://chatapi.viber.com/pa/send_message"),
                        Method = HttpMethod.Post
                    };
                    request.Headers.Add("X-Viber-Auth-Token", WebConfigurationManager.AppSettings["ViberAuthToken"]);
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    var task = client.SendAsync(request)
                        .ContinueWith(async taskwithmsg =>
                        {
                            var innerResponse = await taskwithmsg;
                            /*var result = */
                            await innerResponse.Content.ReadAsStringAsync();
                        });
                    task.Wait();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PromptPhoneNumber(ViberCallbackModel callback, Dialog currentDialog)
        {
            currentDialog.State = DialogStates.ProvidePhoneNumber;
            DialogsStorage.Update(currentDialog);
            SendResponse(callback.sender.id, "Please, enter your phone number", null);
        }

        public void HandleError(ViberCallbackModel callback, Dialog currentDialog)
        {
            DialogsStorage.EndDialog(currentDialog);
            SendResponse(callback.sender.id, "An unknown error occured :( Please, try again.", null);
        }
    }
}