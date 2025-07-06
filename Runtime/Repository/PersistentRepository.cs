using System;
using Klukva.RedKit.Persistence.Common;

namespace Klukva.RedKit.Persistence.Repository
{
    using Serialization;
    using Migration;
    using Storage;
    using Debug;
    
    public class PersistentRepository<T> : IPersistentRepository<T>
    {
        private readonly ISerializationStrategy<T> _serializer;
        private readonly IMigrationPipeline<T> _migration;
        private readonly IStorage _storage;
        private readonly ILogger _logger;

        public PersistentRepository(
            ISerializationStrategy<T> serializer,
            IMigrationPipeline<T> migration,
            IStorage storage,
            ILogger logger)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _migration = migration ?? throw new ArgumentNullException(nameof(migration));
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Save(T data)
        {
            try
            {
                var serialized = _serializer.Serialize(data);
                if (string.IsNullOrWhiteSpace(serialized))
                    throw new InvalidOperationException("Serialization produced empty string.");

                if (_storage.Exists())
                {
                    _storage.Backup();
                    _logger.Log("[Persistence] Backup created.");
                }

                _storage.Write(serialized);
                _logger.Log("[Persistence] Data saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error($"[Persistence] Save failed: {ex}");
            }
        }

        public Result<T> TryLoad()
        {
            if (!_storage.Exists())
            {
                _logger.Warn("[Persistence] No save found.");
                return Result<T>.Fail(default);
            }

            try
            {
                var raw = _storage.Read();

                if (string.IsNullOrWhiteSpace(raw))
                {
                    _logger.Warn("[Persistence] Read returned empty data.");
                    return Result<T>.Fail(default);
                }

                var deserialized = _serializer.Deserialize(raw);
                if (deserialized == null)
                {
                    _logger.Warn("[Persistence] Deserialized data is null.");
                    return Result<T>.Fail(default);
                }

                var migrated = _migration.Apply(deserialized);
                return Result<T>.SuccessResult(migrated);
            }
            catch (Exception ex)
            {
                _logger.Error($"[Persistence] Load failed: {ex}");
                return Result<T>.Fail(default);
            }
        }

        public void Delete()
        {
            try
            {
                _storage.Delete();
                _logger.Log("[Persistence] Save deleted.");
            }
            catch (Exception ex)
            {
                _logger.Error($"[Persistence] Delete failed: {ex}");
            }
        }
    }
}