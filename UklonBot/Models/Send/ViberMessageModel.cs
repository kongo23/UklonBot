using System.Collections.Generic;

namespace UklonBot.Models.Send
{

    public class Sender
    {
        public string name;
        public string avatar;
    }

    public class ViberMessageModel
    {
        public string receiver;
        public int? min_api_version;
        public Sender sender;
        public string tracking_data;
        public string type;
        public string text;
        public string media;
        public string thumbnail;
        public int? size;
        public int? duration;
        public int? sticker_id;
        public Keyboard keyboard;
    }

    public class Keyboard
    {
        public string Type;
        public List<Button> Buttons;
        public string BgColor;
        public bool? DefaultHeight;
    }

    public class Button
    {
        public int? Columns;
        public int? Rows;
        public string BgColor;
        public string BgMediaType;
        public string BgMedia;
        public bool? BgLoop;
        public string ActionType;
        public string ActionBody;
        public string Image;
        public string Text;
        public string TextVAlign;
        public string TextHAlign;
        public int? TextOpacity;
        public string TextSize;
    }

    public static class KeyboardManager
    {
        public static Keyboard CitiesKeyboard
        {
            get
            {
                return new Keyboard
                {
                    Type = "keyboard",
                    DefaultHeight = true,
                    BgColor = "#FFFFFF",
                    Buttons = new List<Button>
                            {
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Kyiv",
                                    ActionBody = "Kyiv"
                                },
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Lviv",
                                    ActionBody = "Lviv"
                                },
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Dnepr",
                                    ActionBody = "Dnepr"
                                }
                            }
                };
            }
        }

        public static Keyboard ReviewOrderKeyboard
        {
            get
            {
                return new Keyboard
                {
                    Type = "keyboard",
                    DefaultHeight = true,
                    BgColor = "#FFFFFF",
                    Buttons = new List<Button>
                            {
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Confirm",
                                    ActionBody = "confirm"
                                },
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Edit",
                                    ActionBody = "edit"
                                }
                            }
                };
            }
        }

        public static Keyboard EditOrderKeyboard
        {
            get
            {
                return new Keyboard
                {
                    Type = "keyboard",
                    DefaultHeight = true,
                    BgColor = "#FFFFFF",
                    Buttons = new List<Button>
                            {
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Add 5 UAH",
                                    ActionBody = "add"
                                },
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Change destination address",
                                    ActionBody = "change"
                                },
                                new Button
                                {
                                    Columns = 6,
                                    Rows = 1,
                                    BgColor = "#2db9b9",
                                    Text = "Cancel request",
                                    ActionBody = "cancel"
                                }
                            }
                };
            }
        }

        public static Keyboard BuildReportingTimeKeyboard(string projectNameSlashTicketName)
        {
            List<Button> buttons = new List<Button>();
            for (double i = 0.25; i <= 4; i += 0.25)
            {
                buttons.Add(new Button
                {
                    Columns = 1,
                    Rows = 1,
                    BgColor = "#2db9b9",
                    Text = i.ToString(),
                    ActionBody = $"time {projectNameSlashTicketName}/{i}"
                });
            }
            //// Home button
            buttons.Add(new Button
            {
                Columns = 1,
                Rows = 1,
                BgColor = "#2db9b9",
                Text = "home",
                ActionBody = "home"
            });

            return new Keyboard
            {
                Type = "keyboard",
                DefaultHeight = true,
                BgColor = "#FFFFFF",
                Buttons = buttons
            };
        }

        public static Keyboard BuildTicketsKeyboard(string senderId, string senderName, string[] ticketsNames, string projectName)
        {
            List<Button> buttons = new List<Button>();
            foreach (string ticketName in ticketsNames)
            {
                buttons.Add(new Button
                {
                    Columns = 2,
                    Rows = 1,
                    BgColor = "#2db9b9",
                    Text = ticketName,
                    ActionBody = $"report {projectName}/{ticketName}"
                });
            }
            //// Home button
            buttons.Add(new Button
            {
                Columns = 2,
                Rows = 1,
                BgColor = "#2db9b9",
                Text = "home",
                ActionBody = "home"
            });

            return new Keyboard
            {
                Type = "keyboard",
                DefaultHeight = true,
                BgColor = "#FFFFFF",
                Buttons = buttons
            };
        }
    }
}