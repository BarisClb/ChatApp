using ChatApp.Application.Protos;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Application.Services
{
    public class TestService
    {
        private readonly string connectionString;

        public TestService(IConfiguration config)
        {
            connectionString = config.GetConnectionString("GRPC") ?? throw new ArgumentNullException("Invalid GRPC connection string.");
        }


        public async Task<ForwardMessageToChatUsersReply> TestGrpcRequest()
        {
            using GrpcChannel channel = GrpcChannel.ForAddress(connectionString);
            var client = new GrpcServiceProto.GrpcServiceProtoClient(channel);
            string[] guids = { "HELLO", "WORLD" };
            ForwardMessageToChatUsersRequest requestBody = new() { Message = "HI" };
            requestBody.Guids.AddRange(guids);
            return await client.ForwardMessageToChatUsersAsync(requestBody);
        }
    }
}
