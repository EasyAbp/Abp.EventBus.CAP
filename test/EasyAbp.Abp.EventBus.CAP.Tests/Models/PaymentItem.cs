using System;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace EasyAbp.Abp.EventBus.CAP.Models
{
    [Serializable]
    public class PaymentItem : ExtensibleObject, IFullAuditedObject
    {
        #region Base properties

        public Guid Id { get; set; }

        [NotNull]
        public string ItemType { get; set; }
        
        public string ItemKey { get; set; }

        public decimal OriginalPaymentAmount { get; set; }

        public decimal PaymentDiscount { get; set; }
        
        public decimal ActualPaymentAmount { get; set; }
        
        public decimal RefundAmount { get; set; }
        
        public decimal PendingRefundAmount { get; set; }

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
        
        public Guid StoreId { get; set; }

        public void SetStoreId(Guid storeId)
        {
            StoreId = storeId;
        }
        
        public PaymentItem()
        {
            ExtraProperties = new ExtraPropertyDictionary();
            
            this.SetDefaultsForExtraProperties();
        }
    }
}
