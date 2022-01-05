using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.Abp.EventBus.CAP.Models
{
    [Serializable]
    public class Payment : ExtensibleObject, IFullAuditedObject, IMultiTenant
    {
        #region Base properties
        
        public Guid Id { get; set; }
        
        public Guid? TenantId { get; set; }
        
        public Guid UserId { get; set; }
        
        [NotNull]
        public string PaymentMethod { get; set; }
        
        [CanBeNull]
        public string PayeeAccount { get; set; }
        
        [CanBeNull]
        public string ExternalTradingCode { get; set; }
        
        [NotNull]
        public string Currency { get; set; }
        
        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }
        
        public decimal ActualPaymentAmount { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }

        public DateTime? CompletionTime { get; set; }
        
        public DateTime? CanceledTime { get; set; }
        
        public List<PaymentItem> PaymentItems { get; set; }
        
        #endregion

        #region Auditing properties

        public DateTime CreationTime { get; set; }
        
        public Guid? CreatorId { get; set; }
        
        public DateTime? LastModificationTime { get; set; }
        
        public Guid? LastModifierId { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public DateTime? DeletionTime { get; set; }
        
        public Guid? DeleterId { get; set; }

        #endregion


        public void SetPaymentItems(List<PaymentItem> paymentItems)
        {
            PaymentItems = paymentItems;
        }
    }
}