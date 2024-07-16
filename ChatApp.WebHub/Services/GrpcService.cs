using ChatApp.WebHub.Protos;
using Grpc.Core;

namespace ChatApp.WebHub.Services
{
    public class GrpcService : GrpcServiceProto.GrpcServiceProtoBase
    {
        //private readonly HubService _hubContext;

        //public GrpcService(HubService hubContext)
        //{
        //    _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        //}


        public override async Task<ForwardMessageToChatUsersReply> ForwardMessageToChatUsers(ForwardMessageToChatUsersRequest request, ServerCallContext context)
        {
            string test1 = "LISTE: ";
            request.Guids.ToList().ForEach(x => test1 += x + " ");
            //await _hubContext.SendMessage("hello");
            return new ForwardMessageToChatUsersReply() { IsSuccess = true, Message = $"{test1}" };
        }
    }
}
