using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WebAppbotbeer.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var botClient = new TelegramBotClient("8169542177:AAF07-1NSmmHC4AZkfocBgi31rLTMyH9h4Q");

        using var cts = new CancellationTokenSource();

        // starting bot
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // getting all types of updates
        };

        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMeAsync();
        Console.WriteLine($" @{me.Username}");
        Console.ReadLine();

        // finishing bot 
        cts.Cancel();
    }

    // Обработка сообщений от пользователей
    static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { Text: { } messageText })
            return;

        var chatId = update.Message.Chat.Id;


        if (messageText == "/app")
        {
            var webAppUrl = "https://beer-bot-front-dmp2.vercel.app/earn";
            Console.WriteLine($"{webAppUrl}");
            var keyboardMarkup = new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithWebApp(
                    text:"Open BeerBot",
                    webApp: new WebAppInfo {Url = webAppUrl}
                    ),
            InlineKeyboardButton.WithCallbackData(
            text: "Login via Telegram",
            callbackData: "login_telegram"
        )

            });
            await botClient.SendTextMessageAsync(
            chatId: update.Message.Chat.Id,
            text: "Press button to open BeerBot or login via Telegram",
            replyMarkup: keyboardMarkup
    );
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId, "u said: " + messageText, cancellationToken: cancellationToken);
        }
    }

    // Обработка ошибок
    static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"error in Telegram API: {apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}