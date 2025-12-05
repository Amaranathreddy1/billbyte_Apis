using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BillByte.Hubs
{
    public class TableHub : Hub
    {
        // These methods can be called by clients (optional).
        // But controllers/services can also call Clients.All directly via IHubContext<TableHub>.

        public async Task BroadcastTableTimerUpdate(string table, string timeDisplay)
        {
            // broadcast to all clients
            await Clients.All.SendAsync("ReceiveTableTimerUpdate", new { table, timeDisplay });
        }

        public async Task BroadcastTableTimerStop(string table)
        {
            await Clients.All.SendAsync("ReceiveTableTimerStop", table);
        }

        public async Task BroadcastTableUpdate(string table, string status)
        {
            await Clients.All.SendAsync("ReceiveTableUpdate", new { table, status });
        }
        public async Task UpdateTableStatus(string table, string status)
        {
            await Clients.All.SendAsync("ReceiveTableStatusUpdate", table, status);
        }
    }

}
