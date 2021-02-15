using Azure.Messaging.ServiceBus;
using Nito.AsyncEx;
using System;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;
using static Shrinkify.ServiceBusOperations;

namespace Shrinkify
{
    public class ServiceBusWaitAndCheck : IAsyncDisposable
    {
        public enum Ended
        {
            Unknown = 0,
            Success = 1,
            Error = 2,
            Timeout = 3,
            Check = 4
        }

        private readonly string _connectionString;

        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
 
        private readonly AsyncManualResetEvent _wait;
        private readonly int _timeout;

        private readonly string _topic;
        private readonly string _subscription;
        private readonly string _filter;

        private readonly Func<Task<bool>> _check;

        private Ended _ended;

        public ServiceBusWaitAndCheck(string connectionString, string topic, string subscription, string filter, Func<Task<bool>> check, int timeout = 60000)
        {
            CheckIsNotNullOrWhitespace(nameof(connectionString), connectionString);
            CheckIsNotNullOrWhitespace(nameof(topic), topic);
            CheckIsNotNullOrWhitespace(nameof(subscription), subscription);
            CheckIsNotNull(nameof(check), check);
            CheckIsNotLessThanOrEqualTo(nameof(timeout), timeout, 0);

            _topic = topic;
            _subscription = subscription;
            _filter = filter;

            _wait = new AsyncManualResetEvent(false);
            _timeout = timeout;
            _connectionString = connectionString;
            _check = check;

            _client = new ServiceBusClient(_connectionString);
            CreateSubscription(_connectionString, _topic, _subscription, _filter).Wait();

            _processor = _client.CreateProcessor(topic, subscription, new ServiceBusProcessorOptions());
            
            _processor.ProcessMessageAsync += _processor_ProcessMessageAsync;
            _processor.ProcessErrorAsync += _processor_ProcessErrorAsync;
        }

        ~ServiceBusWaitAndCheck()
        {
            DisposeInternal().Wait();
        }

        private async Task _processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            EndedHow = Ended.Error;

            _wait.Set();

            await Task.CompletedTask;
        }

        private async Task _processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            EndedHow = Ended.Success;

            _wait.Set();

            await Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeInternal();
            GC.SuppressFinalize(this);
        }

        protected virtual async Task DisposeInternal()
        {
            // Hmmm... why does StopProcessingAsync take so long?
            _ = Task.Run(() =>  { try { _processor.StopProcessingAsync().Wait(); } catch { } });
            try { await DeleteSubscription(_connectionString, _topic, _subscription); } catch { }
            try { await _client.DisposeAsync().AsTask(); } catch { }
        }

        public Ended EndedHow
        {   get { return _ended; }
            private set
            {
                if (_ended != Ended.Unknown)
                    return;

                _ended = value;
            }
        }

public async Task WaitAsync()
{
    await _processor.StartProcessingAsync();
            
    try
    {
        if (!(await _check()))
        {
            await Task.WhenAny(Task.Delay(_timeout), _wait.WaitAsync());

            EndedHow = Ended.Timeout;
        }
        else
        {
            EndedHow = Ended.Check;
        }
    }
    catch
    {
    }
}
    }
}
