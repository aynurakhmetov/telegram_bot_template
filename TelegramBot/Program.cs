using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.IO;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    class Program
    {
        static ITelegramBotClient botClient;
        static List<long> allChatId = new List<long>();
        static List<string> allUsernames = new List<string>();
        static string pathChatId = "/chatid.txt";
        static string pathUsernames = "/usernames.txt";
        static string usernameOfAdmin = "";
        
        static async Task Main()
        {
            botClient = new TelegramBotClient("");

            using (StreamReader chatIdFromTxt = new StreamReader(pathChatId, System.Text.Encoding.Default))
            {
                string line;
                while ((line = chatIdFromTxt.ReadLine()) != null)
                {
                    if (!allChatId.Contains(long.Parse(line)))
                        allChatId.Add(long. Parse(line));
                }
            }
            
            using (StreamReader usernamesFromTxt = new StreamReader(pathUsernames, System.Text.Encoding.Default))
            {
                string line;
                while ((line = usernamesFromTxt.ReadLine()) != null)
                {
                    if (!allUsernames.Contains(line))
                        allUsernames.Add(line);
                }
            }
            

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"I am user {me.Id} and my name is {me.FirstName}");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            
            botClient.StopReceiving();
        }

        // Отправляет сообщение из Бота Админа в Бота Клиента
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (!allChatId.Contains(e.Message.Chat.Id))
            {
                allChatId.Add(e.Message.Chat.Id);
                using (StreamWriter chatIdSw = new StreamWriter(pathChatId, true, System.Text.Encoding.Default))
                {
                    await chatIdSw.WriteLineAsync(e.Message.Chat.Id.ToString());
                }
            }

            if (!allUsernames.Contains(e.Message.Chat.Username))
            {
                allUsernames.Add(e.Message.Chat.Username);
                using (StreamWriter unamesSw = new StreamWriter(pathUsernames, true, System.Text.Encoding.Default))
                {
                    await unamesSw.WriteLineAsync(e.Message.Chat.Username);
                }
            }

            /*foreach (long id in allChatId)
            {
                await botClient.SendTextMessageAsync(
                    chatId: id,
                     "Команда", 
                    replyMarkup: new ReplyKeyboardMarkup(new []{new [] {new KeyboardButton("📑 ЧЕК-ЛИСТЫ")}, 
                        new [] {new KeyboardButton("💼 КЕЙСЫ")}, new []{new KeyboardButton("📹 ВИДЕО")}}, true, true));
            }*/
            
            
            if (e.Message.Text == "/start")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");
                // 557552160

                string s = "Приветствую👋 \nВы подписались на новости в мире Авито и Юле!\n\nЧек-лист:\n\n" +
                           "📑 Как создать продающее объявление на Авито и Юле\n\n" +
                           "https://clck.ru/Vy7ia\n\n" +
                           "Используйте /off чтобы приостановить подписку\n";

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Text == "/checklist" || e.Message.Text == "📑 ЧЕК-ЛИСТЫ")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");
                // 557552160

                string s = "Чек-лист:\n\n📑 Как создать продающее объявление на Авито и Юле\n" +
                           "https://clck.ru/Vy7ia\n";

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Text == "/cases" || e.Message.Text ==  "💼 КЕЙСЫ")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");
                // 557552160

                string s = "💼 Со всеми кейсами нашей компании можно ознакомится по ссылке:\n\n" +
                           "https://creatmedia.ru/category/keys/\n";

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Text == "/video" || e.Message.Text == "📹 ВИДЕО")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");
                // 557552160

                string s = "На нашем Ютуб канале вы найдёте:\n\n" + 
                           "- новости с торговых площадок\n- обзоры сервисов\n- обучающие материалы\n- разборы аккаунтов\n\n" +
                           "Подписывайтесь ⤵️\n\n" +
                           "https://youtube.com/channel/UCNG_l__rft5pLItNhTOHA_w\n";

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Text == "/off")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");
                // 557552160
                
                if (allChatId.Contains(e.Message.Chat.Id))
                    allChatId.Remove(e.Message.Chat.Id);

                string s = "Ваша подписка деактивирована.\n\n" +
                           "Вы всегда можете включить ее снова с помощью команды /on.\n";

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Text == "/on")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");
                // 557552160
                
                if (!allChatId.Contains(e.Message.Chat.Id))
                    allChatId.Add(e.Message.Chat.Id);

                string s = "Ваша подписка снова активирована!\n";

                await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Text == "/help" || e.Message.Text == "/setting")
            {
                Console.WriteLine($"Rec a text mess in chat {e.Message.Chat.Id}.");

                string s = "Команды бота:\n\n" +
                           "/start - начало подписки, получение чек-листа по созданию продающего объявления\n" +
                           "/cheaklist - получить чек-лист по созданию продающего объявления\n" +
                           "/cases - получить кейсы компании\n" +
                           "/video - посмотреть полезные видео\n" +
                           "/off - прекратить подписку\n";

                           await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, text: s);
            }
            
            if (e.Message.Chat.Username == usernameOfAdmin && e.Message.Text != "/start" && e.Message.Text != "/checklist"
            && e.Message.Text != "/cases" && e.Message.Text != "/video" && e.Message.Text != "/off" && e.Message.Text != "/on"
            && e.Message.Text != "/setting" && e.Message.Text != "/help" && e.Message.Text != "📑 ЧЕК-ЛИСТЫ" && e.Message.Text != "💼 КЕЙСЫ" 
            && e.Message.Text != "📹 ВИДЕО")
            {
                if (e.Message.Text != null && e.Message.Text != "/юзеры")
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Text mess in chat {id}.");
                        
                        await botClient.SendTextMessageAsync(chatId: id, text: e.Message.Text);
                        //await botClientAdmin.SendTextMessageAsync(chatId: e.Message.Chat, text: "You said:\n" + e.Message.Text)
                    }
                }
                
                if (e.Message.Photo != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Photo mess in chat {id}.");
                        await botClient.SendPhotoAsync(chatId: id, e.Message.Photo[0].FileId);
                    }
                }

                if (e.Message.Video != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Video mess in chat {id}.");
                        await botClient.SendVideoAsync(chatId: id, e.Message.Video.FileId);
                    }
                }
                
                if (e.Message.VideoNote != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a VideoNote mess in chat {id}.");
                        await botClient.SendVideoNoteAsync(chatId: id, e.Message.VideoNote.FileId);
                    }
                }
                
                if (e.Message.Voice != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Voice mess in chat {id}.");
                        await botClient.SendVoiceAsync(chatId: id, e.Message.Voice.FileId);
                    }
                }
                
                if (e.Message.Document != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Document mess in chat {id}.");
                        await botClient.SendDocumentAsync(chatId: id, e.Message.Document.FileId);
                    }
                }
                
                if (e.Message.Sticker != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Sticker mess in chat {id}.");
                        await botClient.SendStickerAsync(chatId: id, e.Message.Sticker.FileId);
                    }
                }
                
                if (e.Message.Animation != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Animation mess in chat {id}.");
                        await botClient.SendAnimationAsync(chatId: id, e.Message.Animation.FileId);
                    }
                }
                
                /*if (e.Message.Poll != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Poll mess in chat {id}.");
                        await botClient.SendPollAsync(chatId: id, e.Message.Poll.Question, e.Message.Poll.Options);
                    }
                }*/
                
                if (e.Message.Caption != null)
                {
                    foreach (long id in allChatId)
                    {
                        Console.WriteLine($"Rec a Caption mess in chat {id}.");
                        await botClient.SendTextMessageAsync(chatId: id, e.Message.Caption);
                    }
                }
                
            }

            if (e.Message.Chat.Username == usernameOfAdmin && e.Message.Text == "/юзеры")
            {
                foreach (string names in allUsernames)
                {
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat.Id, "@" + names);
                }
            }
        }
    }
}
