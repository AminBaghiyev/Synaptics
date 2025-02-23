using Cassandra;
using Microsoft.Extensions.Configuration;
using Synaptics.Application.Common.Results;
using Synaptics.Application.Interfaces.Services;
using Synaptics.Application.Queries.Message.MessagesBetweenUsers;
using Synaptics.Domain.Entities;
using Synaptics.Domain.Enums;

namespace Synaptics.Infrastructure.Services;

public class CassandraService : ICassandraService
{
    private readonly ISession _session;

    public CassandraService(IConfiguration configuration)
    {
        var cluster = Cluster.Builder()
            .AddContactPoint(configuration["Cassandra:Host"])
            .WithPort(Convert.ToInt32(configuration["Cassandra:Port"]))
            .WithCredentials(configuration["Cassandra:Username"], configuration["Cassandra:SecretKey"])
            .Build();

        using (var initialSession = cluster.Connect())
        {
            initialSession.Execute("CREATE KEYSPACE IF NOT EXISTS synapticsdb WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};");
        }

        _session = cluster.Connect("synapticsdb");

        _session.Execute(@"CREATE TABLE IF NOT EXISTS messages (
            message_id UUID,
            chat_id TEXT,
            sender_id TEXT,
            receiver_id TEXT,
            content TEXT,
            message_type INT,
            sent_at TIMESTAMP,
            PRIMARY KEY (chat_id, sent_at, message_id)
        ) WITH CLUSTERING ORDER BY (sent_at DESC, message_id ASC);");
    }

    private static string GenerateChatId(string user1, string user2)
    {
        string[] sortedUsers = new[] { user1, user2 }.OrderBy(u => u).ToArray();
        return $"{sortedUsers[0]}:{sortedUsers[1]}";
    }

    public async Task SaveMessageAsync(Message message)
    {
        string chatId = GenerateChatId(message.SenderId, message.ReceiverId);
        
        string query = "INSERT INTO messages (message_id, chat_id, sender_id, receiver_id, content, message_type, sent_at) " +
                    "VALUES (?, ?, ?, ?, ?, ?, ?)";
        var preparedStatement = await _session.PrepareAsync(query);
        var boundStatement = preparedStatement.Bind(
            message.MessageId,
            chatId,
            message.SenderId,
            message.ReceiverId,
            message.Content,
            (int)message.MessageType,
            message.SentAt
        );

        await _session.ExecuteAsync(boundStatement);
    }

    public async Task<CassandraPaginatedResult> GetMessagesBetweenUsersAsync(string senderId, string receiverId, byte[]? pagingState = null, int limit = 30)
    {
        string chatId = GenerateChatId(senderId, receiverId);
        string query = "SELECT * FROM messages WHERE chat_id = ? ORDER BY sent_at DESC, message_id ASC";
        
        var preparedStatement = await _session.PrepareAsync(query);
        var boundStatement = preparedStatement.Bind(chatId).SetAutoPage(false).SetPageSize(limit);

        if (pagingState is not null && pagingState.Length > 0) boundStatement.SetPagingState(pagingState);

        var rowSet = await _session.ExecuteAsync(boundStatement);
        
        var messages = rowSet.Select(row => new MessagesBetweenUsersQueryResponse
        {
            MessageId = row.GetValue<Guid>("message_id"),
            Content = row.GetValue<string>("content"),
            MessageType = (MessageTypes)row.GetValue<int>("message_type"),
            SentAt = row.GetValue<DateTime>("sent_at")
        }).ToList();
        
        return new CassandraPaginatedResult
        {
            Messages = messages,
            NextPagingState = rowSet.PagingState
        };
    }
}
