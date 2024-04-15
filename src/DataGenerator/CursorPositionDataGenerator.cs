using Confluent.Kafka;
using SauberData;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataGenerator
{
    public class CursorPositionDataGenerator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Thread _generatorThread;

        private bool _shouldGenerate;
        private object _shouldGenerateLock;
        private bool _shouldStop;
        private object _shouldStopLock;

        public bool ShouldGenerate
        {
            get
            {
                lock (_shouldGenerateLock)
                {
                    return this._shouldGenerate;
                }
            }
            set
            {
                lock (_shouldGenerateLock)
                {
                    this._shouldGenerate = value;
                }
            }
        }

        public bool ShouldStop
        {
            get
            {
                lock (this._shouldStopLock)
                {
                    return this._shouldStop;
                }
            }
            set
            {
                lock (this._shouldStopLock)
                {
                    this._shouldStop = value;
                }
            }
        }

        public CursorPositionDataGenerator()
        {
            this._shouldGenerateLock = new object();
            this.ShouldGenerate = false;
            this._shouldStopLock = new object();
            this.ShouldStop = false;
            this._generatorThread = new Thread(new ThreadStart(GenerateData));
            this._generatorThread.Start();
        }

        private async void GenerateData()
        {
            log.Info("Start cursor data generator thread.");
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                while (!this.ShouldStop)
                {
                    if (this.ShouldGenerate)
                    {
                        // also add windows forms in this project for cursor position
                        // https://stackoverflow.com/questions/1316681/getting-mouse-position-in-c-sharp
                        // https://stackoverflow.com/questions/72058558/how-can-i-add-system-windows-forms-to-wpf-application-when-adding-via-reference
                        Point cursorPosition = Cursor.Position;
                        CursorPositionData data = new CursorPositionData(cursorPosition.X, cursorPosition.Y);

                        string messageValue = JsonSerializer.Serialize(data);
                        try
                        {
                            var result = await producer.ProduceAsync(
                                "cursorPosition",
                                new Message<string, string> { Key = DateTime.Now.ToString(), Value = messageValue }
                            );
                            log.Info($"{messageValue} - {result}");
                            // TODO: manage result
                        }
                        catch (ProduceException<string, string> e)
                        {
                            log.Error($"Delivery failed with {nameof(ProduceException<string, string>)}: {e.Error.Reason}");
                        }
                        catch (Exception e)
                        {
                            log.Error($"Delivery failed: {e.Message}");
                        }
                        // TODO: replace Thread sleep (not too bad since no events happen in this thread anyway that would be blocked but still not best practice)
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
