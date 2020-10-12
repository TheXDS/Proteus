/*
Copyright © 2017-2020 César Andrés Morgan
Licenciado para uso interno solamente.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TheXDS.MCART;
using TheXDS.MCART.Types;
using TheXDS.MCART.Types.Base;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Component.Attributes;
using TheXDS.Proteus.Models;

namespace TheXDS.Proteus.Component
{
    public abstract class SettingsRepository<T> : SettingsRepository where T : Enum
    {
        public override IEnumerable<Setting> Settings => base.Settings.Select(AppendMetadata);

        public Setting this[T setting]
        {
            get
            {
                var s = this[setting.ToString()];                
                return AppendMetadata(s, setting);
            }
            set => this[setting.ToString()] = value;
        }

        private Setting AppendMetadata(Setting s, T setting)
        {
            s.DataType = setting.GetAttr<SettingTypeAttribute>()?.Type ?? typeof(string);
            s.FriendlyName = setting.NameOf();
            return s;
        }
        private Setting AppendMetadata(Setting s)
        {
            return AppendMetadata(s, (T)Enum.Parse(typeof(T), s.Id));
        }

        protected override IEnumerable<KeyValuePair<string, string>> Defaults()
        {
            foreach (T j in typeof(T).GetEnumValues())
            {
                if (j!.HasAttr<DefaultAttribute>(out var v))
                {
                    yield return new KeyValuePair<string, string>(j!.ToString(), v!.Value!);
                }
            }
        }

        protected TValue GetAs<TValue>([CallerMemberName]string value = null!) where TValue : notnull
        {
            return GetAs<TValue>((T)Enum.Parse(typeof(T), value));
        }

        protected TValue GetAs<TValue>(T value) where TValue : notnull
        {
            var s = this[value].Value.OrNull() ?? value.GetAttr<DefaultAttribute>()?.Value ?? default(TValue)?.ToString() ?? "";
            if (typeof(TValue) == typeof(string)) return (TValue)(object)s;
            try
            {
                return typeof(TValue).GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null) is { } m
                    ? (TValue)m.Invoke(null, new[] { s })!
                    : Common.FindConverter<TValue>()?.ConvertFromString(s) is TValue v ? v! : default!;
            }
            catch (Exception ex)
            {
                Proteus.AlertTarget?.Alert("Hubo un problema obteniendo un valor de configuración.", $"No se pudo obtener el valor de configuración {value} a partir del valor almacenado '{s}' debido al siguiente error: {ex.Message}");
                return default!;
            }
        }
    }

    [SeederFor(typeof(UserService))]
    public abstract class SettingsRepository : ISettingsRepository
    {
        private readonly IExposeGuid _implementor;
        public SettingsRepository()
        {
            _implementor = new ExposeGuidImplementor(this);
        }

        protected ConfigRepository Repo => Proteus.Service<UserService>()!.Get<ConfigRepository, Guid>(Guid);

        public virtual IEnumerable<Setting> Settings => Repo.Settings;

        public Guid Guid => _implementor.Guid;

        public string Name => GetType().NameOf();

        public Setting this[string customSetting]
        {
            get
            {
                return Repo.Settings.FirstOrDefault(p => p.Id == customSetting);
            }
            set
            {
                if (this[customSetting] is null)
                    Repo.Settings.Add(new Setting { Id = customSetting, Value = value.Value });
                else
                    this[customSetting].Value = value.Value;
                Proteus.Service<UserService>()!.SaveAsync();
            }
        }

        public Task<DetailedResult> SeedAsync(IFullService service, IStatusReporter? reporter)
        {
            reporter?.UpdateStatus($"Creando repositorio de configuración {Guid}");
            var r = new ConfigRepository { Id = Guid };
            foreach (var j in Defaults()) r.Settings.Add(j);
            return service.AddAsync(r);
        }

        protected virtual IEnumerable<KeyValuePair<string, string>> Defaults()
        {
            yield break;
        }

        public async Task<bool> ShouldRunAsync(IReadAsyncService service, IStatusReporter? reporter)
        {
            reporter?.UpdateStatus($"Comprobando repositorio de configuración {Guid}...");
            return await service.GetAsync<ConfigRepository, Guid>(Guid) is null;
        }
    }
}