using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SoftServe.ITAcademy.BackendDubbingProject.Streaming.Core.Hubs
{
    internal class StreamHub : Hub
    {
        private static int _count;
        private static bool _needWait;
        private static bool _started;
        private string _adminId;

        public override async Task OnConnectedAsync()
        {
            _count++;

            await base.OnConnectedAsync();

            var numberOfConnections = (_count - 1).ToString();

            await Clients.User(_adminId).SendAsync("updateCount", numberOfConnections);

            if (_needWait)
            {
                var time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                await Clients.Caller.SendAsync("ReceiveMessage", "Start", time);
            }

            if (_started)
            {
                await Clients.User(_adminId).SendAsync("Late Connect", Context.ConnectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _count--;

            await base.OnDisconnectedAsync(exception);

            var numberOfConnections = (_count - 1).ToString();

            await Clients.User(_adminId).SendAsync("updateCount", numberOfConnections);
        }

        public async Task SendMessage(string message)
        {
            switch (message)
            {
                case "Start":
                    _adminId = Context.ConnectionId;
                    _needWait = true;
                    break;
                case "End":
                    _adminId = null;
                    _needWait = false;
                    _started = false;
                    break;
                default:
                    _needWait = false;
                    _started = true;
                    break;
            }

            await Clients.Others.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageAndTime(string message, long offset)
        {
            _needWait = false;
            _started = true;
            await Clients.Others.SendAsync("ReceiveMessage", message, offset);
        }

        public async Task SendMessageToUser(string message, long offset, bool paused, string connectionId)
        {
            _needWait = false;
            _started = true;
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message, offset, paused);
        }
    }
}