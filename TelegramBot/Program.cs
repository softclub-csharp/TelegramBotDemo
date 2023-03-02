using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var tokens = new List<string>(){
    "5311577744:AAGUrQmj5FmLWahvf1eVWYLxZHDNAN0RVAc",
    "5821230516:AAHtiCvmVivL4qRG6GhkCPeSuubszaUUx1U"
};
var bots = new List<TelegramBotClient>();
foreach (var token in tokens) bots.Add(new TelegramBotClient(token));
await ListenAsync();


async Task ListenAsync()
{
    int lastUpdateId = 0;
    while (true)
    {
        foreach (var bot in bots)
        {
            var updates = await bot.GetUpdatesAsync(lastUpdateId + 1);
            // Loop through each update
            foreach (var update in updates)
            {
                Console.WriteLine($"Bot name : {bot.GetMeAsync().Result.Username}");
                // Update the last update ID
                lastUpdateId = update.Id;
                Console.WriteLine($"Update.Type = {update.Type.ToString()}");
                if(update.Type == UpdateType.Message)
                    Console.WriteLine($"{update.Message.Chat.Username} Wrote message to me: {update.Message.Text} ");
                // Check if the update is a new chat member update
                if (update.Type == UpdateType.MyChatMember && update.MyChatMember.NewChatMember != null)
                {
                    var newChatStatus = update.MyChatMember.NewChatMember.Status;
                    Console.WriteLine($"update.MyChatMember.NewChatMember.Status = {update.MyChatMember.NewChatMember.Status.ToString()}");

                    if (newChatStatus != ChatMemberStatus.Kicked && newChatStatus != ChatMemberStatus.Restricted &&
                        newChatStatus != ChatMemberStatus.Left)
                    {
                        await bot.SendTextMessageAsync(update.MyChatMember.Chat.Id, "Hi everyone, I'm a bot! Nice to meet you.");
                        Console.WriteLine($"New chat member id = {update.MyChatMember.Chat.Id} name : {update.MyChatMember.Chat.Title ?? update.MyChatMember.Chat.Username}");
                    }
                }
            }

            await Task.Delay(1000);
        }

    }
}



