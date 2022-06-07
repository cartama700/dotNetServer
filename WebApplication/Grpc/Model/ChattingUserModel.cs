using ChattingGrpc;
using Grpc.Core;

namespace API.Grpc.Model
{
    public record ChattingUserModel
    {
        public long PlayerId { get; set; }

        public string Name { get; set; }

        public int RoomId { get; set; }

        public IAsyncStreamWriter<ChatRes> Stream { get; set; }
    }
}
