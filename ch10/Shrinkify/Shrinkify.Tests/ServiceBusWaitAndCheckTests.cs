using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static Shrinkify.ServiceBusOperations;
using static Shrinkify.ServiceBusWaitAndCheck;
using static Shrinkify.TestManager;

namespace Shrinkify.Functional.Tests
{
    [TestClass]
    public class ServiceBusWaitAndCheckTests
    {
        private readonly IDependencies<Test> _d;

        public class TestMessage
        {
            public TestMessage()
            {
            }

            public string Message { get; set; }
        }

        public class ImageUploaded
        {
            private bool _uploaded;

            public ImageUploaded()
            {
                _uploaded = false;
            }

            public void Uploaded()
            {
                _uploaded = true;
            }

            public bool IsUploaded => _uploaded;
        }

        public ServiceBusWaitAndCheckTests()
        {
            _d = GetService<IDependencies<Test>>();
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckFalseTimeoutTest()
        {
            const int TIMEOUT = 10000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var filter = "foobar";

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(false); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds >= TIMEOUT, "Did not wait for the timeout.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (TIMEOUT + ACCEPTABLELATENCY), "Took to long to return.");
                Assert.IsTrue(endedHow == Ended.Timeout || endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }

        }


        [TestMethod]
        public async Task ServiceBusWaitAndCheckZeroTimeUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 0;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            var endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    Task.Delay(WAITTIME).Wait();
                    imageUploaded.Uploaded();
                    SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Check || endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckImmediateUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 0;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                imageUploaded.Uploaded();

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Check);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckOneSecondUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 1000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    Task.Delay(WAITTIME).Wait();
                    imageUploaded.Uploaded();
                    SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Check);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckTwoSecondUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 2000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    Task.Delay(WAITTIME).Wait();
                    imageUploaded.Uploaded();
                    SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckFiveSecondUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 5000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    Task.Delay(WAITTIME).Wait();
                    imageUploaded.Uploaded();
                    SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckTenSecondUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 10000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    Task.Delay(WAITTIME).Wait();
                    imageUploaded.Uploaded();
                    SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckFifteenSecondUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 15000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    Task.Delay(WAITTIME).Wait();
                    imageUploaded.Uploaded();
                    SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < TIMEOUT, "Did not receive message in time.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckTwentySecondUploadTest()
        {
            const int TIMEOUT = 20000;
            const int WAITTIME = 20000;
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    try
                    {
                        Task.Delay(WAITTIME).Wait();
                        imageUploaded.Uploaded();
                        SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                    }
                    catch
                    {
                    }
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds > TIMEOUT, "Released before timeout.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
                Assert.IsTrue(endedHow == Ended.Timeout || endedHow == Ended.Success);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckRandomSecondUploadTest()
        {
            var r = new Random();
            const int TIMEOUT = 20000;
            int WAITTIME = r.Next(1, 22000);
            const int ACCEPTABLELATENCY = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var messageToSend = new TestMessage { Message = $"{subscription}" };
            var filter = "foobar";
            var imageUploaded = new ImageUploaded();

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                _ = Task.Run(() =>
                {
                    try
                    {
                        Task.Delay(WAITTIME).Wait();
                        imageUploaded.Uploaded();
                        SendMessageAsync(_d.Settings.ServiceBusAccount, topic, messageToSend, subscriptionFilter: filter).Wait();
                    }
                    catch
                    {
                    }
                });

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(imageUploaded.IsUploaded); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds > WAITTIME, "Did not wait long enough.");
                Assert.IsTrue(sw.ElapsedMilliseconds < (WAITTIME + ACCEPTABLELATENCY), "Did not receive message in time.");
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

        [TestMethod]
        public async Task ServiceBusWaitAndCheckTrueTest()
        {
            const int TIMEOUT = 10000;
            const int SHOULDBELESSTHAN = 2000;

            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            var filter = "foobar";

            await EnsureTopicExists(_d.Settings.ServiceBusAccount, topic);

            Ended endedHow = Ended.Unknown;

            try
            {
                var sw = new Stopwatch();
                sw.Start();

                await using (var waitAndCheck = new ServiceBusWaitAndCheck(_d.Settings.ServiceBusAccount, topic, subscription, filter, () => { return Task.FromResult(true); }, TIMEOUT))
                {
                    await waitAndCheck.WaitAsync();
                    endedHow = waitAndCheck.EndedHow;
                }

                sw.Stop();

                Assert.IsTrue(sw.ElapsedMilliseconds <= SHOULDBELESSTHAN);
            }
            finally
            {
                await DeleteTopic(_d.Settings.ServiceBusAccount, topic);
            }
        }

    }
}
