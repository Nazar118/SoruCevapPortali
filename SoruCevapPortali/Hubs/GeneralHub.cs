using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SoruCevapPortali.Hubs
{
    public class GeneralHub : Hub
    {
        // Bu metot tetiklendiğinde, sisteme bağlı HERKESE (Admin dahil) mesaj gider.
        public async Task SendNewQuestionNotification()
        {
            await Clients.All.SendAsync("ReceiveQuestionNotification");
        }
    }
}