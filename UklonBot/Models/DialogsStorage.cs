using System;
using System.Collections.Generic;
using System.Linq;

namespace UklonBot.Models
{
    public static class DialogsStorage
    {
        private static readonly List<Dialog> Dialogs = new List<Dialog>();

        public static Dialog FirstOrDefault(Func<Dialog, bool> predicate)
        {
            return Dialogs.FirstOrDefault(predicate);
        }

        public static void Add(Dialog dialogToAdd)
        {
            Dialogs.RemoveAll(d => d.ViberUserId == dialogToAdd.ViberUserId);
            Dialogs.Add(dialogToAdd);
        }

        public static void Update(Dialog dialogToUpdate)
        {
            Dialogs.RemoveAll(d => d.ViberUserId == dialogToUpdate.ViberUserId);
            Dialogs.Add(dialogToUpdate);
        }

        public static void EndDialog(Dialog dialogToEnd)
        {
            Dialogs.RemoveAll(d => d.ViberUserId == dialogToEnd.ViberUserId);
        }
    }
}