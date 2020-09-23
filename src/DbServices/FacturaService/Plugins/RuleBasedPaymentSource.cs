using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using TheXDS.MCART.Types.Extensions;
using TheXDS.Proteus.Api;
using TheXDS.Proteus.Models;
using TheXDS.Proteus.Models.Base;

namespace TheXDS.Proteus.Plugins
{
    public abstract class RuleBasedPaymentSource<T, TId> : PaymentSource where T : ModelBase<TId>, new() where TId : notnull, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Colección con las reglas de validez del origen de pago.
        /// </summary>
        protected static readonly HashSet<(Func<T?, PaymentInfo, bool> Predicate, string Message)> Failures = new HashSet<(Func<T?, PaymentInfo, bool> Predicate, string Message)>();
        
        /// <summary>
        /// Obtiene el mensaje a mostrar cuando se solicite obtener el objeto
        /// de pago para este origen.
        /// </summary>
        protected virtual string Prompt => $"Ingrese o escanee {Name}";

        private bool Fails(TId id, PaymentInfo info, [NotNullWhen(false)] out T? entity)
        {
            entity = Proteus.Service<FacturaService>()!.Get<T, TId>(id);
            return Fails(entity, info);
        }
        private bool Fails(T? entity, PaymentInfo info)
        {
            if (Failures.FirstOrDefault(p => p.Predicate(entity, info)) is {Message: string msg })
            {
                Proteus.MessageTarget?.Stop(msg);
                return true;
            }
            return false;
        }

        public override PaymentInfo? Automatic(Factura? f)
        {
            var entity = GetEntity(f!, null);
            if (entity is null)
            {
                return PaymentInfo.Invalid;
            }
            var v = GetAmount(entity, f);
            if (v is null) return null;
            var info = new PaymentInfo(v.Value, entity.StringId);
            if (Fails(entity, info)) return PaymentInfo.Invalid;
            return info;
        }

        protected virtual decimal? GetAmount(T? entity, Factura? f) => null;
        protected virtual T? GetEntity(Factura fact, PaymentInfo? info)
        {
            TId id = GetId(info?.Tag);
            if (id.CompareTo(default) == 0 && (!Proteus.InputTarget?.Get(Prompt, ref id) ?? false))
            {
                return null;
            }
            return Proteus.Service<FacturaService>()!.Get<T, TId>(id);
        }
        public override Task<Payment?> TryPayment(Factura fact, PaymentInfo info)
        {
            var entity = GetEntity(fact, info);
            if (Fails(entity, info))
            { 
                return Task.FromResult<Payment?>(null);
            }
            OnGeneratePayment(fact, entity!, info);
            return base.TryPayment(fact, info);
        }

        private TId GetId(string? tag)
        {
            if (tag is null) return default!;
            if (typeof(TId) == typeof(string)) return (TId)(object)tag.OrEmpty();
            try
            {
                return MCART.Common.FindConverter<TId>()?.ConvertFromString(tag ?? "0") is TId i ? i : default(TId) ?? typeof(TId).New<TId>();
            }
            catch
            {
                return default!;
            }
        }

        protected abstract void OnGeneratePayment(Factura f, T entity, PaymentInfo info);
    }
}
