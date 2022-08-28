using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyTelegram.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var cts = new CancellationTokenSource(); // токен отмены
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            //5733089302:AAH9YPC_Ts6czskJa6pHHc-ukbtd2CBAC6s    ANTONIO
            //var botClient = new TelegramBotClient("5733089302:AAH9YPC_Ts6czskJa6pHHc-ukbtd2CBAC6s"); // конструктор бота
           var botClient = new TelegramBotClient("5733089302:AAH9YPC_Ts6czskJa6pHHc-ukbtd2CBAC6s"); // конструктор бота
            botClient.StartReceiving(HandleUpdatesAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token); // компонуєм і починаєм отримувати обнови з ТГ
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Начал прослушку. User {me.Id} and My name is {me.FirstName}");
            Console.ReadLine();
            cts.Cancel();



            async Task HandleUpdatesAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                if (update.Type == UpdateType.Message && update?.Message?.Text != null)
                {
                    await HandleMassege(botClient, update.Message);
                    return;
                }
            }


            async Task HandleMassege(ITelegramBotClient botClient, Message message)
            {
                if (message.Text == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Вас вітає бот Дениса @denblackson");
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/help to get info");

                    return;
                }



                if (message.Text == "/help" || message.Text == "/help@the_ANTONIO_bot")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/week  - який зараз тиждень");
                    await botClient.SendTextMessageAsync(message.Chat.Id, "/PlayMyCoCSize");

                    return;
                }

                


                if (message.Text == "/week@the_ANTONIO_bot" || message.Text == "/week")
                {
                    DateTime dateOfSeptember = new DateTime(2022, 9, 1);

                    if (((DateTime.Now - dateOfSeptember).Days / 7) % 2 != 0)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "It is the week number 1 ", replyToMessageId: message.MessageId);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "It is the week number 2 ", replyToMessageId: message.MessageId);
                    }
                    return;
                }

                if (message.Text == "/PlayMyCoCSize" || message.Text == "/PlayMyCoCSize@the_ANTONIO_bot")
                {
                    Random rnd = new Random();
                    int value = rnd.Next(1, 30);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Your COC size is: " + value.ToString() + "cm", replyToMessageId: message.MessageId);
                    if (value >= 25)
                    {
                        await botClient.SendAnimationAsync(message.Chat.Id, "https://i.gifer.com/VA72.gif");
                    }
                    return;
                }

            }


            Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken) // обробка ошибок
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                    => $"Ошибка телеграм API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };
                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}





